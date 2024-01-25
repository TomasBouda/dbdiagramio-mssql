using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using DbDiagramIo.MsSql.Objects;

namespace DbDiagramIo.MsSql
{
	public class MsSqlSchemaReader
	{
		private const string ForeignKeysQuery = "SELECT constraints.CONSTRAINT_NAME as constraint_name, column_source.TABLE_SCHEMA AS source_table_schema, column_source.TABLE_NAME as source_table, column_source.COLUMN_NAME as source_column, column_destination.TABLE_SCHEMA AS destination_table_schema, column_destination.TABLE_NAME as destination_table, column_destination.COLUMN_NAME as destination_column FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS constraints join INFORMATION_SCHEMA.KEY_COLUMN_USAGE column_source on constraints.CONSTRAINT_NAME = column_source.CONSTRAINT_NAME join INFORMATION_SCHEMA.KEY_COLUMN_USAGE column_destination on constraints.UNIQUE_CONSTRAINT_NAME = column_destination.CONSTRAINT_NAME and column_source.ORDINAL_POSITION = column_destination.ORDINAL_POSITION;";
		private const string ColumnsQuery = "select table_schema, table_name, column_name, ordinal_position, DATA_TYPE from information_schema.COLUMNS;";

		public string ConnectionString { get; }

		public MsSqlSchemaReader(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public DbmlSchema GetDbmlSchema()
		{
			var tables = new HashSet<Table>();
			var foreignKeys = new HashSet<ForeignKey>();

			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				using (SqlCommand cmd = new SqlCommand(ColumnsQuery, connection))
				{
					connection.Open();
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							string tableSchema = (string)reader["table_schema"];
							string tableName = (string)reader["table_name"];
							string name = (string)reader["column_name"];
							int ordinalPosition = int.Parse(reader["ordinal_position"].ToString(), CultureInfo.InvariantCulture);
							string sqlType = (string)reader["DATA_TYPE"];

							var table = tables.SingleOrDefault(t => t.Name == tableName && t.Schema == tableSchema);
							if (table == null)
							{
								table = new Table(tableName, tableSchema);
								tables.Add(table);
							}

							var column = new Column(name, sqlType, ordinalPosition);
							table.AddColumn(column);
						}
					}
				}

				using (SqlCommand cmd = new SqlCommand(ForeignKeysQuery, connection))
				{
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var sourceTable = new Table((string)reader["source_table"], (string)reader["source_table_schema"]);
							var destinationTable = new Table((string)reader["destination_table"], (string)reader["destination_table_schema"]);

							string sourceColumnName = (string)reader["source_column"];
							string destinationColumnName = (string)reader["destination_column"];

							ForeignKey fk = new ForeignKey(sourceTable, sourceColumnName, destinationTable, destinationColumnName);

							foreignKeys.Add(fk);
						}
					}
				}

			}

			return new DbmlSchema(tables, foreignKeys);
		}
	}
}