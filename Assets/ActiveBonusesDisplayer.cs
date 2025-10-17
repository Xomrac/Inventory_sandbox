using Diablo5.Bonuses;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace Diablo5
{

	public class ActiveBonusesDisplayer : MonoBehaviour
	{
		[SerializeField] private BonusDisplayer[] _bonusDisplayers;
        
		private BonusManager _bonusManager;

		private void Start()
		{
			_bonusManager = ServiceLocator.Global.GetService<BonusManager>();
			if (_bonusManager == null)
			{
				Debug.LogError("No BonusManager found in the scene.");
				gameObject.SetActive(false);
				return;
			}
			_bonusManager.BonusAdded += OnBonusAdded;
			foreach (var displayer in _bonusDisplayers)
			{
				displayer.gameObject.SetActive(false);
			}
		}

		private void OnBonusAdded(ABonusData data, Timer duration)
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