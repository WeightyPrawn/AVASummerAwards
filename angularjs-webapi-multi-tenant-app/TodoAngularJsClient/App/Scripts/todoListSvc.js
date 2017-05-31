'use strict';
angular.module('todoApp')
.factory('todoListSvc', ['$http', function ($http) {
    var apiBaseUrl = 'https://localhost:44326/';
    return {
        getItems : function(){
            return $http.get(apiBaseUrl + 'api/TodoList');
        },
        getItem : function(id){
            return $http.get(apiBaseUrl + 'api/TodoList/' + id);
        },
        postItem : function(item){
            return $http.post(apiBaseUrl + 'api/TodoList/', item);
        },
        putItem : function(item){
            return $http.put(apiBaseUrl + 'api/TodoList/', item);
        },
        deleteItem : function(id){
            return $http({
                method: 'DELETE',
                url: apiBaseUrl + 'api/TodoList/' + id
            });
        }
    };
}]);