'use strict';

maestroApp.controller('ButtonBarController',
    function ButtonBarController($scope, controlService) {
        $scope.play = function() {
            controlService.play();
        };
        
        $scope.stop = function () {
            controlService.stop();
        };
        
        $scope.pause = function () {
            controlService.pause();
        };
        
        $scope.next = function () {
            controlService.next();
        };
        
        $scope.back = function () {
            controlService.back();
        };
    }
);