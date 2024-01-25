using DbDiagramIo.MsSql.Objects.Base;

namespace DbDiagramIo.MsSql.Objects
{
	public class ForeignKey : IDbmlObject
	{
		public Table SourceTable { get; }
		public string SourceColumnName { get; }

		public Table TargetTable { get; }
		public string TargetColumnName { get; }

		public ForeignKey(Table sourceTable, string sourceColumnName, Table targetTable, string targetColumnName)
		{
			SourceTable = sourceTable;
			SourceColumnName = sourceColumnName;
			TargetTable = targetTable;
			TargetColumnName = targetColumnName;
		}

		public string ToDbml()
		{
			return $"Ref: \"{SourceTable.Schema}.{SourceTable.Name}\".{SourceColumnName} > \"{TargetTable.Schema}.{TargetTable.Name}\".{TargetColumnName}";
		}
	}
}
