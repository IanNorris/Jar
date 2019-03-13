let globalDataModel = null;

let onLoaded = function(){
	
		
	let app = new Vue({
		el: '#app',
		data: {
			budget: null,
		}
	});
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
