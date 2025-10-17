namespace InventorySandbox.Buffs
{
	using Interactables;

	public class ChestCooldownBuff : IBuff
	{
		private readonly float _bonusAmount;
		private readonly Chest _target;
		private readonly float _originalDuration;

		public ChestCooldownBuff(Chest target, float bonusAmount, float duration)
		{
			_target = target;
			_bonusAmount = bonusAmount;
			Duration = duration;
			_originalDuration = target.BaseCooldownDuration;
		}

		public void Apply()
		{
			float newSpeed = _target.BaseCooldownDuration * _bonusAmount;
			_target.ChangeCooldownDuration(newSpeed);
		}

		public void Remove()
		{
			_target.ChangeCooldownDuration(_originalDuration);
		}

		public float Duration { get; }
		public object Target => _target;
	}

}