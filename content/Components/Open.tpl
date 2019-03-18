<div class="authentication-wrapper authentication-3">
  <div class="authentication-inner">
	<!-- Side container -->
	<!-- Do not display the container on extra small, small and medium screens -->
	<div class="d-none d-lg-flex col-lg-8 align-items-center ui-bg-cover ui-bg-overlay-container p-4"
	style="background-image: url(&#39;Images/Coins.webp&#39;);">
	  <div class="ui-bg-overlay bg-dark opacity-75"></div>
	  <!-- Text -->
	  <div class="w-100 text-white px-5">
		<h1 class="display-2 font-weight-bolder mb-4">START YOUR JARS BUDGET</h1>
		<div class="text-large font-weight-light">With Jars you&#39;ll always know what you have available to spend. Planning a
		trip? Start a Jar. Always wanted to renovate the kitchen? Start a Jar. The Jars system lets you assign your income to a
		purpose so you&#39;ll always feel in control of your money.</div>
	  </div>
	  <!-- /.Text -->
	</div>
	<!-- / Side container -->
	<!-- Form container -->
	<div class="d-flex col-lg-4 align-items-center bg-white p-5">
	  <!-- Inner container -->
	  <!-- Have to add `.d-flex` to control width via `.col-*` classes -->
	  <div v-if="!newBudgetObj" class="d-flex col-sm-7 col-md-5 col-lg-12 px-0 px-xl-4 mx-auto">
			<div class="w-100">
				<div v-if="settings && settings.Budgets.length">
				<h4 class="text-center font-weight-normal mb-0">Select a budget</h4>
				<!-- Form -->
				<form class="my-5" v-on:submit.prevent="openBudget">
					<div class="mb-2">
					<div class="list-group">
						<a v-for="(budget,index) in settings.Budgets" href="javascript:void(0)"
						class="list-group-item list-group-item-action flex-column align-items-start"
						v-bind:class="{ 'active': index == selectedBudget }" v-on:click="selectBudget( index )">
						<div class="d-flex justify-content-between w-100">
							<h5 class="mb-1">{{budget.Name}}</h5>
							<small>{{budget.LastAccessed | asDateAgo}}</small>
						</div>
						</a>
					</div>
					</div>
					<div class="text-center text-muted mb-3">
					<a href="javascript:void(0)" v-on:click="openExistingBudget" style="text-align: center">Open a budget</a>
					</div>
					<div>
					<div class="form-group">
						<div>
						<label class="form-label d-flex justify-content-between align-items-end">Password</label>
						</div>
						<input type="password" ref="password" class="form-control" v-model="password" />
					</div>
					<div class="d-flex justify-content-between align-items-center m-0">
					<label class="custom-control custom-checkbox m-0">
					<input type="checkbox" class="custom-control-input" /> 
					<span class="custom-control-label">Remember password</span></label> 
					<button type="submit" class="btn btn-primary">Open</button></div>
					</div>
				</form>
				<!-- / Form -->
				<div class="text-center text-muted">Don&#39;t have a budget yet? 
				<a href="javascript:void(0)" v-on:click="getNewBudgetPath">Create one</a></div>
				</div>
				<div v-if="settings &amp;&amp; settings.Budgets.length == 0">
				<h4 class="text-center font-weight-normal mb-5">New here?</h4>
				<div class="text-center text-muted">
					<a href="javascript:void(0)" v-on:click="getNewBudgetPath">Create a new budget</a>
				</div>
				<div class="text-center text-muted mt-3 mb-2">
					<p>or</p>
				</div>
				<div class="text-center text-muted">
					<a href="javascript:void(0)" style="text-align: center">Open an existing budget</a>
				</div>
		  </div>
		</div>
		</div>
		
		<div v-if="newBudgetObj" class="d-flex col-sm-7 col-md-5 col-lg-12 px-0 px-xl-4 mx-auto">
			<div class="w-100">
				<div v-if="settings && settings.Budgets.length">
				<h4 class="text-center font-weight-normal mb-0">Create a new budget</h4>
				<!-- Form -->
				<form class="my-5" v-on:submit.prevent="createNewBudget">
					<div class="mb-2">
					<h4 style="text-align: center">{{newBudgetObj.Name}}</h4>
					</div>
					<div>
					<div class="form-group">
						<div>
						<label class="form-label d-flex justify-content-between align-items-end">Password</label>
						</div>
						<input type="password" ref="password" class="form-control" v-model="password" />
					</div>
					<div class="form-group">
						<div>
						<label class="form-label d-flex justify-content-between align-items-end">Re-type Password</label>
						</div>
						<input type="password" ref="password2" class="form-control" v-model="password2" />
					</div>
					<div class="d-flex justify-content-between align-items-center m-0">
					<div></div> 
					<button type="submit" class="btn btn-primary" v-bind:disabled="password != password2">Create</button></div>
					</div>
				</form>
				<div class="text-center text-muted mt-3">
					<a href="javascript:void(0)" style="text-align: center" v-on:click="cancelNew">Nevermind...</a>
				</div>
		  </div>
		</div>
	  </div>
	</div>
	<!-- / Form container -->
  </div>
</div>