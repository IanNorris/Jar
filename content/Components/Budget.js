"use strict";

Vue.component('jar-budget', {
	template: '#BudgetTemplate',
	data: function () {
		return {
			earliestBudgetMonth: '2019-03-03 00:00',
			lastBudgetMonth: '2020-09-03 00:00',
			activeMonth: '2020-07-01 00:00',
			year: 2020,
		};
	},
	created: async function () {
		
	},
	methods: {
		getShortMonth: function (month) {
			return month.format('MMM YYYY');
		}
	},
	computed: {
		months: function () {
			var monthsOut = [];
			var thisMonth = moment(`${this.year+1}-12-31 00:00`).startOf('year').subtract(12, 'months');
			for (var months = 0; months < 12; months++) {
				monthsOut.push({
					month: thisMonth,
					shortMonth: this.getShortMonth(thisMonth),
					enabled: thisMonth.isBetween(moment(this.earliestBudgetMonth).startOf('month'), moment(this.lastBudgetMonth).startOf('month'), undefined, '[]'),
					active: thisMonth.isSame(moment(this.activeMonth), 'month')
				});
				thisMonth = thisMonth.add(1, 'months');
			}

			return monthsOut;
		}
	},
	filters: {
		
	}
});