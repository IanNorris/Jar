"use strict";

let hasInitialized = false;
let globalApp = null;

let onLoaded = async function () {
	moment.locale(navigator.language);

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
let onReadyEvent = async function () {
	if (--waitingFor == 0) {
		await onLoaded();
	}
};

$('body').on("jarTemplatesLoaded", async function (event) {
	await onReadyEvent();

	//Tell the backend to inject the API bindings
	//This will also call onReadyEvent as well.
	window.chrome.webview.postMessage({
		Target: "",
		Function: "InjectBindings",
		Payload: "{ CallbackName: \"onReadyEvent\" }",
		Callback: 0
	});
});