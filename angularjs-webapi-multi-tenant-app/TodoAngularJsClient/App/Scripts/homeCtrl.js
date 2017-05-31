'use strict';
angular.module('todoApp')
.controller('homeCtrl', ['$rootScope', '$scope', 'adalAuthenticationService', '$location',
    function ($rootScope, $scope, adalService, $location) {
    $scope.login = function () {
        adalService.login();
    };
    $scope.logout = function () {
        adalService.logOut();
    };
    $scope.isActive = function (viewLocation) {        
        return viewLocation === $location.path();
    };

    

    //Additional code
    $scope.signupProcessCompleted = $rootScope.signupProcessCompleted;
    console.log('$rootScope.signupProcessCompleted: ' + $rootScope.signupProcessCompleted);
        $scope.signup = function() {
            console.log('starting sign-up process...');

            var adal = new AuthenticationContext();
            adal.config.displayCall = function adminFlowDisplayCall(urlNavigate) {
                //important step. Tells ADAL to present consent screen to admin
                urlNavigate += '&prompt=admin_consent';
                adal.promptUser(urlNavigate);
            };
            adal.login();
            adal.config.displayCall = null;
        };

        
    }]);