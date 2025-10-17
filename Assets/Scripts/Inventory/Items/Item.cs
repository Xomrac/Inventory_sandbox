using System;
using InventorySandbox.Buffs;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace InventorySandbox.InventorySystem.Items
{

	[CreateAssetMenu(fileName = "NewItem", menuName = "InventorySandbox/Items/New Item")]
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

		[SerializeField] private ABuffData[] _buffs = Array.Empty<ABuffData>();
		public ABuffData[] Buffs => _buffs;

		public void Use()
		{
			if (_buffs.Length == 0) return;
			var manager = ServiceLocator.Global.GetService<BuffsManager>();
			if (manager == null)
			{
				Debug.LogWarning("No BonusManager found in the scene.");
				return;
			}
			foreach (ABuffData buff in _buffs)
			{
				manager.ResolveBonus(buff);
			}
		}
	}

}