namespace InventorySandbox.InventorySystem.UI
{

	using System;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	/// <summary>
	/// Component that previews the item being dragged in a drag-and-drop inventory system.
	/// </summary>
	public class DragAndDropPreviewer : MonoBehaviour
	{
		[SerializeField] private RectTransform _previewTransform;
		[SerializeField] private Image _icon;
		[SerializeField] private Image _background;
		[SerializeField] private TextMeshProUGUI _quantityLabel;

		private StackDisplayer _draggedSlot;

		public event Action<StackDisplayer, StackDisplayer> ItemDropped;

		private void Awake()
		{
			Close();
		}

		private void Update()
		{
			if (_previewTransform != null && _previewTransform.gameObject.activeSelf)
			{
				_previewTransform.position = Input.mousePosition;
				if (Input.GetMouseButtonUp(0))
				{
					Drop();
				}
			}
		}

		public void OnSlotBeganDrag(StackDisplayer slot)
		{
			if (slot == null || slot.Item == null || _previewTransform == null)
			{
				return;
			}
			_draggedSlot = slot;
			_previewTransform.gameObject.SetActive(true);
			_previewTransform.position = Input.mousePosition;
			_icon.sprite = slot.Item.Icon;
			_background.color = slot.Item.Tier.Color;
			_quantityLabel.text = $"x{slot.StackQuantity}";
		}

		public void Close()
		{
			_previewTransform.gameObject.SetActive(false);
		}

		private void Drop()
		{
			Close();
			var pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};
			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);

			foreach (RaycastResult result in results)
			{
				var dropSlot = result.gameObject.GetComponent<StackDisplayer>();
				if (dropSlot != null)
				{
					ItemDropped?.Invoke(_draggedSlot, dropSlot);
					_draggedSlot = null;
					break;
				}
			}
		}
	}

}