"use strict";

Vue.component('jar-jars', {
	template: '#JarsTemplate',
	data: function () {
		return {
			allJars: [],
			jarTypes: []
		};
	},
	created: async function () {
		await this.getJars();
		await this.getJarTypes();
	},
	mounted: function () {
		let self = this;
		Vue.vueDragula.eventBus.$on('drop', function (args) {
			if (args[0] == "jar-list") {
				self.onJarReorder();
			}
		});
	},
	methods: {
		getJars: async function () {
			this.allJars = await Jars.GetAllJars();
		},
		getJarTypes: async function () {
			this.jarTypes = await Jars.GetJarTypes();
		},
		closeJars: async function () {
			this.$parent.openBudget();
		},
		onJarReorder: async function () {
			await Budgets.OnJarReorder(this.allJars);
		}
	}
});