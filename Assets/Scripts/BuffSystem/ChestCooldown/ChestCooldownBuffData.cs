using NaughtyAttributes;

namespace InventorySandbox.Buffs
{
	using System.Collections.Generic;
	using Interactables;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ChestCooldownBonus", menuName = "InventorySandbox/Buffs/New Chest Cooldown Buff")]
	public class ChestCooldownBuffData : ABuffData
	{
		[InfoBox("Multiplier applied to the chest's cooldown time.")]
		[SerializeField] private float _cooldownReduction = .5f;
		public float CooldownReduction => _cooldownReduction;

		public override bool TryToResolve(out List<IBuff> bonuses)
		{
			bonuses = new List<IBuff>();
			Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
			if (chests == null || chests.Length == 0) return false;
			foreach (Chest chest in chests)
			{
				bonuses.Add(BuffsFactory.CreateChestCooldownBuff(chest, _cooldownReduction, Duration));
			}
			return true;
		}
	}

}