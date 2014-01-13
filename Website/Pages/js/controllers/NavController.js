'use strict';

maestroApp.controller('NavController',
    function NavController($scope, savedPlaylistService) {
        $scope.SavedPlaylists = [];
        savedPlaylistService.getSavedPlaylists(function (savedPlaylists) {
            $scope.SavedPlaylists = savedPlaylists;
        });
        
        $scope.$on('savedPlaylistsChanged', function (event, savedPlaylists) {
            $scope.SavedPlaylists = savedPlaylists;
        });
        
        $scope.playSavedPlaylist = function (savedPlaylist) {
            savedPlaylistService.playSavedPlaylist(savedPlaylist.Name);
        };

        $scope.enqueueSavedPlaylist = function (savedPlaylist) {
            savedPlaylistService.enqueueSavedPlaylist(savedPlaylist.Name);
        };

        
    }
);