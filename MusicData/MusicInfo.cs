﻿namespace MusicData
{
    public class MusicInfo
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string FullPath { get; set; }
        public uint TrackNumber { get; set; }
        public string[] SavedPlaylists { get; set; }
    }
}
