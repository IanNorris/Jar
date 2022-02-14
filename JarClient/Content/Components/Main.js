"use strict";

Vue.component('jar-main', {
	template: '#MainTemplate',
	data: function () {
		return {
			budgetName: '',
			activePage: 0,
			selectedAccount: null,
			dateRangeStart: moment().subtract(30, 'days'),
			dateRangeEnd: moment(),
			accounts: [],
			selectedAccountTransactions: [],
			sideNavWidth: 0.0,
		};
	},
	created: async function () {
		this.MainPage_OffMainMenu = -1;
		this.MainPage_Home = 0;
		this.MainPage_Budgets = 1;
		this.MainPage_Reports = 2;
		this.MainPage_Accounts = 3;
		this.MainPage_NewAccount = 4;
		this.MainPage_Settings = 5;

		await this.getAccounts();
		this.sideNavWidth = await Settings.GetSideNavWidth();

		this.$parent.showLoader = false;
	},
	methods: {
		selectAccount: async function (index) {
			this.selectedAccount = this.accounts[index];
			this.activePage = this.MainPage_OffMainMenu;
			await this.getTransactions();
		},
		getAccounts: async function () {
			this.accounts = await Accounts.GetAccounts();
			this.budgetName = await GetBudgetName();
		},
		getTransactions: async function () {
			if (this.selectedAccount) {
				this.selectedAccountTransactions = await Transactions.GetDisplayTransactions(this.dateRangeStart.toDate(), this.dateRangeEnd.toDate(), this.selectedAccount.Id);
			}
		},
		signOut: function () {
			globalApp.showOpen = true;
			globalApp.showBudget = false;
		},
		openHome: function () {
			this.activePage = this.MainPage_Home;
			this.selectedAccount = null;
		},
		openBudget: function () {
			this.activePage = this.MainPage_Budgets;
			this.selectedAccount = null;
		},
		openReports: function () {
			this.activePage = this.MainPage_Reports;
			this.selectedAccount = null;
		},
		openAccounts: function () {
			this.activePage = this.MainPage_Accounts;
			this.selectedAccount = null;
		},
		dateRangeChanged: async function (start, end) {
			this.dateRangeStart = start;
			this.dateRangeEnd = end;
			if (this.selectedAccount) {
				await this.getTransactions();
			}
		},
		dragComplete: function (newSize) {
			Settings.SetSideNavWidth(newSize);
		}
	},
	computed: {
		shouldHeightFix: function () {
			if (this.activePage == this.MainPage_OffMainMenu) {
				return true;
			}
			return false;
		}
	},
	filters: {
		asDate: function (date) {
			return moment(date).format('L');
		},
		asCurrency: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				style: 'currency',
				currency: 'GBP',
				minimumFractionDigits: 2,
				maximumFractionDigits: 2
			});

			return formatter.format(amount / 100.0);
		},
		asCurrencyRoundDown: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				style: 'currency',
				currency: 'GBP',
				minimumFractionDigits: 0,
				maximumFractionDigits: 0
			});

			return formatter.format(Math.floor(amount / 100.0));
		},
		asNumeric: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				minimumFractionDigits: 2,
				maximumFractionDigits: 2
			});

			return formatter.format(amount / 100.0);
		},
		asNumericRoundDown: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				minimumFractionDigits: 0,
				maximumFractionDigits: 0
			});

			return formatter.format(Math.floor(amount / 100.0));
		}
	}
});