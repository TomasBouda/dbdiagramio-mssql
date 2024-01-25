using System.Text.RegularExpressions;

namespace DbDiagramIo.Core.Extensions
{
	public static class StringExtensions
	{
		public static bool Like(this string toSearch, string toFind, RegexOptions regexOptions = RegexOptions.None)
		{
			return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\")
				.Replace(toFind, ch => @"\" + ch)
				.Replace('_', '.')
				.Replace("%", ".*") + @"\z", RegexOptions.Singleline | regexOptions)
				.IsMatch(toSearch);
		}
	}
}