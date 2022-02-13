"use strict";

let hasInitialized = false;
let globalApp = null;

let onLoaded = function () {
	let app = new Vue({
		el: '#app',
		data: {
			showOpen: true,
			showBudget: false,
			showLoader: false,
		}
	});

	globalApp = app;
};

let waitingFor = 2;
let onReadyEvent = function () {
	if (--waitingFor == 0) {
		onLoaded();
	}
};

let entryPoint = async function () {
	if (hasInitialized) {
		return;
	}
	hasInitialized = true;

	moment.locale(navigator.language);

	let result = await Accounts.MyMagicFunction("abc", "123");
	console.log(result);


	onReadyEvent();
}

$('body').on("jarTemplatesLoaded", function (event) {
	onReadyEvent();
});

if (Accounts) {
	entryPoint();
}