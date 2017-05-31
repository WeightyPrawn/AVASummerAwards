'use strict';
var app = angular.module('todoApp', ['ngRoute', 'AdalAngular']);

   app.config([
        '$routeProvider', '$httpProvider', 'adalAuthenticationServiceProvider',
        function($routeProvider, $httpProvider, adalProvider) {

            $routeProvider.when("/Home",
                {
                    controller: "homeCtrl",
                    templateUrl: "/App/Views/Home.html",
                })
                .when("/TodoList",
                {
                    controller: "todoListCtrl",
                    templateUrl: "/App/Views/TodoList.html",
                    requireADLogin: true,
                })
                .when("/UserData",
                {
                    controller: "userDataCtrl",
                    templateUrl: "/App/Views/UserData.html",
                })
                .otherwise({ redirectTo: "/Home" });

            var endpoints = {
                // Map the location of a request to an API to a the identifier of the associated resource
                //url:apiResourceId
                "https://localhost:44326/":
                    "https://marcusengelhartavanade.onmicrosoft.com/AvaSummerAwards"
            };

            adalProvider.init(
                {
                    instance: 'https://login.microsoftonline.com/',
                    tenant: 'common', //for multi-tenant
                    clientId: '884c99db-e371-4707-be34-56b1c064db0f', //copy from Azure active directory application for this client
                    extraQueryParameter: '',
                    endpoints: endpoints
                    //cacheLocation: 'localStorage', // enable this for IE, as sessionStorage does not work for localhost.
                },
                $httpProvider
            );

        }
    ])
    .run(function ($rootScope, $window) {
        //Additional code. This is the point where Adal will let us know whether admin has selected YES/No
        var signupProcessCompleted = $window.location.hash.indexOf('admin_consent=True') > -1;
        $rootScope.signupProcessCompleted = signupProcessCompleted;
       });
