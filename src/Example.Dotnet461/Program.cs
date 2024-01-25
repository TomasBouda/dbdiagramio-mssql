using DbDiagramIo.MsSql;
using System;

namespace Example.Dotnet461
{
	class Program
	{
		// TODO: set your own database connection string here
		private const string __ConnectionString = "Server=(local)\\SQLEXPRESS;Database=master;Integrated Security=true;";

		static void Main(string[] args)
		{
			(Table[] tables, ForeignKey[] foreignKeys) = MsSqlSchemaReader.GetSchemaDescriptor(__ConnectionString);

			foreach (Table table in tables)
			{
				Console.WriteLine(table.ToDbDbiagramCode());
			}

			foreach (ForeignKey fk in foreignKeys)
			{
				Console.WriteLine(fk.ToDbDiagramDto());
			}
		}
	}
}
