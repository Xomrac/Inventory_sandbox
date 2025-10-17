namespace InventorySandbox.Interactables
{
	using UnityEngine;
	
	/// <summary>
	/// Base class for all interactable objects in the game.
	/// Handles focus and interaction logic.
	/// </summary>
	public abstract class AInteractable : MonoBehaviour
	{
		[SerializeField] protected GameObject _affordanceCanvas;
		protected bool _isInteractable = true;
		public bool IsInteractable => _isInteractable;

		protected virtual void Awake()
		{
			if (_affordanceCanvas != null)
			{
				_affordanceCanvas.SetActive(false);
			}
		}

		public virtual void GainFocus()
		{
			if (!_isInteractable) return;
			if (_affordanceCanvas != null)
			{
				_affordanceCanvas.SetActive(true);
			}
		}

		public virtual void LoseFocus()
		{
			if (_affordanceCanvas != null)
			{
				_affordanceCanvas.SetActive(false);
			}
		}

		public abstract void Interact(ACharacterInteraction interactor);
	}

}