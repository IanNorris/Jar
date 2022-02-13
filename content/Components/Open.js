"use strict";

Vue.component('jar-open', {
	template: '#OpenTemplate',
	data: function () {
		return {
			budgets: [],
			selectedBudget: 0,
			password: '',
			password2: '',
			rememberPassword: false,
			newBudgetObj: null
		};
	},
	async created() {
		this.budgets = await Settings.GetBudgets();
	},
	methods: {
		selectBudget: function (index) {
			this.selectedBudget = index;
			if (this.$refs.password) {
				this.$refs.password.focus();
			}
		},
		openBudget: async function () {
			this.$parent.showLoader = true;
			var result = await OpenBudget(this.selectedBudget, this.budgets[this.selectedBudget].Path, this.password);
			if (result) {
				globalApp.showOpen = false;
				globalApp.showBudget = true;
			}
			else {
				this.$parent.showLoader = false;
			}
		},
		newBudget: async function () {
			this.newBudgetObj = await NewBudget();
		},
		openExistingBudget: async function () {
			await OpenExistingBudget();
		},
		getNewBudgetPath: async function () {
			let self = this;
			self.newBudgetObj = await GetNewBudgetPath();
			self.newBudgetObj.Password = "";
		},
		createNewBudget: async function () {
			let self = this;
			var result = await CreateNewBudget(self.newBudgetObj.Path, this.password);
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