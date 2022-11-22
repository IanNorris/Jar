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
			this.allJars = await Budgets.GetAllJars();
		},
		closeJars: async function () {
			this.$parent.openBudget();
		},
		onJarReorder: async function () {
			await Budgets.OnJarReorder(this.allJars);
		}
	}
});