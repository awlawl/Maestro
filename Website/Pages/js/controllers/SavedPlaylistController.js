'use strict';

maestroApp.controller('SavedPlaylistController',
    function SavedPlaylistController($scope, $rootScope, savedPlaylistService,controlService) {
        $scope.SavedPlaylists = [];
        $scope.CurrentPlaylistSongs = [];
        $scope.newSavedPlaylistName = "";

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
            controlService.enqueueSong(song.IdValue, function() {
                toastr.info(song.Title + " added to the queue.");
            });

        };
        
        $scope.play = function (song) {
            controlService.enqueueSong(song.IdValue, function () {
                controlService.playFromPlaylist(song.IdValue);
                toastr.info("Now playing " + song.Title + ".");
            });
        };

        $scope.removeSongFromSavedPlaylist = function(song) {
            savedPlaylistService.removeSongFromSavedPlaylist(song.IdValue, $scope.selectedSavedPlaylist.Name, function() {
                refreshSongsForSavedPlaylist($scope.selectedSavedPlaylist);
            });
        };

        $scope.newSavedPlaylist = function () {
            savedPlaylistService.newSavedPlaylist($scope.newSavedPlaylistName, function () {
                refreshSavedPlaylistList(function() {
                    
                    for (var i = 0; i < $scope.SavedPlaylists.length; i++) {
                        if ($scope.newSavedPlaylistName == $scope.SavedPlaylists[i].Name) {
                            $scope.selectedSavedPlaylist = $scope.SavedPlaylists[i];
                            break;
                        }
                    }
                    
                    $scope.newSavedPlaylistName = "";
                });
            });
        };

        $scope.enqueueSavedPlaylist = function() {
            savedPlaylistService.enqueueSavedPlaylist($scope.selectedSavedPlaylist.Name);
            toastr.info("Playlist " + $scope.selectedSavedPlaylist.Name + " added to the queue.");
        };

        $scope.playSavedPlaylist = function() {
            savedPlaylistService.playSavedPlaylist($scope.selectedSavedPlaylist.Name);
            toastr.info("Now playing " + $scope.selectedSavedPlaylist.Name + " playlist.");
        };

        var refreshSongsForSavedPlaylist = function (newValue) {
            savedPlaylistService.getSongsForSavedPlaylist(newValue.Name, function(songs) {
                $scope.CurrentPlaylistSongs = songs;
            });
        };

        var refreshSavedPlaylistList = function(callback) {
            savedPlaylistService.getSavedPlaylists(function(savedPlaylists) {
                $scope.SavedPlaylists = savedPlaylists;
                if (callback)
                    callback();
                
                $rootScope.$broadcast('savedPlaylistsChanged', savedPlaylists);
            });
        };


        refreshSavedPlaylistList();
    }
);