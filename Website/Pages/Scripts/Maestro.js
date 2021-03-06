﻿var MaestroViewModel = function () {
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

var PlaylistViewModel = function (PlaylistItem) {
    var self = this;
    this.Song = ko.observable(PlaylistItem.Song);
    this.PlaylistIndex = ko.observable(PlaylistItem.PlaylistIndex);
    this.PlayFomPlaylist = function(e) {
        playFromPlaylist(e.Song().IdValue);
    };
    this.RemoveFromPlaylist = function (e) {
        removeSongFromPlaylist(e.Song().IdValue, self.PlaylistIndex());
        console.log("want to remove " + e.Song().IdValue + " " + self.PlaylistIndex());
    };
};

var SearchResultViewModel = function (Song) {
    var self = this;
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

    this.Remove = function (e) {
        console.log("Want to remove " + e.Song.FullPath + " from playlist " + viewModel.selectedSavedPlaylist());
        removeSongFromSavedPlaylist(e.Song.IdValue);
    }

    this.isSongInSavedPlaylist = function (savedPlaylistName) {
        var index = self.Song.SavedPlaylists.indexOf(savedPlaylistName);
        
        return index>=0;
    }

    this.toggleSongInPlaylist = function (e) {
        //console.dir(e);

        if (self.isSongInSavedPlaylist(e.name())) {
            console.log("I would remove this from the playlist");
        } else {
            addSongToSavedPlaylist(self, e);
        }
    }
    

}

var SavedPlaylist = function (name) {
    this.name = ko.observable(name);
    
}

var channel;
var viewModel = new MaestroViewModel();

$(document).ready(function () {
    $("#btnPlay").click(play);
    $("#btnStop").click(stop);
    $("#btnNext").click(next);
    $("#btnBack").click(back);
    $("#btnPause").click(pause);
    $("#btnSearch").click(doSearch);
    $("#btnAddPlaylist").click(addNewPlaylist);
    $("#btnEnqueueSavedPlaylist").click(enqueueSavedPlaylist);
    $("#btnPlaySavedPlaylist").click(playSavedPlaylist);

    setupPubnub();

    $("#txtSearch").keypress(function (e) {
        if (e.which == 13) {
            doSearch()
        }
    });


    ko.applyBindings(viewModel);
});

function setupPubnub() {
    $.ajax({
        url: "/PubNubChannel",
        dataType: "json"
    }).done(function (data) {
        channel = data.Channel;
        console.log("pubhnub channel: " + channel);
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
    });
}

function publishMessage(message) {
        PUBNUB.publish({
            channel: channel,
            message: message,
            callback: function (info) {
                console.log("Wrote message");
                if (!info[0]) {
                    console.log("!!!!!!!!!!!!!!!!!!!! Failed! -> " + info[1]);

                }
            }
        });
}

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
    if (songInfo) {
        viewModel.artist(setField(songInfo.Artist));
        viewModel.title(songInfo.Title);
        viewModel.album(songInfo.Album);
        //viewModel.searchResults([]);
    }

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

function enqueueSavedPlaylist() {
    var name = viewModel.selectedSavedPlaylist();
    $.ajax({
        url: "/enqueueSavedPlaylist/" + encodeURI(name),
        type: "POST",
        dataType: "json"
    })
       .done(function (data) {
           popupInfoToaster(name + " playlist added to queue ");
           getPlaylist();
       });
}

function playSavedPlaylist() {
    var name = viewModel.selectedSavedPlaylist();
    $.ajax({
        url: "/PlaySavedPlaylistWithShuffle/" + encodeURI(name),
        type: "POST",
        dataType: "json"
    })
       .done(function (data) {
           popupInfoToaster(name + " playlist now playing ");
           getPlaylist();
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

function addSongToSavedPlaylist(searchResult, savedPlaylist) {
    var playlistName = savedPlaylist.name();
    var song = searchResult.Song;
    $.ajax({
        url: "/SavedPlaylist/" + encodeURI(playlistName) + "/" + encodeURI(song.IdValue),
        type: "POST",
        dataType: "json"
    })
       .done(function (data) {
           viewModel.selectedSavedPlaylist(playlistName);
           song.SavedPlaylists.push(playlistName);
           viewModel.savedPlaylistSongList.valueHasMutated()
       });
}

function removeSongFromSavedPlaylist(songId) {
    var name = viewModel.selectedSavedPlaylist();
    $.ajax({
        url: "/SavedPlaylist/" + encodeURI(name) + "/" + encodeURI(songId),
        type: "DELETE",
        dataType: "json"
    })
        .done(function (data) {
            refreshSongsForSavedPlaylist();
        });
}

function removeSongFromPlaylist(songId, index) {
    $.ajax({
        url: "/Playlist/" + encodeURI(songId) + "/" + encodeURI(index),
        type: "DELETE",
        dataType: "json"
    });
        /*.done(function (data) {
            refreshSongsForSavedPlaylist();
        });*/
}