using System.Collections.Generic;
using DbDiagramIo.MsSql.Objects;
using DbDiagramIo.MsSql.Objects.Base;

namespace DbDiagramIo.MsSql
{
	public class DbmlSchema : IDbmlObject
	{
		public HashSet<Table> Tables { get; }
		public HashSet<ForeignKey> ForeignKeys { get; }

		public DbmlSchema(HashSet<Table> tables, HashSet<ForeignKey> foreignKeys)
		{
			Tables = tables;
			ForeignKeys = foreignKeys;
		}
		
		public string ToDbml()
		{
			throw new System.NotImplementedException();
		}
	}
}