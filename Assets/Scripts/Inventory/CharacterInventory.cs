namespace InventorySandbox.InventorySystem
{
	using UI;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using XomracCore.Patterns.SL;

	/// <summary>
	/// Wraps an inventory for a character and handles input to toggle the inventory display.
	/// </summary>
	public class CharacterInventory : MonoBehaviour
	{
		[SerializeField] private InputActionReference _toggleInventoryAction;

		private bool _inventoryOpen = false;
		private InventoryDisplayer _inventoryDisplayer;
		private Inventory _inventory;
		public Inventory Inventory => _inventory;

		private void Awake()
		{
			_inventory = new Inventory();
			_toggleInventoryAction.action.performed += OnInputButtonPressed;
		}

		private void Start()
		{
			_inventoryDisplayer = ServiceLocator.Global.GetService<InventoryDisplayer>();
		}

		private void OnInputButtonPressed(InputAction.CallbackContext _)
		{
			if (!_inventoryDisplayer) return;

			_inventoryOpen = !_inventoryOpen;
			if (_inventoryOpen)
			{
				_inventoryDisplayer.DisplayInventory(_inventory);
				SetGameState(GameStates.Inventory);
			}
			else
			{
				_inventoryDisplayer.Close();
				SetGameState(GameStates.Gameplay);
			}
		}

		private void SetGameState(GameStates state)
		{
			if (ServiceLocator.Global.TryGetService(out GameStateManager gameStateManager))
			{
				gameStateManager.SetState(state);
			}
		}
	}

}