using System.Collections.Generic;
using UnityEngine;

namespace Diablo5.Bonuses
{

	public abstract class ABonusData : ScriptableObject
	{
		[SerializeField] protected float _duration = 10f;
		public float Duration => _duration;

		[SerializeField] private Sprite _icon;

		public Sprite Icon => _icon;

		public abstract bool TryToResolve(BonusManager bonusManager, out List<IBonus> bonuses); 
	}

}