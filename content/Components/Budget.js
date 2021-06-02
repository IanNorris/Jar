"use strict";

Vue.component('jar-budget', {
	template: '#BudgetTemplate',
	data: function () {
		return {
			earliestBudgetMonth: '2019-03-03',
			lastBudgetMonth: '2020-09-03',
			activeMonth: '2020-07-01',
			activeYear: moment().year(),
		};
	},
	created: async function () {
		
	},
	methods: {
		getShortMonth: function (month) {
			return month.format('MMM YYYY');
		},
		selectActiveYear: function (activeYear) {
			this.activeYear = activeYear;
			var availableMonths = this.months;

			this.activeMonth = null;
			if (availableMonths.length != 0) {
				for (var index = availableMonths.length - 1; index >= 0; index--) {
					if (availableMonths[index].enabled) {
						this.activeMonth = availableMonths[index].month.format('YYYY-MM-DD');
						break;
					}
				}
			}
			
		},
		selectActiveMonth: function (activeMonth) {
			if (activeMonth.enabled) {
				this.activeMonth = activeMonth.month.format('YYYY-MM-DD');
			}
		}
	},
	computed: {
		months: function () {
			var monthsOut = [];
			var thisMonth = moment(`${this.activeYear+1}-12-31 00:00`).startOf('year').subtract(12, 'months');
			for (var months = 0; months < 12; months++) {
				monthsOut.push({
					month: moment(thisMonth),
					shortMonth: this.getShortMonth(thisMonth),
					enabled: thisMonth.isBetween(moment(this.earliestBudgetMonth).startOf('month'), moment(this.lastBudgetMonth).startOf('month'), undefined, '[]'),
					active: thisMonth.isSame(moment(this.activeMonth), 'month')
				});
				thisMonth = thisMonth.add(1, 'months');
			}

			return monthsOut;
		},
		years: function () {
			var yearsOut = [];
			var year = moment(this.earliestBudgetMonth).startOf('year');
			var endYear = moment(this.lastBudgetMonth).startOf('year');
			while (year.isSameOrBefore(endYear)) {
				yearsOut.push(year.year());
				year = year.add(1, 'year');
			}

			return yearsOut;
		}
	},
	filters: {
		
	}
});