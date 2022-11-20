<div class="layout-container">

    <div id="layout-navbar" class="navbar navbar-expand-lg align-items-lg-center bg-white">
        <div class="navbar-nav align-items-lg">
            <h2>Budget - {{budgetTitle}}</h2>
        </div>
        <div class="navbar-nav align-items-lg ml-auto">
            <div class="btn-group">
                <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    {{
activeYear
                    }}
                </button>
                <div class="dropdown-menu daterangepicker ltr opensleft daterangepicker-noposition dropdown-menu-right" x-placement="bottom-end">
                    <a class="dropdown-item" v-for="(year,index) in years" v-bind:class="{ active: activeYear == year }" v-on:click="selectActiveYear(year)">{{year}}</a>
                </div>
            </div>
        </div>

        <div class="navbar-nav align-items-lg ml-2">
            <button type="button" class="btn btn-outline-primary rounded-pill" v-on:click="openJars">Configure Jars</button>
        </div>
    </div>

    <div class="btn-group month-bar">
        <button type="button" class="btn btn-default" v-for="(month,index) in months" v-bind:class="{ disabled: !month.enabled, active: month.active }" v-on:click="selectActiveMonth(month)">{{month.shortMonth}}</button>
    </div>

    <div class="card" v-for="(section,index) in jarSections">
        <div class="card-header">
            <a class="collapsed d-flex justify-content-between text-body no-transition budget-section" data-toggle="collapse" :href="'#accordion'+index+'-child'">
                {{section.name}} ({{section.jars.length}})
                <div class="collapse-icon"></div>
            </a>
        </div>
        <div :id="'accordion'+index+'-child'" class="collapse no-transition show">
            <div v-for="(jar,index) in section.jars">
                <div class="card-body">
                    <div class="card-title with-elements">
                        <h5 class="m-0 mr-2">{{jar.Jar.Name}}</h5>
                        <div class="card-title-elements">

                        </div>
                        <div class="card-title-elements ml-md-auto">
                            <span class="badge badge-outline-danger">{{jar.MonthlyAssignedValue | asCurrency}}</span>
                            <span class="badge badge-primary badge-pill">{{jar.CarriedOverValue | asCurrency}}</span>
                        </div>
                    </div>
                    <p class="card-text">Some stuff</p>
                </div>
            </div>
        </div>
    </div>
</div>
