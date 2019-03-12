<div class="layout-wrapper layout-2">
	<div class="layout-inner">
		<!-- Layout sidenav -->
		<div id="layout-sidenav" class="layout-sidenav sidenav sidenav-vertical bg-dark">

			<!-- Links -->
			<ul class="sidenav-inner py-1">
			
				<li class="sidenav-item">
					<a class="sidenav-link active"><i class="sidenav-icon fa fa-home"></i>
						<div>Home</div>
					</a>
				</li>
			
				<li class="sidenav-item">
					<a class="sidenav-link"><i class="sidenav-icon fa fa-chart-pie"></i>
						<div>Budget</div>
					</a>
				</li>
				
				<li class="sidenav-item">
					<a class="sidenav-link"><i class="sidenav-icon fa fa-chart-line"></i>
						<div>Reports</div>
					</a>
				</li>
				
				<li class="sidenav-divider mb-1"></li>
			
				<li class="sidenav-header small font-weight-semibold pl-3">
					<div class="float-left">Accounts</div>
					<div class="pl-1 ml-auto float-right">
						<a class="sidenav-link"><i class="fa fa-cog"></i></a>
					</div>
				</li>
				
				<li v-for="account in accounts" class="sidenav-item" v-if="account.IsOpen">
					<a class="sidenav-link">
						<div>{{account.Name}}</div>
						<div class="pl-1 ml-auto">
						<div :class="{ 'badge': true, 'badge-success': account.LastBalance >= 0, 'badge-danger': account.LastBalance < 0}">{{account.LastBalance | asCurrencyRoundDown}}</div>
						</div>
					</a>
				</li>
			</ul>

			<div class="sidenav-bottom">
				<div class="dropdown show">
						<a class="dropdown-toggle sidenav-text" role="button" id="budgetMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
							<i class="fa fa-cog"></i> Family Budget
						</a>
						
						<div class="dropdown-menu" aria-labelledby="budgetMenuLink">
							<a class="dropdown-item"><i class="fa fa-cog"></i> Settings</a>
							<div class="dropdown-divider"></div>
							<a class="dropdown-item"><i class="fa fa-sign-out-alt"></i> Sign out</a>
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
								<tr v-for="(t,index) in transactions" :class="{ 'odd': index % 2 !== 0, 'even': index % 2 === 0}">
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