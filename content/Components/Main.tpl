<div class="layout-wrapper layout-2">
  <div class="layout-inner">
	<!-- Layout sidenav -->
	<div id="layout-sidenav" class="layout-sidenav sidenav sidenav-vertical bg-success-darker sidenav-dark">
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
	<div class="layout-container">
	  <!-- Layout content -->
	  <div class="layout-content">
		<!-- Content -->
		<div v-if="selectedAccount" class="container-fluid flex-grow-1 container-p-y">
		  <div>
			<h1>{{selectedAccount.Name}}</h1>
			<p>Account balance: {{selectedAccount.LastBalance | asCurrency}}</p>
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
		<div v-if="!selectedAccount && showHome">
			<h1>Home</h1>
		</div>
		<div v-if="!selectedAccount && showBudget">
				<h1>Budget</h1>
		</div>
		<div v-if="!selectedAccount && showReports">
				<h1>Reports</h1>
		</div>
		<div v-if="showAccounts" class="container-fluid flex-grow-1 container-p-y">
				<h4 class="font-weight-bold py-3 mb-4">
					Tasks
				</h4>

				<div class="row">
					<div class="col">

				<div class="card">
						<div class="card-header py-3">
							<button type="button" class="btn btn-primary"><i class="ion ion-md-add"></i>&nbsp; Add task</button>&nbsp;
							<button type="button" class="btn btn-default md-btn-flat"><i class="ion ion-md-close"></i>&nbsp; Clear</button>
						</div>
						<div class="card-body">
							<p class="text-muted small">Today</p>
							<div class="task-list custom-controls-stacked">

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Buy products</span>
										<span class="badge badge-warning font-weight-normal ml-2">Other</span>
										<span class="ui-todo-badge badge badge-outline-default font-weight-normal ml-2">58 mins left</span>
									</label>
									<div class="d-flex align-items-center float-right">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Reply to emails</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Write blog post</span>
										<span class="ui-todo-badge badge badge-outline-default font-weight-normal ml-2">20 hours left</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input" checked>
										<span class="custom-control-label">Wash my car</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

							</div>
						</div>
						<hr class="m-0">
						<div class="card-body">
							<p class="text-muted small">Tomorrow</p>
							<div class="task-list custom-controls-stacked">

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Buy antivirus</span>
										<span class="badge badge-warning font-weight-normal ml-2">Other</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Jane's Happy Birthday</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Call mom</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

							</div>
						</div>
						<hr class="m-0">
						<div class="card-body">
							<p class="text-muted small">Next week</p>
							<div class="task-list custom-controls-stacked">

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">New blog layout</span>
										<span class="badge badge-success font-weight-normal ml-2">Clients</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Create UI design model</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">New icons set for an iOS app</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Create ad campaign banners set</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Edit the draft for the icons</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Fix validation issues and commit</span>
										<span class="badge badge-danger font-weight-normal ml-2">Important</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Support tickets list doesn't support commas</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<div class="task-list-item">
									<label class="ui-todo-item custom-control custom-checkbox">
										<input type="checkbox" class="custom-control-input">
										<span class="custom-control-label">Help Web devs with HTML integration</span>
									</label>
									<div class="d-flex align-items-center">
										<div class="task-list-handle ion ion-ios-move text-lightest small mr-3"></div>
										<div class="task-list-actions btn-group">
											<button type="button" class="btn btn-default btn-xs btn-round icon-btn borderless md-btn-flat hide-arrow dropdown-toggle" data-toggle="dropdown"><i class="ion ion-ios-more"></i></button>
											<div class="dropdown-menu dropdown-menu-right">
												<a class="dropdown-item" href="javascript:void(0)">Edit</a>
												<a class="dropdown-item" href="javascript:void(0)">Remove</a>
											</div>
										</div>
									</div>
								</div>

								<!-- TODO: REMOVE! -->
								<!-- <script>
									dragula(Array.prototype.slice.call(document.querySelectorAll('.task-list')), {
										moves: function (el, container, handle) {
											return handle.classList.contains('task-list-handle');
										}
									});
								</script>-->
							</div>
						</div>
					</div>

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