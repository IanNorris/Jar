using System;
using System.IO;

namespace Jar.Import
{
	public class ImportQIF : IImport
	{
		public string[] Extensions()
		{
			return new string[] { ".qif" };
		}

		public string FormatName()
		{
			return "Quicken";
		}

		public void Import(DataModel Model, string Filename, int Account, int Currency, int BatchId)
		{
			Model.Connection.BeginTransaction();

			var lines = File.ReadAllLines(Filename);

			var outputTransaction = new Model.Transaction();

			foreach (var line in lines)
			{
				if(line.Length == 0)
				{
					continue;
				}

				switch(line[0])
				{
					case '!': //Header
					case 'C': //Cleared status
					case 'N': //Check number
					case 'A': //Address
						continue;

					case '^':
						{
							outputTransaction.Currency = Currency;
							outputTransaction.Account = Account;
							outputTransaction.ImportBatch = BatchId;
							Model.Connection.Insert(outputTransaction);
							outputTransaction = new Model.Transaction();
							break;
						}

					case 'D':
						{
							outputTransaction.Date = DateTime.Parse(line.Substring(1));
							break;
						}

					case 'T':
						{
							outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(line.Substring(1)));
							break;
						}

					case 'U':
						{
							outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(line.Substring(1)));
							break;
						}

					case 'P':
						{
							outputTransaction.Payee = line.Substring(1);
							break;
						}

					case 'M':
						{
							outputTransaction.Memo = line.Substring(1);
							break;
						}

					case 'L':
						{
							outputTransaction.ImportedCategory = line.Substring(1);
							break;
						}

				}
			}

			Model.Connection.Commit();
		}
	}
}
