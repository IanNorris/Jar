using System.Text.RegularExpressions;

namespace Jar.Model
{
	public class TransactionFilter
	{
		public string MustStartWith { get; set; }
		public string MustContain { get; set; }
		public string Institution { get; set; }
		public string Regex { get; set; }
		public string PayeeOutput { get; set; }
		public string ReferenceOutput { get; set; }

		public Regex CompiledRegex { get; set; }
	}
}
