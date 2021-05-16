Vue.component('datepicker', {
	template: '#DatePickerTemplate',
	props: ['startDate', 'endDate'],
	mounted: function () {
		var element = this.$refs.button;
		var start = this.startDate;
		var end = this.endDate;

		var self = this;

		function rangeChangedCallback(start, end) {
			$(element).html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));

			self.$emit('daterange-changed', start, end);
		}

		$(element).daterangepicker({
			startDate: start,
			endDate: end,
			ranges: {
				'The last 7 days': [moment().subtract(6, 'days'), moment()],
				'The last 30 days': [moment().subtract(29, 'days').startOf('day'), moment()],
				'The last 3 months': [moment().subtract(3, 'months').startOf('month'), moment()],
				'The last 6 months': [moment().subtract(6, 'months').startOf('month'), moment()],
				'The last year': [moment().subtract(1, 'years').startOf('year'), moment()],
				'The last two years': [moment().subtract(2, 'years').startOf('year'), moment()],
				'This calendar month': [moment().startOf('month'), moment().endOf('month')],
				'Last calendar month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
				'Everything': [moment('1900-01-01'), moment()]
			},
			opens: 'left',
			linkedCalendars: false,
		}, rangeChangedCallback);

		rangeChangedCallback(start, end);
	},
	methods: {

	}
});