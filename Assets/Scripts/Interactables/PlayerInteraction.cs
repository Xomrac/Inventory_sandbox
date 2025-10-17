namespace InventorySandbox.Interactables
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using XomracCore.Patterns.SL;
	/// <summary>
	/// Handles player searching and interaction with interactable objects in the game world.
	/// </summary>
	public class PlayerInteraction : ACharacterInteraction
	{
		[SerializeField] private float _checkRate = 0.3f;
		[SerializeField] private float _checkRadius = 0.5f;
		[SerializeField] private LayerMask _interactableLayer;
		[SerializeField] private InputActionReference _interactInputAction;

		private AInteractable _currentInteractable;
		private Coroutine _searchCoroutine;

		private void Awake()
		{
			_interactInputAction.action.performed += OnInteract;
			StartSearchCoroutine();
		}

		private void OnEnable()
		{
			StartSearchCoroutine();
			_interactInputAction.action.Enable();
			_currentInteractable?.GainFocus();
		}

		private void Start()
		{
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.GameStateChanged += OnGameStateChanged;
			}
		}

		private void OnDisable()
		{
			StopSearchCoroutine();
			_interactInputAction.action.Disable();
			_currentInteractable?.LoseFocus();
		}
		
		private void OnInteract(InputAction.CallbackContext _)
		{
			Interact();
		}

		private void OnGameStateChanged(GameStates newState)
		{
			enabled = newState == GameStates.Gameplay;
		}

		private void StartSearchCoroutine()
		{
			if (_searchCoroutine != null) StopCoroutine(_searchCoroutine);
			_searchCoroutine = StartCoroutine(SearchInteractables());
		}

		private void StopSearchCoroutine()
		{
			if (_searchCoroutine != null)
			{
				StopCoroutine(_searchCoroutine);
				_searchCoroutine = null;
			}
		}

		// Continuously searches for interactable objects within a specified radius at defined intervals.
		// It's costly and useless to do it every frame.
		private IEnumerator SearchInteractables()
		{
			while (true)
			{
				CheckForInteractables();
				yield return new WaitForSeconds(_checkRate);
			}
		}

		private void CheckForInteractables()
		{
			Collider[] hits = Physics.OverlapSphere(transform.position, _checkRadius, _interactableLayer);
			AInteractable nearest = null;
			float nearestDistance = float.MaxValue;

			foreach (Collider hit in hits)
			{
				if (hit.TryGetComponent(out AInteractable interactable))
				{
					float distance = (transform.position - interactable.transform.position).sqrMagnitude;
					if (distance < nearestDistance)
					{
						nearest = interactable;
						nearestDistance = distance;
					}
				}
			}

			if (nearest != _currentInteractable)
			{
				_currentInteractable?.LoseFocus();
				_currentInteractable = nearest;
				_currentInteractable?.GainFocus();
			}
			else if (nearest == null)
			{
				_currentInteractable?.LoseFocus();
				_currentInteractable = null;
			}
		}

		private void Interact()
		{
			Debug.Log("Interact pressed");
			_currentInteractable?.Interact(this);
		}
	}

}