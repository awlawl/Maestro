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
    public class TranscoderTests
    {
        [Test]
        public void CreatesProperCommandLine()
        {
            var pathToFFMPEG = @"c:\things\ffmpeg.exe";
            var transcoder = new Transcoder(pathToFFMPEG);
            var filePath = @"c:\temp\test.m4a";
            var expectedPath = "-i \"c:\\temp\\test.m4a\" -acodec mp3 -ac 2 -ab 160k \"c:\\temp\\test.mp3\"";
            var expectedOutput = "c:\\temp\\test.mp3";

            var commandLine = transcoder.GetCommandLine(filePath);
            Console.WriteLine(commandLine);

            Assert.AreEqual(pathToFFMPEG, commandLine.FileName, "The command line filename must be correct.");
            Assert.AreEqual(expectedPath, commandLine.Arguments, "The command arguments must be correct.");
            Assert.AreEqual(expectedOutput, commandLine.OutputPath, "The output path must be correct.");
           
        }
    }
}
