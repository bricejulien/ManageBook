'use strict';
var app = angular.module("ManageBookApp", ['ngRoute']);
app.config(['$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/Entry/Index/:param1?', {
            templateUrl: '/Views/Entry/Index.cshtml',
            controller: 'ManageBookCtrl'
        });
        //$locationProvider.html5Mode(true);
    }
]);
app.controller('ManageBookCtrl', ['$scope', '$routeParams', '$http', '$window', '$location', '$timeout', 'EntryService',
    'UserService', 'ProjectService', '$filter',
    function ($scope, $routeParams, $http, $window, $location, $timeout, EntryService, UserService, ProjectService, $filter) {
        //var myProjectId = $routeParams.param1;
        //console.log($routeParams.id);
        //console.log(location.search);
        //console.log(window.location.search.replace("?projectid=", ""));
        $scope.detailProjectId = window.location.search.replace("?projectid=", "");
        $scope.message = "Now viewing home!";
        $scope.showRowNewEntry = false;
        $scope.DateofToday = $filter("date")(Date.now(), 'yyyy-MM-dd');
        $scope.Date = $scope.DateofToday;
        $scope.Invoiced = false;
        $scope.ProjectId = 1;
        var undefined;
        $scope.priorities = [
            { name: 'X', value: 0 },
            { name: '1', value: 1 },
            { name: '2', value: 2 },
            { name: '3', value: 3 },
            { name: '!!!', value: 4 }
        ];
        //$scope.priorities = [0,1,2,3,4];
        $scope.Priority = $scope.priorities[0].value;
        $scope.sortType = 'Priority'; // set the default sort type
        $scope.sortReverse = true; // set the default sort order
        $scope.newEntry = [{}];
        $scope.allUsers = [{}];
        $scope.user = [{}];
        $scope.allProjects = [{}];
        $scope.companies = [];
        $scope.addRow = function () {
            //$scope.companies.push({ 'name': $scope.name, 'employees': $scope.employees, 'headoffice': $scope.headoffice });
            //$scope.name = '';
            //$scope.employees = '';
            //$scope.headoffice = '';
            $scope.companies.push({
                'Priority': $scope.Priority, 'Description': $scope.Description, 'UserId': $scope.UserId,
                'Date': $scope.Date, 'InvoicableHours': $scope.InvoicableHours, 'ActualHours': $scope.ActualHours,
                'Invoiced': $scope.Invoiced, 'ProjectId': $scope.ProjectId
            });
            $scope.newEntry.Priority = $scope.Priority;
            $scope.newEntry.Description = $scope.Description;
            $scope.newEntry.UserId = $scope.UserId;
            $scope.newEntry.Date = $scope.Date;
            $scope.newEntry.InvoicableHours = $scope.InvoicableHours;
            $scope.newEntry.ActualHours = $scope.ActualHours;
            $scope.newEntry.Invoiced = $scope.Invoiced;
            $scope.newEntry.ProjectId = $scope.ProjectId;
            // then we add the new entry in the database
            this.addEntry();
            $scope.Priority = $scope.priorities[0].value;
            $scope.Description = '';
            $scope.UserId = '';
            $scope.Date = $scope.DateofToday;
            $scope.InvoicableHours = '';
            $scope.ActualHours = '';
            $scope.Invoiced = false;
            $scope.ProjectId = 1;
        };
        //$scope.getUserName = function (id:string) {
        //    UserService.getUserName($scope,id);
        //};
        /*$scope.getUserName = function (id) {
            return $http({
                method: "GET",
                url: "/api/UserAPI/GetApplicationUser/" + id,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                $scope.user = data;
            }).error(function (data) {
                console.log(data);
            });;
        };*/
        $scope.addEntry = function () {
            var data = {
                "Priority": $scope.newEntry.Priority,
                "Description": $scope.newEntry.Description,
                "UserId": $scope.newEntry.UserId,
                "Date": $scope.newEntry.Date,
                "InvoicableHours": $scope.newEntry.InvoicableHours,
                "ActualHours": $scope.newEntry.ActualHours,
                "Invoiced": $scope.newEntry.Invoiced,
                "ProjectId": $scope.newEntry.ProjectId
            };
            $http.post('/api/EntryAPI/PostEntry', JSON.stringify(data), {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).success(function (data) {
                $scope.newEntry.Priority = 0;
                $scope.newEntry.Description = "";
                $scope.newEntry.UserId = "";
                $scope.newEntry.Date = $scope.DateofToday;
                $scope.newEntry.InvoicableHours = 0;
                $scope.newEntry.ActualHours = 0;
                $scope.newEntry.Invoiced = false;
                $scope.newEntry.ProjectId = 0;
            });
        };
        $scope.saveEntry = function (id) {
            var modifiedEntry = $filter('filter')($scope.companies, { Id: id }, true);
            ////console.log($scope.companies);
            ////console.log(modifiedEntry);
            //console.log(modifiedEntry[0].Priority);
            //console.log(modifiedEntry[0].Description);
            //console.log(modifiedEntry[0].UserId);
            //console.log(modifiedEntry[0].Date);
            //console.log(modifiedEntry[0].InvoicableHours);
            //console.log(modifiedEntry[0].ActualHours);
            //console.log(modifiedEntry[0].Invoiced);
            //console.log(modifiedEntry[0].ProjectId);
            var data = {
                "Id": id,
                //"Entry": modifiedEntry,
                //"Entry.Priority": modifiedEntry.Priority
                "Priority": modifiedEntry[0].Priority,
                "Description": modifiedEntry[0].Description,
                "UserId": modifiedEntry[0].UserId,
                "Date": modifiedEntry[0].Date,
                "InvoicableHours": modifiedEntry[0].InvoicableHours,
                "ActualHours": modifiedEntry[0].ActualHours,
                "Invoiced": modifiedEntry[0].Invoiced,
                "ProjectId": modifiedEntry[0].ProjectId
            };
            $http.put('/api/EntryAPI/PutEntry/' + id, JSON.stringify(data), {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).success(function (data) {
                angular.element(document.querySelector('#checkIcon' + id)).css('visibility', 'visible');
                $timeout(function () {
                    angular.element(document.querySelector('#checkIcon' + id)).css('visibility', 'hidden');
                }, 3000);
            });
        };
        $scope.filterEntries = function (project, user, filteredStartDate, filteredEndDate, filterPriorityA, filterPriorityB, filterPriorityC, filterPriorityD, filterPriorityE) {
            var current = new Date();
            var startDate = new Date(current.getFullYear() - 40, 1, 1);
            startDate = $filter('date')(startDate, 'MM/dd/yyyy');
            var endDate = new Date(current.getFullYear() + 3, 1, 1);
            endDate = $filter('date')(endDate, 'MM/dd/yyyy');
            if (typeof project == "undefined") {
                project = "undefined";
            }
            if (typeof user == "undefined") {
                user = "undefined";
            }
            if (!(typeof filteredStartDate == "undefined")) {
                startDate = $filter('date')(new Date(filteredStartDate), 'MM/dd/yyyy');
            }
            if (!(typeof filteredEndDate == "undefined")) {
                endDate = $filter('date')(new Date(filteredEndDate), 'MM/dd/yyyy');
            }
            return $http({
                method: "GET",
                url: "/api/EntryAPI/Filter?project=" + project + "&user=" + user + "&startDate="
                    + startDate + "&endDate=" + endDate + "&priorityA=" + filterPriorityA + "&priorityB=" + filterPriorityB
                    + "&priorityC=" + filterPriorityC + "&priorityD=" + filterPriorityD + "&priorityE=" + filterPriorityE,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                $scope.allEntries = data;
                $scope.companies = data;
            }).error(function (data) {
                //console.log(data);
            });
            ;
        };
        $scope.toggleShowRow = function () {
            $scope.showRowNewEntry = !$scope.showRowNewEntry;
        };
        $scope.changeValuePriority = function (id, item) {
            var index = 0;
            index = $scope.companies.map(function (e) { return e.Id; }).indexOf(id);
            $scope.companies[index].Priority = item;
        };
        $scope.changeValueUserId = function (id, item) {
            var index = 0;
            index = $scope.companies.map(function (e) { return e.Id; }).indexOf(id);
            $scope.companies[index].UserId = item;
        };
        $scope.changeValueProjectId = function (id, item) {
            var index = 0;
            index = $scope.companies.map(function (e) { return e.Id; }).indexOf(id);
            $scope.companies[index].ProjectId = item;
        };
        //$scope.saveEntry = function (newPriority, newDescription, newDate, newInvoicableHours, newActualHours, newInvoiced, newProjectId,
        //    newUserId) {
        ////var data = {
        ////    "Priority": 5, "Description": "testDesc", "Date": $scope.DateofToday, "InvoicableHours": 1.5,
        ////    "ActualHours": 2, "Invoiced": true, "ProjectId": 1, "Information": "newInfo", "UserId": "c656d8d8-b9a3-45b0-9807-58ce1dc0ea00"
        ////};
        //    var data = {
        //        "Priority": newPriority, "Description": newDescription, "Date": newDate, "InvoicableHours": newInvoicableHours,
        //        "ActualHours": newActualHours, "Invoiced": newInvoiced, "ProjectId": newProjectId, /*"Information": newInformation,*/
        //        "UserId": newUserId
        //    };
        //    $http.post(
        //        '/api/EntryAPI/PostEntry',
        //        JSON.stringify(data),
        //        {
        //            headers: {
        //                'Content-Type': 'application/json'
        //            }
        //        }
        //    ).success(function (data) {
        //        $scope.person = data;
        //    });
        //}
        $scope.getUserName = function (id) {
            //var theUserName = "user not found";
            //for (var key in $scope.allUsers) {
            //    // skip loop if the property is from prototype
            //    if (!$scope.allUsers.hasOwnProperty(key)) continue;
            //    var obj = $scope.allUsers[key];
            //    for (var prop in obj) {
            //        // skip loop if the property is from prototype
            //        if (!obj.hasOwnProperty(prop)) continue;
            //        // your code
            //        //console.log(prop + " = " + obj[prop]);
            //        if (prop == "Id" && obj[prop] == id) {
            //            theUserName = obj["UserName"];
            //        }
            //    }
            //}
            //return theUserName;
            var theUserName = "user NOT FOUND";
            for (var i = 0, len = $scope.allUsers.length; i < len; i++) {
                if ($scope.allUsers[i].Id == id) {
                    theUserName = $scope.allUsers[i].UserName;
                }
            }
            return theUserName;
        };
        $scope.getProjectName = function (id) {
            //var theProjectName = "project not found";
            //console.log($scope.allProjects);
            //for (var key in $scope.allProjects) {
            //    // skip loop if the property is from prototype
            //    if (!$scope.allUsers.hasOwnProperty(key)) continue;
            //    var obj = $scope.allProjects[key];
            //    for (var prop in obj) {
            //        // skip loop if the property is from prototype
            //        if (!obj.hasOwnProperty(prop)) continue;
            //        // your code
            //        //console.log(prop + " = " + obj[prop]);
            //        if (prop == "Id" && obj[prop] == id) {
            //            theProjectName = obj["Name"];
            //        }
            //    }
            //}
            //return theProjectName;
            var theProjectName = "project NOT FOUND";
            for (var i = 0, len = $scope.allProjects.length; i < len; i++) {
                if ($scope.allProjects[i].Id == id) {
                    theProjectName = $scope.allProjects[i].Name;
                }
            }
            return theProjectName;
        };
        $scope.getPriorityName = function (value) {
            var theName = "X";
            for (var i = 0, len = $scope.priorities.length; i < len; i++) {
                if ($scope.priorities[i].value == value) {
                    theName = $scope.priorities[i].name;
                }
            }
            return theName;
        };
        $scope.allEntries = {};
        EntryService.getAllEntries($scope);
        $scope.allUsers = {};
        UserService.getAllUsers($scope);
        $scope.allProjects = {};
        ProjectService.getAllProjects($scope);
    }]);
app.service('EntryService', ['$http', function ($http) {
        this.getAllEntries = function ($scope) {
            return $http({
                method: "GET",
                url: "/api/EntryAPI/AllEntries?projectid=" + $scope.detailProjectId,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                console.log(data);
                /*convert string to date */
                //for (var i = 0, len = data.length; i < len; i++) {
                //    data[i].Date = new Date(data[i].Date);
                //}
                $scope.allEntries = data;
                $scope.companies = data;
            }).error(function (data) {
                //console.log(data);
            });
            ;
        };
    }]);
app.service('UserService', ['$http', function ($http) {
        this.getAllUsers = function ($scope) {
            return $http({
                method: "GET",
                url: "/api/UserAPI/AllUsers",
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                $scope.allUsers = data;
            }).error(function (data) {
                //console.log(data);
            });
            ;
        };
        this.getUserName = function ($scope, id) {
            return $http({
                method: "GET",
                url: "/api/UserAPI/GetApplicationUser/" + id,
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                $scope.user = data;
            }).error(function (data) {
                //console.log(data);
            });
            ;
        };
    }]);
app.service('ProjectService', ['$http', function ($http) {
        this.getAllProjects = function ($scope) {
            return $http({
                method: "GET",
                url: "/api/ProjectAPI/AllProjects",
                headers: { 'Content-Type': 'application/json' }
            }).success(function (data) {
                $scope.allProjects = data;
                //console.log("debut all projects");
                //console.log($scope.allProjects);
                //console.log("fin all projects");
            }).error(function (data) {
                //console.log(data);
            });
            ;
        };
    }]);
//# sourceMappingURL=ManageBookApp.js.map