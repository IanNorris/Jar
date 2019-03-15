let globalDataModel = null;
let globalApp = null;

let onLoaded = function(){
	let app = new Vue({
		el: '#app',
		data: {
			showOpen: true,
			showBudget: false,
		}
	});
	
	globalApp = app;
};

let waitingFor = 2;
let onReadyEvent = function(){
	if( --waitingFor == 0 ) {
		onLoaded();
	}
};

(async function() {
	await CefSharp.BindObjectAsync("dataModel", "dataModel");

	globalDataModel = await dataModel;
	
	onReadyEvent();
})();

$('body').on( "jarTemplatesLoaded", function( event ) {
	onReadyEvent();
} );
