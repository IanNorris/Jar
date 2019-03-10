(async function() {
	await CefSharp.BindObjectAsync("data", "dataModel");
	
	let newData = {
		trans: await data.getTransactionsBetweenDates(moment('2018/04/01', 'YYYY/MM/DD').toDate(), moment('2018/04/30', 'YYYY/MM/DD').toDate() ),
		accounts: await data.getAccounts()
	};
		
	let app = new Vue({
		el: '#app',
		data: newData,
		filters: {
			asDate: function(date) {
				return moment(date).format('L');
			},
			asCurrency: function(amount) {
				return '£' + (amount / 100.0);
			},
			asCurrencyRoundDown: function(amount) {
				return '£' + Math.floor((amount / 100.0));
			}
		}
	});
	
	/*let transactions = await data.getTransactions();
	console.log(transactions);
	for( let t = 0; t < transactions.length; t++ ) {
		console.log(transactions[t]);
		let marker = document.getElementById('mark');
		marker.insertAdjacentHTML('afterend', '<p>' + transactions[t].Payee + '-' + transactions[t].Memo + '</p>');
	}*/
})();