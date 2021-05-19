"use strict";

Vue.component('jar-open', {
	template: '#OpenTemplate',
	data: function () {
		return {
			settings: null,
			budgets: [],
			selectedBudget: 0,
			password: '',
			password2: '',
			rememberPassword: false,
			newBudgetObj: null
		};
	},
	async created() {
		this.settings = await settings;
		this.budgets = await this.settings.GetBudgets();
	},
	methods: {
		selectBudget: function (index) {
			this.selectedBudget = index;
			if (this.$refs.password) {
				this.$refs.password.focus();
			}
		},
		openBudget: async function () {
			var result = globalDataModel.OpenBudget(this.selectedBudget, this.budgets[this.selectedBudget].Path, this.password);
			if (result) {
				globalApp.showOpen = false;
				globalApp.showBudget = true;
			}
		},
		newBudget: async function () {
			this.newBudgetObj = await globalDataModel.NewBudget();
		},
		openExistingBudget: async function () {
			await globalDataModel.OpenExistingBudget();
		},
		getNewBudgetPath: async function () {
			let self = this;
			self.newBudgetObj = await globalDataModel.GetNewBudgetPath();
			self.newBudgetObj.Password = "";
		},
		createNewBudget: async function () {
			let self = this;
			var result = await globalDataModel.CreateNewBudget(self.newBudgetObj.Path, this.password);
			if (result) {
				globalApp.showOpen = false;
				globalApp.showBudget = true;
			}
		},
		cancelNew: function () {
			this.newBudgetObj = null;
		}
	},
	filters: {
		asDateAgo: function (date) {
			return moment(date).fromNow();
		}
	}
});