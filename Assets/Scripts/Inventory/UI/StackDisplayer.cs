namespace InventorySandbox.InventorySystem.UI
{

	using System;
	using Items;
	using NaughtyAttributes;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Component responsible for displaying an item stack in the inventory UI.
	/// Handles user interactions such as clicking and dragging.
	/// </summary>
	public class StackDisplayer : MonoBehaviour, IBeginDragHandler, IPointerClickHandler, IEndDragHandler, IDragHandler
	{
		[SerializeField] private Image _icon;
		[SerializeField] private TMPro.TextMeshProUGUI _quantityText;
		[SerializeField] private Image _background;
		[SerializeField] private GameObject _fillState;
		[SerializeField] private GameObject _emptyState;

		public event Action<StackDisplayer> BeganDrag;

		[SerializeField, ReadOnly] private int _stackQuantity;
		[SerializeField, ReadOnly] private int _slotIndex;
		[SerializeField, ReadOnly] private Item _item;
		public int SlotIndex => _slotIndex;

		private Action<StackDisplayer> OnSelect;

		public Item Item => _item;
		public bool IsEmpty => Item == null || _stackQuantity <= 0;
		public int StackQuantity => _stackQuantity;

		public void Initialize(Action<StackDisplayer> onSelect, int index)
		{
			OnSelect = onSelect;
			_slotIndex = index;
		}

		public void SetItem(Item item, int stackQuantity)
		{
			_item = item;
			_stackQuantity = stackQuantity;
			UpdateSlotUI(Item.Icon, $"x{_stackQuantity}", Item.Tier.Color, true);
		}

		public void ClearSlot()
		{
			_item = null;
			_stackQuantity = 0;
			UpdateSlotUI(null, string.Empty, Color.clear, false);
		}

		private void UpdateSlotUI(Sprite icon, string quantityText, Color backgroundColor, bool isFilled)
		{
			_icon.sprite = icon;
			_quantityText.text = quantityText;
			_background.color = backgroundColor;
			_fillState.SetActive(isFilled);
			_emptyState.SetActive(!isFilled);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log("Begin dragging item");
			if (IsEmpty) return;

			BeganDrag?.Invoke(this);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (IsEmpty) return;
			Debug.Log("Clicked on item");
			OnSelect?.Invoke(this);
		}

		// Unused interface methods
		// they are required by the implemented interfaces to make OnBeginDrag work
		public void OnEndDrag(PointerEventData eventData) { }

		public void OnDrag(PointerEventData eventData) { }
	}

}