﻿using SQLite;
using System;

namespace Jar.Model
{
	public class Transaction
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public DateTime Date { get; set; }
		public string Payee { get; set; }
		public string Memo { get; set; }
		public string Note { get; set; }
		public int Category { get; set; }
		public int Currency { get; set; }
		public int ConversionRate { get; set; }
		public int Amount { get; set; }
	}
}
