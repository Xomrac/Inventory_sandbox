namespace XomracCore.Characters.Base
{
	using UnityEngine;
	using Patterns.SL;

	public abstract class ACharacterMover : MonoBehaviour
	{
		[SerializeField] protected float _movementSpeed = 5f;
		public float MovementSpeed => _movementSpeed;

		[SerializeField] protected float _rotationSmoothness = .2f;
		public float RotationSmoothness => _rotationSmoothness;

		protected Rigidbody _rigidbody;
		protected Vector2 _movementDirection;
		protected float _currentSpeed;

		protected virtual void Awake() { }

		protected virtual void Start()
		{
			_currentSpeed = _movementSpeed;
			if (ServiceLocator.Of(this).TryGetService(out Rigidbody rb))
			{
				_rigidbody = rb;
			}
			else
			{
				Debug.LogWarning("ACharacterMover requires a Rigidbody component on the same GameObject to function properly.");
				enabled = false;
			}
		}

		public void ChangeMovementSpeed(float newSpeed)
		{
			_currentSpeed = newSpeed;
		}
		
		protected abstract void Rotate();

		protected abstract void Move();

		protected abstract void Stop();

	}

}