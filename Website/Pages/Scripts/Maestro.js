var MaestroViewModel = function () {
    var self = this;
    this.artist = ko.observable("");
    this.album = ko.observable("");
    this.title = ko.observable("");
    this.albumArt = ko.observable("");
    this.currentSongIndex = ko.observable("");
    this.playlist = ko.observableArray([]);
    this.searchResults = ko.observableArray([]);
    this.savedPlaylistSongList = ko.observableArray([]);
    this.savedPlaylists = ko.observableArray([]);
    this.selectedSavedPlaylist = ko.observable("");
    this.setSelectedSavedPlaylist = function (pl) {
        self.selectedSavedPlaylist(pl.name());
        refreshSongsForSavedPlaylist();
    };
};

var PlaylistViewModel = function(PlaylistItem) {
    this.Song = ko.observable(PlaylistItem.Song);
    this.PlaylistIndex = ko.observable(PlaylistItem.PlaylistIndex);
    this.PlayFomPlaylist = function(e) {
        playFromPlaylist(e.Song().IdValue);
    };
};

var SearchResultViewModel = function (Song) {
    this.Song = Song;
    this.AddToPlaylist = function (e) {
        console.log("Want to add " + e.Song.FullPath);
        enqueueSong(e.Song.IdValue);
    }
    this.Play = function (e) {
        console.log("Want to play " + e.Song.FullPath);
        enqueueSong(e.Song.IdValue, function () {
            playFromPlaylist(e.Song.IdValue);
        });
    }
}

var SavedPlaylist = function (name) {
    this.name = ko.observable(name);
}

var channel = 'maestrotest';
var viewModel = new MaestroViewModel();

$(document).ready(function () {
    $("#btnPlay").click(play);
    $("#btnStop").click(stop);
    $("#btnNext").click(next);
    $("#btnBack").click(back);
    $("#btnPause").click(pause);
    $("#btnSearch").click(doSearch);
    $("#btnAddPlaylist").click(addNewPlaylist);

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

    $("#txtSearch").keypress(function (e) {
        if (e.which == 13) {
            doSearch()
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

function playFromPlaylist(id) {
    //publishMessage({ action: "PlayFromPlaylist", data: { PlaylistIndex: playlistIndex } });
    $.ajax({
        url: "/play/" + encodeURI(id),
        type: "POST",
        dataType: "json"
    })
       .done(function (data) {
           popupInfoToaster(data.Title + " Now playing ");
           getPlaylist();
       });

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
    //viewModel.searchResults([]);

    refreshAlbumArt();
    getPlaylist();
    refreshSavedPlaylistList();

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

function getPlaylist(callback) {
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
            if (callback)
                callback();
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



function doSearch() {
    var searchText = $("#txtSearch").val();
    //alert("searching for " + searchText);
    $.ajax({
        url: "/search/" + encodeURI(searchText),
        dataType: "json"
    })
       .done(function (data) {
           console.dir(data);
           
           var results = data.Songs.map(function (song) {
               return new SearchResultViewModel(song);
           });

           viewModel.searchResults(results);
           $("#txtSearch").focus();
       });
}

function enqueueSong (id, callback) {
    $.ajax({
        url: "/enqueue/" + encodeURI(id),
        type: "POST",
        dataType: "json"
    })
       .done(function (data) {
           popupInfoToaster(data.Title + " added to queue ");
           getPlaylist(callback);
       });
}

function refreshSavedPlaylistList(callback) {
    $.ajax({
        url: "/SavedPlaylist/",
        dataType: "json"
    })
      .done(function (data) {
          var models = data.map(function (pl) { return new SavedPlaylist(pl.Name); });
          viewModel.savedPlaylists(models);
          if (callback)
              callback();
      });
}

function refreshSongsForSavedPlaylist() {
    $.ajax({
        url: "/SavedPlaylist/" + encodeURI(viewModel.selectedSavedPlaylist()),
        dataType: "json"
    })
      .done(function (data) {
          var results = data.map(function (song) {
              return new SearchResultViewModel(song);
          });

          viewModel.savedPlaylistSongList(results);
      });
}

function addNewPlaylist() {
    var playlistName = $("#txtNewPlaylist").val();
    //alert("searching for " + searchText);
    $.ajax({
        url: "/SavedPlaylist/" + encodeURI(playlistName),
        type:"POST",
        dataType: "json"
    })
       .done(function (data) {
           refreshSavedPlaylistList(function () {
               viewModel.selectedSavedPlaylist(playlistName);
           });
       });
}