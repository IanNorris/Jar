using System;
using SQLite;

namespace Jar.Model
{
	/// <summary>
	/// A checkpoint is the account balance at a given point in time.
	/// It is used as an anchor point to calculate displayed balances
	/// and as a way of tracking changes (eg if a transaction is edited).
	/// </summary>
	public class AccountCheckpoint
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public int AccountId { get; set; }	
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Balance { get; set; }

		//
		//NOTE: Adding new fields defaults to NULL
		//
	}
}
