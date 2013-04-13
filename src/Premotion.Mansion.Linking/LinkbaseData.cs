using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Premotion.Mansion.Core;
using Premotion.Mansion.Core.Data;

namespace Premotion.Mansion.Linking
{
	/// <summary>
	/// Contains the data for a <see cref="Linkbase"/>.
	/// </summary>
	[JsonObject]
	public class LinkbaseData
	{
		#region Factory Method
		/// <summary>
		/// Creates an empty <see cref="LinkbaseData"/> for the given <paramref name="record"/>.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="record"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static LinkbaseData Create(IMansionContext context, Record record)
		{
			// validate arguments
			if (context == null)
				throw new ArgumentNullException("context");
			if (record == null)
				throw new ArgumentNullException("record");

			// create the linkbase
			return new LinkbaseData {
				Id = record.Get<string>(context, "guid"),
				Links = new List<Link>()
			};
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the ID of this <see cref="Linkbase"/>.
		/// </summary>
		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }
		/// <summary>
		/// Gets the <see cref="Link"/>s of this <see cref="Linkbase"/>.
		/// </summary>
		[JsonProperty("links", Required = Required.Always)]
		public List<Link> Links { get; set; }
		#endregion
	}
}