Vue.component( 'jar-open', {
    template: '#OpenTemplate',
    data: function() {
        return {
			settings: null,
			selectedBudget: 0,
			password: '',
        };
    },
    created: function(){
        this.getSettings();
    },
	mounted: function(){
		if( this.$refs.password ) {
			this.$refs.password.focus();
		}
	},
    methods: {
        getSettings: async function() {
            this.settings = await globalDataModel.getSettings();
			this.password = this.settings.Budgets[this.selectedBudget].Password;
        },
		selectBudget: function(index) {
			this.selectedBudget = index;
			this.$refs.password.focus();
		},
		openBudget: async function() {
			globalDataModel.openBudget( this.settings.Budgets[this.selectedBudget].Path, this.password ).then( function(result) {
				if( result ) {
					globalApp.showOpen = false;
					globalApp.showBudget = true;
				}
			} );
		}
    },
	filters: {
        asDateAgo: function(date) {
            return moment(date).fromNow();
        }
    }
});