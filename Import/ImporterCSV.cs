using System;
using Microsoft.VisualBasic.FileIO;

namespace Jar.Import
{
	class ImporterCSV : IImport
	{
		public string[] Extensions()
		{
			return new string[] { ".csv" };
		}

		public string FormatName()
		{
			return "CSV";
		}

		public void Import(DataModel Model, string Filename, int Account, int Currency, int BatchId)
		{
			Model.Connection.BeginTransaction();
			using (TextFieldParser parser = new TextFieldParser(Filename))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				bool firstRow = true;
				while (!parser.EndOfData)
				{
					//Processing row
					string[] fields = parser.ReadFields();

					if(firstRow)
					{
						firstRow = false;
						continue;
					}

					var outputTransaction = new Model.Transaction();
					outputTransaction.ImportBatch = BatchId;
					outputTransaction.Currency = Currency;
					outputTransaction.Account = Account;
					outputTransaction.Date = DateTime.Parse(fields[1]);
					outputTransaction.Payee = fields[2];

					if(!string.IsNullOrWhiteSpace(fields[5]))
					{
						outputTransaction.Amount = -(long)Math.Round(100 * decimal.Parse(fields[5]));
					}

					if (!string.IsNullOrWhiteSpace(fields[6]))
					{
						outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(fields[6]));
					}

					Model.Connection.Insert(outputTransaction);
				}
			}

			Model.Connection.Commit();
		}
	}
}
