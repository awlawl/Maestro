'use strict';

maestroApp.factory('savedPlaylistService', function($http, $log) {
    return {
        getSavedPlaylists: function(callback) {
            $http({ method: 'GET', url: '/SavedPlaylist' })
                .success(function(data, status, headers, config) {
                    callback(data);
                })
                .error(function(data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        },
        getSongsForSavedPlaylist: function(savedPlaylist, callback) {
            $http({
                method: 'GET',
                url: "/SavedPlaylist/" + encodeURI(savedPlaylist),
            })
                .success(function (data, status, headers, config) {
                    callback(data);
                })
                .error(function (data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        },
        
        removeSongFromSavedPlaylist: function(id, savedPlaylist, callback) {
            $http({
                method: 'DELETE',
                url: "/SavedPlaylist/" + encodeURI(savedPlaylist) + "/" + encodeURI(id),
            })
                .success(function () {
                    callback();
                })
                .error(function (data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        }
        
       
    };
});