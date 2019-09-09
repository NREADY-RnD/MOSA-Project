// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildTransformationFile : BuildBaseTemplate
	{
		protected List<string> Filters;
		protected string Namespace;
		protected string Classname;

		public BuildTransformationFile(string destinationPath, string destinationFile, string @namespace, string classname, List<string> filters)
			: base(null, destinationPath, destinationFile)
		{
			Filters = filters;
			Namespace = @namespace;
			Classname = classname;
		}

		protected override void Body()
		{
			//Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();

			Lines.AppendLine($"namespace {Namespace}"); // Mosa.Compiler.Framework.Transformation.Auto
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// Transformations");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine($"\tpublic static class {Classname}"); // AutoTransformations
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly List<BaseTransformation> List = new List<BaseTransformation> {");

			foreach (var name in BuildTransformationFiles.Transformations)
			{
				bool include = false;

				foreach (string filter in Filters)
				{
					var regex = new Regex(filter);

					var match = regex.Match(name);

					if (match.Success)
					{
						include = true;
						break;
					}
				}

				if (include)
				{
					var newname = name.Replace(Namespace, string.Empty);

					Lines.AppendLine($"\t\t\tnew {newname}(),");
				}
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
