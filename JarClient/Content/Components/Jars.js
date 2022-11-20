"use strict";

Vue.component('jar-jars', {
	template: '#JarsTemplate',
	data: function () {
		return {
			allJars: []
		};
	},
	created: async function () {
		await this.getJars();
	},
	methods: {
		getJars: async function () {
			this.allJars = await Budgets.GetAllJars();
		},
		closeJars: async function () {
			this.$parent.openBudget();
		}
	}
});