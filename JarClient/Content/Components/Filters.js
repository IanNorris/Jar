Vue.filter("asDate", function (date) {
	return moment(date).format('L');
});

Vue.filter("asCurrency", function (amount) {
	var formatter = Intl.NumberFormat(navigator.language, {
		style: 'currency',
		currency: 'GBP',
		minimumFractionDigits: 2,
		maximumFractionDigits: 2
	});

	return formatter.format(amount / 100.0);
});

Vue.filter("asCurrencyRoundDown", function (amount) {
	var formatter = Intl.NumberFormat(navigator.language, {
		style: 'currency',
		currency: 'GBP',
		minimumFractionDigits: 0,
		maximumFractionDigits: 0
	});

	return formatter.format(Math.floor(amount / 100.0));
});

Vue.filter("asNumeric", function (amount) {
	var formatter = Intl.NumberFormat(navigator.language, {
		minimumFractionDigits: 2,
		maximumFractionDigits: 2
	});

	return formatter.format(amount / 100.0);
});

Vue.filter("asNumericRoundDown", function (amount) {
	var formatter = Intl.NumberFormat(navigator.language, {
		minimumFractionDigits: 0,
		maximumFractionDigits: 0
	});

	return formatter.format(Math.floor(amount / 100.0));
});
