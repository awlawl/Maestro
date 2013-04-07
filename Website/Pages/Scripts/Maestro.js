var MaestroViewModel = function () {
    this.artist = ko.observable("");
    this.album = ko.observable("");
    this.title = ko.observable("");
    this.albumArt = ko.observable("");
}

var channel = 'maestrotest';
var hostRootUrl = '';
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

function handleMessage(message) {
    if (message.action == "Now Playing") {
        nowPlaying(message.data);
    } else if (message.action == "Ping Reply") {
        pingReply(message.data);
    }
}

function nowPlaying(songInfo) {
    viewModel.artist(setField(songInfo.Artist));
    viewModel.title(songInfo.Title);
    viewModel.album(songInfo.Album);
    refreshAlbumArt();
}

function pingReply(pingData) {
    hostRootUrl = pingData.HostRootUrl;
    refreshAlbumArt();
}

function refreshAlbumArt() {
    viewModel.albumArt(hostRootUrl + "/AlbumArt?" + (new Date()).getTime().toString());
}

function setField(val) {
    if (!val)
        val = '...';

    return val;
}

