'use strict';

maestroApp.factory('searchService', function($http, $log) {
    return {
        search: function(searchText, callback) {
            $http({
                method: 'GET',
                url: "/search/" + encodeURI(searchText),
            })
                .success(function(data, status, headers, config) {
                    callback(data);
                })
                .error(function(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        }
    };
});