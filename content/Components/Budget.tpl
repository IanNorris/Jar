<div class="layout-container">

    <div id="layout-navbar" class="navbar navbar-expand-lg align-items-lg-center bg-white">
        <div class="navbar-nav align-items-lg">
            <h2>Budget</h2>
        </div>

        <div class="navbar-nav align-items-lg ml-auto">
            <div class="btn-group">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">{{activeYear}}</button>
                <div class="dropdown-menu daterangepicker ltr opensleft daterangepicker-noposition dropdown-menu-right" x-placement="bottom-end">
                    <a class="dropdown-item" v-for="(year,index) in years" v-bind:class="{ active: activeYear == year }" v-on:click="selectActiveYear(year)">{{year}}</a>
                </div>
            </div>
        </div>

        <div class="navbar-nav align-items-lg ml-2">
            <button type="button" class="btn btn-outline-primary rounded-pill">Configure Jars</button>
        </div>
    </div>

    <div class="btn-group month-bar">
        <button type="button" class="btn btn-default" v-for="(month,index) in months" v-bind:class="{ disabled: !month.enabled, active: month.active }" v-on:click="selectActiveMonth(month)">{{month.shortMonth}}</button>
    </div>    

</div>