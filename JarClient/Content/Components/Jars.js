"use strict";

Vue.component('jar-jars', {
	template: '#JarsTemplate',
	data: function () {
		return {
			allJars: [],
			jarTypes: [],
			newJarName: '',
			newJarTargetStart: moment('1900-01-01'),
			newJarTargetEnd: moment(),
			newJarType: 2,
			newJarTargetAmount: 0,
			newJarMaxAmount: 0,
			newJarCarryOver: false,
			newJarFlagTransactionCount: false,
			newJarFlagTotalAmount: false,
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

		$('.select2').select2({
			allowClear: true,
			tags: true,
			dropdownParent: $('#modal-new-jar'),
			placeholder: 'Select a caterory...',
			ajax: {
				transport: async function (params, success, failure) {
					let categories = await Jars.GetCategories();
					success({ results: categories });
				}
			},
			createTag: function (params) {
				var newTerm = $.trim(params.term);

				if (newTerm === '') {
					return null;
				}

				return { id: newTerm, text: newTerm };
			}
		}).change(function () { });

		$('#new-jar-form').validate({
			focusInvalid: false,
			rules: {
				'jarName': {
					required: true
				},
				'jarType': {
					required: true
				}
			},

			submitHandler: async function (form) {
				//Do nothing, we handle this elsewhere
				//but we don't want it actually submitting the form.
			},

			errorPlacement: function errorPlacement(error, element) {
				var $parent = $(element).parents('.form-group');

				// Do not duplicate errors
				if ($parent.find('.jquery-validation-error').length) { return; }

				$parent.append(
					error.addClass('jquery-validation-error small form-text invalid-feedback')
				);
			},
			highlight: function (element) {
				var $el = $(element);
				$el.addClass('is-invalid');

				// Select2 and Tagsinput
				if ($el.hasClass('select2-hidden-accessible') || $el.attr('data-role') === 'tagsinput') {
					$el.parent().addClass('is-invalid');
				}
			},
			unhighlight: function (element) {
				$(element).parents('.form-group').find('.is-invalid').removeClass('is-invalid');
			}
		});

		// HACK: Opening the daterangepicker is submitting the form, causing the validation to fire
		// and I can't figure out why. So we completely disable the submit path and validate and submit manually...
		// There's no callstack, the event comes from nowhere, and it doesn't happen if you
		// step through either...
		$('#new-jar-form').off('submit').on('submit', e => e.preventDefault() );
	},
	methods: {
		onAddJar: async function () {
			if ($('#new-jar-form').valid()) {
				$('#new-jar-submit').prop("disabled", true);

				let category = $('#jarCategoryInput').val();
				let categoryNumeric = parseInt(category);
				let isCategoryNumberic = !Number.isNaN(categoryNumeric);

				await Jars.CreateNewJar({
					CategoryId: isCategoryNumberic ? categoryNumeric : 0,
					Name: this.newJarName,
					Type: this.newJarType,
					MonthlyValue: parseFloat(this.newJarTargetAmount),
					TargetValue: parseFloat(this.newJarMaxAmount),
					TargetDate: this.newJarTargetStart.toISOString(),
					CarryOver: this.newJarCarryOver,
					FlagTransactionCount: this.newJarFlagTransactionCount,
					FlagTotalAmount: this.newJarFlagTotalAmount
				}, isCategoryNumberic ? '' : category);

				await this.getJars();

				this.newJarName = '';

				setTimeout(() => {
					$('#new-jar-submit').prop("disabled", false);
					$('#modal-new-jar').modal('toggle');
				}, 0);
			}
		},
		onSubmitForm: function () {

		},
		newJarTargetDateChanged: async function (start, end) {
			this.newJarTargetStart = start;
			this.newJarTargetEnd = end;
		},
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
	},
	computed: {
		targetDescription() {
			let now = moment.utc();
			let dateDescription = this.newJarTargetStart.format('MMMM D, YYYY');
			if (dateDescription == moment('1900-01-01').format('MMMM D, YYYY')) {
				if (this.newJarMaxAmount != null && this.newJarMaxAmount != '' && this.newJarMaxAmount > 0 && this.newJarTargetAmount != null && this.newJarTargetAmount != '' && this.newJarTargetAmount > 0) {
					let months = Math.round(this.newJarMaxAmount / this.newJarTargetAmount);
					let newDate = now.add(months, 'month').format('MMMM D, YYYY');
					return `Saving ${this.newJarTargetAmount} a month, target of ${this.newJarMaxAmount} will be hit by ${newDate}.`;
				}
				else {
					let totalAmount = 12 * this.newJarTargetAmount;
					return `Saving ${this.newJarTargetAmount} a month, which is ${totalAmount} a year.`;
				}
				
			}
			else {
				var dateMonths = this.newJarTargetStart.diff(now, 'months');
				let totalAmount = dateMonths * this.newJarTargetAmount;
				return `Saving ${this.newJarTargetAmount} a month, which is ${totalAmount} by ${dateDescription}.`;
			}
		}
	}
});