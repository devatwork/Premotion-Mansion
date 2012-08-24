namespace Premotion.Mansion.Core.Patterns.Prioritized
{
	/// <summary>
	/// Represents a prioritized object. Prioritized objects are executed in order of their relative <see cref="Priority"/> compared to other prioritized objects within the same set.
	/// </summary>
	public interface IPrioritized
	{
		#region Properties
		/// <summary>
		/// Gets the relative priority of this object. The higher the priority, earlier this object is executed.
		/// </summary>
		int Priority { get; }
		#endregion
	}
}