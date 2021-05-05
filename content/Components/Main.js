Vue.component('jar-main', {
    template: '#MainTemplate',
    data: function() {
        return {
            budgetName: '',
            activePage: 0,
            selectedAccount: null,
            accounts: [],
            transactions: []
        };
    },
    created: function () {
        this.MainPage_OffMainMenu = -1;
        this.MainPage_Home = 0;
        this.MainPage_Budgets = 1;
        this.MainPage_Reports = 2;
        this.MainPage_Accounts = 3;
        this.MainPage_NewAccount = 4;
        this.MainPage_Settings = 5;

		this.getAccounts();
    },
    methods: {
        selectAccount: async function(index) {
            this.selectedAccount = this.accounts[index];
            this.activePage = this.MainPage_OffMainMenu;
            this.getTransactions();
        },
        getAccounts: async function() {
            this.accounts = await globalDataModel.getAccounts();
            this.budgetName = await globalDataModel.getBudgetName();
        },
        getTransactions: async function () {
            if (this.selectedAccount) {
                this.transactions = await globalDataModel.getTransactionsBetweenDates(moment('2020/03/01', 'YYYY/MM/DD').toDate(), moment('2022/01/01', 'YYYY/MM/DD').toDate(), this.selectedAccount.Id);
            }
        },
        signOut: function() {
            globalApp.showOpen = true;
			globalApp.showBudget = false;
        },
        openHome: function() {
            this.activePage = this.MainPage_Home;
            this.selectedAccount = null;
        },
        openBudget: function() {
            this.activePage = this.MainPage_Budgets;
            this.selectedAccount = null;
        },
        openReports: function() {
            this.activePage = this.MainPage_Reports;
            this.selectedAccount = null;
        },
        openAccounts: function() {
            this.activePage = this.MainPage_Accounts;
            this.selectedAccount = null;
        },
    },
    filters: {
        asDate: function(date) {
            return moment(date).format('L');
        },
        asCurrency: function(amount) {
            return '£' + (amount / 100.0).toFixed(2);
        },
        asCurrencyRoundDown: function(amount) {
            return '£' + Math.floor((amount / 100.0)).toFixed(2);
        }
    }
});