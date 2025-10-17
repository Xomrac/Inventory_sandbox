using System;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace Diablo5
{

	public enum GameStates
	{
		Gameplay,
		Inventory,
		Paused
	}

	public class GameStateManager : MonoBehaviour
	{

		public event Action<GameStates> GameStateChanged;

		private GameStates _currentState = GameStates.Gameplay;
		public GameStates CurrentState
		{
			get => _currentState;
			set{
				if (_currentState == value) return;
				_currentState = value;
				GameStateChanged?.Invoke(_currentState);
			}
		}
		
		void Awake()
		{
			ServiceLocator.Global.AddService(this);
		}
		
		public void SetState(GameStates newState)
		{
			CurrentState = newState;
		}
		
	}
}