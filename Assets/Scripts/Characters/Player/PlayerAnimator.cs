using UnityEngine;

namespace XomracCore.Characters
{
	using Base;
	public class PlayerAnimator : ACharacterAnimator
	{
		private static readonly int Speed = Animator.StringToHash("Speed");

		public void SetSpeed(float speed)
		{
			_animator.SetFloat(Speed, speed);
		}
	}

}