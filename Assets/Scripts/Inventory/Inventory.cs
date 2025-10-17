using System;
using System.Collections.Generic;
using System.Linq;
using Diablo5.InventorySystem.Items;
using UnityEngine;

namespace Diablo5.InventorySystem
{

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

	public class Inventory
	{
		private int _capacity = 20;
		public int Capacity => _capacity;

		private Dictionary<Item, ItemStack> _items;
		public Dictionary<Item, ItemStack> Items => _items;

		public event Action InventoryChanged;
		public event Action<Item> ItemAdded; 

		public int ItemCount => _items.Values.Sum(stack => stack.Quantity);
		public int UniqueItemCount => _items.Count;

		public Inventory(int capacity = 20, Dictionary<Item, ItemStack> items = null)
		{
			_capacity = capacity;
			_items = items != null ? new Dictionary<Item, ItemStack>(items) : new Dictionary<Item, ItemStack>();
		}

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


		public void SwapItemIndexes(Item a, Item b)
		{
			int aIndex = _items[a].slotIndex;
			int bIndex = _items[b].slotIndex;
			ItemStack aStack = _items[a];
			ItemStack bStack = _items[b];
			aStack.slotIndex = bIndex;
			bStack.slotIndex = aIndex;
			_items[a] = aStack;
			_items[b] = bStack;
			InventoryChanged?.Invoke();
		}
		
		public void changeItemIndex(Item item, int newIndex)
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
		
		private bool IsIndexOccupied(int index)
		{
			return _items.Values.Any(stack => stack.slotIndex == index);
		}

		public bool TryRemoveUniqueItem(Item item)
		{
			if (!_items.ContainsKey(item))
			{
				Debug.Log("Item not found in inventory");
				return false;
			}

			int indexToRemove = _items[item].slotIndex;
			_items.Remove(item);
			ShiftSlotIndexes(indexToRemove + 1, -1);
			InventoryChanged?.Invoke();
			return true;
		}

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
				int indexToRemove = stackData.slotIndex;
				ShiftSlotIndexes(indexToRemove + 1, -1);
				_items.Remove(item);
			}
			else
			{
				_items[item] = stackData;
			}
			InventoryChanged?.Invoke();
			return true;
		}

		public void ShiftSlotIndexes(int startingIndex, int shiftValue)
		{
			foreach (var key in _items.Keys.ToList())
			{
				ItemStack stackData = _items[key];
				if (stackData.slotIndex >= startingIndex)
				{
					stackData.slotIndex += shiftValue;
					_items[key] = stackData;
				}
			}
			InventoryChanged?.Invoke();
		}
	}

}