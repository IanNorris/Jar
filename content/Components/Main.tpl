<div class="layout-wrapper layout-2">
  <div class="layout-inner">
	<!-- Layout sidenav -->
	<div id="layout-sidenav" class="layout-sidenav sidenav sidenav-vertical bg-dark">
	  <!-- Links -->
	  <ul class="sidenav-inner py-1">
		<li class="sidenav-item">
		  <a class="sidenav-link active">
			<div>Home</div>
		  </a>
		</li>
		<li class="sidenav-item">
		  <a class="sidenav-link">
			<div>Budget</div>
		  </a>
		</li>
		<li class="sidenav-item">
		  <a class="sidenav-link">
			<div>Reports</div>
		  </a>
		</li>
		<li class="sidenav-header small font-weight-semibold pl-3">
		  <div class="float-left">Accounts</div>
		  <div class="pl-1 ml-auto float-right">
			<a class="sidenav-link"></a>
		  </div>
		</li>
		<li v-for="account in accounts" class="sidenav-item" v-if="account.IsOpen">
		  <a class="sidenav-link">
			<div>{{account.Name}}</div>
			<div class="pl-1 ml-auto">
			  <div>{{account.LastBalance | asCurrencyRoundDown}}</div>
			</div>
		  </a>
		</li>
	  </ul>
	  <div class="sidenav-bottom">
		<div class="dropdown show">
		  <a class="dropdown-toggle sidenav-text" role="button" id="budgetMenuLink" data-toggle="dropdown" aria-haspopup="true"
		  aria-expanded="false">Family Budget</a>
		  <div class="dropdown-menu" aria-labelledby="budgetMenuLink">
			<a class="dropdown-item">Settings</a>
			<div class="dropdown-divider"></div>
			<a class="dropdown-item" v-on:click="signOut"> Sign out</a>
		  </div>
		</div>
	  </div>
	</div>
	<!-- / Layout sidenav -->
	<!-- Layout container -->
	<div class="layout-container">
	  <!-- Layout content -->
	  <div class="layout-content">
		<!-- Content -->
		<div class="container-fluid flex-grow-1 container-p-y">
		  <div>
			<h1>Account 1</h1>
			<p>Account balance: £26,981.73</p>
		  </div>
		  <div>
			<div class="table-responsive">
			  <table class="datatables-demo table table-striped table-bordered">
				<thead>
				  <tr>
					<th>Date</th>
					<th>Payee</th>
					<th>Memo</th>
					<th>Note</th>
					<th>Amount</th>
				  </tr>
				</thead>
				<tbody>
				  <tr v-for="(t,index) in transactions">
					<td>{{t.Date | asDate}}</td>
					<td>{{t.Payee}}</td>
					<td>{{t.Memo}}</td>
					<td>{{t.Note}}</td>
					<td>{{t.Amount | asCurrency}}</td>
				  </tr>
				</tbody>
			  </table>
			</div>
		  </div>
		</div>
	  </div>
	  <!-- / Content -->
	</div>
	<!-- Layout content -->
  </div>
  <!-- / Layout container -->
</div>
<!-- / Layout wrapper -->