Vue.component( 'jar-open', {
    template: '#OpenTemplate',
    data: function() {
        return {
			settings: null,
			selectedBudget: 0,
			password: '',
			password2: '',
			rememberPassword: false,
			newBudgetObj: null,
        };
    },
    created: function(){
        this.getSettings();
	},
	methods: {
        getSettings: async function() {
			this.settings = await globalDataModel.getSettings();
			if (this.selectedBudget > 0) {
				this.password = this.settings.Budgets[this.selectedBudget].Password;
			}
        },
		selectBudget: function(index) {
			this.selectedBudget = index;
			if (this.$refs.password) {
				this.$refs.password.focus();
			}
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
					if( result ) {
						this.newBudgetObj = result;
					}
				});
			} );
		},
		openExistingBudget: function() {
			let self = this;
			globalDataModel.openExistingBudget().then( function(result) {
				globalDataModel.getSettings().then( function(result) {
					self.settings.Budgets = result.Budgets;
					self.selectedBudget = self.settings.Budgets.length - 1;
					globalDataModel.writeSettings();
				});
			} );
		},
		getNewBudgetPath: function() {
			let self = this;
			globalDataModel.getNewBudgetPath().then( function(result) {
				self.newBudgetObj = result;
				self.newBudgetObj.Password = "";
			} );
		},
		createNewBudget: function() {
			let self = this;
			globalDataModel.createNewBudget( self.newBudgetObj.Path, this.password ).then( function(result) {
				if( result ) {
					globalApp.showOpen = false;
					globalApp.showBudget = true;
				}
			} );
		},
		cancelNew: function() {
			this.newBudgetObj = null;
		}
    },
	filters: {
        asDateAgo: function(date) {
            return moment(date).fromNow();
        }
	}
});