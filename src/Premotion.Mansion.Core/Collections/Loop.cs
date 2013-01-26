using System;
using System.Collections.Generic;

namespace Premotion.Mansion.Core.Collections
{
	/// <summary>
	/// Represents a loop.
	/// </summary>
	public class Loop : PropertyBag
	{
		#region Constructors
		/// <summary>
		/// Constructs a loop for the specified dataset.
		/// </summary>
		/// <param name="reader">The dataset throught which to loop.</param>
		/// <param name="start">The start of the loop.</param>
		/// <param name="end">The end of the loop.</param>
		public Loop(IPropertyBagReader reader, int start, int end)
		{
			// validate arguments
			if (reader == null)
				throw new ArgumentNullException("reader");
			if (start < 0)
				throw new ArgumentOutOfRangeException("start", start, "Should be zero or greater");
			if (end < 0)
				throw new ArgumentOutOfRangeException("end", end, "Should be zero or greater");
			if (end < start)
				throw new ArgumentOutOfRangeException("end", end, "Should equal or greater than start");

			// set values
			Reader = reader;
			Start = start;
			End = end;
			offset = 0;
		}
		/// <summary>
		/// Constructs a loop for the specified dataset.
		/// </summary>
		/// <param name="dataset">The dataset throught which to loop.</param>
		public Loop(Dataset dataset)
		{
			// validate arguments
			if (dataset == null)
				throw new ArgumentNullException("dataset");

			// set values
			Reader = new DatasetReader(dataset);
			offset = dataset.IsPaged ? (dataset.CurrentPage - 1)*dataset.PageSize : 0;
			start = 0;
			end = dataset.RowCount - 1;
		}
		#endregion
		#region Properties
		/// <summary>
		/// Gets the start of this loop.
		/// </summary>
		public int Start
		{
			get { return start; }
			private set
			{
				start = value;
				Set("start", current);
			}
		}
		/// <summary>
		/// Gets the end of this loop.
		/// </summary>
		public int End
		{
			get { return end; }
			private set
			{
				end = value;
				Set("end", current);
			}
		}
		/// <summary>
		/// Gets the current position in this loop.
		/// </summary>
		public int Current
		{
			get { return current; }
			private set
			{
				current = value;
				Set("current", current);
				Set("displayCurrent", current + 1);
			}
		}
		/// <summary>
		/// Gets the offset of the current position..
		/// </summary>
		public int Offset
		{
			get { return offset; }
		}
		/// <summary>
		/// Gets the <see cref="Reader"/> on which this loop is based.
		/// </summary>
		private IPropertyBagReader Reader { get; set; }
		/// <summary>
		/// Enumerates the rows in this loop.
		/// </summary>
		public IEnumerable<IPropertyBag> Rows
		{
			get
			{
				Current = offset;
				var loopCurrent = 0;
				foreach (var row in Reader)
				{
					if (loopCurrent >= start && loopCurrent <= End)
						yield return row;

					Current = ++loopCurrent;
				}
			}
		}
		/// <summary>
		/// Flag indicating whether the current item is the first item.
		/// </summary>
		public bool IsFirst
		{
			get { return Current == Start; }
		}
		/// <summary>
		/// Flag indicating whether the current item is the last item.
		/// </summary>
		public bool IsLast
		{
			get { return Current == End; }
		}
		#endregion
		#region Private Fields
		private readonly int offset;
		private int current;
		private int end;
		private int start;
		#endregion
	}
}