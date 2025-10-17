

namespace Diablo5
{
	using System.Collections.Generic;
	using System.Linq;
	using InventorySystem.Items;
	using UnityEngine;

	public static class LootTable
	{
		public static Item GetWeightedItem(IEnumerable<Item> _droppableItems)
		{
			IEnumerable<Item> droppableItems = _droppableItems.ToList();
			if (!droppableItems.Any()) return null;
			float totalWeight = droppableItems.Sum(item => item.Tier.DropWeight);
			float randomValue = Random.Range(0, totalWeight);
			float counter = 0f;
			foreach (Item item in droppableItems)
			{
				counter += item.Tier.DropWeight;
				if (randomValue - counter <= 0)
				{
					return item;
				}
			}
			return null;
		}
	}

}