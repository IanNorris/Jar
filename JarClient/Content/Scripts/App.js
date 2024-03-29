"use strict";

let hasInitialized = false;
let globalApp = null;

let onLoaded = async function () {
	moment.locale(navigator.language);

	jQuery.validator.addMethod("notNone", function (value, element) {
		return this.optional(element) || value != "None";
	});

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

$('body').on("jarTemplatesLoaded", async function (event) {
	//Tell the backend to inject the API bindings
	//This will also call onReadyEvent as well.
	window.chrome.webview.postMessage({
		Target: "",
		Function: "InjectBindings",
		Payload: "{ CallbackName: \"onLoaded\" }",
		Callback: 0
	});
});