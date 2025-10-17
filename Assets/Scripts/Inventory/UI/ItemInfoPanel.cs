namespace InventorySandbox.InventorySystem.UI
{

	using Items;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Panel to display detailed information about an item in the inventory.
	/// Allows the user to use or remove the item.
	/// </summary>
	public class ItemInfoPanel : MonoBehaviour
	{
		[SerializeField] private RectTransform _panel;
		[SerializeField] private Image _icon;
		[SerializeField] private TextMeshProUGUI _nameLabel;
		[SerializeField] private TextMeshProUGUI _tierLabel;
		[SerializeField] private TextMeshProUGUI _itemTypeLabel;
		[SerializeField] private TextMeshProUGUI _descriptionLabel;
		[SerializeField] private Button _useButton;
		[SerializeField] private Button _removeButton;

		private Inventory _inventory;
		private StackDisplayer _selectedSlot;
		private Item SelectedItem => _selectedSlot.Item;

		private void Awake()
		{
			_panel.gameObject.SetActive(false);
			_useButton.onClick.AddListener(() => OnUseButtonClicked(SelectedItem));
			_removeButton.onClick.AddListener(() => OnRemoveButtonClicked(SelectedItem));
		}

		public void Initialize(Inventory inventory)
		{
			_inventory = inventory;
		}

		public void Show(StackDisplayer slot)
		{
			_selectedSlot = slot;
			_panel.gameObject.SetActive(true);
			_icon.sprite = SelectedItem.Icon;
			_nameLabel.text = SelectedItem.Name;
			_tierLabel.text = SelectedItem.Tier.Name;
			_tierLabel.color = SelectedItem.Tier.Color;
			_descriptionLabel.text = SelectedItem.Description;
			_itemTypeLabel.text = SelectedItem.ItemType.ToString();
			_useButton.gameObject.SetActive(SelectedItem.ItemType == ItemType.Consumable);
		}

		public void Close()
		{
			_panel.gameObject.SetActive(false);
		}

		private void OnRemoveButtonClicked(Item item)
		{
			if (_inventory == null) return;
			Debug.Log(!_inventory.TryRemoveUniqueItem(item) ? "Failed to remove item from inventory" : $"Removed item {item.Name} from inventory");
			Close();
		}

		private void OnUseButtonClicked(Item item)
		{
			if (_inventory == null) return;
			bool isLastItem = _selectedSlot.StackQuantity <= 1;
			if (_inventory.TryRemoveItem(item))
			{
				if (isLastItem)
				{
					Close();
				}
				item.Use();
			}
			else
			{
				Debug.Log("Failed to remove item from inventory");
			}
		}
	}

}