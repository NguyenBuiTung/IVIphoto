using TakeimgIVI.Camera;
using TakeimgIVI.Function;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace TakeimgIVI
{
    public class Processimage
    {
        public static string Foldersaveimg = @"D:\Image_Pcs_IVI\";
        public static string Folderfull { get => Foldersaveimg + DateTime.Now.ToString("dd-MM-yyyy") + @"\" + (Model.Modelnow != null? Model.Modelnow.Name:"NoModel"); }
        public static string FolderOKGmes { get => Folderfull + @"\OKGMES"; }
        public static string FolderNGGmes { get => Folderfull + @"\NGGMES"; }
        public static string FolderNOGmes { get => Folderfull + @"\NOGMES"; }
        public static string FolderErGmes { get => Folderfull + @"\ERRORGMES"; }
        public static string FolderNobarcode { get => Folderfull + @"\NOBARCODE"; }

        public static Brush Colortxt = Brushes.LimeGreen;

        // vẽ vào ảnh không barcode
        public static Bitmap DrawToImageNoBarcode(Bitmap TrueBitmap)
        {
            if (TrueBitmap == null) return null;

            using (Graphics g = Graphics.FromImage(TrueBitmap))
            {
                RectangleF rectf = new RectangleF(0, 50, TrueBitmap.Width, TrueBitmap.Height / 4);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                string WIP = Constants.Barcode;
                if (string.IsNullOrEmpty(Constants.Barcode))
                {
                    WIP = DateTime.Now.ToString("HH-mm-ss");
                }
                g.DrawString($"Manual Trigger\nTime: {DateTime.Now.ToString("dd/mm/yyyy HH:mm:ss")}", new Font("Tahoma", 60), Colortxt, rectf);
            }

            return TrueBitmap;
        }

        // lưu ảnh manual
        public static void SaveImageManual(Bitmap TrueBitmap)
        {
            try
            {
                TrueBitmap = DrawToImageNoBarcode(TrueBitmap);

                string path = "D:" + $"\\PicturesManual\\{DateTime.Now.ToString("yyyyMMdd")}\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                TrueBitmap.Save(path + $"\\{DateTime.Now.ToString("HHmmss")}.jpg",ImageFormat.Jpeg);
                DelegateToUI.PushToListBox("Save Manual Image successfully");

            }
            catch { }
        }

        //Crop image
        public static Bitmap CropImage(Bitmap TrueBitmap, float widthbegin, float widthend, float heightbegin, float heightend)
        {
            int width = TrueBitmap.Width;
            int height = TrueBitmap.Height;

            int pxwidthbg = (int)(width * widthbegin);
            int pxwidthe = (int)(width * (widthend - widthbegin));
            int pxheightbg = (int)(height * heightbegin);
            int pxheighte = (int)(height * (heightend - heightbegin));

            if(pxwidthe == 0 || pxheighte == 0) { return null; }

            Rectangle cropArea = new Rectangle(pxwidthbg, pxheightbg, pxwidthe, pxheighte);

            return TrueBitmap.Clone(cropArea, TrueBitmap.PixelFormat);
        }

        public static Bitmap MergeImages(Image image1, Image image2, int space)
        {
            Bitmap bitmap = new Bitmap(image1.Width + image2.Width + space, Math.Max(image1.Height, image2.Height));
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Black);
                g.DrawImage(image1, 0, 0);
                g.DrawImage(image2, image1.Width + space, 0);
            }
            Image img = bitmap;

            return bitmap;
        }

        public static bool Saveimageauto(Bitmap bmp)
        {
            if (bmp == null) return false;
            try
            {
                if (!Directory.Exists(Folderfull))
                {
                    Directory.CreateDirectory(Folderfull);
                }
                if (!Directory.Exists(FolderOKGmes))
                {
                    Directory.CreateDirectory(FolderOKGmes);
                }
                if (!Directory.Exists(FolderNGGmes))
                {
                    Directory.CreateDirectory(FolderNGGmes);
                }
                if (!Directory.Exists(FolderNobarcode))
                {
                    Directory.CreateDirectory(FolderNobarcode);
                }
                if (!Directory.Exists(FolderErGmes))
                {
                    Directory.CreateDirectory(FolderErGmes);
                }
                if (!Directory.Exists(FolderNOGmes))
                {
                    Directory.CreateDirectory(FolderNOGmes);
                }
                

                Bitmap bmp1 = new Bitmap(bmp);
                string WIP = Constants.Barcode;

                if (Status.GMES && GMES_Data.ACK == "0") //GMESTEST
                {
                    FormGmesResult formGmes = new FormGmesResult();
                    formGmes.ShowDialog();
                }

                if (string.IsNullOrEmpty(Constants.Barcode))
                {
                    WIP = DateTime.Now.ToString("HH-mm-ss") + "_NoBarcode";
                    bmp1.Save(Getpathimg(FolderNobarcode, WIP),ImageFormat.Jpeg);
                    DelegateToUI.PushToListBox("Save Image No barcode successfully");
                }
                else
                {
                    if (Status.GMES)
                    {
                        if (GMES_Data.ACK == "0")
                        {
                            if (FormGmesResult.OKGMES)
                            {
                                WIP += "_OKGMES";
                                bmp1.Save(Getpathimg(FolderOKGmes, WIP), ImageFormat.Jpeg);
                            }
                            else
                            {
                                WIP += "_NGGMES";
                                bmp1.Save(Getpathimg(FolderNGGmes, WIP), ImageFormat.Jpeg);
                            }
                            DelegateToUI.PushToListBox("Save Image Send Gmes successfully");
                        }
                        else
                        {
                            WIP += "_GMES_ERROR";
                            bmp1.Save(Getpathimg(FolderErGmes, WIP), ImageFormat.Jpeg);
                            DelegateToUI.PushToListBox("Save Image GMES Error successfully");
                        }
                    }
                    else
                    {
                        WIP += "_NONGMES";
                        bmp1.Save(Getpathimg(FolderNOGmes, WIP), ImageFormat.Jpeg);
                        DelegateToUI.PushToListBox("Save Image NO GMES successfully");
                    }

                }
                
                return true;
            }
            catch (Exception ex) 
            {
                DelegateToUI.PushToListBox("Save Iamge Error : " + ex);
                return false;
            }
        }

        static string Getpathimg(string folder, string WIP)
        {
            return folder + @"\" + Regex.Replace(WIP, @"[^a-zA-Z0-9" + " _" + "]+", string.Empty) + ".jpeg";
        }

        // vẽ vào ảnh
        public static Bitmap DrawToImage(Bitmap bmp)
        {
            if (bmp == null) return null;
            using (Graphics g = Graphics.FromImage(bmp))
            {
                RectangleF rectf = new RectangleF(0, 50, bmp.Width, bmp.Height / 4);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                string WIP = Constants.Barcode;
                if (string.IsNullOrEmpty(Constants.Barcode))
                {
                    WIP = DateTime.Now.ToString("HH-mm-ss") + "_NoBarcode";
                }
                g.DrawString($"WIP: {WIP}\nTime: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}\nModel: {Model.Modelnow.Name}", new Font("Tahoma", 60), Colortxt, rectf);
            }
            return bmp;
        }

    }


}
