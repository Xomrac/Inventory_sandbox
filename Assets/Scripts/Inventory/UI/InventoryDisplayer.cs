namespace InventorySandbox.InventorySystem.UI
{

	using Items;
	using XomracCore.Patterns.SL;
	using UnityEngine;

	/// <summary>
	/// Component responsible for displaying an inventory UI.
	/// Manages item slots, item selection, and drag-and-drop functionality.
	/// </summary>
	public class InventoryDisplayer : MonoBehaviour
	{

		[SerializeField] private GameObject _canvas;

		/// <summary>
		/// The panel displaying detailed information about an item.
		/// and interaction options.
		/// </summary>
		[SerializeField] private ItemInfoPanel _infoPanel;

		/// <summary>
		/// The object responsible for handling drag-and-drop previewing.
		/// </summary>
		[SerializeField] private DragAndDropPreviewer _dragAndDropPreviewer;

		private StackDisplayer[] _slots;
		private Inventory _currentInventory;

		private void Awake()
		{
			ServiceLocator.Global.AddService(this);
			InitializeSlots();
			_dragAndDropPreviewer.ItemDropped += OnItemDropped;
			Close();
		}

		/// <summary>
		/// Displays the specified inventory in the UI.
		/// </summary>
		/// <param name="inventory">The inventory to display.</param>
		public void DisplayInventory(Inventory inventory)
		{
			// Unsubscribe from the previous inventory's events if applicable
			if (_currentInventory != null)
			{
				_currentInventory.InventoryChanged -= OnCurrentInventoryChanged;
			}
			_currentInventory = inventory;
			_infoPanel.Initialize(_currentInventory);
			if (_currentInventory == null)
			{
				Debug.Log("Current inventory is null");
				return;
			}
			_currentInventory.InventoryChanged += OnCurrentInventoryChanged;
			DisplayStacks();
		}

		/// <summary>
		/// Closes the inventory UI and clears the displayed slots.
		/// </summary>
		public void Close()
		{
			if (_currentInventory == null)
			{
				_canvas.SetActive(false);
				foreach (StackDisplayer slot in _slots)
				{
					slot.ClearSlot();
				}
				return;
			}
			_currentInventory.InventoryChanged -= OnCurrentInventoryChanged;
			_currentInventory = null;
			_infoPanel.Close();
			_dragAndDropPreviewer.Close();
			_canvas.SetActive(false);
		}

		private void InitializeSlots()
		{
			_slots = GetComponentsInChildren<StackDisplayer>(true);
			for (int index = 0; index < _slots.Length; index++)
			{
				StackDisplayer slot = _slots[index];
				slot.Initialize(OnItemSelected, index);
				slot.BeganDrag += _dragAndDropPreviewer.OnSlotBeganDrag;
			}
		}

		/// <summary>
		/// Handles the event when an item is dropped on top of another slot.
		/// </summary>
		/// <param name="draggedSlot">The slot from which the item was dragged.</param>
		/// <param name="destinationSlot">The slot where the item was dropped.</param>
		private void OnItemDropped(StackDisplayer draggedSlot, StackDisplayer destinationSlot)
		{
			if ((draggedSlot == destinationSlot) || (destinationSlot == null)) return;

			if (destinationSlot.Item == null)
			{
				_currentInventory.ChangeItemIndex(draggedSlot.Item, destinationSlot.SlotIndex);
				return;
			}

			_currentInventory.SwapItemIndexes(draggedSlot.Item, destinationSlot.Item);
		}

		/// <summary>
		/// Handles the event when an item slot is selected.
		/// </summary>
		/// <param name="selectedSlot">The selected item slot.</param>
		private void OnItemSelected(StackDisplayer selectedSlot)
		{
			if (_infoPanel == null)
			{
				Debug.Log("No context menu assigned");
				return;
			}
			_infoPanel.Show(selectedSlot);
		}

		/// <summary>
		/// Handles the event when the current inventory changes.
		/// Updates the displayed stacks.
		/// </summary>
		private void OnCurrentInventoryChanged()
		{
			DisplayStacks();
		}

		/// <summary>
		/// Displays the item stacks in the inventory UI.
		/// </summary>
		private void DisplayStacks()
		{
			if (_currentInventory == null)
			{
				Debug.Log("Current inventory is null");
				return;
			}
			_canvas.SetActive(true);
			foreach (StackDisplayer slot in _slots)
			{
				slot.ClearSlot();
			}
			foreach ((Item item, ItemStack data) in _currentInventory.Items)
			{
				StackDisplayer slot = _slots[data.slotIndex];
				slot.gameObject.SetActive(true);
				slot.SetItem(item, data.Quantity);
			}
		}
	}

}