namespace InventorySandbox.Buffs
{
	using System.Collections.Generic;
	using NaughtyAttributes;
	using UnityEngine;

	/// <summary>
	/// Base class for defining buff data in the editor.
	/// Basically a wrapper to serialize IBuff and make the system designer friendly
	/// </summary>
	public abstract class ABuffData : ScriptableObject
	{
		[InfoBox("Duration of the bonus effect in seconds, set to negative for infinite duration.")] [SerializeField] protected float _duration = 10f;
		public float Duration => _duration;

		[SerializeField] private Sprite _icon;

		public Sprite Icon => _icon;

		/// <summary>
		/// Attempts to resolve bonuses based on the current buff data.
		/// </summary>
		/// <param name="bonuses">
		/// An output parameter that will contain the list of resolved bonuses if the operation is successful.
		/// </param>
		/// <returns>
		/// A boolean value indicating whether the bonuses were successfully resolved.
		/// </returns>
		public abstract bool TryToResolve(out List<IBuff> bonuses);
	}

}