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

            <p>When you make a transaction, it needs to be assigned to a jar. When all transactions are assigned, the Jars get filled from top to bottom, with negative Jars being filled first.</p>

            <p>Jars uses a form of double-entry bookkeeping. This means every expense must be covered by a credit from somewhere else. Because your Jars are filled from top to bottom, your most important Jars should be at the top.
            The most aspirational jars should be at the bottom. The last bucket will receive whatever money is left over.</p>

            <table class="table table-borderless">
                <tbody>
                    <tr v-for="(type,index) in jarTypes">
                        <th>{{type.Name}}</th>
                        <td>{{type.Description}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div class="modal fade" id="modal-new-jar" data-backdrop="static">
        <div class="modal-dialog">
        <form class="modal-content" id="new-jar-form" onsubmit="return false;">
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
                    <input name="jarName" v-model="newJarName" type="text" class="form-control">
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col">
                    <label class="form-label">Category</label>
                    <select name="jarCategory" id="jarCategoryInput" class="form-control select2"></select>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col">
                    <label class="form-label">Jar type</label>
                    <select name="jarType" class="form-control" v-model="newJarType">
                        <option v-for="(type,index) in jarTypes" :key="type.Value" v-bind:value="type.Value">{{type.Name}}</option>
                    </select>
                    </div>
                </div>

                <div v-if="newJarType != 1">
                    <div class="form-row">
                        <div class="form-group col mb-0">
                        <label class="form-label">Target date</label>
                        <datepicker class="form-control jar-target-field" v-on:daterange-changed="newJarTargetDateChanged" v-bind:startDate="newJarTargetStart" v-bind:endDate="newJarTargetEnd" v-bind:singleDate="true" v-bind:future="true" v-bind:allowNone="true" />
                        </div>
                    </div>

                    <div class="form-row">
                         <div class="form-group col mb-0">
                        <label class="form-label">Desired monthly amount</label>
                        <input name="targetAmount" type="number" min="0" class="form-control jar-target-field" placeholder="Amount to try to save (or estimated amount) each month" v-model="newJarTargetAmount">
                        </div>
                        <div class="form-group col mb-0">
                        <label class="form-label">Maximum Jar amount</label>
                        <input name="maxAmount" type="number" min="0" class="form-control jar-target-field" placeholder="The max amount this Jar should contain" v-model="newJarMaxAmount">
                        </div>
                    </div>

                    <div class="form-row mt-4">
                        <div class="form-group col mb-0">
                            <i>{{targetDescription}}</i>
                        </div>
                    </div>
                 </div>

                <div class="form-row mt-4">
                    <div class="form-group col mb-0">
                        <label class="custom-control custom-checkbox">
                            <input name="allowCarryOver" type="checkbox" v-model="newJarCarryOver" class="custom-control-input">
                            <span class="custom-control-label">Carry over balance</span>
                        </label>

                        <label class="custom-control custom-checkbox">
                            <input name="flagAmountDeviations" type="checkbox" v-model="newJarFlagTransactionCount" class="custom-control-input">
                            <span class="custom-control-label">Alert me if the amount is unusual</span>
                        </label>

                        <label class="custom-control custom-checkbox">
                            <input name="flagCountDeviations" type="checkbox" v-model="newJarFlagTotalAmount" class="custom-control-input">
                            <span class="custom-control-label">Alert me if the transaction count is unusual</span>
                        </label>
                    </div>
                </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button type="button" id="new-jar-submit" class="btn btn-primary" v-on:click="onAddJar">Add</button>
                </div>
            </form>
        </div>
        </div>
    </div>
</div>
