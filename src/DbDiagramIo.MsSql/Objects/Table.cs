using System.Collections.Generic;
using System.Text;
using DbDiagramIo.MsSql.Objects.Base;

namespace DbDiagramIo.MsSql.Objects
{
	public class Table : IDbmlObject
	{
		public string Schema { get; }
		public string Name { get; }

		private readonly SortedList<int, Column> _columns = new SortedList<int, Column>();

		internal void AddColumn(Column column)
		{
			_columns.Add(column.OrdinalPosition, column);
			column.Table = this;
		}

		public Table(string name, string schema = "dbo")
		{
			Name = name;
			Schema = schema;
		}

		public string ToDbml()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine($"Table \"{Schema}.{Name}\" {{");

			foreach (Column column in _columns.Values)
			{
				stringBuilder.AppendLine($"\t{column.ToDbml()}");
			}

			stringBuilder.AppendLine("}");

			return stringBuilder.ToString();
		}

		public override string ToString()
		{
			return $"{Schema}.{Name}";
		}
	}
}
