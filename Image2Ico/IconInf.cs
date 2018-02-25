using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Image2Ico
{
    /// <summary>  
    /// Icon File Information
    /// </summary>  
    internal sealed class IconInf : IDisposable
    {
        // Icon File Header
        private Byte[] _header =
        {
            (Byte)0,(Byte)0,	// idreserved,reserved (must be 0)
            (Byte)1,(Byte)0,	// idtype,resource type (1 for icons)
            (Byte)1,(Byte)0,	// idcount,how many images?
		};
        private Byte _width = 0;
        private Byte _height = 0;
        private Byte _colorNum = 0;
        private Byte _reserved = 0;
        private Int16 _planes = 1;
        private Int16 _pixelBit = 32;
        private Int32 _imageSize = 0;
        private Int32 _imageOffSet = 6 + 16;
        private Byte[] _imageData = null;

        public Byte[] Header { get => this._header; }                                           // Icon file header
        public Byte Width { get => this._width; set => this._width = value; }                   // Width
        public Byte Height { get => this._height; set => this._height = value; }                // Height
        public Byte ColorNum { get => this._colorNum; }                                         // Number of colors in image (0 if >=8bpp)
        public Byte Reserved { get => this._reserved; }                                         // Reserved ( must be 0)
        public Int16 Planes { get => this._planes; }                                           // Color planes
        public Int16 PixelBit { get => this._pixelBit; }                                       // Bits per pixel
        public Int32 ImageSize { get => this._imageSize; set => this._imageSize = value; }     // How many bytes in this resource
        public Int32 ImageOffSet { get => this._imageOffSet; }                                 // Where in the file is this image
        public Byte[] ImageData { get => this._imageData; set => this._imageData = value; }     // Image data

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(Boolean disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                this._width = 0;
                this._height = 0;
                this._imageSize = 0;
                this._imageData = null;
            }

            // Free any unmanaged objects here.
            disposed = true;
        }

        // Flag: Has Dispose already been called?
        Boolean disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
    }
}
