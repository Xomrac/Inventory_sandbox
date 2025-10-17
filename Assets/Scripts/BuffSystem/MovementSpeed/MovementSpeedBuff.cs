using XomracCore.Characters.Base;

namespace InventorySandbox.Buffs
{

	public class MovementSpeedBuff : IBuff
	{
		private readonly float _bonusAmount;
		private readonly ACharacterMover _target;
		private readonly float _originalSpeed;

		public MovementSpeedBuff(ACharacterMover target, float bonusAmount, float duration)
		{
			_target = target;
			_bonusAmount = bonusAmount;
			Duration = duration;
			_originalSpeed = target.MovementSpeed;
		}

		public void Apply()
		{
			var newSpeed = _target.MovementSpeed * _bonusAmount;
			_target.ChangeMovementSpeed(newSpeed);
		}

		public void Remove()
		{
			_target.ChangeMovementSpeed(_originalSpeed);
		}

		public float Duration { get; }
		public object Target => _target;

	}

}