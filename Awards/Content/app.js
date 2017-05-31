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
            $http.get(URLS.CATEGORIES + '?user=' + data.user).success(function (resp) {
                data.categories = resp;
                console.log(data);
            })
        }

        init();

        return {
            vote: function (category, nomination) {
                console.log(category);
                console.log(nomination);
                var reqModel = {
                    user: data.user,
                    vote: {
                        NomineeID: nomination.ID || 1
                    }
                };

                return $http.post(URLS.VOTE, reqModel)
            },

            removeVote: function (category, nomination) {

                console.log(category);
                console.log(nomination);

            },

            data: data
        }
    }])