using Diablo5.InventorySystem.Items;
using XomracCore.Patterns.SL;

namespace Diablo5.InventorySystem
{

	using UnityEngine;

	public class InventoryDisplayer : MonoBehaviour
	{
		[SerializeField] private GameObject _canvas;
		[SerializeField] private ItemInfoPanelDisplayer _infoPanel;
		[SerializeField] private DragAndDropPreviewer _dragAndDropPreviewer;

		private UIInventorySlot[] _inventorySlots;
		private int _maxSlots;

		private Inventory _currentInventory;

		private void Awake()
		{
			ServiceLocator.Global.AddService(this);
			_inventorySlots = GetComponentsInChildren<UIInventorySlot>(true);
			for (int index = 0; index < _inventorySlots.Length; index++)
			{
				UIInventorySlot slot = _inventorySlots[index];
				slot.Initialize(OnItemSelected, index);
				slot.BeganDrag += _dragAndDropPreviewer.OnSlotBeganDrag;
			}
			_maxSlots = _inventorySlots.Length;
			_dragAndDropPreviewer.ItemDropped += OnItemDropped;

			Close();
		}

		private void OnItemDropped(UIInventorySlot draggedSlot, UIInventorySlot destinationSlot)
		{
			if ((draggedSlot == destinationSlot) || (destinationSlot == null)) return;

			if (destinationSlot.Item == null)
			{
				_currentInventory.changeItemIndex(draggedSlot.Item, destinationSlot.SlotIndex);
				return;
			}

			_currentInventory.SwapItemIndexes(draggedSlot.Item, destinationSlot.Item);
		}

		private void OnItemSelected(UIInventorySlot selectedSlot)
		{
			if (_infoPanel == null)
			{
				Debug.Log("No context menu assigned");
				return;
			}
			_infoPanel.Show(selectedSlot);
		}

		public void DisplayInventory(Inventory inventory)
		{
			// just in case we were displaying another inventory before
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
			Display();
		}

		private void OnCurrentInventoryChanged()
		{
			Display();
		}

		private void Display()
		{
			if (_currentInventory == null)
			{
				Debug.Log("Current inventory is null");
				return;
			}
			_canvas.SetActive(true);
			foreach (UIInventorySlot slot in _inventorySlots)
			{
				slot.ClearSlot();
			}
			foreach ((Item item, ItemStack data) in _currentInventory.Items)
			{
				UIInventorySlot slot = _inventorySlots[data.slotIndex];
				slot.gameObject.SetActive(true);
				slot.SetItem(item, data.Quantity);
			}
		}

		public void Close()
		{
			if (_currentInventory == null)
			{
				_canvas.SetActive(false);
				foreach (UIInventorySlot slot in _inventorySlots)
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

	}

}