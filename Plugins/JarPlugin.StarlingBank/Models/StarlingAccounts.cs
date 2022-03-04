using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarPlugin.StarlingBank.Models
{
	internal class StarlingAccounts : StarlingError
	{
		public List<StarlingAccount> Accounts { get; set; } = new List<StarlingAccount>();
	}
}
