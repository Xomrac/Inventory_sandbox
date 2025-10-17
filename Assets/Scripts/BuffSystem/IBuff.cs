namespace InventorySandbox.Buffs
{
	/// <summary>
	/// Represents a buff that can be applied to and removed from a target.
	/// </summary>
	public interface IBuff
	{
		void Apply();
		
		void Remove();
		
		float Duration { get; }
		
		object Target { get; }
	}
}