using System.Collections.Generic;
using UnityEngine;

namespace Diablo5.Bonuses
{

	[CreateAssetMenu(fileName = "ChestCooldownBonus", menuName = "Diablo5/Bonuses/New Chest Cooldown Bonus")]
	public class ChestCooldownBonusData : ABonusData
	{
		[SerializeField] private float _cooldownReduction = .5f;
		public float CooldownReduction => _cooldownReduction;

		public override bool TryToResolve(BonusManager bonusManager, out List<IBonus> bonuses)
		{
			bonuses = new List<IBonus>();
			Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
			if (chests == null || chests.Length == 0) return false;
			foreach (Chest chest in chests)
			{
				bonuses.Add(BonusFactory.CreateCooldownBonus(chest, _cooldownReduction, Duration));
			}
			return true;
		}
	}

}