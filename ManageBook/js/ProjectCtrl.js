angular.module("ManageBookApp").controller('ProjectCtrl', ['$scope', '$routeParams', '$http', '$window', '$location', '$timeout', 'EntryService',
    'UserService', 'ProjectService', '$filter',
    function ($scope, $routeParams, $http, $window, $location, $timeout, EntryService, UserService, ProjectService, $filter) {
        $scope.detailProjectId = window.location.search.replace("?projectid=", "");
        $scope.showRowNewProject = false;
        $scope.DateofToday = $filter("date")(Date.now(), 'yyyy-MM-dd');
        $scope.Date = $scope.DateofToday;
        var undefined;
        $scope.priorities = [
            { name: 'X', value: 0 },
            { name: '1', value: 1 },
            { name: '2', value: 2 },
            { name: '3', value: 3 },
            { name: '!!!', value: 4 }
        ];
        $scope.Priority = $scope.priorities[0].value;
        $scope.sortType = 'Priority'; // set the default sort type
        $scope.sortReverse = true; // set the default sort order
        $scope.Actions = 0;
        $scope.Contacts = 0;
        $scope.ExpectedHours = 0;
        $scope.ActualHours = 0;
        $scope.QuickbookId = 0;
        $scope.Invoice = 0;
        $scope.newProject = [{}];
        $scope.allProjects = [];
        $scope.addRow = function () {
            $scope.allProjects.push({
                'Priority': $scope.Priority, 'Name': $scope.Name, 'Invoice': $scope.Invoice,
                'ExpectedHours': $scope.ExpectedHours, 'ActualHours': $scope.ActualHours, 'ContactPerson': $scope.ContactPerson,
                'PhoneNumber': $scope.PhoneNumber, 'QuickbookId': $scope.QuickbookId, 'Retainer': $scope.Retainer,
                'Actions': $scope.Actions, 'Contacts': $scope.Contacts
            });
            $scope.newProject.Priority = $scope.Priority;
            $scope.newProject.Name = $scope.Name;
            $scope.newProject.Invoice = $scope.Invoice;
            $scope.newProject.ExpectedHours = $scope.ExpectedHours;
            $scope.newProject.ActualHours = $scope.ActualHours;
            $scope.newProject.ContactPerson = $scope.ContactPerson;
            $scope.newProject.PhoneNumber = $scope.PhoneNumber;
            $scope.newProject.QuickbookId = $scope.QuickbookId;
            $scope.newProject.Retainer = $scope.Retainer;
            $scope.newProject.Actions = $scope.Actions;
            $scope.newProject.Contacts = $scope.Contacts;
            // then we add the new project in the database
            this.addProject();
            $scope.newProject.Priority = $scope.priorities[0].value;
            $scope.newProject.Name = '';
            $scope.newProject.Invoice = 0;
            $scope.newProject.ExpectedHours = 0;
            $scope.newProject.ActualHours = 0;
            $scope.newProject.ContactPerson = '';
            $scope.newProject.PhoneNumber = '';
            $scope.newProject.QuickbookId = 0;
            $scope.newProject.Retainer = false;
            $scope.newProject.Actions = 0;
            $scope.newProject.Contacts = 0;
        };
        $scope.addProject = function () {
            var data = {
                "Priority": $scope.newProject.Priority,
                "Name": $scope.newProject.Name,
                "Invoice": $scope.newProject.Invoice,
                "ExpectedHours": $scope.newProject.ExpectedHours,
                "ActualHours": $scope.newProject.ActualHours,
                "ContactPerson": $scope.newProject.ContactPerson,
                "PhoneNumber": $scope.newProject.PhoneNumber,
                "QuickbookId": $scope.newProject.QuickbookId,
                "Retainer": $scope.newProject.Retainer,
                "Actions": $scope.newProject.Actions,
                "Contacts": $scope.newProject.Contacts
            };
            $http.post('/api/ProjectAPI/PostProject', JSON.stringify(data), {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).success(function (data) {
                angular.element(document.querySelector('#checkIconAdd')).css('visibility', 'visible');
                $timeout(function () {
                    angular.element(document.querySelector('#checkIconAdd')).css('visibility', 'hidden');
                }, 3000);
                $scope.newProject.Priority = $scope.priorities[0].value;
                $scope.newProject.Name = '';
                $scope.newProject.Invoice = 0;
                $scope.newProject.ExpectedHours = 0;
                $scope.newProject.ActualHours = 0;
                $scope.newProject.ContactPerson = '';
                $scope.newProject.PhoneNumber = '';
                $scope.newProject.QuickbookId = 0;
                $scope.newProject.Retainer = false;
                $scope.newProject.Actions = 0;
                $scope.newProject.Contacts = 0;
            });
        };
        $scope.saveProject = function (id) {
            var modifiedProject = $filter('filter')($scope.allProjects, { Id: id }, true);
            var data = {
                "Id": id,
                "Priority": modifiedProject[0].Priority,
                "Name": modifiedProject[0].Name,
                "Invoice": modifiedProject[0].Invoice,
                "ExpectedHours": modifiedProject[0].ExpectedHours,
                "ActualHours": modifiedProject[0].ActualHours,
                "ContactPerson": modifiedProject[0].ContactPerson,
                "PhoneNumber": modifiedProject[0].PhoneNumber,
                "QuickbookId": modifiedProject[0].QuickbookId,
                "Retainer": modifiedProject[0].Retainer,
                "Actions": modifiedProject[0].Actions,
                "Contacts": modifiedProject[0].Contacts
            };
            $http.put('/api/ProjectAPI/PutProject/' + id, JSON.stringify(data), {
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
        $scope.toggleShowRow = function () {
            $scope.showRowNewProject = !$scope.showRowNewProject;
        };
        $scope.changeValuePriority = function (id, item) {
            var index = 0;
            index = $scope.allProjects.map(function (e) { return e.Id; }).indexOf(id);
            $scope.allProjects[index].Priority = item;
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
        $scope.allProjects = {};
        ProjectService.getAllProjects($scope);
    }]);
//# sourceMappingURL=ProjectCtrl.js.map