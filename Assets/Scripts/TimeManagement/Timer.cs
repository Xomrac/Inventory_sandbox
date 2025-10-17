namespace XomracCore.TimeManagement
{

	using System;
	using UnityEngine;

	/// <summary>
	/// Represents a timer that tracks elapsed time, supports pausing, resetting, and updating progress.
	/// </summary>
	public class Timer
	{
		/// <summary>
		/// The total duration of the timer in seconds.
		/// </summary>
		private float _duration;

		/// <summary>
		/// The elapsed time since the timer started in seconds.
		/// </summary>
		private float _elapsedTime;

		private bool _isPaused;
		public bool IsPaused => _isPaused;

		public float Progress => Mathf.Clamp01(_elapsedTime / _duration);

		/// <summary>
		/// Indicates whether the timer is infinite (duration less than 0).
		/// </summary>
		private bool _isInfinite;

		/// <summary>
		/// Event triggered when the timer updates, providing the current progress in a 0-1 range.
		/// </summary>
		public event Action<float> Updated;
		public event Action Completed;
		public event Action Canceled;

		/// <summary>
		/// Initializes a new instance of the <see cref="Timer"/> class with the specified duration.
		/// </summary>
		/// <param name="duration">The total duration of the timer in seconds. Use a negative value for an infinite timer.</param>
		public Timer(float duration)
		{
			_duration = duration;
			_elapsedTime = 0f;
			_isInfinite = duration < 0f;
		}

		public void Resume()
		{
			_isPaused = false;
		}

		public void Pause()
		{
			_isPaused = true;
		}

		public void Cancel()
		{
			Canceled?.Invoke();
		}

		public void Reset()
		{
			_elapsedTime = 0f;
		}

		/// <summary>
		/// Sets the progress of the timer as a value between 0 and 1.
		/// </summary>
		/// <param name="progress">The progress value to set, clamped between 0 and 1.</param>
		public void SetProgress(float progress)
		{
			_elapsedTime = Mathf.Clamp01(progress) * _duration;
		}

		/// <summary>
		/// Modifies the duration of the timer and adjusts the elapsed time accordingly.
		/// </summary>
		/// <param name="newDuration">The new duration of the timer in seconds.</param>
		public void ModifyDuration(float newDuration)
		{
			_isInfinite = newDuration < 0f;
			_duration = newDuration;
			_elapsedTime = !_isInfinite ? Mathf.Clamp(_elapsedTime, 0f, _duration) : 0f;
			Updated?.Invoke(Progress);
		}

		/// <summary>
		/// Updates the timer by advancing the elapsed time if it is not paused or infinite.
		/// </summary>
		public void Tick()
		{
			if (!_isPaused && !_isInfinite)
			{
				Update(Time.deltaTime);
			}
		}

		/// <summary>
		/// Updates the timer with a specified delta time.
		/// </summary>
		/// <param name="deltaTime">The time increment to add to the elapsed time.</param>
		private void Update(float deltaTime)
		{
			if (_elapsedTime < _duration)
			{
				_elapsedTime += deltaTime;
				Updated?.Invoke(Progress);
				if (_elapsedTime >= _duration)
				{
					_elapsedTime = _duration;
					Completed?.Invoke();
				}
			}
		}
	}

}