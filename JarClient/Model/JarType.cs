using System.ComponentModel;

namespace Jar.Model
{
	public enum JarType
	{
		[Description(
			@"Any source of income, such as wages, side-hustle income, rental income etc.
			You can create more than one source of income if you want them to be displayed separately, such as if you want your and your partner's income to not be combined when showing a breakdown of your budget.")]
		Income = 1,

		[Description(@"Any payments that should be tracked separately, but where you are not adhering to a budget. This might be your mortgage or utility bills. 
            You aren't trying to apply a budget to this spending, but you do want to track it. You can set an estimate if you know it, or one will be calculated for you. The value of the Jar will always be zero
            as your jar value is calculated to exactly match the transactions in it.")]
		Transaction = 2,

		[Description(@"Budgets are used when you want to keep a limit on spending in an area.
            This might be for indulgences, hobbies and other activities that you might spend too much on.
            For example if you like video games, this will allow you to decide if you can afford a new game this month.
            The value of Budgets can roll over from month-to-month.")]
		Budget = 3,

		[Description(@"A buffer allows you to prepare for large spends that you know are coming, but happen regularly. For example saving up for your car insurance if you pay it annually. You can either set a monthly amount, or set a target date and repeating cycle and target value and let Jars calculate the rest.
             Buffers will warn you if you're not going to hit your target. A buffer could also just contain a target amount and be used as a ""rainy day"" fund for unexpected expenses.")]
		Buffer = 4,

		[Description(@"Goals are things you are saving for, such as holidays, rennovations, new computers etc. They typically last multiple months or years and are closed when completed. If a target amount is specified, Jars will inform you when you might hit the target based on your deposits so far.")]
		Goal = 5,

		[Description(@"Savings are very long term jars for your money to grow in. This might be for a retirement etc.")]
		Savings = 6,
	}
}
