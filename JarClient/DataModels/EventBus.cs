using JarPluginApi;

namespace Jar.DataModels
{
	public class EventBus
	{
		public delegate void OnTransactionMateriallyChangedDelegate(Transaction transaction, bool dateChanged, bool amountChanged, bool categoryChanged);

		public OnTransactionMateriallyChangedDelegate OnTransactionMateriallyChanged;
	}
}
