$(document).on('click', function (e) {
    if ($(e.target).closest('nav').length < 1 ) {
        $('#main-menu').collapse('hide');
    }
});

angular.module('AvaSummerAwards', [])
    .controller('MainController', ['VoteService', '$interval', function (VoteService, $interval) {
        var ctrl = this;
        ctrl.data = VoteService.data;

        ctrl.vote = VoteService.vote;
        ctrl.removeVote = VoteService.removeVote;

    }])

    .factory('VoteService', ['$http', function ($http) {
        var data = {
            user: 'kalkon'
        };

        var URLS = {
            CATEGORIES: '/api/Categories',
            VOTE: '/api/Votes'
        };

        function init() {
            getData();
        };
        function getData() {
            $http.get(URLS.CATEGORIES + '?user=' + data.user).success(function (resp) {
                data.categories = resp;
                console.log(data);
            });
        };
        init();

        return {
            vote: function (nomination) {
                console.log(nomination);
                var reqModel = {
                    NomineeID: nomination.ID || 1
                };

                $http.post(URLS.VOTE + "?user=" + data.user, reqModel)
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

                return $http.delete(URLS.VOTE + "/" + nomination.Vote.ID + "?user=" + data.user)
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