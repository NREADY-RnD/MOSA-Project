// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Common.Configuration
{
	public static class Reader
	{
		private static char[] whitespace = new char[] { '\t', ' ' };

		public static Settings Import(string filename)
		{
			var lines = File.ReadAllLines(filename);
			return Import(lines);
		}

		public static Settings Import(IList<string> lines)
		{
			var settings = new Settings();

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

				if (data.StartsWith("\'") || data.StartsWith("-"))
				{
					string item = string.Empty;

					if (data.StartsWith("\'"))
						item = data.Trim('\'');
					else if (data.StartsWith("-"))
						item = data.Substring(1).Trim(whitespace);

					var parent = GetSubPropertyName(lastProperty, level + lastLevel);
					var property = settings.CreateProperty(parent);

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

					var property = settings.CreateProperty(fullname);
					property.Value = value;

					lastLevel = CountLevels(property.Name) + level;
					continue;
				}
			}

			return settings;
		}

		public static Settings ParseArguments(string[] args, List<Argument> arguments)
		{
			var settings = new Settings();
			var defaultMap = FindDefaultArgumentMap(arguments);

			for (int at = 1; at < args.Length; at++)
			{
				var arg = args[at];
				var map = FindArgumentMap(arg, arguments);

				if (map == null)
				{
					if (defaultMap == null)
						continue;

					var property = settings.CreateProperty(defaultMap.Setting);

					if (defaultMap.IsList)
					{
						property.List.Add(arg);
					}
					else
					{
						property.Value = arg;
					}

					continue;
				}
				else
				{
					var property = settings.CreateProperty(map.Setting);

					if (map.Value != null)
					{
						property.Value = map.Value;
					}
					else if (map.IsList)
					{
						property.List.Add(args[++at]);
					}
					else
					{
						property.Value = args[++at];
					}
				}
			}

			return settings;
		}

		#region Internal Methods

		private static string ParsePropertyName(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var name = data.Substring(0, pos).Trim(whitespace);

			return name;
		}

		private static string ParsePropertyValue(string data)
		{
			var pos = data.IndexOf(':');

			if (pos == 0)
				return null;

			var value = data.Substring(pos + 1).Trim(whitespace);

			return value;
		}

		private static string TrimOutComment(string line)
		{
			int pos = line.IndexOf('#');

			if (pos < 0)
				return line;

			return line.Substring(0, pos);
		}

		private static string GetSubPropertyName(string name, int level)
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

		private static int DetermineLevel(string line)
		{
			for (int i = 0; i < line.Length; i++)
			{
				char c = line[i];

				if (c != '\t')
					return i;
			}

			return 0;
		}

		private static int CountLevels(string name)
		{
			int count = 0;

			foreach (var c in name)
			{
				if (c == '.')
					count++;
			}

			return count;
		}

		#endregion Internal Methods

		private static Argument FindArgumentMap(string arg, List<Argument> map)
		{
			foreach (var entry in map)
			{
				if (entry.Name == arg)
					return entry;
			}

			return null;
		}

		private static List<Argument> FindArguments(string arg, List<Argument> arguments)
		{
			List<Argument> map = new List<Argument>();

			foreach (var entry in arguments)
			{
				if (entry.Name == arg)
				{
					map.Add(entry);
				}
			}
			return map;
		}

		private static Argument FindDefaultArgumentMap(List<Argument> map)
		{
			foreach (var entry in map)
			{
				if (entry.Value == null && entry.Name == null)
				{
					return entry;
				}
			}

			return null;
		}
	}
}
