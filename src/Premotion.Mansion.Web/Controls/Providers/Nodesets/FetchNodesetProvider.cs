﻿using System;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Attributes;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Web.Controls.Providers.Nodesets
{
	/// <summary>
	/// Provides data from a static <see cref="NodesetProvider"/>.
	/// </summary>
	public class FetchNodesetProvider : NodesetProvider
	{
		#region Nested type: FetchNodesetProviderFactoryTag
		/// <summary>
		/// Creates <see cref="FetchNodesetProvider"/>s.
		/// </summary>
		[Named(Constants.DataProviderTagNamespaceUri, "fetchNodesetProvider")]
		public class FetchNodesetProviderFactoryTag : DatasetProviderFactoryTag<NodesetProvider>
		{
			#region Overrides of DataProviderFactoryTag
			/// <summary>
			/// Creates the data provider.
			/// </summary>
			/// <param name="context">The <see cref="MansionWebContext"/>.</param>
			/// <returns>Returns the created data provider.</returns>
			protected override NodesetProvider Create(MansionWebContext context)
			{
				return new FetchNodesetProvider(GetRequiredAttribute<Nodeset>(context, "source"));
			}
			#endregion
		}
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a static nodeset provider.
		/// </summary>
		/// <param name="nodeset">The <see cref="Nodeset"/> from which the data is provided.</param>
		public FetchNodesetProvider(Nodeset nodeset)
		{
			// validate arguments
			if (nodeset == null)
				throw new ArgumentNullException("nodeset");

			// set values
			this.nodeset = nodeset;
		}
		#endregion
		#region Overrides of DataProvider<Nodeset>
		/// <summary>
		/// Retrieves the data from this provider.
		/// </summary>
		/// <param name="context">The <see cref="MansionContext"/>.</param>
		/// <returns>Returns the retrieve data.</returns>
		protected override Nodeset DoRetrieve(MansionContext context)
		{
			return nodeset;
		}
		#endregion
		#region Private Fields
		private readonly Nodeset nodeset;
		#endregion
	}
}