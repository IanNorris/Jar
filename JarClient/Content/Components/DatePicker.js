Vue.component('datepicker', {
	template: '#DatePickerTemplate',
	props: ['startDate', 'endDate', 'singleDate', 'future', 'allowNone'],
	mounted: function () {
		var element = this.$refs.button;
		var start = this.startDate;
		var end = this.endDate;
		var singleDate = this.singleDate;
		var future = this.future;
		var allowNone = this.allowNone;

		var self = this;

		function rangeChangedCallback(start, end) {
			if (singleDate) {
				var newDateString = start.format('MMMM D, YYYY');
				if (newDateString == moment('1900-01-01').format('MMMM D, YYYY')) {
					$(element).html('None');
					$(element).attr('value', 'None');
				}
				else {
					$(element).html(newDateString);
					$(element).attr('value', newDateString);
				}
			}
			else {
				let newValue = start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY');
				$(element).html(newValue);
				$(element).attr('value', newValue);
			}

			self.$emit('daterange-changed', start, end);
		}

		let ranges = future ? {
			'None': [moment('1900-01-01'), moment()],
			'Next month': [moment().add(1, 'months'), moment()],
			'Two months': [moment().add(2, 'months'), moment()],
			'Three months': [moment().add(3, 'months'), moment()],
			'Four months': [moment().add(4, 'months'), moment()],
			'Six months': [moment().add(6, 'months'), moment()],
			'One year': [moment().add(1, 'year'), moment()],
			'Two years': [moment().add(2, 'year'), moment()],
			'Five years': [moment().add(5, 'year'), moment()],
		} : {
			'None': [moment('1900-01-01'), moment()],
			'The last 7 days': [moment().subtract(6, 'days'), moment()],
			'The last 30 days': [moment().subtract(29, 'days').startOf('day'), moment()],
			'The last 3 months': [moment().subtract(3, 'months').startOf('month'), moment()],
			'The last 6 months': [moment().subtract(6, 'months').startOf('month'), moment()],
			'The last year': [moment().subtract(1, 'years').startOf('year'), moment()],
			'The last two years': [moment().subtract(2, 'years').startOf('year'), moment()],
			'This calendar month': [moment().startOf('month'), moment().endOf('month')],
			'Last calendar month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
			'Everything': [moment('1900-01-01'), moment()]
		};

		if (!allowNone) {
			delete ranges["None"];
		}
		
		$(element).daterangepicker({
			showDropdowns: true,
			showCustomRangeLabel: !singleDate,
			singleDatePicker: singleDate,
			startDate: start,
			endDate: end,
			ranges: ranges,
			opens: 'left',
			linkedCalendars: false,
		}, rangeChangedCallback);

		rangeChangedCallback(start, end);
	},
	methods: {

	}
});