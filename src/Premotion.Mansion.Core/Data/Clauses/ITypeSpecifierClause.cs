using System.Collections.Generic;
using Premotion.Mansion.Core.Types;

namespace Premotion.Mansion.Core.Data.Clauses
{
	/// <summary>
	/// 	Spefies the type of node.
	/// </summary>
	public interface ITypeSpecifierClause
	{
		#region Properties
		/// <summary>
		/// 	Gets the types this clause defines.
		/// </summary>
		IEnumerable<ITypeDefinition> Types { get; }
		#endregion
	}
}