using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using XomracCore.Bootstrap;
using XomracCore.Patterns.SL;

namespace InventorySandbox
{
	/// <summary>
	/// System to manage the current game state and notify listeners of state changes.
	/// </summary>
	public class GameStateManager : MonoBehaviour, IBootstrappable
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

		public async UniTask Bootstrap()
		{
			ServiceLocator.Global.AddService(this);
			await UniTask.Yield();
		}

		public void SetState(GameStates newState)
		{
			CurrentState = newState;
		}
	}

}