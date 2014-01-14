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
        },
        newSavedPlaylist: function(newSavedPlaylistName, callback) {
            $http({
                method: 'POST',
                url: "/SavedPlaylist/" + encodeURI(newSavedPlaylistName),
            })
                .success(function (data) {
                    callback(data);
                })
                .error(function (data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        },
        enqueueSavedPlaylist: function(savedPlaylistName) {
            $http({
                method: 'POST',
                url: "/EnqueueSavedPlaylistWithShuffle/" + encodeURI(savedPlaylistName),
            })
                .success(function () {
                    
                })
                .error(function (data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        },
        playSavedPlaylist: function (savedPlaylistName) {
            $http({
                method: 'POST',
                url: "/PlaySavedPlaylistWithShuffle/" + encodeURI(savedPlaylistName),
            })
                .success(function () {

                })
                .error(function (data, status, headers, config) {
                    $log.warn(data, status, headers, config);
                });
        },
        addSongToSavedPlaylist: function(id, savedPlaylistName, callback) {
            $http({
                method: 'POST',
                url: "/SavedPlaylist/" + encodeURI(savedPlaylistName) + "/" + encodeURI(id),
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