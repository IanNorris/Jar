Vue.component( 'jar-main', {
    template: '#MainTemplate',
    data: function() {
        return {
            showHome: true,
            showBudget: false,
            showReports: false,
            showAccounts: false,
            addNewAccount: false,
            selectedAccount: null,
            accounts: [],
            transactions: []
        };
    },
    created: function(){
		this.getAccounts();
    },
    methods: {
        selectAccount: async function(index) {
            this.selectedAccount = this.accounts[index];

            this.getTransactions();
        },
        getAccounts: async function() {
            this.accounts = await globalDataModel.getAccounts();
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
            this.showHome = true;
            this.showBudget = false;
            this.showReports = false;
            this.showAccounts = false;
            this.selectedAccount = null;
        },
        openBudget: function() {
            this.showHome = false;
            this.showBudget = true;
            this.showReports = false;
            this.showAccounts = false;
            this.selectedAccount = null;
        },
        openReports: function() {
            this.showHome = false;
            this.showBudget = false;
            this.showReports = true;
            this.showAccounts = false;
            this.selectedAccount = null;
        },
        openAccounts: function() {
            this.showHome = false;
            this.showBudget = false;
            this.showReports = false;
            this.showAccounts = true;
            this.selectedAccount = null;
        },
    },
    filters: {
        asDate: function(date) {
            return moment(date).format('L');
        },
        asCurrency: function(amount) {
            return '£' + (amount / 100.0);
        },
        asCurrencyRoundDown: function(amount) {
            return '£' + Math.floor((amount / 100.0));
        }
    }
});