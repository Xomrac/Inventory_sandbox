using System;
using UnityEngine;

namespace Diablo5
{

	public class Timer
	{
		private float _duration;
		private float _elapsedTime;
		private bool _isPaused;
		public bool IsPaused => _isPaused;
		public float Progress => Mathf.Clamp01(_elapsedTime / _duration);

		public event Action Completed;
		public event Action<float> Updated;
		public event Action Canceled;
        
		public Timer(float duration)
		{
			_duration = duration;
			_elapsedTime = 0f;
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
        
		public void SetProgress(float progress)
		{
			_elapsedTime = Mathf.Clamp01(progress) * _duration;
		}
		
		public void ModifyDuration(float newDuration)
		{
			_duration = newDuration;
			_elapsedTime = Mathf.Clamp(_elapsedTime, 0f, _duration);
			Updated?.Invoke(Progress);
		}

		public void Tick()
		{
			if (!_isPaused)
			{
				Update(Time.deltaTime);
			}
		}
		
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