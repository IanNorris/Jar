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
		},
		newBudget: function() {
			globalDataModel.newBudget().then( function(result) {
				globalDataModel.getSettings().then( function(result) {
					this.settings = result;
				});
			} );
		},
		openExistingBudget: function() {
			let self = this;
			globalDataModel.openExistingBudget().then( function(result) {
				globalDataModel.getSettings().then( function(result) {
					self.settings.Budgets = result.Budgets;
					self.selectedBudget = self.settings.Budgets.length - 1;
				});
			} );
		}
    },
	filters: {
        asDateAgo: function(date) {
            return moment(date).fromNow();
        }
    }
});