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

        controlService.getVolume(function(volume) {
            $scope.volume = volume * 100.0;
            
            $scope.$watch('volume', function (newValue, oldValue) {
                if (newValue != oldValue) {
                    console.log("volume changed from " + oldValue + " to " + newValue);
                    controlService.changeVolume(newValue/100);
                }
            });
        });


    }
);