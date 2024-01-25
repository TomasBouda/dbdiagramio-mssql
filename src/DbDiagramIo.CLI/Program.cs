using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;
using DbDiagramIo.Core;
using DbDiagramIo.Core.Extensions;
using DbDiagramIo.MsSql;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateLogger();

Log.Information("DbDiagramIo.CLI v{Version}", Assembly.GetExecutingAssembly().GetName().Version);
	
Parser.Default.ParseArguments<Options>(args)
	.WithParsed(o =>
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Is(o.LogEventLevel)
			.WriteTo.Console()
			.CreateLogger();

		if (!string.IsNullOrEmpty(o.ConnectionString))
		{
			var schemaReader = new MsSqlSchemaReader(o.ConnectionString);
			var schema = schemaReader.GetDbmlSchema();
			var dbml = new StringBuilder();
			
			var regexOptions = RegexOptions.IgnoreCase;
			var excludedSchemas = o.ExcludedSchemas?.Where(s => !string.IsNullOrEmpty(s)).ToList();
			var excludedTables = o.ExcludedTables?.Where(s => !string.IsNullOrEmpty(s)).ToList();
			
			Log.Debug("{Count} tables in total", schema.Tables.Count);
			
			schema.Tables
				.Where(t => excludedSchemas.AllSafe(s => !t.Schema.Like(s, regexOptions), true) && excludedTables.AllSafe(s => !t.Name.Like(s, regexOptions), true))
				.ToList()
				.ForEach(t => dbml.AppendLine(t.ToDbml()));
			
			Log.Debug("{Count} foreign keys in total", schema.ForeignKeys.Count);
			
			schema.ForeignKeys
				.Where(f => o.ExcludedSchemas.AllSafe(s => !f.SourceTable.Schema.Like(s, regexOptions), true) && o.ExcludedTables.AllSafe(s => !f.SourceTable.Name.Like(s, regexOptions), true)
			                                                        && o.ExcludedSchemas.AllSafe(s => !f.TargetTable.Schema.Like(s, regexOptions), true) && o.ExcludedTables.AllSafe(s => !f.TargetTable.Name.Like(s, regexOptions), true))
				.ToList()
				.ForEach(t => dbml.AppendLine(t.ToDbml()));

			if (!string.IsNullOrEmpty(o.OutputFile))
			{
				var dirName = Path.GetDirectoryName(o.OutputFile);
				if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
				{
					Directory.CreateDirectory(dirName);
				}
				
				
				File.WriteAllText(o.OutputFile, dbml.ToString());
				
				Log.Information("Dbml file created at {OutputFile}", o.OutputFile);
			}
			else
			{
				Log.Information("Dbml output:");
				Console.WriteLine(dbml.ToString());
			}
		}
		else
		{
			Log.Error("No connection string provided!");
		}
	});

public class Options
{
	[Option('v', "verbosity", Required = false, HelpText = "Sets the serilog log level.", Default = LogEventLevel.Information)]
	public LogEventLevel LogEventLevel { get; set; }
	
	[Option('c', "connection-string", Required = true, HelpText = "The connection string to the database.")]
	public string ConnectionString { get; set; } = null!;
	
	[Option("exclude-schemas", Required = false, HelpText = "The schemas to exclude from the dbml output. You can use LIKE syntax here.", Separator = ';')]
	public IEnumerable<string>? ExcludedSchemas { get; set; }
	
	[Option("exclude-tables", Required = false, HelpText = "The schemas to exclude from the dbml output. You can use LIKE syntax here e.g. 'AspNet%' to exclude all tables starting with 'AspNet'.", Separator = ';')]
	public IEnumerable<string>? ExcludedTables { get; set; }
	
	[Option('o', "output-file", Required = false, HelpText = "The file to write the dbml output to. If not provided, the output will be written to the console.")]
	public string? OutputFile { get; set; }
}	