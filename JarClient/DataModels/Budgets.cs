using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		static List<Model.Jar> jars = new List<Model.Jar>()
			{
				new Model.Jar
				{
					Id = 1256,
					Name = "My Income",
					Type = JarType.Income,
					MonthlyValue = 200000,
				},

				new Model.Jar
				{
					Id = 58216,
					Name = "Wife Income",
					Type = JarType.Income,
					MonthlyValue = 300000,
				},

				new Model.Jar
				{
					Id = 6783,
					Name = "Mortgage",
					Type = JarType.Transaction,
					MonthlyValue = 100000,
				},

				new Model.Jar
				{
					Id = 527,
					Name = "Utilities",
					Type = JarType.Transaction,
					MonthlyValue = 120000,
				},

				new Model.Jar
				{
					Id = 371,
					Name = "Insurance",
					Type = JarType.Buffer,
					TargetValue = 100000,
					TargetDate = DateTime.UtcNow.AddYears(1),
				},

				new Model.Jar
				{
					Id = 154,
					Name = "Takeout",
					Type = JarType.Budget,
					MonthlyValue = 10000,
				},

				new Model.Jar
				{
					Id = 7173,
					Name = "Holiday",
					Type = JarType.Goal,
					TargetValue = 700000,
					TargetDate= DateTime.UtcNow.AddMonths(4),
				},
			};

		public IEnumerable<Model.Jar> GetAllJars()
		{
			return jars.OrderBy(x => x.Order);
		}

		public async Task OnJarReorder(IEnumerable<Model.Jar> newOrder)
		{
			var index = 0;

			foreach(var jar in newOrder)
			{
				jar.Order = index;

				index++;
			}

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
						FlagTotalAmount = true,
						Filters = null,
						FlagTransactionCount = true,
						CategoryId = -1,
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
						FlagTotalAmount = true,
						Filters = null,
						FlagTransactionCount = true,
						CategoryId = -1,
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
						FlagTotalAmount = true,
						Filters = null,
						FlagTransactionCount = true,
						CategoryId = -1,
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
						FlagTotalAmount = true,
						Filters = "Budget Insurance",
						FlagTransactionCount = true,
						CategoryId = -1,
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
						FlagTotalAmount = true,
						Filters = "Budget Insurance",
						FlagTransactionCount = true,
						CategoryId = -1,
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
						FlagTotalAmount = true,
						Filters = null,
						FlagTransactionCount = true,
						CategoryId = -1,
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
