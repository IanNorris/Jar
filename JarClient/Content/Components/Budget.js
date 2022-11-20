"use strict";

Vue.component('jar-budget', {
	template: '#BudgetTemplate',
	data: function () {
		return {
			earliestBudgetMonth: '2019-03-03',
			lastBudgetMonth: '2020-09-03',
			activeMonth: moment().format('YYYY-MM-DD'),
			activeYear: moment().year(),
			selectedMonthJars: [],
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
				this.getJars();
			}
			else {
				this.selectedMonthJars = [];
			}
		},
		getJars: async function () {
			this.selectedMonthJars = await Budgets.GetDisplayJars(this.activeMonth);
		},
		openJars: function () {
			this.$parent.openJars();
		}
	},
	computed: {
		budgetTitle: function () {
			return moment(this.activeMonth).format('MMMM YYYY');
		},
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
		},
		jarSections: function () {
			var mappings = [
				{ type: 0, name: "Income" },
				{ type: 1, name: "Transactional" },
				{ type: 2, name: "Budgets" },
				{ type: 3, name: "Buffers" },
				{ type: 4, name: "Goals" },
				{ type: 5, name: "Savings" },
			];

			for (var mapping in mappings) {
				mappings[mapping].jars = this.selectedMonthJars.filter(j => j.Jar.Type == mappings[mapping].type);
			}

			return mappings;
		}
	},
	filters: {
		asCurrency: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				style: 'currency',
				currency: 'GBP',
				minimumFractionDigits: 2,
				maximumFractionDigits: 2
			});

			return formatter.format(amount / 100.0);
		},
		asCurrencyRoundDown: function (amount) {
			var formatter = Intl.NumberFormat(navigator.language, {
				style: 'currency',
				currency: 'GBP',
				minimumFractionDigits: 0,
				maximumFractionDigits: 0
			});

			return formatter.format(Math.floor(amount / 100.0));
		},
	}
});