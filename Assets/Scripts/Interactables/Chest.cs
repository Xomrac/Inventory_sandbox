namespace Diablo5
{

	using System.Collections;
	using Characters;
	using InventorySystem.Items;
	using UnityEngine;
	using XomracCore.Patterns.SL;

	public class Chest : AInteractable
	{
		[SerializeField] private Animator _chestAnimator;
		[SerializeField] private float _baseCooldownDuration = 10f;
		public float BaseCooldownDuration => _baseCooldownDuration;
		[SerializeField] private Item[] _droppableItems;
		[SerializeField] private CooldownDisplayer _cooldownDisplayer;

		private float _cooldownDuration;
		private Timer _currentCooldown;
		private Coroutine _cooldownUpdate;
		private bool _isOnCooldown;

		private static readonly int Open = Animator.StringToHash("Open");
		private static readonly int Close = Animator.StringToHash("Close");

		protected override void Awake()
		{
			base.Awake();
			LoseFocus();
			_cooldownDuration = _baseCooldownDuration;
			_cooldownDisplayer.gameObject.SetActive(false);
		}

		private void Start()
		{
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.GameStateChanged += OnGameStateChanged;
			}
		}

		private void OnDestroy()
		{
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.GameStateChanged -= OnGameStateChanged;
			}
		}

		public void ChangeCooldownDuration(float duration)
		{
			_cooldownDuration = duration;
			_currentCooldown?.ModifyDuration(_cooldownDuration);
		}

		public override void Interact(ACharacterInteraction interactor)
		{
			if (!_isInteractable) return;

			_isInteractable = false;
			Item droppedItem = LootTable.GetWeightedItem(_droppableItems);
			var inventory = ServiceLocator.Of(interactor).GetService<CharacterInventory>();

			if (droppedItem && interactor && inventory)
			{
				inventory.Inventory.TryAddItem(droppedItem);
			}
			
			_chestAnimator.SetTrigger(Open);
			LoseFocus();
			RestartCooldown();
		}

		private void OnGameStateChanged(GameStates newState)
		{
			if (newState == GameStates.Gameplay)
			{
				_currentCooldown?.Resume();
			}
			else
			{
				_currentCooldown?.Pause();
			}
		}

		private void RestartCooldown()
		{
			if (_cooldownUpdate != null)
			{
				StopCoroutine(_cooldownUpdate);
			}
			_cooldownUpdate = StartCoroutine(Cooldown());
		}

		private IEnumerator Cooldown()
		{
			_isOnCooldown = true;
			_cooldownDisplayer.gameObject.SetActive(true);
			_currentCooldown = new Timer(_cooldownDuration);
			_currentCooldown.Completed += OnCompleted;
			_cooldownDisplayer.DisplayCooldown(_currentCooldown);

			while (_isOnCooldown)
			{
				_currentCooldown.Tick();
				yield return null;
			}
		}

		private void OnCompleted()
		{
			_isOnCooldown = false;
			StopCooldown();
			_chestAnimator.SetTrigger(Close);
			_isInteractable = true;
		}

		private void StopCooldown()
		{
			if (_cooldownUpdate != null)
			{
				StopCoroutine(_cooldownUpdate);
				_cooldownUpdate = null;
			}

			_cooldownDisplayer.gameObject.SetActive(false);
			_currentCooldown.Completed -= OnCompleted;
			_currentCooldown = null;
		}
	}

}