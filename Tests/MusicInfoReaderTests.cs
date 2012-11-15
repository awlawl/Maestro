﻿using System.Linq;
using System.Collections.Generic;
using MusicData;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MusicInfoReaderTests
    {
        [Test]
        public void ReadTestMP3()
        {
            var testFilePath = @"..\..\..\TestFiles\one.mp3";
            var artist = "awl";
            var title = "one";
            var album = "TestAlbum";
            
            var musicInfoReader = new MusicInfoReader();

            MusicInfo result = musicInfoReader.GetInfoForFile(testFilePath);

            Assert.AreEqual(title, result.Title, "The title must be correct.");
            Assert.AreEqual(artist, result.Artist, "The artist must be correct.");
            Assert.AreEqual(album, result.Album, "It must be for the correct album.");
            Assert.AreEqual(testFilePath, result.FullPath,"The full path must be correct.");
            
        }

        [Test]
        public void GetMusicInfoForDirectory()
        {
            var testDirectory = @"..\..\..\TestFiles";
            var title = "one";
            
            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> result = musicInfoReader.CrawlDirectory(testDirectory);

            Assert.Greater(result.Count, 0, "There must be more than one item in the result.");

            var oneMp3 = result.FirstOrDefault(X => X.FullPath.IndexOf("one.mp3") > -1);

            Assert.IsNotNull(oneMp3, "There must be an entry for one.mp3.");
            Assert.AreEqual(title,oneMp3.Title, "The title must be correct.");
        }

        [Test]
        public void GetMusicInfoForDirectoryWithSubdirectories()
        {
            var testDirectory = @"..\..\..\TestFiles";
            var title = "four";

            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> result = musicInfoReader.CrawlDirectory(testDirectory);

            Assert.Greater(result.Count, 0, "There must be more than one item in the result.");

            var fourMp3 = result.FirstOrDefault(X => X.FullPath.IndexOf("four.mp3") > -1);

            Assert.IsNotNull(fourMp3, "There must be an entry for the mp3 file.");
            Assert.AreEqual(title, fourMp3.Title, "The title must be correct.");
        }

        [Test]
        public void GetMusicInfoForDirectoryWithoutMusic()
        {
            var testDirectory = ".";

            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> result = musicInfoReader.CrawlDirectory(testDirectory);

            Assert.IsNotNull(result, "The result must not be null.");
            Assert.AreEqual(0, result.Count, "There must not be any results.");
        }

        
    }
}