using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Image2Ico
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ImageInf imageInf = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "PNG,JPG,BMP|*.png;*.jpg;*.bmp",
                Title = "Select An Image File",
                Multiselect = false
            };
            var result = dlg.ShowDialog();
            if (result == false)
            {
                return;
            }
            try
            {
                imageInf = new ImageInf(dlg.FileName);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Not Image Exception")
                {
                    MessageBox.Show("File is not an image");
                    return;
                }
            }
            imgPreview.Source = new BitmapImage(new Uri(imageInf.FilePath));
            txbType.Text = imageInf.Type.ToString();
            txbWidth.Text = imageInf.Width.ToString();
            txbHeight.Text = imageInf.Height.ToString();
            txbTip.Visibility = Visibility.Hidden;

            if (imageInf.Width >= imageInf.Height && imageInf.Width > 512)
            {
                iudWidth.Value = 512;
                iudHeight.Value = 512 * imageInf.Height / imageInf.Width;
            }
            else if (imageInf.Width < imageInf.Height && imageInf.Height > 512)
            {
                iudHeight.Value = 512;
                iudWidth.Value = 512 * imageInf.Width / imageInf.Height;
            }
            else if (imageInf.Height <= 512 && imageInf.Width <= 512)
            {
                iudHeight.Value = imageInf.Height;
                iudWidth.Value = imageInf.Width;
            }
        }

        private void btnSave_Click(Object sender, RoutedEventArgs e)
        {
            if (imageInf == null || imageInf.Image == null)
            {
                MessageBox.Show("Please Select An Image");
                return;
            }

            this.imageInf.TargetWidth = (Int32)iudWidth.Value;
            this.imageInf.TargetHeight = (Int32)iudHeight.Value;

            SaveFileDialog dlg = new SaveFileDialog()
            {
                FileName = imageInf.FileName,
                Filter = "Icon|*.ico"
            };
            var result = dlg.ShowDialog();
            if (result == false)
            {
                return;
            }
            try
            {
                using (var icon = ImageHelper.ConvertToIcon(imageInf))
                {
                    using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create))
                    {
                        icon.Save(fileStream);
                        fileStream.Flush();
                    }
                }
                MessageBox.Show("Succeed");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Flag
        private Boolean isValueChanging = false;

        private void iudWidth_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (this.imageInf == null)
                return;
            if (this.cbxLock.IsChecked == true && !isValueChanging)
            {
                isValueChanging = true;
                if (this.iudWidth.Value == null)
                    this.iudWidth.Value = (Int32)e.OldValue;
                if (this.iudWidth.Value == 0)
                    this.iudWidth.Value = (Int32)e.OldValue;
                var value = (Int32)(imageInf.Height * this.iudWidth.Value / imageInf.Width);
                if (value > 1024)
                {
                    this.iudHeight.Value = 1024;
                    this.iudWidth.Value = (Int32)(imageInf.Width * this.iudHeight.Value / imageInf.Height);
                }
                else
                {
                    this.iudHeight.Value = value;
                }
                isValueChanging = false;
            }
        }

        private void iudHeight_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            if (this.imageInf == null)
                return;
            if (this.cbxLock.IsChecked == true && !isValueChanging)
            {
                isValueChanging = true;
                if (this.iudHeight.Value == null)
                    this.iudHeight.Value = (Int32)e.OldValue;
                if (this.iudHeight.Value == 0)
                    this.iudHeight.Value = (Int32)e.OldValue;
                var value = (Int32)(imageInf.Width * this.iudHeight.Value / imageInf.Height);
                if (value > 1024)
                {
                    this.iudWidth.Value = 1024;
                    this.iudHeight.Value = (Int32)(imageInf.Height * this.iudWidth.Value / imageInf.Width);
                }
                else
                {
                    this.iudWidth.Value = value;
                }
                isValueChanging = false;
            }
        }
    }
}
