"use strict";

var callbackIndex = 0;
var callbackSink = new Map();

var callCallback = function (callbackIndex, returnPayload) {
	var functionToCall = callbackSink[callbackIndex];
	callbackSink.delete(callbackIndex);
	functionToCall(returnPayload);
};

var sendEvent = function (target, targetFunction, payload, callbackFunction) {

	var newCallbackIndex = callbackFunction ? ++callbackIndex : 0;
	if (callbackFunction) {
		callbackSink[newCallbackIndex] = callbackFunction;
	}

	window.chrome.webview.postMessage({
		"Target": target,
		"Function": targetFunction,
		"Payload": JSON.stringify(payload),
		"Callback": newCallbackIndex
	});
}
