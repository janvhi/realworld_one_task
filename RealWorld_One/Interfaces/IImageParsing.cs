using System;
using System.IO;

namespace RealWorld_One.Interfaces
{
    public interface IImageParsing
    {
        Byte[] ReadImageStreamToByte(Stream imgStream);
        Byte[] RotateImageAndConvertToByteArray(Byte[] ImgByte);
    }
}
