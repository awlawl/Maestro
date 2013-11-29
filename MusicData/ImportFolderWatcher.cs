using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace MusicData
{
    public class ImportFolderWatcher
    {
        private const string UNKNOWN_ALBUM_NAME = "Unknown Album";
        private const string UNKNOWN_ARTIST_NAME = "Unknown Artist";

        private IImportFolderInteractor _folderInteractor = null;
        private IMusicInfoReader _musicInfoReader = null;
        private ILibraryRepository _library = null;
        private string _folderToWatch = "";
        private ITranscoder _transcoder = null;
        private string _libraryRootFolder = @"c:\temp";

        private Thread _workerThread = null;

        public ImportFolderWatcher(ILibraryRepository library) : this(new ImportFolderInteractor(), new MusicInfoReader(),library, new Transcoder() )
        {
            _folderToWatch = System.Configuration.ConfigurationManager.AppSettings["ImportWatchFolder"];
            _libraryRootFolder = System.Configuration.ConfigurationManager.AppSettings["LibraryRootFolder"];
        }

        public ImportFolderWatcher(IImportFolderInteractor folder, IMusicInfoReader musicInfoReader, ILibraryRepository library, ITranscoder transcoder )
        {
            _folderInteractor = folder;
            _musicInfoReader = musicInfoReader;
            _library = library;
            _transcoder = transcoder;
        }

        public void ProcessFiles()
        {
            var files = _folderInteractor.GetFilesForFolder(_folderToWatch);
            foreach (var file in files)
            {
                if (SupportedFileTypes.IsSupportedFileType(file))
                {
                    var filePath = file;
                    if (SupportedFileTypes.RequiresTranscoding(file))
                    {
                        var newFile = _transcoder.Transcode(filePath);
                        _folderInteractor.DeleteFile(filePath);
                        filePath = newFile;
                    }

                    var musicInfo = _musicInfoReader.GetInfoForFile(filePath);
                    var album = string.IsNullOrEmpty(musicInfo.Album) ? UNKNOWN_ALBUM_NAME : musicInfo.Album;
                    var artist = string.IsNullOrEmpty(musicInfo.Artist) ? UNKNOWN_ARTIST_NAME : musicInfo.Artist;

                    musicInfo.FullPath = MoveToLibraryFolder(filePath, artist, album);
                    _library.AddOrUpdateMusicInLibrary(musicInfo);
                }
            }
        }

       

        private string MoveToLibraryFolder(string file, string artist, string album)
        {
            var artistPath = _libraryRootFolder + "\\" + artist;
            var albumPath = artistPath + "\\" + album;
            var newFullPath = albumPath + "\\" + Path.GetFileName(file);

            if (!_folderInteractor.DirectoryExists(artistPath))
                _folderInteractor.CreateDirectory(artistPath);

            if (!_folderInteractor.DirectoryExists(albumPath))
                _folderInteractor.CreateDirectory(albumPath);

            Log.Debug("Moving file to library folder: " + newFullPath);

            _folderInteractor.DeleteFile(newFullPath);
            _folderInteractor.MoveFile(file, newFullPath);
            return newFullPath;
        }

        private void WatchingWorkerThread()
        {
            Log.Debug("Now watching " + _folderToWatch);

            while (true)
            {
                Thread.Sleep(3000);
                try
                {
                    ProcessFiles();
                }
                catch (Exception exc)
                {
                    Log.Debug("Error while watching: " + exc.ToString());
                    Thread.Sleep(60 * 1000);
                }
            }
        }

        public void Start()
        {
            _workerThread = new Thread(new ThreadStart(WatchingWorkerThread));
            _workerThread.Name = "WatchFolderWorker";
            _workerThread.Start();
        }

        public void Stop()
        {
            try
            {
                _workerThread.Abort();
            }
            finally { }
        }


    }
}
