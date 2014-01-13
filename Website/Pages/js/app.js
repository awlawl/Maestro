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
    })
//stolen from http://stackoverflow.com/questions/15417125/submit-form-on-pressing-enter-with-angularjs
.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });

                event.preventDefault();
            }
        });
    };
});