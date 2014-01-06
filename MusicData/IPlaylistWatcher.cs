
using System.Collections.Generic;

namespace MusicData
{
    public interface IPlaylistWatcher
    {
        void AttachToPlaylist(Playlist playlist, ILibraryRepository library);
        void SongStarting(MusicInfo song);
        void SongEnding(MusicInfo song);
        void PlaylistChanged();
    }
}
