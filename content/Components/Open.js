Vue.component( 'jar-open', {
    template: '#OpenTemplate',
    data: function() {
        return {
        };
    },
    created: function(){
        this.getCurrentBudget();
    },
    methods: {
        getCurrentBudget: async function() {
            this.accounts = await globalDataModel.getAccounts();
        }
    }
});