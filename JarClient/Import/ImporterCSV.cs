using System;
using System.Collections.Generic;
using JarPluginApi;
using Microsoft.VisualBasic.FileIO;

namespace Jar.Import
{
	class ImporterCSV : IImport
	{
		public ImportType Type()
		{
			return ImportType.File;
		}

		public string[] Extensions()
		{
			return new string[] { ".csv" };
		}

		public string FormatName()
		{
			return "CSV";
		}

		public List<Transaction> Import(string AccountName, string Filename, int Account, int Currency, int BatchId)
		{
			var outputList = new List<Transaction>();

			using (TextFieldParser parser = new TextFieldParser(Filename))
			{
				parser.TextFieldType = FieldType.Delimited;
				parser.SetDelimiters(",");
				bool firstRow = true;
				while (!parser.EndOfData)
				{
					//Processing row
					string[] fields = parser.ReadFields();

					if (firstRow)
					{
						firstRow = false;
						continue;
					}

					var outputTransaction = new Transaction();
					outputTransaction.ImportBatchId = BatchId;
					outputTransaction.Currency = Currency;
					outputTransaction.AccountId = Account;
					outputTransaction.Date = DateTime.Parse(fields[1]);
					outputTransaction.Payee = fields[2];

					if (!string.IsNullOrWhiteSpace(fields[5]))
					{
						outputTransaction.Amount = -(long)Math.Round(100 * decimal.Parse(fields[5]));
					}

					if (!string.IsNullOrWhiteSpace(fields[6]))
					{
						outputTransaction.Amount = (long)Math.Round(100 * decimal.Parse(fields[6]));
					}

					outputList.Add(outputTransaction);
				}
			}

			return outputList;
		}
	}
}
