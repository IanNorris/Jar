<div class="layout-wrapper layout-2">
	<div class="layout-inner flexbox-height-fix">
		<div id="layout-sidenav" class="layout-sidenav sidenav sidenav-vertical sidenav-no-animation bg-primary-dark sidenav-dark">

			<div class="app-brand demo">
				<span class="app-brand-logo">
					<img src="ToReplace/savings.svg" />
				</span>
				<a class="app-brand-text demo sidenav-text font-weight-normal ml-2">Jar</a>
			</div>

			<div class="sidenav-divider mt-0"></div>

			<ul class="sidenav-inner py-1">
				<li class="sidenav-item" v-on:click="openHome" v-bind:class="{ active: activePage == MainPage_Home }">
					<a class="sidenav-link"><i class="sidenav-icon fas fa-home"></i> Home</a>
				</li>
				<li class="sidenav-item" v-on:click="openBudget" v-bind:class="{ active: activePage == MainPage_Budgets }">
					<a class="sidenav-link"><i class="sidenav-icon fas fa-piggy-bank"></i> Budget</a>
				</li>
				<li class="sidenav-item" v-on:click="openReports" v-bind:class="{ active: activePage == MainPage_Reports }">
					<a class="sidenav-link"><i class="sidenav-icon fas fa-briefcase"></i> Reports</a>
				</li>
				<li class="sidenav-header small font-weight-semibold pl-3">
					<div class="float-left">Accounts</div>
					<div class="pl-1 ml-auto float-right">
						<a class="sidenav-link" v-on:click="openAccounts" v-bind:class="{ active: activePage == MainPage_Accounts }"><i class="fa fa-cog"></i></a>
					</div>
				</li>
				<li v-for="(account,index) in accounts" class="sidenav-item" v-if="account.IsOpen" v-bind:class="{ active: selectedAccount == accounts[index] }">
					<a class="sidenav-link" v-on:click="selectAccount(index)">
						<div>{{account.Name}}</div>
						<div class="pl-1 ml-auto">
							<div v-bind:class="{ 'font-positive-value': account.LastBalance >= 0, 'font-negative-value': account.LastBalance < 0 }">{{account.LastBalance | asCurrencyRoundDown}}</div>
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

			<split-resize classes="sidenav-drag" variable="--sidenav-width" :initial-value="sideNavWidth" :dragComplete="dragComplete" />
		</div>
		<div class="layout-container" v-if="selectedAccount">
			<div class="fill-vertical-space-wrapper">
				<div id="layout-navbar" class="navbar navbar-expand-lg align-items-lg-center bg-white">
					<div class="navbar-nav align-items-lg">
						<h2>{{selectedAccount.Name}}</h2>
					</div>
					<div class="navbar-nav align-items-lg ml-auto">
						<datepicker v-on:daterange-changed="dateRangeChanged" v-bind:startDate="dateRangeStart" v-bind:endDate="dateRangeEnd" />
					</div>
				</div>
				<div class="table-responsive fill-vertical-space-inner">
					<table class="table table-striped table-bordered fixed-header-table">
						<thead>
							<tr>
								<th class="table-header-status">Status</th>
								<th class="table-header-date">Date</th>
								<th class="table-header-payee">Payee</th>
								<th class="table-header-memo">Memo</th>
								<th class="table-header-amount table-cell-align-right">Amount</th>
								<th class="table-header-balance table-cell-align-right">Balance</th>
							</tr>
						</thead>
						<tbody>
							<tr v-for="(t,index) in transactions">
								<td>
									<i v-bind:class="{ 'fas fa-check-circle font-success': t.IsAccepted, 'far fa-question-circle': !t.IsAccepted }"></i>
									&nbsp;<i class="fa-flag" v-bind:class="{ 'far': t.Flag == 0, 'fas flag-color-1': t.Flag == 1, 'fas flag-color-2': t.Flag == 2, 'fas flag-color-3': t.Flag == 3, 'fas flag-color-4': t.Flag == 4, 'fas flag-color-5': t.Flag == 5 }"></i>
								</td>
								<td>{{t.Date | asDate}}</td>
								<td v-bind:title="t.OriginalPayee">{{t.Payee}} <span class="text-muted">{{t.Reference}}</span></td>
								<td>{{t.Memo}}</td>
								<td class="table-cell-align-right" v-bind:class="{ 'font-positive-value': t.Amount >= 0, 'font-negative-value': t.Amount < 0 }">{{t.Amount | asNumeric}}</td>
								<td class="table-cell-align-right">{{0 | asNumeric}}</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div class="fill-empty"></div>
				<div id="layout-footer" class="footer bg-white navbar navbar-expand-lg" v-if="selectedAccount">
					<div class="align-items-lg">
					</div>
					<div class="align-items-lg ml-auto">
						<h2>{{selectedAccount.LastBalance | asCurrency}}</h2>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
