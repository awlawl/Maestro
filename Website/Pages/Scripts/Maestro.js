var MaestroViewModel = function() {
    this.artist = ko.observable("");
    this.album = ko.observable("");
    this.title = ko.observable("");
    this.albumArt = ko.observable("");
    this.currentSongIndex = ko.observable("");
    this.playlist = ko.observableArray([]);
};

var PlaylistViewModel = function(PlaylistItem) {
    this.Song = ko.observable(PlaylistItem.Song);
    this.PlaylistIndex = ko.observable(PlaylistItem.PlaylistIndex);
    this.PlayFomPlaylist = function(e) {
        playFromPlaylist(e.PlaylistIndex());
    };
};

var channel = 'maestrotest';
var viewModel = new MaestroViewModel();

$(document).ready(function () {
    $("#btnPlay").click(play);
    $("#btnStop").click(stop);
    $("#btnNext").click(next);
    $("#btnBack").click(back);
    $("#btnPause").click(pause);

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
            ping();

        }
    });


    ko.applyBindings(viewModel);
});

function publishMessage(message) {

    PUBNUB.publish({
        channel: 'maestrotest',
        message: message,
        callback: function (info) {
            console.log("Wrote message");
            if (!info[0]) {
                console.log("!!!!!!!!!!!!!!!!!!!! Failed! -> " + info[1]);

            }
        }
    });
};

function play() {
    publishMessage({ action: "Play" });
}

function stop() {
    publishMessage({ action: "Stop" });
}

function next() {
    publishMessage({ action: "Next" });
}

function back() {
    publishMessage({ action: "Back" });
}

function pause() {
    publishMessage({ action: "Pause" });
}

function ping() {
    publishMessage({ action: "Ping" });
}

function playFromPlaylist(playlistIndex) {
    publishMessage({ action: "PlayFromPlaylist", data: { PlaylistIndex: playlistIndex } });
};

function handleMessage(message) {
    if (message.action == "Now Playing") {
        nowPlaying(message.data);
    } else if (message.action == "Ping Reply") {
        pingReply(message.data);
    } else if (message.action == "AddedSongToLibrary") {
        newSongAdded(message.data);
    }

}

function nowPlaying(songInfo) {
    viewModel.artist(setField(songInfo.Artist));
    viewModel.title(songInfo.Title);
    viewModel.album(songInfo.Album);
    refreshAlbumArt();
    getPlaylist();
}

function pingReply(pingData) {
    refreshAlbumArt();
}

function refreshAlbumArt() {
    viewModel.albumArt("/AlbumArt?" + (new Date()).getTime().toString());
}

function setField(val) {
    if (!val)
        val = '...';

    return val;
}

function getPlaylist() {
    $.ajax({
        url: "/Playlist",
        dataType: "json"
    })
        .done(function(data) {
            viewModel.currentSongIndex(data.CurrentSongIndex);
            //viewModel.playlist(data.Playlist);

            var newPlaylist = [];

            for (var i = 0; i < data.Playlist.length; i++) {
                newPlaylist.push(new PlaylistViewModel(data.Playlist[i]));
            }

            viewModel.playlist(newPlaylist);
        });
}

function popupInfoToaster(message) {
    toastr.info(message);
}

function newSongAdded(data) {
    var message = data.firstSong.Artist + " - " + data.firstSong.Title;

    if (data.howMany > 1)
        message += " and " + data.howMany + " others";

    message += " added to library";
    popupInfoToaster(message);
}

