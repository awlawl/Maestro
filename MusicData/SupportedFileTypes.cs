using System.IO;
using System.Linq;

namespace MusicData
{
    public class SupportedFileTypes
    {
        public static bool IsSupportedFileType(string filePath)
        {
            var extention = Path.GetExtension(filePath).ToLower();
            var allowedExtentions = new string[] { ".mp3", ".m4a", ".ogg" };
            return allowedExtentions.Contains(extention);
        }

        public static bool RequiresTranscoding(string filePath)
        {
            var extention = Path.GetExtension(filePath).ToLower();
            var transCodingExtentions = new string[] { ".m4a", ".ogg", ".mov" };
            return transCodingExtentions.Contains(extention);
        }

        public static bool CanBePlayed(string filePath)
        {
            var extention = Path.GetExtension(filePath).ToLower();
            var canPlay = new string[] { ".mp3"};
            return canPlay.Contains(extention);
        } 
    }
}
