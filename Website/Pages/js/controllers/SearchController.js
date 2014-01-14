'use strict';

maestroApp.controller('SearchController',
    function SearchController($scope, searchService, controlService, savedPlaylistService) {
        $scope.searchText = "";

        $scope.SearchResults = [];
        $scope.haveSearchedOnce = false;

        $scope.search = function () {
            searchService.search($scope.searchText, function(results) {
                $scope.SearchResults = results.Songs;
                $scope.haveSearchedOnce = true;

            });
        };

        $scope.shouldShowSearchResults = function() {
            return $scope.SearchResults.length > 0;
        };
        
        $scope.shouldShowNotFoundMessage = function () {
            return (!$scope.shouldShowSearchResults()) && $scope.haveSearchedOnce;
        };
        
        $scope.playFromResults = function (song) {
            controlService.playFromPlaylist(song.Song.IdValue);
        };

        $scope.enqueue = function(song) {
            controlService.enqueueSong(song.IdValue, function () { });
            toastr.info(song.Title + " added to the queue.");
        };
        
        $scope.play = function (song) {
            controlService.enqueueSong(song.IdValue, function() {
                controlService.playFromPlaylist(song.IdValue);
                toastr.info("Now playing " + song.Title + ".");
            });
        };
        
        $scope.isSongInSavedPlaylist = function(song, savedPlaylist) {
            var index = song.SavedPlaylists.indexOf(savedPlaylist.Name);
            return index >= 0;
        };

        $scope.toggleSongInSavedPlaylist = function (song, savedPlaylist) {
            if ($scope.isSongInSavedPlaylist(song, savedPlaylist)) {
                savedPlaylistService.removeSongFromSavedPlaylist(song.IdValue, savedPlaylist.Name, function () {
                    song.SavedPlaylists=_.reject(song.SavedPlaylists, function(pl) { return pl == savedPlaylist.Name; });
                });
            } else {
                savedPlaylistService.addSongToSavedPlaylist(song.IdValue, savedPlaylist.Name, function() {
                    song.SavedPlaylists.push(savedPlaylist.Name);
                });
            }
        
        };
        
        $scope.SavedPlaylists = [];
        savedPlaylistService.getSavedPlaylists(function (savedPlaylists) {
            $scope.SavedPlaylists = savedPlaylists;
        });
        
    }
);