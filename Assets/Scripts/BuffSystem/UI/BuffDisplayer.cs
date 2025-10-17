namespace InventorySandbox.Buffs.UI
{
	using Buffs;
	using UnityEngine;
	using UnityEngine.UI;
	using XomracCore.TimeManagement;

	
	/// <summary>
	/// component responsible for displaying a buff's icon and duration on screen.
	/// </summary>
	public class BuffDisplayer : MonoBehaviour
	{
		[SerializeField] private Image _mask;
		[SerializeField] private Image _icon;

		private Timer _duration;

		public bool TrySetup(ABuffData data, Timer duration)
		{
			if (_duration != null) return false;
			_icon.sprite = data.Icon;
			_duration = duration;
			_duration.Updated += OnDurationUpdated;
			_duration.Completed += OnDurationCompleted;
			_duration.Canceled += OnDurationCanceled;
			_mask.fillAmount = 1f;
			return true;
		}

		private void OnDurationCanceled()
		{
			OnDurationCompleted();
		}

		private void OnDurationCompleted()
		{
			_duration.Updated -= OnDurationUpdated;
			_duration.Completed -= OnDurationCompleted;
			_duration = null;
			gameObject.SetActive(false);
		}

		private void OnDurationUpdated(float progress)
		{
			progress = 1f - progress;
			_mask.fillAmount = progress;
		}

	}

}