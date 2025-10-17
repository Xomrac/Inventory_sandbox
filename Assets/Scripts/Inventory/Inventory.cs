namespace InventorySandbox.InventorySystem
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Items;
	using UnityEngine;

	/// <summary>
	/// Represents a stack of items in the inventory, including the quantity and the slot index.
	/// </summary>
	public struct ItemStack
	{
		public int Quantity;
		public int slotIndex;

		public ItemStack(int quantity, int slotIndex)
		{
			Quantity = quantity;
			this.slotIndex = slotIndex;
		}
	}

	/// <summary>
	/// Represents an inventory system that manages items and their stacks.
	/// </summary>
	public class Inventory
	{
		/// <summary>
		/// The maximum capacity of unique items in the inventory.
		/// </summary>
		private int _capacity = 20;

		public int Capacity => _capacity;

		/// <summary>
		/// A dictionary storing items and their corresponding stacks.
		/// </summary>
		private Dictionary<Item, ItemStack> _items;

		public Dictionary<Item, ItemStack> Items => _items;

		/// <summary>
		/// Event triggered when the inventory changes.
		/// </summary>
		public event Action InventoryChanged;

		/// <summary>
		/// Event triggered when an item is added to the inventory.
		/// </summary>
		public event Action<Item> ItemAdded;

		/// <summary>
		/// Gets the total count of items in the inventory.
		/// </summary>
		public int ItemCount => _items.Values.Sum(stack => stack.Quantity);

		/// <summary>
		/// Gets the count of unique items in the inventory.
		/// </summary>
		public int UniqueItemCount => _items.Count;

		public Inventory(int capacity = 20, Dictionary<Item, ItemStack> items = null)
		{
			_capacity = capacity;
			_items = items != null ? new Dictionary<Item, ItemStack>(items) : new Dictionary<Item, ItemStack>();
		}

		/// <summary>
		/// Attempts to add an item to the inventory.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="quantity">The quantity of the item to add.</param>
		/// <param name="index">The slot index to place the item in.</param>
		/// <returns>True if the item was added successfully; otherwise, false.</returns>
		public bool TryAddItem(Item item, int quantity = 1, int index = -1)
		{
			if (ItemCount + quantity > Capacity)
			{
				Debug.Log("Not enough space in inventory");
				return false;
			}

			if (_items.ContainsKey(item))
			{
				ItemStack newStackData = _items[item];
				newStackData.Quantity += quantity;
				_items[item] = newStackData;
			}
			else
			{
				int slotIndex = index == -1 ? 0 : index;
				if (IsIndexOccupied(slotIndex))
				{
					slotIndex = GetFirstAvailableIndex();
					if (slotIndex == -1)
					{
						return false;
					}
				}
				_items[item] = new ItemStack(quantity, slotIndex);
			}
			ItemAdded?.Invoke(item);
			InventoryChanged?.Invoke();
			return true;
		}

		/// <summary>
		/// Swaps the slot indexes of two items in the inventory.
		/// </summary>
		/// <param name="a">The first item.</param>
		/// <param name="b">The second item.</param>
		public void SwapItemIndexes(Item a, Item b)
		{
			ItemStack aStack = _items[a];
			ItemStack bStack = _items[b];
			(aStack.slotIndex, bStack.slotIndex) = (bStack.slotIndex, aStack.slotIndex);
			_items[a] = aStack;
			_items[b] = bStack;
			InventoryChanged?.Invoke();
		}

		/// <summary>
		/// Changes the slot index of an item in the inventory.
		/// </summary>
		/// <param name="item">The item to move.</param>
		/// <param name="newIndex">The new slot index for the item.</param>
		public void ChangeItemIndex(Item item, int newIndex)
		{
			if (!_items.ContainsKey(item))
			{
				Debug.Log("Item not found in inventory");
				return;
			}
			if (IsIndexOccupied(newIndex))
			{
				Debug.Log("Index already occupied");
				return;
			}
			ItemStack stack = _items[item];
			stack.slotIndex = newIndex;
			_items[item] = stack;
			InventoryChanged?.Invoke();
		}

		/// <summary>
		/// Gets the first available slot index in the inventory.
		/// </summary>
		/// <returns>The first available slot index, or -1 if none are available.</returns>
		private int GetFirstAvailableIndex()
		{
			var occupiedIndexes = new List<int>(_items.Values.Select(stack => stack.slotIndex));
			for (int i = 0; i < Capacity; i++)
			{
				if (!occupiedIndexes.Contains(i))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Checks if a slot index is occupied.
		/// </summary>
		/// <param name="index">The slot index to check.</param>
		/// <returns>True if the index is occupied; otherwise, false.</returns>
		private bool IsIndexOccupied(int index)
		{
			return _items.Values.Any(stack => stack.slotIndex == index);
		}

		/// <summary>
		/// Attempts to remove a unique item from the inventory.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed successfully; otherwise, false.</returns>
		public bool TryRemoveUniqueItem(Item item)
		{
			if (!_items.ContainsKey(item))
			{
				Debug.Log("Item not found in inventory");
				return false;
			}
			_items.Remove(item);
			InventoryChanged?.Invoke();
			return true;
		}

		/// <summary>
		/// Attempts to remove a specific quantity of an item from the inventory.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <param name="quantity">The quantity to remove.</param>
		/// <returns>True if the items were removed successfully; otherwise, false.</returns>
		public bool TryRemoveItem(Item item, int quantity = 1)
		{
			if (!_items.ContainsKey(item))
			{
				Debug.Log("Item not found in inventory");
				return false;
			}

			ItemStack stackData = _items[item];
			if (stackData.Quantity < quantity)
			{
				Debug.Log("Not enough items to remove");
				return false;
			}

			stackData.Quantity -= quantity;
			if (stackData.Quantity == 0)
			{
				_items.Remove(item);
			}
			else
			{
				_items[item] = stackData;
			}
			InventoryChanged?.Invoke();
			return true;
		}
	}

}