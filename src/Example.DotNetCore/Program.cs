using DbDiagramIo.MsSql;
using System;
using System.Linq;
using DbDiagramIo.MsSql.Objects;

namespace Example.DotNetCore
{
	class Program
	{
		private const string __ConnectionString = "Server=.;Database=LUP_Package;Integrated Security=true;";

		static void Main(string[] args)
		{
			var schemaReader = new MsSqlSchemaReader(__ConnectionString);
			var schema = schemaReader.GetDbmlSchema();
			
			foreach (Table table in schema.Tables.Where(t => t.Schema != "HangFire" && !t.Name.StartsWith("AspNet")))
			{
				Console.WriteLine(table.ToDbml());
			}

			foreach (ForeignKey fk in schema.ForeignKeys.Where(f => f.SourceTable.Schema != "HangFire" && f.TargetTable.Schema != "HangFire"
				         && f.TargetTable.Name != "WinUsers" && f.SourceTable.Name != "WinUsers"
				         && !f.SourceTable.Name.StartsWith("AspNet")&& !f.TargetTable.Name.StartsWith("AspNet")))
			{
				Console.WriteLine(fk.ToDbml());
			}
		}
	}
}
