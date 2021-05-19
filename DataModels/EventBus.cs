using Jar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jar.DataModels
{
	public class EventBus
	{
		public delegate void OnTransactionMateriallyChangedDelegate(Transaction transaction, bool dateChanged, bool amountChanged, bool categoryChanged);

		public OnTransactionMateriallyChangedDelegate OnTransactionMateriallyChanged;
	}
}
