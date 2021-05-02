<div class="layout-wrapper layout-2">
	<div class="layout-inner flexbox-height-fix">
		<!-- Layout sidenav -->
		<div id="layout-sidenav" class="layout-sidenav sidenav sidenav-vertical bg-primary-dark sidenav-dark">
			<!-- Links -->
			<ul class="sidenav-inner py-1">
				<li class="sidenav-item">
					<a class="sidenav-link active" v-on:click="openHome">
						<div>Home</div>
					</a>
				</li>
				<li class="sidenav-item">
					<a class="sidenav-link" v-on:click="openBudget">
						<div>Budget</div>
					</a>
				</li>
				<li class="sidenav-item">
					<a class="sidenav-link" v-on:click="openReports">
						<div>Reports</div>
					</a>
				</li>
				<li class="sidenav-header small font-weight-semibold pl-3">
					<div class="float-left">Accounts</div>
					<div class="pl-1 ml-auto float-right">
						<a class="sidenav-link" v-on:click="openAccounts"><i class="fa fa-cog"></i></a>
					</div>
				</li>
				<li v-for="(account,index) in accounts" class="sidenav-item" v-if="account.IsOpen">
					<a class="sidenav-link" v-on:click="selectAccount(index)">
						<div>{{account.Name}}</div>
						<div class="pl-1 ml-auto">
							<div>{{account.LastBalance | asCurrency}}</div>
						</div>
					</a>
				</li>
			</ul>
			<div class="sidenav-bottom">
				<div class="dropdown show">
					<a class="dropdown-toggle sidenav-text" role="button" id="budgetMenuLink" data-toggle="dropdown" aria-haspopup="true"
					   aria-expanded="false">{{budgetName}}</a>
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
		<div class="layout-container" v-if="selectedAccount">
			<div class="fill-vertical-space-wrapper">
				<div id="layout-navbar" class="navbar bg-white">
					<h2>{{selectedAccount.Name}}</h2>
				</div>
				<div class="table-responsive fill-vertical-space-inner">
					<table class="table table-striped table-bordered full-height fixed-header-table">
						<thead>
							<tr>
								<th>Date</th>
								<th>Payee</th>
								<th>Memo</th>
								<th>Amount</th>
								<th>Balance</th>
							</tr>
						</thead>
						<tbody>
							<tr v-for="(t,index) in transactions">
								<td>{{t.Date | asDate}}</td>
								<td v-bind:title="t.OriginalPayee">{{t.Payee}} <span class="text-muted">{{t.Reference}}</span></td>
								<td>{{t.Memo}}</td>
								<td>{{t.Amount | asCurrency}}</td>
								<td>{{0 | asCurrency}}</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div class="fill-empty"></div>
				<div id="layout-footer" class="footer bg-white" v-if="selectedAccount">
					<p>Account balance: {{selectedAccount.LastBalance | asCurrency}}</p>
				</div>
			</div>
			<!-- / Content -->
		</div>
		<!-- Layout content -->
	</div>
	<!-- / Layout container -->
</div>
<!-- / Layout wrapper -->