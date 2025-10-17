using XomracCore.Characters.Base;

namespace Diablo5.Bonuses
{

	public static class BonusFactory
	{
		public static MovementBonus CreateMovementBonus(ACharacterMover CharacterMover, float bonusAmount, float duration)
		{
			return new MovementBonus(CharacterMover, bonusAmount, duration);
		}
		
		public static SpawnRateBonus CreateCooldownBonus(Chest chest,float bonusAmount, float duration)
		{
			return new SpawnRateBonus(chest, bonusAmount, duration);
		}
	}

}