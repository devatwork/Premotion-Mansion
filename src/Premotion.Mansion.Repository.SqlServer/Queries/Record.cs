using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Premotion.Mansion.Repository.SqlServer.Queries
{
	/// <summary>
	/// Adapter for <see cref="IDataRecord"/>.
	/// </summary>
	public class Record
	{
		#region Constructors
		/// <summary>
		/// Constructs this record.
		/// </summary>
		/// <param name="record"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public Record(IDataRecord record)
		{
			// validate arguments
			if (record == null)
				throw new ArgumentNullException("record");

			// set value
			this.record = record;
		}
		#endregion
		#region Implementation of IDataRecord
		/// <summary>
		/// Gets the number of columns in the current row.
		/// </summary>
		/// <returns>
		/// When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public int FieldCount
		{
			get { return record.FieldCount; }
		}
		/// <summary>
		/// Gets the name for the field to find.
		/// </summary>
		/// <returns>
		/// The name of the field or the empty string (""), if there is no value to return.
		/// </returns>
		/// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public string GetName(int i)
		{
			return record.GetName(i);
		}
		/// <summary>
		/// Return the value of the specified field.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Object"/> which will contain the field value upon return.
		/// </returns>
		/// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public object GetValue(int i)
		{
			valueRead.Add(i);
			return record.GetValue(i);
		}
		/// <summary>
		/// Return the index of the named field.
		/// </summary>
		/// <returns>
		/// The index of the named field.
		/// </returns>
		/// <param name="name">The name of the field to find. </param><filterpriority>2</filterpriority>
		public int GetOrdinal(string name)
		{
			return record.GetOrdinal(name);
		}
		/// <summary>
		/// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
		/// </summary>
		/// <returns>
		/// The actual number of bytes read.
		/// </returns>
		/// <param name="i">The zero-based column ordinal. </param><param name="fieldOffset">The index within the field from which to start the read operation. </param><param name="buffer">The buffer into which to read the stream of bytes. </param><param name="bufferoffset">The index for <paramref name="buffer"/> to start the read operation. </param><param name="length">The number of bytes to read. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			valueRead.Add(i);
			return record.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}
		/// <summary>
		/// Gets the 32-bit signed integer value of the specified field.
		/// </summary>
		/// <returns>
		/// The 32-bit signed integer value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public int GetInt32(int i)
		{
			valueRead.Add(i);
			return record.GetInt32(i);
		}
		/// <summary>
		/// Gets the string value of the specified field.
		/// </summary>
		/// <returns>
		/// The string value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public string GetString(int i)
		{
			valueRead.Add(i);
			return record.GetString(i);
		}
		/// <summary>
		/// Return whether the specified field is set to null.
		/// </summary>
		/// <returns>
		/// true if the specified field is set to null; otherwise, false.
		/// </returns>
		/// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
		public bool IsDBNull(int i)
		{
			valueRead.Add(i);
			return record.IsDBNull(i);
		}
		#endregion
		#region Custom Methods
		/// <summary>
		/// Gets the ordinals of the fields who's value has not been read yet.
		/// </summary>
		/// <returns>Returns the ordinals of the unread fields.</returns>
		public IEnumerable<int> GetUnreadOrdinals()
		{
			return Enumerable.Range(0, record.FieldCount - 1).Except(valueRead).ToArray();
		}
		#endregion
		#region Private Fields
		private readonly IDataRecord record;
		private readonly List<int> valueRead = new List<int>();
		#endregion
	}
}