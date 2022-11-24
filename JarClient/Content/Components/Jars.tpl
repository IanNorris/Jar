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
        <form class="modal-content" id="new-jar-form">
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
                <input name="jarName" type="text" class="form-control">
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col">
                <label class="form-label">Category</label>
                <select name="jarCategory" class="form-control select2"></select>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col">
                <label class="form-label">Jar type</label>
                <select name="jarType" class="form-control">
                    <option v-for="(type,index) in jarTypes" :key="type.Value" v-bind:value="type.Value">{{type.Name}}</option>
                </select>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col mb-0">
                <label class="form-label">Target date</label>
                <datepicker class="form-control jar-target-field" v-on:daterange-changed="newJarTargetDateChanged" v-bind:startDate="newJarTargetStart" v-bind:endDate="newJarTargetEnd" v-bind:singleDate="true" v-bind:future="true" v-bind:allowNone="true" />
                </div>
                <div class="form-group col mb-0">
                <label class="form-label">Target amount</label>
                <input name="targetAmount" type="text" class="form-control jar-target-field" placeholder="Amount to save">
                </div>
            </div>
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            <button type="submit" class="btn btn-primary">Add</button>
            </div>
        </form>
        </div>
    </div>
    </div>
</div>
