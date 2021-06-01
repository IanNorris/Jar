<div class="layout-container">

    <div id="layout-navbar" class="navbar navbar-expand-lg align-items-lg-center bg-white">
        <div class="navbar-nav align-items-lg">
            <h2>Budget</h2>
        </div>

        <div class="navbar-nav align-items-lg ml-auto">
            <button type="button" class="btn btn-outline-primary rounded-pill">Configure Jars</button>
        </div>
    </div>

    <div class="btn-group month-bar">
        <button type="button" class="btn btn-default" v-for="(month,index) in months" v-bind:class="{ disabled: !month.enabled, active: month.active }">{{month.shortMonth}}</button>
    </div>    

</div>