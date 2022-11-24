﻿"use strict";

Vue.component('jar-jars', {
	template: '#JarsTemplate',
	data: function () {
		return {
			allJars: [],
			jarTypes: [],
			newJarTargetStart: moment('1900-01-01'),
			newJarTargetEnd: moment(),

			newJarTargetAmount: 0.0
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

				return { id: newTerm + '_PLACEHOLDER', text: newTerm };
			}
		}).change(function () {
			
		});

		$('#new-jar-form').validate({
			focusInvalid: true,
			rules: {
				'jarName': {
					required: true
				},
				'jarType': {
					required: true
				}
			},

			submitHandler: function (form) {

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
				var $parent = $el.parents('.form-group');

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
	},
	methods: {
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
				let totalAmount = 12 * this.newJarTargetAmount;
				return `Saving ${this.newJarTargetAmount} a month, which is ${totalAmount} a year.`;
			}
			else {
				var dateMonths = this.newJarTargetStart.diff(now, 'months');
				let totalAmount = dateMonths * this.newJarTargetAmount;
				return `Saving ${this.newJarTargetAmount} a month, which is ${totalAmount} by ${dateDescription}.`;
			}
		}
	}
});