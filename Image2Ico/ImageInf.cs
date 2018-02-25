using System;
using System.Drawing;
using System.IO;
using static Image2Ico.ImageHelper;

namespace Image2Ico
{
    /// <summary>
    /// Image Information
    /// </summary>
    internal sealed class ImageInf
    {
        private String _filePath;
        private FileExt _type;
        private Image _image;
        private Int32 _targetWidth;
        private Int32 _targetHeight;

        public String FilePath { get => this._filePath; }
        public FileExt Type { get => this._type; }
        public String FileName { get => Path.GetFileNameWithoutExtension(this._filePath); }
        public Image Image { get => this._image; }
        public Int32 Width
        {
            get
            {
                if (this.Image == null) { return 0; }
                return Image.Width;
            }
        }
        public Int32 Height
        {
            get
            {
                if (this.Image == null) { return 0; }
                return Image.Height;
            }
        }

        public Int32 TargetWidth { get => this._targetWidth; set => this._targetWidth = value; }
        public Int32 TargetHeight { get => this._targetHeight; set => this._targetHeight = value; }

        public ImageInf(String filePath)
        {
            var type = GetFileType(filePath);
            if (type == FileExt.NULL)
            {
                throw new Exception("Not Image Exception");
            }
            this._type = type;
            this._filePath = filePath;
            // Default is 64
            this._targetWidth = 64;
            this.TargetHeight = 64;
            // Don't use Image.FromFile cause it cannot release file,using Image.FromStream instead
            //this._image = Image.FromFile(FilePath);
            using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                this._image = Image.FromStream(fileStream);
            }
        }

    }
}
