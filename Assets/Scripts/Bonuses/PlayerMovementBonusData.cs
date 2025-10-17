using System.Collections.Generic;
using UnityEngine;
using XomracCore.Characters.PlayerCharacter;
using XomracCore.Patterns.SL;

namespace Diablo5.Bonuses
{

	[CreateAssetMenu(fileName = "PlayerMovementBonus", menuName = "Diablo5/Bonuses/New Player Movement Bonus")]
	public class PlayerMovementBonusData : ABonusData
	{
		[SerializeField] private float _bonusAmount = 1.5f;
		public float BonusAmount => _bonusAmount;

		public override bool TryToResolve(BonusManager bonusManager, out List<IBonus> bonuses)
		{
			bonuses = new List<IBonus>();
			var player = FindFirstObjectByType<Player>();
			if (player == null) return false;
			if (ServiceLocator.Of(player).TryGetService(out PlayerMover playerMover))
			{
				bonuses.Add(BonusFactory.CreateMovementBonus(playerMover, _bonusAmount, Duration));
				return true;
			}
			return false;
		}
	}

}