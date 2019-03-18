Vue.component( 'jar-main', {
    template: '#MainTemplate',
    data: function() {
        return {
            transactions: [],
            accounts: []
        };
    },
    created: function(){
		this.getAccounts();
		this.getTransactions();
    },
    methods: {
        getAccounts: async function() {
            this.accounts = await globalDataModel.getAccounts();
        },
        getTransactions: async function() {
            this.transactions = await globalDataModel.getTransactionsBetweenDates(moment('2018/04/01', 'YYYY/MM/DD').toDate(), moment('2018/04/30', 'YYYY/MM/DD').toDate() );
        },
        signOut: function() {
            globalApp.showOpen = true;
			globalApp.showBudget = false;
        }
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