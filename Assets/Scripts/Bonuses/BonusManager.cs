using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using XomracCore.Characters.PlayerCharacter;
using XomracCore.Patterns.SL;

namespace Diablo5.Bonuses
{

	public class BonusManager : MonoBehaviour
	{
		private Dictionary<IBonus, Timer> _activeBonuses = new();

		private event Action Ticked;
		public event Action<ABonusData,Timer> BonusAdded;

		private void Awake()
		{
			ServiceLocator.Global.AddService(this);
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.GameStateChanged += OnGameStateChanged;
			}
		}

		private void OnGameStateChanged(GameStates newState)
		{
			enabled = newState == GameStates.Gameplay;
			if (!enabled)
			{
				foreach (Timer cooldown in _activeBonuses.Values)
				{
					cooldown.Pause();
				}
			}
			else
			{
				foreach (Timer cooldown in _activeBonuses.Values)
				{
					cooldown.Resume();
				}
			}
			
		}

		private void Update()
		{
			Ticked?.Invoke();
		}

		public void ResolveBonus(ABonusData bonusData)
		{
			if (bonusData == null) return;
			var storedBonusTypes = new List<Type>();
			if (bonusData.TryToResolve(this, out List<IBonus> bonuses))
			{
				foreach (IBonus bonus in bonuses)
				{
					StoreBonus(bonus, out bool isRefreshed);
					if (isRefreshed || storedBonusTypes.Contains(bonus.GetType())) continue;
					storedBonusTypes.Add(bonus.GetType());
					BonusAdded?.Invoke(bonusData,_activeBonuses[bonus]);
				}
			}
		}

		public void RemoveBonus<T>(object target) where T : IBonus
		{
			RemoveBonuses(bonus => bonus is T typedBonus && typedBonus.Target == target);
		}

		public void RemoveAllBonusesFrom(object target)
		{
			RemoveBonuses(bonus => bonus.Target == target);
		}

		public void RemoveAllBonuses()
		{
			RemoveBonuses(_ => true);
		}

		private void RemoveBonuses(Func<IBonus, bool> check)
		{
			List<IBonus> bonusesToRemove = _activeBonuses.Keys.Where(check).ToList();

			foreach (IBonus bonus in bonusesToRemove)
			{
				bonus.Remove();
				Ticked -= _activeBonuses[bonus].Tick;
				_activeBonuses[bonus].Cancel();
				_activeBonuses.Remove(bonus);
			}
		}
		
		

		private void StoreBonus(IBonus bonus, out bool isRefreshed)
		{
			if (_activeBonuses.ContainsKey(bonus))
			{
				_activeBonuses[bonus].Reset();
				isRefreshed = true;
			}
			else
			{
				var cooldown = new Timer(bonus.Duration);
				cooldown.Completed += () => OnCooldownCompleted(bonus);
				Ticked += cooldown.Tick;
				_activeBonuses.Add(bonus, cooldown);
				bonus.Apply();
				isRefreshed = false;
			}
		}

		private void OnCooldownCompleted(IBonus bonus)
		{
			bonus.Remove();
			_activeBonuses.Remove(bonus);
		}
	}

}