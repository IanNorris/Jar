Vue.component( 'jar-open', {
    template: '#OpenTemplate',
    data: function() {
        return {
			settings: null,
			selectedBudget: 0
        };
    },
    created: function(){
        this.getSettings();
    },
	mounted: function(){
		this.$refs.password.focus();
	},
    methods: {
        getSettings: async function() {
            this.settings = await globalDataModel.getSettings();
        },
		selectBudget: function(index) {
			this.selectedBudget = index;
			this.$refs.password.focus();
		}
    },
	filters: {
        asDateAgo: function(date) {
            return moment(date).fromNow();
        }
    }
});