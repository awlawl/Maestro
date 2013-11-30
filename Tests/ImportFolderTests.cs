using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MusicData;

namespace Tests
{
    [TestFixture]
    public class ImportFolderTests
    {
        [Test]
        public void NoFiles()
        {
            var importFolderInteractor = new FakeFolderInteractor(new string[0]);
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, null, null, null);

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(0, importFolderInteractor.MovedFiles.Count(), "There must not be any moved files");
        }

        [Test]
        public void OneMp3file()
        {
            var fakeFiles = new string[] { "one.mp3" };
            var fakeMusicInfo = new MusicInfo() {
                Album = "onealbum",
                Artist = "oneartist"
            };

            var fakeMusicInfoReader = new FakeMusicInfoReader(fakeMusicInfo);
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles);
            var library = new MemoryLibraryRepository();
            library.ClearLibrary();
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, fakeMusicInfoReader, library, null);

            importFolderWatcher.ProcessFiles();

            var expectedLibraryPath = @"c:\temp\" + fakeMusicInfo.Artist + "\\" + fakeMusicInfo.Album + "\\" + fakeFiles[0];

            Assert.AreEqual(1, importFolderInteractor.MovedFiles.Count(), "There must one moved file");
            Assert.AreEqual(fakeFiles[0], importFolderInteractor.MovedFiles[0].SourceFile, "The file to move must be correct.");
            Assert.AreEqual(expectedLibraryPath, importFolderInteractor.MovedFiles[0].DestinationFile, "The file must be moved to the correct folder.");

            var libraryFiles = library.GetAllMusic();

            Assert.AreEqual(1, libraryFiles.Count(), "There must be one file in the library.");
            Assert.AreEqual(fakeMusicInfo.Artist, libraryFiles[0].Artist, "The artist added to the library must be correct.");
            Assert.AreEqual(expectedLibraryPath, libraryFiles[0].FullPath, "The full path of the file in the library must be correct.");
        }

        [Test]
        public void OneMp3file_NoInfo()
        {
            var fakeFiles = new string[] { "one.mp3" };
            var fakeMusicInfo = new MusicInfo()
            {
                Album = "",
                Artist = ""
            };

            var fakeMusicInfoReader = new FakeMusicInfoReader(fakeMusicInfo);
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles);
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, fakeMusicInfoReader, new MemoryLibraryRepository(), null);

            importFolderWatcher.ProcessFiles();

            var expectedLibraryPath = @"c:\temp\Unknown Artist\Unknown Album\" + fakeFiles[0];

            Assert.AreEqual(1, importFolderInteractor.MovedFiles.Count(), "There must one moved file");
            Assert.AreEqual(fakeFiles[0], importFolderInteractor.MovedFiles[0].SourceFile, "The file to move must be correct.");
            Assert.AreEqual(expectedLibraryPath, importFolderInteractor.MovedFiles[0].DestinationFile, "The file must be moved to the correct folder.");
        }

        [Test]
        public void UnknownFileType()
        {
            var fakeFiles = new string[] { "readme.txt" };
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles);
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, null, null,null);

            importFolderWatcher.ProcessFiles();

            Assert.AreEqual(0, importFolderInteractor.MovedFiles.Count(), "There must no moved files.");
        }

        [Test]
        public void TranscodesOtherFormats()
        {
            var fakeFiles = new string[] { "test.m4a" };
            var fakeMusicInfo = new MusicInfo()
            {
                Album = "Test",
                Artist = "Test2"
            };

            var transcodedFile = "test.mp3";
            var fakeMusicInfoReader = new FakeMusicInfoReader(fakeMusicInfo);
            var importFolderInteractor = new FakeFolderInteractor(fakeFiles);
            var fakeTranscoder = new FakeTranscoder();
            var library = new MemoryLibraryRepository();
            library.ClearLibrary();
            var importFolderWatcher = new ImportFolderWatcher(importFolderInteractor, fakeMusicInfoReader, library, fakeTranscoder);

            importFolderWatcher.ProcessFiles();

            var expectedLibraryPath = @"c:\temp\" + fakeMusicInfo.Artist + "\\" + fakeMusicInfo.Album + "\\" + transcodedFile;

            Assert.AreEqual(1, importFolderInteractor.MovedFiles.Count(), "There must one moved file");
            Assert.AreEqual(transcodedFile, importFolderInteractor.MovedFiles[0].SourceFile, "The file to move must be correct.");
            Assert.AreEqual(expectedLibraryPath, importFolderInteractor.MovedFiles[0].DestinationFile, "The file must be moved to the correct folder.");
            Assert.IsTrue(importFolderInteractor.DeletedFiles.Contains(fakeFiles[0]), "The original file must be deleted.");
            Assert.IsTrue(importFolderInteractor.DeletedFiles.Contains(expectedLibraryPath), "The library path must be deleted.");
            Assert.AreEqual(2, importFolderInteractor.DeletedFiles.Count(), "There must be 2 attempted file deletes.");

        }

        [Test]
        public void SanitizeFolderNames()
        {
            var importFolder = new ImportFolderWatcher(new MemoryLibraryRepository());
            var badFolderName = "[]/\\&~?*|<>\";:+";
            var expectedName = "               ";

            var result = importFolder.SanitizeFolderName(badFolderName);

            Assert.AreEqual(expectedName, result, "The folder name must replace the bad characters with spaces.");
        }


    }

    public class FakeFolderInteractor : IImportFolderInteractor
    {
        public FakeFolderInteractor(string[] fakeFiles)
        {
            DeletedFiles = new List<string>();
            MovedFiles = new List<dynamic>();
            Files = fakeFiles;
        }

        public List<dynamic> MovedFiles { get; set; }
        public string[] Files { get; set; }
        public List<string> DeletedFiles { get; set; }

        public string[] GetFilesForFolder(string folder)
        {
            return Files;
        }

        public void DeleteFile(string file)
        {
            DeletedFiles.Add(file);
        }


        public void MoveFile(string sourceFile, string destinationFile)
        {
            MovedFiles.Add(new { SourceFile = sourceFile, DestinationFile = destinationFile});
        }


        public bool DirectoryExists(string folder)
        {
            return true;
        }

        public void CreateDirectory(string folder)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeMusicInfoReader : IMusicInfoReader
    {
        private MusicInfo _musicInfo = null;

        public FakeMusicInfoReader(MusicInfo musicInfo)
        {
            _musicInfo = musicInfo;
        }

        public MusicInfo GetInfoForFile(string fullPath)
        {
            return _musicInfo;
        }
    }

    public class FakeTranscoder : ITranscoder
    {
        public string Transcode(string inputFile)
        {
            return inputFile.Replace(".m4a", ".mp3");
        }
    }

}
