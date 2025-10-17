using UnityEngine;

namespace InventorySandbox.InventorySystem.Items
{

	[CreateAssetMenu(fileName = "NewTier", menuName = "InventorySandbox/Items/New Tier")]
	public class Tier : ScriptableObject
	{
		[SerializeField] private string _name;
		public string Name => _name;

		[SerializeField] private Color _color = Color.white;
		public Color Color => _color;

		[SerializeField] private float _dropWeight = 1f;
		public float DropWeight => _dropWeight;
	}

}