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

			editingMemo: -1,
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
		this.MainPage_Jars = 6;

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
		openJars: function () {
			this.activePage = this.MainPage_Jars;
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
		},
		importAccount: async function () {
			await Transactions.ImportTransactionsFromFile(this.selectedAccount.Id);

			await this.getAccounts();
			await this.getTransactions();
		},
		setEditMemo: async function (event, edit) {
			let currentEventTargetParent = $(event.currentTarget).parent();
			this.editingMemo = edit;

			Vue.nextTick(function () {
				currentEventTargetParent.children('input').focus();
			})
		}
	},
	computed: {
		shouldHeightFix: function () {
			if (this.activePage == this.MainPage_OffMainMenu) {
				return true;
			}
			return false;
		}
	}
});