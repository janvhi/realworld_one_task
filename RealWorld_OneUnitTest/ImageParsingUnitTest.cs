using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RealWorld_One.Services;
using RealWorld_One.Interfaces;
using System.Drawing;
using System.IO;

namespace RealWorld_OneUnitTest
{
    [TestClass]
    public class ImageParsingUnitTest
    {
        IImageParsing imageParsing = new ImageParsing();

        /// <summary>
        /// Assert and verify ReadImageStreamToByte function
        /// It should return Byte[] 
        /// </summary>
        [TestMethod]
        public void Test_ReadImageStreamToByte()
        {
            //given
            FileStream catFileStream = new FileStream("TestData/cat.jpeg", FileMode.Open);

            // when
            byte[] catByteArray = imageParsing.ReadImageStreamToByte(catFileStream);


            // then
            Assert.IsNotNull(catByteArray);
            Assert.IsInstanceOfType(catByteArray, typeof(byte[]));
            Assert.IsTrue(catByteArray.Length > 0, "Length of cat byte array should be greater than zero");
        }

        /// <summary>
        /// Assert and verify RotateImageAndConvertToByteArray function
        /// It should rotate the image byte[] 
        /// </summary>
        [TestMethod]
        public void Test_RotateImageAndConvertToByteArray()
        {
            //when
            byte[] actualReversedCatByteArray = imageParsing.RotateImageAndConvertToByteArray(File.ReadAllBytes("TestData/cat.jpeg"));

            // then
            byte[] expectedReversedGrayScaleCatByteArray = File.ReadAllBytes("TestData/cat_reversed.jpeg");
            Assert.IsNotNull(actualReversedCatByteArray);
            Assert.IsInstanceOfType(actualReversedCatByteArray, typeof(byte[]));
            Assert.IsTrue(actualReversedCatByteArray.Length > 0, "Length of cat byte array should be greater than zero");
            CollectionAssert.AreEqual(actualReversedCatByteArray, expectedReversedGrayScaleCatByteArray);
        }

    }
       
}
