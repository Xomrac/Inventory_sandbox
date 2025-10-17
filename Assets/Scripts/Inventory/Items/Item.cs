using System;
using Diablo5.Bonuses;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace Diablo5.InventorySystem.Items
{

	[CreateAssetMenu(fileName = "NewItem", menuName = "Diablo5/Items/New Item")]
	public class Item : ScriptableObject
	{
		[SerializeField] private string _name;
		public string Name => _name;

		[SerializeField] private Sprite _icon;
		public Sprite Icon => _icon;

		[SerializeField] private Tier _tier;
		public Tier Tier => _tier;

		[SerializeField] private ItemType _itemType;
		public ItemType ItemType => _itemType;

		[SerializeField, TextArea(3, 3)] private string _description;
		public string Description => _description;

		[SerializeField] private ABonusData[] _bonuses = Array.Empty<ABonusData>();
		public ABonusData[] Bonuses => _bonuses;

		public void Use()
		{
			Debug.Log($"Using item: {Name}");
			if (_bonuses.Length == 0) return;

			foreach (var bonus in _bonuses)
			{
				if (ServiceLocator.Global.TryGetService(out BonusManager manager))
				{
					manager.ResolveBonus(bonus);
				}
				else
				{
					Debug.LogWarning("No BonusManager found in the scene.");
				}
			}
		}
	}
}