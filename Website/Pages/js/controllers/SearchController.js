'use strict';

maestroApp.controller('SearchController',
    function SearchController($scope, searchService, controlService) {
        $scope.searchText = "";

        $scope.SearchResults = [];
        $scope.haveSearchedOnce = false;

        $scope.search = function () {
            $scope.haveSearchedOnce = true;
            searchService.search($scope.searchText, function(results) {
                $scope.SearchResults = results.Songs;
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
            controlService.enqueueSong(song.IdValue, function() {});
        };
        
        $scope.play = function (song) {
            controlService.enqueueSong(song.IdValue, function() {
                controlService.playFromPlaylist(song.IdValue);
            });
        };
    }
);