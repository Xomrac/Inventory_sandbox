namespace InventorySandbox.Buffs
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Cysharp.Threading.Tasks;
	using UI;
	using UnityEngine;
	using XomracCore.Bootstrap;
	using XomracCore.Patterns.SL;
	using XomracCore.TimeManagement;

	//TODO: rework buff system to better handle stacking and unique buffs or same buff applied multiple times to same or different targets
	public class BuffsManager : MonoBehaviour, IBootstrappable
	{
		[SerializeField] private ActiveBuffsDisplayer _activeBuffsDisplayer;

		private readonly Dictionary<IBuff, Timer> _activeBuffs = new();

		private event Action Ticked;
		public event Action<ABuffData, Timer> UniqueBuffAdded;

		public async UniTask Bootstrap()
		{
			ServiceLocator.Global.AddService(this);
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.GameStateChanged += OnGameStateChanged;
			}
			_activeBuffsDisplayer?.Initialize(this);
			await UniTask.Yield();
		}

		private void OnGameStateChanged(GameStates newState)
		{
			enabled = newState == GameStates.Gameplay;
			if (!enabled)
			{
				foreach (Timer cooldown in _activeBuffs.Values)
				{
					cooldown.Pause();
				}
			}
			else
			{
				foreach (Timer cooldown in _activeBuffs.Values)
				{
					cooldown.Resume();
				}
			}
		}

		private void Update()
		{
			Ticked?.Invoke();
		}

		/// <summary>
		/// Resolves a bonus based on the provided buff data. This method processes the buff data,
		/// determines if new bonuses can be resolved, and stores them if applicable. It also triggers
		/// the UniqueBonusAdded event for any newly added unique bonuses.
		/// </summary>
		/// <param name="buffData">The data representing the buff to be resolved.</param>
		public void ResolveBonus(ABuffData buffData)
		{
			if (buffData == null) return;
			var storedBonusTypes = new List<Type>();

			foreach (KeyValuePair<IBuff, Timer> buff in _activeBuffs.Where(buff => !storedBonusTypes.Contains(buff.Key.GetType())))
			{
				storedBonusTypes.Add(buff.Key.GetType());
			}

			if (buffData.TryToResolve(out List<IBuff> bonuses))
			{
				foreach (IBuff bonus in bonuses)
				{
					// Store the resolved bonus
					StoreBonus(bonus);
					// If the bonus type is already stored, skip triggering the event
					if (storedBonusTypes.Contains(bonus.GetType())) continue;
					// Add the new bonus type to the list and trigger the UniqueBonusAdded event
					storedBonusTypes.Add(bonus.GetType());
					UniqueBuffAdded?.Invoke(buffData, _activeBuffs[bonus]);
				}
			}
		}

		public void RemoveBonus<T>(object target) where T : IBuff
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

		private void RemoveBonuses(Func<IBuff, bool> check)
		{
			List<IBuff> bonusesToRemove = _activeBuffs.Keys.Where(check).ToList();

			foreach (IBuff bonus in bonusesToRemove)
			{
				bonus.Remove();
				Ticked -= _activeBuffs[bonus].Tick;
				_activeBuffs[bonus].Cancel();
				_activeBuffs.Remove(bonus);
			}
		}

		private void StoreBonus(IBuff buff)
		{
			if (_activeBuffs.TryGetValue(buff, out Timer activeBuff))
			{
				activeBuff.ModifyDuration(buff.Duration);
				activeBuff.Reset();
			}
			else
			{
				var cooldown = new Timer(buff.Duration);
				cooldown.Completed += () => OnCooldownCompleted(buff);
				Ticked += cooldown.Tick;
				_activeBuffs.Add(buff, cooldown);
				buff.Apply();
			}
		}

		private void OnCooldownCompleted(IBuff buff)
		{
			buff.Remove();
			_activeBuffs.Remove(buff);
		}

	}

}