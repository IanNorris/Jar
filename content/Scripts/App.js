"use strict";

let globalDataModel = null;
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

(async function () {
	moment.locale(navigator.language);

	await CefSharp.BindObjectAsync("dataModel");
	await CefSharp.BindObjectAsync("settings");
	await CefSharp.BindObjectAsync("accounts");
	await CefSharp.BindObjectAsync("accountCheckpoints");
	await CefSharp.BindObjectAsync("transactions");
	await CefSharp.BindObjectAsync("budgets");

	globalDataModel = await dataModel;

	onReadyEvent();
})();

$('body').on("jarTemplatesLoaded", function (event) {
	onReadyEvent();
});
