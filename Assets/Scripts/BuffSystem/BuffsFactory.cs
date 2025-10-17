namespace InventorySandbox.Buffs
{

	using Interactables;
	using XomracCore.Characters.Base;

	//it may be overkill right now, but it's better to setup this workflow for future cases
	public static class BuffsFactory
	{
		public static MovementSpeedBuff CreateMovementSpeedBuff(ACharacterMover CharacterMover, float bonusAmount, float duration)
		{
			return new MovementSpeedBuff(CharacterMover, bonusAmount, duration);
		}

		public static ChestCooldownBuff CreateChestCooldownBuff(Chest chest, float bonusAmount, float duration)
		{
			return new ChestCooldownBuff(chest, bonusAmount, duration);
		}
	}

}