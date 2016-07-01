'use strict';
var app = angular.module("ManageBookApp", ['ngRoute']);
app.config(['$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/Entry/Index/:param1?', {
            templateUrl: '/Views/Entry/Index.cshtml',
            controller: 'EntryCtrl'
        });
        //$locationProvider.html5Mode(true);
    }
]);
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