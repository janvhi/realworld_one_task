using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using RealWorld_One.Interfaces;

namespace RealWorld_One.Services
{
    public  class ImageParsing : IImageParsing
    {
        /// <summary>
        /// Read the Image stream coming from API
        /// Convert the stream image to Byte[]
        /// </summary>
        /// <param name="URL">API which returns the Content-Type as image/jpeg</param>
        /// <returns>returns the converted Byte[]</returns>
        public Byte[] ReadImageStreamToByte(Stream imgStream)
        { 
            using (var ms =  new MemoryStream())
            {
                imgStream.CopyTo(ms);
                return  ms.ToArray();
            }
        }
        /// <summary>
        /// Rotate the image Byte[] to rotated Byte[]
        /// </summary>
        /// <param name="ImgByte"> Byte[] which need to be rotate</param>
        /// <returns> rotated byte[]</returns>
        public  Byte[]  RotateImageAndConvertToByteArray(byte[] ImgByte)
        {
            using (MemoryStream ms = new MemoryStream(ImgByte))
            {
                //create the image from memory stream
                Image test_image = Image.FromStream(ms);

                //rotate the image 180 degrees
                test_image.RotateFlip(RotateFlipType.Rotate180FlipNone);

                //convert the rotated image back to byte array
                ImageConverter _imageConverter = new ImageConverter();
                return (byte[])_imageConverter.ConvertTo(test_image, typeof(byte[]));

            }
        }
        
    }
}
