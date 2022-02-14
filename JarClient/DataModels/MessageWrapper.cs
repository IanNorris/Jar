
namespace Jar.DataModels
{
	public class MessageWrapper
	{
		public string Target { get; set; }
		public string Function { get; set; }
		public string Payload { get; set; }
		public long Callback { get; set; }
	}
}
