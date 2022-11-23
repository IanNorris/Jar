using Jar.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Jar.DataModels
{
	public class EnumValue
	{
		public static IEnumerable<EnumValue> Get<T>() where T : System.Enum
		{
			var type = typeof(T);

			var enumValue = Enum.GetValues(typeof(JarType));
			var outputList = new List<EnumValue>(enumValue.Length);

			foreach (int value in enumValue)
			{
				var name = Enum.GetName(typeof(T), value);
				var member = typeof(T).GetMember(name).First();
				var descriptionAttribute = (DescriptionAttribute)member.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() ?? throw new InvalidDataException($"Enum {typeof(T).Name} value {value} does not have a Description attribute.");

				outputList.Add(new EnumValue(value, name, descriptionAttribute.Description));
			}

			return outputList;
		}

		private EnumValue(int integer, string name, string description)
		{
			Integer = integer;
			Name = name;
			Description = description;
		}

		public int Integer { get; }
		public string Name { get; }
		public string Description { get; }

	}
}
