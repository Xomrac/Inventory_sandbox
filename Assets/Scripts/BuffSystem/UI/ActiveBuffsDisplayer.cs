namespace InventorySandbox.Buffs.UI
{
	using Buffs;
	using UnityEngine;
	using XomracCore.TimeManagement;
	
	/// <summary>
	/// Manages the display of active buffs on the UI.
	/// </summary>
	public class ActiveBuffsDisplayer : MonoBehaviour
	{
		[SerializeField] private BuffDisplayer[] _bonusDisplayers;
        
		private BuffsManager _buffsManager;
		
		public void Initialize(BuffsManager buffsManager)
		{
			_buffsManager = buffsManager;
			_buffsManager.UniqueBuffAdded += OnNewBuffAdded;
			foreach (BuffDisplayer displayer in _bonusDisplayers)
			{
				displayer.gameObject.SetActive(false);
			}
		}

		private void OnNewBuffAdded(ABuffData data, Timer duration)
		{
			foreach (var displayer in _bonusDisplayers)
			{
				if (displayer.TrySetup(data, duration))
				{
					displayer.gameObject.SetActive(true);
					return;
				}
			}
			Debug.LogWarning("No available BonusDisplayer to show the new bonus.");
		}

		
	}

}