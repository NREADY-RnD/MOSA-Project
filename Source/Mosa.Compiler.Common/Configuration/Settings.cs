// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Common.Configuration
{
	public sealed class Settings
	{
		private List<Property> Properties = new List<Property>();
		private Dictionary<string, Property> Lookups = new Dictionary<string, Property>();

		public Property CreateProperty(string fullname)
		{
			if (!Lookups.TryGetValue(fullname, out Property property))
			{
				property = new Property()
				{
					Name = fullname
				};

				Properties.Add(property);
				Lookups.Add(fullname, property);
			}

			return property;
		}

		public void UpdateProperty(Property property)
		{
			var remove = GetProperty(property.Name);

			if (remove != null)
			{
				Lookups.Remove(property.Name);
				Properties.Remove(remove);
			}

			Properties.Add(property);
			Lookups.Add(property.Name, property);
		}

		public Property GetProperty(string fullname)
		{
			if (Lookups.TryGetValue(fullname, out Property property))
			{
				return property;
			}

			return null;
		}

		public string GetValue(string fullname, string defaultValue, bool defaultIfBlank = false)
		{
			var property = GetProperty(fullname);

			if (property == null)
				return defaultValue;

			if (defaultIfBlank && string.IsNullOrEmpty(property.Value))
				return defaultValue;

			return property.Value;
		}

		public string GetValue(string fullname)
		{
			return GetProperty(fullname).Value;
		}

		public List<string> GetValueList(string fullname)
		{
			return GetProperty(fullname).List;
		}

		public bool IsConfigurationValueTrue(string fullname)
		{
			return GetProperty(fullname).IsValueTrue;
		}

		public bool IsConfigurationValueFalse(string fullname)
		{
			return GetProperty(fullname).IsValueFalse;
		}

		public bool GetValueBoolean(string fullname, bool defaultValue)
		{
			var property = GetProperty(fullname);

			if (property == null)
				return defaultValue;

			if (property.IsValueFalse)
				return false;

			if (property.IsValueTrue)
				return true;

			return defaultValue;
		}
	}
}
