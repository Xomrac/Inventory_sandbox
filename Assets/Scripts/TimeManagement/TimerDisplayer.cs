namespace XomracCore.TimeManagement
{
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Component to display a timer's progress using a UI Image fill.
	/// </summary>
	public class TimerDisplayer : MonoBehaviour
	{
		[SerializeField] private Image _fillImage;

		private Timer _cooldown;

		public void DisplayCooldown(Timer cooldown)
		{
			if (cooldown == null) return;
			_cooldown = cooldown;
			_fillImage.fillAmount = _cooldown.Progress;
			gameObject.SetActive(true);
			_cooldown.Updated += OnUpdated;
		}

		private void OnUpdated(float progress)
		{
			_fillImage.fillAmount = progress;
			if (progress >= 1f)
			{
				gameObject.SetActive(false);
				_cooldown.Updated -= OnUpdated;
				_cooldown = null;
			}
		}
	}
}