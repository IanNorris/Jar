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

		public async Task<IEnumerable<Select2Value>> GetCategories()
		{
			var categories = _database.Connection.Table<Category>().OrderBy(c => c.Name);

			return categories.Select( c => new Select2Value { Id = c.Id, Text = c.Name } );
		}

		public async Task CreateNewJar(Model.Jar newJar, string newCategoryName)
		{
			if(newCategoryName != null)
			{
				var category = new Category();
				category.Name = newCategoryName;

				_database.Connection.Insert(category);

				newJar.CategoryId = category.Id;
			}

			_database.Connection.Insert(newJar);
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
