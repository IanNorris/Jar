using System;
using System.Collections.Generic;
using Jar.Model;

namespace Jar.DataModels
{
	public class Budgets
	{
		public Budgets(EventBus eventBus)
		{
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		public IEnumerable<DisplayJar> GetDisplayJars(string budgetMonth)
		{
			return new List<DisplayJar>()
			{
				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 0,
						Name = "Income",
						CarryOver = true,
						Type = JarType.Income,
						TargetValue = 0,
						MonthlyValue = 0,
						TargetDate = DateTime.MinValue,
						FlagTotalAmount = 1,
						Filters = null,
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					TotalValue = 123456,
					AverageValue = 120000,
					PreviousValue = 120000,
					CarriedOverValue = 123456,
					JarTotalToNext = 123456,
					MonthlyAssignedValue = 123456,
				},

				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 1,
						Name = "Mortgage",
						CarryOver = false,
						Type = JarType.Transaction,
						TargetValue = 0,
						MonthlyValue = 0,
						TargetDate = DateTime.MinValue,
						FlagTotalAmount = 1,
						Filters = null,
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					TotalValue = 90034,
					AverageValue = 90034,
					PreviousValue = 90034,
					CarriedOverValue = 0,
					JarTotalToNext = 33422,
					MonthlyAssignedValue = 90034,
				},

				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 1,
						Name = "Utilities",
						
						CarryOver = false,
						Type = JarType.Transaction,
						TargetValue = 0,
						MonthlyValue = 0,
						TargetDate = DateTime.MinValue,
						FlagTotalAmount = 1,
						Filters = null,
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					AverageValue = 8032,
					PreviousValue = 8032,
					CarriedOverValue = 0,
					TotalValue = 8032,
					JarTotalToNext = 25390,
					MonthlyAssignedValue = 8032,
				},

				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 1,
						Name = "Car Insurance",
						CarryOver = true,
						Type = JarType.Buffer,
						TargetDate = DateTime.UtcNow.AddMonths(3),
						TargetValue = 40000,
						MonthlyValue = 3000,
						FlagTotalAmount = 1,
						Filters = "Budget Insurance",
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					AverageValue = 3000,
					PreviousValue = 22000,
					TotalValue = 25000,
					CarriedOverValue = 22000,
					JarTotalToNext = 22890,
					MonthlyAssignedValue = 2500,
				},

				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 1,
						Name = "Takeout",
						CarryOver = true,
						Type = JarType.Budget,
						TargetDate = DateTime.MinValue,
						MonthlyValue = 2500,
						TargetValue = 2500,
						FlagTotalAmount = 1,
						Filters = "Budget Insurance",
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					AverageValue = 2345,
					PreviousValue = 2213,
					CarriedOverValue = 345,
					TotalValue = 3045,
					JarTotalToNext = 20545,
					MonthlyAssignedValue = 2700,
				},

				new DisplayJar
				{
					Jar = new Model.Jar
					{
						Id = 1,
						Name = "Holiday",
						CarryOver = true,
						Type = JarType.Goal,
						TargetDate = DateTime.MinValue,
						TargetValue = 900000,
						MonthlyValue = 3000,
						FlagTotalAmount = 1,
						Filters = null,
						FlagTransactionCount = 1,
						ParentId = -1,
					},

					AverageValue = 0,
					PreviousValue = 0,
					CarriedOverValue = 600000,
					MonthlyAssignedValue = 20545,
					TotalValue = 603000,
					JarTotalToNext = 20545,
				},
			};
		}

		private Database _database;
	}
}
