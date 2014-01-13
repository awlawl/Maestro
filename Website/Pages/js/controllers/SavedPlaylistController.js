'use strict';

maestroApp.controller('SavedPlaylistController',
    function SavedPlaylistController($scope, savedPlaylistService,controlService) {
        $scope.SavedPlaylists = [];
        $scope.CurrentPlaylistSongs = [];

        $scope.savedPlaylistSelect = function (playlistName) {
            $scope.selectedSavedPlaylist = playlistName;
        };

        $scope.selectedSavedPlaylist = "";

        $scope.$watch('selectedSavedPlaylist', function (newValue) {
            refreshSongsForSavedPlaylist(newValue);
        });

        $scope.shouldShowSavedPlaylistDetails = function() {
            return $scope.selectedSavedPlaylist != "";
        };

        $scope.shouldShowSavedPlaylistSongs = function() {
            return $scope.CurrentPlaylistSongs.length > 0;
        };

        $scope.enqueue = function(song) {
            controlService.enqueueSong(song.IdValue, function (){});
        };
        
        $scope.play = function (song) {
            controlService.enqueueSong(song.IdValue, function () {
                controlService.playFromPlaylist(song.IdValue);
            });
        };

        $scope.removeSongFromSavedPlaylist = function(song) {
            savedPlaylistService.removeSongFromSavedPlaylist(song.IdValue, $scope.selectedSavedPlaylist.Name, function() {
                refreshSongsForSavedPlaylist($scope.selectedSavedPlaylist);
            });
        };

        var refreshSongsForSavedPlaylist = function (newValue) {
            savedPlaylistService.getSongsForSavedPlaylist(newValue.Name, function(songs) {
                $scope.CurrentPlaylistSongs = songs;
            });
        };

        var refreshSavedPlaylistList = function() {
            savedPlaylistService.getSavedPlaylists(function(savedPlaylists) {
                $scope.SavedPlaylists = savedPlaylists;
            });
        };


        refreshSavedPlaylistList();
    }
);