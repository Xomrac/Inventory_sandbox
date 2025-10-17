using System;

namespace Diablo5.Bonuses
{

	public interface IBonus
	{
		void Apply();
		void Remove();
		
		float Duration { get; }
		
		Object Target { get; }
	}

}