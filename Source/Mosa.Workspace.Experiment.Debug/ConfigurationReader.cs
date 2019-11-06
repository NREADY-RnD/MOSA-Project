// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Workspace.Experiment.Debug
{
	public sealed class ConfigurationReader
	{
		private sealed class Property
		{
			public string Name { get; set; }

			public string Value { get; set; }

			public List<string> List { get; } = new List<string>();

			public override string ToString()
			{
				if (Value == null)
					return Name;
				else
					return $"{Name}={Value}";
			}

			public bool IsValueTrue
			{
				get
				{
					if (string.IsNullOrEmpty(Value))
						return false;

					return Value.ToLower().StartsWith("true") || Value.ToLower().StartsWith("y");
				}
			}

			public bool IsValueFalse
			{
				get
				{
					if (string.IsNullOrEmpty(Value))
						return false;

					return Value.ToLower().StartsWith("false") || Value.ToLower().StartsWith("n");
				}
			}
		}

		private List<Property> Properties = new List<Property>();
		private Dictionary<string, Property> Lookups = new Dictionary<string, Property>();

		private static char[] whitespace = new char[] { '\t', ' ' };

		public void Import(string filename)
		{
			var lines = File.ReadAllLines(filename);
			Import(lines);
		}

		public void Import(IList<string> lines)
		{
			string lastProperty = string.Empty;
			int lastLevel = 0;

			foreach (var l in lines)
			{
				if (l == null)
					continue;

				var line = TrimOutComment(l).TrimEnd(whitespace);

				var level = DetermineLevel(line);

				var data = line.Substring(level).Trim();

				if (data.Length == 0)
					continue;

				if (data.StartsWith('\'') || data.StartsWith('-'))
				{
					string item = string.Empty;

					if (data.StartsWith('\''))
						item = data.Trim('\'');
					else if (data.StartsWith('-'))
						item = data.Substring(1).Trim(whitespace);

					var parent = GetSubPropertyName(lastProperty, level + lastLevel);
					var property = GetProperty(parent);

					property.List.Add(item);

					lastLevel = CountLevels(property.Name) + level - 1;
					continue;
				}
				else
				{
					var name = ParsePropertyName(data);
					var value = ParsePropertyValue(data);

					var parent = GetSubPropertyName(lastProperty, level);

					var fullname = parent.Length == 0 ? name : $"{parent}.{name}";

					lastProperty = fullname;

					var property = GetProperty(fullname);
					property.Value = value;

					lastLevel = CountLevels(property.Name) + level;
					continue;
				}
			}
		}

		private Property GetProperty(string fullname)
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

		private string ParsePropertyName(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var name = data.Substring(0, pos).Trim(whitespace);

			return name;
		}

		private string ParsePropertyValue(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var value = data.Substring(pos + 1).Trim(whitespace);

			return value;
		}

		private string TrimOutComment(string line)
		{
			int pos = line.IndexOf('#');

			if (pos < 0)
				return line;

			return line.Substring(0, pos);
		}

		private string GetSubPropertyName(string name, int level)
		{
			if (level == 0)
				return string.Empty;

			int count = 0;

			for (int i = 0; i < name.Length; i++)
			{
				var c = name[i];

				if (c == '.')
				{
					count++;

					if (count == level)
					{
						return name.Substring(0, i);
					}
				}
			}

			return name;
		}

		private int DetermineLevel(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (c != '\t')
					return i;
			}

			return 0;
		}

		private int CountLevels(string name)
		{
			int count = 0;

			foreach (var c in name)
			{
				if (c == '.')
					count++;
			}

			return count;
		}
	}
}
