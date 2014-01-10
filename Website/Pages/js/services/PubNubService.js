'use strict';

maestroApp.factory('pubNubService', function ($http, $log) {
    var channel = "";

    var handleMessage = function(message) {
        if (message.action == "Now Playing") {
            nowPlayingCallback(message.data);
        } else if (message.action == "Ping Reply") {
            pingReply(message.data);
        } else if (message.action == "AddedSongToLibrary") {
            newSongAdded(message.data);
        }
    };

    $http({ method: 'GET', url: '/PubNubChannel' })
        .success(function (data) {
            channel = data.Channel;
            console.log("pubnub channel: " + channel);
            PUBNUB.subscribe({
                channel: channel,
                callback: function (message) {
                    console.log(JSON.stringify(message));
                    handleMessage(message);

                },
                error: function () {
                    // The internet is gone.
                    console.log("###Connection Lost");
                },
                connect: function () {
                    console.log("Now listening.");
                }
            });
        })
        .error(function (data, status, headers, config) {
            $log.warn(data, status, headers, config);
        });

    var nowPlayingCallback = null;

    return {
        onNowPlaying: function(callback) { nowPlayingCallback = callback }
    };
});