using System.Collections.Generic;
using JarPluginApi;

namespace Jar.DataModels
{
	public class EventBus
	{
		public delegate void OnTransactionMateriallyChangedDelegate(List<Transaction> transactions, bool dateChanged, bool amountChanged, bool categoryChanged);

		public OnTransactionMateriallyChangedDelegate OnTransactionMateriallyChanged;
	}
}
