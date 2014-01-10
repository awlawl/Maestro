'use strict';

var maestroApp = angular.module('maestroApp', ['ngResource','ngRoute'])
    .config(function($routeProvider) {
        $routeProvider.when('/savedPlaylists', {
            templateUrl: 'templates/SavedPlaylistPanel.html',
            controller: 'SavedPlaylistController'
        });
        
        $routeProvider.when('/home', {
            templateUrl: 'templates/MainPanel.html',
            controller: 'MainPanelController'
        });
        
        $routeProvider.when('/search', {
            templateUrl: 'templates/SearchPanel.html',
            controller: 'SearchController'
        });

        $routeProvider.otherwise({ redirectTo: '/home' });
    });

