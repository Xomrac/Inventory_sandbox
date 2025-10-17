namespace XomracCore.Characters.Base
{
	using UnityEngine;

	public abstract class ACharacterAnimator : MonoBehaviour
	{
		protected Animator _animator;

		protected virtual void Awake()
		{
			_animator = GetComponent<Animator>();
		}
	}

}