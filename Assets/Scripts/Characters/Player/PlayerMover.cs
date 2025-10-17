namespace XomracCore.Characters.PlayerCharacter
		{
		    using Diablo5;
		    using Patterns.SL;
		    using UnityEngine;
		    using UnityEngine.InputSystem;
		    using Base;
		
		    /// <summary>
		    /// Handles player movement, including input processing, movement, and rotation.
		    /// </summary>
		    public class PlayerMover : ACharacterMover
		    {
		        [SerializeField] private InputActionReference _moveAction; 
		
		        private Transform _camera; 
		        private PlayerAnimator _animator;
		
		        protected override void Awake()
		        {
		            base.Awake();
		            _moveAction.action.performed += OnMove;
		            _moveAction.action.canceled += OnMovementCanceled;
		        }

		        protected override void Start()
		        {
		            base.Start();
		            _camera = Camera.main.transform;
		            _animator = ServiceLocator.Of(this).GetService<PlayerAnimator>();
		
		            if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
		            {
		                gameStateManager.GameStateChanged += OnGameStateChanged;
		            }
		        }
				
		        private void FixedUpdate()
		        {
		            Move();
		            Rotate();
		        }
				
		        private void OnDestroy()
		        {
		            _moveAction.action.performed -= OnMove;
		            _moveAction.action.canceled -= OnMovementCanceled;
		        }
		

		        private void OnMovementCanceled(InputAction.CallbackContext _)
		        {
		            Debug.Log("Movement Canceled");
		            Stop();
		        }
				
		        private void OnMove(InputAction.CallbackContext context)
		        {
		            var inputDirection = context.ReadValue<Vector2>(); 
		            if (inputDirection == Vector2.zero)
		            {
		                Stop();
		                return;
		            }
					// Adjust input direction based on camera orientation
			        _movementDirection = GetMovementDirection(inputDirection);
		        }
				
		        private void OnGameStateChanged(GameStates newState)
		        {
		            if (newState != GameStates.Gameplay)
		            {
		                _moveAction.action.Disable();
		                Stop();
		            }
		            else
		            {
		                _moveAction.action.Enable();
		            }
		        }
		
		        
		        protected override void Move()
		        {
		            if (_movementDirection == Vector2.zero)
		            {
						// If there's no movement input, stop the player and set animation speed to 0 to play the idle animation in the blend tree.
		                _animator?.SetSpeed(0);
		                return;
		            }
					// Set the blend tree speed parameter to control movement animations.
		            _animator?.SetSpeed(_currentSpeed);
		            Vector3 movementDelta = new Vector3(_movementDirection.x, 0, _movementDirection.y) * (_currentSpeed * Time.fixedDeltaTime);
		            _rigidbody.MovePosition(_rigidbody.position + movementDelta);
		        }
				
		        protected override void Stop()
		        {
		            _movementDirection = Vector2.zero; 
		            _rigidbody.linearVelocity = Vector3.zero; 
		            _rigidbody.angularVelocity = Vector3.zero; 
		            _animator?.SetSpeed(0);
		        }
		
		        /// <summary>
		        /// Rotates the player to face the movement direction.
		        /// </summary>
		        protected override void Rotate()
		        {
		            if (_movementDirection == Vector2.zero) return;
					
		            float angle = Mathf.Atan2(_movementDirection.y, _movementDirection.x) * Mathf.Rad2Deg;
			        // Adjust angle to match Unity's coordinate system.
					// counter-clockwise rotations are positive in Unity, while the calculated angle assumes clockwise rotations are positive.
					float unityAngle = -angle;
			        // Offset by 90 degrees to align with the forward direction.
					float finalAngle = unityAngle + 90;
		            
					_rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, Quaternion.AngleAxis(finalAngle, Vector3.up), _rotationSmoothness);
		        }
		
		        /// <summary>
		        /// Calculates the movement direction based on input and camera orientation.
		        /// </summary>
		        /// <param name="inputDirection">The input direction.</param>
		        /// <returns>The calculated movement direction.</returns>
		        private Vector2 GetMovementDirection(Vector2 inputDirection)
		        {
			        // Flatten and normalize the camera's forward and right vector.					
		            Vector3 forward = new Vector3(_camera.forward.x, 0, _camera.forward.z).normalized; 
		            Vector3 right = new Vector3(_camera.right.x, 0, _camera.right.z).normalized;
					
		            Vector3 desiredMoveDirection = forward * inputDirection.y + right * inputDirection.x; 
		            return new Vector2(desiredMoveDirection.x, desiredMoveDirection.z).normalized;
		        }
		    }
		}