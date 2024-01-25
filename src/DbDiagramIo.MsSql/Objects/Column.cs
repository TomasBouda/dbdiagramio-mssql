using DbDiagramIo.MsSql.Objects.Base;

namespace DbDiagramIo.MsSql.Objects
{
	internal class Column : IDbmlObject
	{
		/// <summary>
		/// The table this column belongs to
		/// </summary>
		public Table Table { get; set; }

		public string Name { get; }
		public int OrdinalPosition { get; private set; }
		public string SqlType { get; }

		private string DbDiagramType
		{
			get
			{
				string sqlTypeLowerCase = SqlType.ToLowerInvariant();

				// Convert types into dbdiagram.io types
				switch (sqlTypeLowerCase)
				{
					case "nvarchar":
						return "varchar";
					case "datetime2":
						return "datetime";
					default:
						return sqlTypeLowerCase;
				}
			}
		}

		public Column(string name, string type, int position)
		{
			Name = name;
			SqlType = type;
			OrdinalPosition = position;
		}

		public string ToDbml()
		{
			return $"{Name} {DbDiagramType}";
		}
	}
}
