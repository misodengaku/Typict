using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Bright.Subsystem
{
    public class ImageReader
    {
        #region Singleton
        static ImageReader instance = null;
        public static ImageReader Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageReader();
                return instance;
            }
        }
        #endregion

        public IEnumerable<string> AcceptExtents()
        {
            foreach (var ext in GetSystemAcceptExt())
                yield return ext;
        }

        /// <summary>
        /// システムがサポートする画像フォーマットの拡張子の配列を取得します。
        /// </summary>
        /// <returns>読み込める拡張子のString配列</returns>
        public IEnumerable<string> GetSystemAcceptExt()
        {
            ImageCodecInfo[] decoders = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo ici in decoders)
            {
                //セミコロン区切りで渡されるので
                string[] exts = ici.FilenameExtension.Split(new char[] { ';' });
                foreach (string ext in exts)
                    yield return ext;
            }
        }

        public Image LoadImage(string path)
        {
            Image loaded = null;
            if (!System.IO.File.Exists(path))
                //ファイルが見つからない
                throw new System.IO.FileNotFoundException("Image not found:" + path);

            loaded = K.Snippets.Images.ImageSafeReader(path, false);

            if (loaded == null)
                throw new Exception("Image cannot read");
            return loaded;
        }

        public Image CreateThumbnail(string path)
        {
            Image loaded = null;
            if (!System.IO.File.Exists(path))
                //ファイルが見つからない
                throw new System.IO.FileNotFoundException("Image not found:" + path);

            loaded = K.Snippets.Images.ImageSafeReader(path, false);

            if (loaded == null)
                throw new Exception("Image cannot read");
            var thumbnail = CreateThumbnail(loaded);
            loaded.Dispose();
            return thumbnail;
        }

        public Image CreateThumbnail(Image img)
        {
            return img.GetThumbnailImage(
                Core.Config.DisplayConfig.ThumbnailSize.Width,
                Core.Config.DisplayConfig.ThumbnailSize.Height,
                () => false, IntPtr.Zero);
        }

    }
}
