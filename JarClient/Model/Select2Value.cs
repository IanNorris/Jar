using Newtonsoft.Json;

namespace Jar.Model
{
	public class Select2Value
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }
	}
}
