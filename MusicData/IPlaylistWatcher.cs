
using System.Collections.Generic;

namespace MusicData
{
    public interface IPlaylistWatcher
    {
        void AttachToPlaylist(Playlist playlist, ILibraryRepository library);
        void PlaySong(MusicInfo filename);
    }
}
