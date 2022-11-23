"use strict";

Vue.component('jar-jars', {
	template: '#JarsTemplate',
	data: function () {
		return {
			allJars: [],
			jarTypes: [],
			newJarTargetStart: moment('1900-01-01'),
			newJarTargetEnd: moment(),
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
		newJarTargetDateChanged: async function () {
			
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
	}
});