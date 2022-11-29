using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jar.Model;

namespace Jar.DataModels
{
	public class Jars
	{
		public Jars(EventBus eventBus)
		{
		}

		public void SetDatabase(Database database)
		{
			_database = database;
		}

		public async Task<IEnumerable<Select2Value>> GetCategories(string substring)
		{
			var categories = _database.Connection.Table<Category>().OrderBy(c => c.Name);

			var unfiltered = categories.Select( c => new Select2Value { Id = c.Id, Text = c.Name } ).ToList();

			if(string.IsNullOrEmpty(substring))
			{
				return unfiltered;
			}

			var startsWith = unfiltered.Where(c => c.Text.StartsWith(substring, StringComparison.OrdinalIgnoreCase)).ToList();
			var contains = unfiltered.Where(c => c.Text.Contains(substring, StringComparison.OrdinalIgnoreCase)).Except(startsWith);

			startsWith.AddRange(contains);

			return startsWith;
		}

		public async Task CreateNewJar(Model.Jar newJar, string newCategoryName)
		{
			if (!string.IsNullOrEmpty(newCategoryName))
			{
				var category = new Category();
				category.Name = newCategoryName;

				_database.Connection.Insert(category);

				newJar.CategoryId = category.Id;
			}

			_database.Connection.Insert(newJar);
		}

		public async Task EditJar(Model.Jar jar, string newCategoryName)
		{
			if (!string.IsNullOrEmpty(newCategoryName))
			{
				var category = new Category();
				category.Name = newCategoryName;

				_database.Connection.Insert(category);

				jar.CategoryId = category.Id;
			}

			_database.Connection.Update(jar);
		}

		public IEnumerable<EnumValue> GetJarTypes()
		{
			return EnumValue.Get<JarType>();
		}

		public IEnumerable<Model.Jar> GetAllJars()
		{
			var jars = _database.Connection.Table<Model.Jar>().OrderBy(c => c.Order);

			return jars;
		}

		public async Task OnJarReorder(IEnumerable<Model.Jar> newOrder)
		{
			var index = 0;

			foreach (var jar in newOrder)
			{
				jar.Order = index;

				_database.Connection.Update(jar);

				index++;
			}

			
		}

		private Database _database;
	}
}
