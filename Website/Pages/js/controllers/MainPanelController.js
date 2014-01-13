'use strict';

maestroApp.controller('MainPanelController',
    function MainPanelController($scope, currentStatusService, pubNubService, controlService) {

        var getNowPlaying = function() {
            currentStatusService.getNowPlaying(function(nowPlaying) {
                $scope.NowPlaying = nowPlaying;
            });
        };

        var getPlaylist = function() {
            currentStatusService.getPlaylist(function(playlist) {
                for (var i = 0; i < playlist.Playlist.length; i++) {
                    if (playlist.Playlist[i].PlaylistIndex == playlist.CurrentSongIndex)
                        playlist.Playlist[i].RowClass = "currentPlaylistSong";
                }
                $scope.Playlist = playlist.Playlist;
            });
        };

        pubNubService.onNowPlaying(function(message) {
            getNowPlaying();
            getPlaylist();
        });

        getNowPlaying();
        getPlaylist();

        $scope.playFromPlaylist = function(song) {
            controlService.playFromPlaylist(song.Song.IdValue);
        };
        
        $scope.removeFromPlaylist = function (song) {
            controlService.removeFromPlaylist(song.Song.IdValue, song.PlaylistIndex);
        };

    }
);