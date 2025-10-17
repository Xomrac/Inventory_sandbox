using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Diablo5.InventorySystem
{

	public class DragAndDropPreviewer : MonoBehaviour
	{
		[SerializeField] private RectTransform _previewTransform;
		[SerializeField] private Image _icon;
		[SerializeField] private Image _background;
		[SerializeField] private TextMeshProUGUI _quantityLabel;

		private UIInventorySlot _draggedSlot;

		public event Action<UIInventorySlot, UIInventorySlot> ItemDropped;

		private void Awake()
		{
			Close();
		}

		public void OnSlotBeganDrag(UIInventorySlot slot)
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

		private void Update()
		{
			if (_previewTransform != null && _previewTransform.gameObject.activeSelf)
			{
				_previewTransform.position = Input.mousePosition;
				if (Input.GetMouseButtonUp(0))
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
						var dropSlot = result.gameObject.GetComponent<UIInventorySlot>();
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

	}

}