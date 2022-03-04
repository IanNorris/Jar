using System.Collections.Generic;

namespace JarPlugin.StarlingBank.Models
{
	internal class StarlingError
	{
		public bool Success { get; set; } = true;
		public List<StarlingErrorMessage> Errors { get; set; } = new List<StarlingErrorMessage>();
	}
}
