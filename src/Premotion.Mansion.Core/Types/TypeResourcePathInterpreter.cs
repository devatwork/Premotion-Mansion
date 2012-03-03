using System;
using System.Collections.Generic;
using System.Linq;
using Premotion.Mansion.Core.IO;
using Premotion.Mansion.Core.Patterns.Voting;

namespace Premotion.Mansion.Core.Types
{
	/// <summary>
	/// Implements <see cref="ResourcePathInterpreter"/> for <see cref="TypeResourcePath"/>.
	/// </summary>
	public class TypeResourcePathInterpreter : ResourcePathInterpreter
	{
		#region Nested type: TypeResourcePath
		/// <summary>
		/// Represents a resource for a type.
		/// </summary>
		public class TypeResourcePath : IResourcePath
		{
			#region Constructors
			/// <summary>
			/// Constructs a relative resource path.
			/// </summary>
			/// <param name="type">The type for which to load.</param>
			/// <param name="extension">The extension of the resource.</param>
			/// <param name="overridable">Flag indicating whether this type is overridable.</param>
			private TypeResourcePath(string type, string extension, bool overridable) : this(new[] {type}, extension, overridable)
			{
			}
			/// <summary>
			/// Constructs a relative resource path.
			/// </summary>
			/// <param name="types">The types for which to load.</param>
			/// <param name="extension">The extension of the resource.</param>
			/// <param name="overridable">Flag indicating whether this type is overridable.</param>
			public TypeResourcePath(IEnumerable<string> types, string extension, bool overridable)
			{
				// validate arguments
				if (types == null)
					throw new ArgumentNullException("types");
				if (string.IsNullOrEmpty(extension))
					throw new ArgumentNullException("extension");

				// set values
				Overridable = overridable;
				Paths = types.Select(x => ResourceUtils.Combine("Types", x, x + "." + extension));
			}
			#endregion
			#region Factory Methods
			/// <summary>
			/// Creates a type definition resource path.
			/// </summary>
			/// <param name="type">The name of the type.</param>
			/// <returns>Returns the created path.</returns>
			public static IResourcePath CreateTypeDefinitionResourcePath(string type)
			{
				return new TypeResourcePath(type, "xdef", false);
			}
			#endregion
			#region Properties
			/// <summary>
			/// Gets a flag indicating whether this resource is overridable or not.
			/// </summary>
			public bool Overridable { get; private set; }
			/// <summary>
			/// Gets the relative path to this resource.
			/// </summary>
			public IEnumerable<string> Paths { get; private set; }
			#endregion
		}
		#endregion
		#region Implementation of ICandidate<IPropertyBag>
		/// <summary>
		/// Requests this voter to cast a vote.
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="subject">The subject.</param>
		/// <returns>Returns the result of the vote.</returns>
		protected override VoteResult DoVote(IContext context, IPropertyBag subject)
		{
			// check if a path is specified
			string type;
			return subject.TryGet(context, "type", out type) ? VoteResult.MediumInterest : VoteResult.Refrain;
		}
		#endregion
		#region Implementation of IInterpreter<in IPropertyBag,out IResourcePath>
		/// <summary>
		/// Interprets the input..
		/// </summary>
		/// <param name="context">The <see cref="IContext"/>.</param>
		/// <param name="input">The input which to interpret.</param>
		/// <returns>Returns the interpreted result.</returns>
		protected override IResourcePath DoInterpret(IContext context, IPropertyBag input)
		{
			// get the path
			var type = input.Get<ITypeDefinition>(context, "type");
			if (type == null)
				throw new InvalidOperationException("The type must be specified");
			var extension = input.Get(context, "extension", string.Empty).TrimStart('.');
			if (string.IsNullOrEmpty(extension))
				throw new InvalidOperationException("The extension must be specified");
			var overridable = input.Get(context, "overridable", true);

			// return the resource path
			return new TypeResourcePath(type.Hierarchy.Select(x => x.Name), extension, overridable);
		}
		#endregion
	}
}