namespace DbDiagramIo.MsSql.Objects.Base
{
	/// <summary>
	/// Defines object that can be converted to dbdiagram.io format
	/// https://dbml.dbdiagram.io/home/#intro
	/// </summary>
	public interface IDbmlObject
	{
		string ToDbml();
	}
}