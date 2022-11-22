<div class="layout-container">

    <div id="layout-navbar" class="navbar navbar-expand-lg align-items-lg-center bg-white">
        <div class="navbar-nav align-items-lg">
            <h2>Jars</h2>
        </div>
        <div class="navbar-nav align-items-lg ml-auto">
            
        </div>

        <div class="navbar-nav align-items-lg ml-2">
            <button type="button" class="btn btn-outline-primary rounded-pill" v-on:click="closeJars">Close</button>
        </div>
    </div>

    <div class="row ml-1">

        <div class="col-4 p-3">
            <button type="button" class="btn btn-primary m-4" data-toggle="modal" data-target="#modal-new-jar">Show</button>

            <div v-dragula="allJars" bag="jar-list">
                <div class="card mb-2" v-for="(jar,index) in allJars" :key="jar.Id">
                    <div class="card-header">
                        {{jar.Name}} - {{jar.Type}}
                    </div>
                </div>
            </div>
        </div>

        <div class="col-8 p-3">
            <p>Here you can configure your Jars! Jars are just containers for tracking your money. No money is actually moved, it just allows you to track your spending and saving targets.</p>

            <p>When you make a transaction, it is assigned to a jar. Depending on the jar type you either need to pre-allocate the funds for it, or the jar value will be automatically calculated based on the transactions in it (and will remain zero).</p>

            <p>Jars uses a form of double-entry bookkeeping. This means every expense must be covered by a credit from somewhere else. Your Jars are filled from top to bottom. Your most important Jars should be at the top,
            with the jars becoming decreasingly important as you go down. The last bucket will receive whatever money is left over.</p>

            <table class="table table-borderless">
                <tbody>
                    <tr>
                        <th>Income</th>
                        <td>Any source of income, such as wages, side-hustle income, rental income etc.<br/>
                        You can create more than one source of income if you want them to be displayed separately, such as if you want your and your partner's income to not be combined when showing a breakdown of your budget.</td>
                    </tr>

                    <tr>
                        <th>Transaction</th>
                        <td>Any payments that should be tracked separately, but where you are not adhering to a budget. This might be your mortgage or utility bills. 
                        You aren't trying to apply a budget to this spending, but you do want to track it. You can set an estimate if you know it, or one will be calculated for you. The value of the Jar will always be zero
                        as your jar value is calculated to exactly match the transactions in it.</td>
                    </tr>

                    <tr>
                        <th>Budget</th>
                        <td>Budgets are used when you want to keep a limit on spending in an area.
                        This might be for indulgences, hobbies and other activities that you might spend too much on.
                        For example if you like video games, this will allow you to decide if you can afford a new game this month.
                        The value of Budgets can roll over from month-to-month.</td>
                    </tr>

                    <tr>
                        <th>Buffer</th>
                        <td>A buffer allows you to prepare for large spends that you know are coming, but happen regularly. For example saving up for your car insurance if you pay it annually. You can either set a monthly amount, or set a target date and repeating cycle and target value and let Jars calculate the rest.
                        Buffers will warn you if you're not going to hit your target. A buffer could also just contain a target amount and be used as a "rainy day" fund for unexpected expenses.</td>
                    </tr>

                    <tr>
                        <th>Goal</th>
                        <td>Goals are things you are saving for, such as holidays, rennovations, new computers etc. They typically last multiple months or years and are closed when completed. If a target amount is specified, Jars will inform you when you might hit the target based on your deposits so far.</td>
                    </tr>

                    <tr>
                        <th>Savings</th>
                        <td>Savings are very long term jars for your money to grow in. This might be for a retirement etc.</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div class="modal fade" id="modal-new-jar">
        <div class="modal-dialog">
        <form class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title">
                New Jar
                <br>
                <small class="text-muted">Add a new Jar to classify your spending or income.</small>
            </h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">×</button>
            </div>
            <div class="modal-body">
            <div class="form-row">
                <div class="form-group col">
                <label class="form-label">Jar name</label>
                <input type="text" class="form-control">
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col">
                <label class="form-label">Jar type</label>
                <select class="form-control">
                    <option>Income</option>
                    <option>Transaction</option>
                    <option>Budget</option>
                    <option>Buffer</option>
                    <option>Goal</option>
                    <option>Savings</option>
                </input>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col mb-0">
                <label class="form-label">Target date</label>
                <input type="text" class="form-control" placeholder="DD / MM">
                </div>
                <div class="form-group col mb-0">
                <label class="form-label">Target amount</label>
                <input type="text" class="form-control" placeholder="Amount to save">
                </div>
            </div>
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary">Add</button>
            </div>
        </form>
        </div>
    </div>
    </div>
</div>
