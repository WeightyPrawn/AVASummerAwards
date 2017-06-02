$(document).on('click', function (e) {
    if ($(e.target).closest('nav').length < 1) {
        $('#main-menu').collapse('hide');
    }
});

angular.module('AvaSummerAwards', ['ngRoute', 'AdalAngular'])
    .config(['$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider',
        function ($routeProvider, $httpProvider, adalProvider) {
            $routeProvider
                .when("/",
                {
                    controller: "AppController",
                    templateUrl: "/App",
                    requireADLogin: true,
                })
                .otherwise({ redirectTo: "" });
            var endpoints = {
                // Map the location of a request to an API to a the identifier of the associated resource
                //url:apiResourceId
            };
            endpoints[siteAddress] = "https://marcusengelhartavanade.onmicrosoft.com/AvaSummerAwards";

            adalProvider.init(
                {
                    instance: 'https://login.microsoftonline.com/',
                    tenant: 'common', //for multi-tenant
                    clientId: '884c99db-e371-4707-be34-56b1c064db0f', //copy from Azure active directory application for this client
                    extraQuerryParameter: 'nux=1',
                    endpoints: endpoints
                    //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
                },
                $httpProvider
            );
        }])
    .controller('AppController', ['VoteService', '$interval', '$scope', '$rootScope', 'adalAuthenticationService', function (VoteService, $interval, $scope, $rootScope, adalService) {
        console.log($rootScope.userInfo)
        $scope.data = VoteService.data;

        $scope.vote = VoteService.vote;
        $scope.removeVote = VoteService.removeVote;
        $scope.$on("adal:loginFailure", function () {
            console.log("login failure");
            adalService.login();
        });

    }]).controller('MainController', ['$scope', '$rootScope', 'adalAuthenticationService', '$location', function ($scope, $rootScope, adalService, $location) {
        $scope.logout = function () {
            adalService.logOut();
        };
        $scope.login = function () {
            $location.path('/');
            console.log($rootScope.userInfo)
        };
    }])
    .factory('VoteService', ['$http', function ($http) {
        var apiBaseUrl = siteAddress;
        var data = {};

        var URLS = {
            CATEGORIES: '/api/Categories',
            VOTE: '/api/Votes'
        };

        function init() {
            getData();
        };
        function getData() {
            $http.get(apiBaseUrl + URLS.CATEGORIES).success(function (resp) {
                data.categories = resp;
                console.log(data);
            }).error(function (error) {
                console.log(error);
                alert("Please allow 3rd party cookies to use app.");
            });
        };
        init();

        return {
            vote: function (nomination) {
                console.log(nomination);
                var reqModel = {
                    NomineeID: nomination.ID || 1
                };

                $http.post(apiBaseUrl + URLS.VOTE, reqModel)
                    .then(function successCallback(response) {
                        // this callback will be called asynchronously
                        // when the response is available
                        getData();
                        return response;
                    }, function errorCallback(response) {
                        // called asynchronously if an error occurs
                        // or server returns response with an error status.
                    });
            },

            removeVote: function (nomination) {
                console.log(nomination);

                return $http.delete(apiBaseUrl + URLS.VOTE + "/" + nomination.Vote.ID)
                .then(function successCallback(response) {
                    // this callback will be called asynchronously
                    // when the response is available
                    getData();
                    return response;
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            },

            data: data
        }
    }])