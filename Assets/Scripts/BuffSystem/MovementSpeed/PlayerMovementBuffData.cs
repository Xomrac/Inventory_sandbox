using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using XomracCore.Characters.PlayerCharacter;
using XomracCore.Patterns.SL;

namespace InventorySandbox.Buffs
{

	[CreateAssetMenu(fileName = "PlayerMovementBonus", menuName = "InventorySandbox/Buffs/New Player Movement Buff")]
	public class PlayerMovementBuffData : ABuffData
	{
		[InfoBox("Multiplier applied to the player's movement speed.")]
		[SerializeField] private float _bonusAmount = 1.5f;
		public float BonusAmount => _bonusAmount;

		public override bool TryToResolve(out List<IBuff> bonuses)
		{
			bonuses = new List<IBuff>();
			var player = FindFirstObjectByType<Player>();
			if (player == null) return false;
			if (ServiceLocator.Of(player).TryGetService(out PlayerMover playerMover))
			{
				bonuses.Add(BuffsFactory.CreateMovementSpeedBuff(playerMover, _bonusAmount, Duration));
				return true;
			}
			return false;
		}
	}

}