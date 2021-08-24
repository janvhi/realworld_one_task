using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RealWorld_One.Services;
using RichardSzalay.MockHttp;
using System;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using RealWorld_One.Interfaces;
using Moq;
using RealWorld_One.Controllers;
using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace RealWorld_OneUnitTest
{
    [TestClass]
    public class CatController_UnitTest
    {
        MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
        private readonly ILogger<CatController> mocklogger = new Mock<ILogger<CatController>>().Object;

        /// <summary>
        /// Rotate the cat coming from cataas.com
        /// assert and verify the result
        /// </summary>
        [TestMethod]
        public void GetCat_ShouldRotateCat()
        {
            // given
            var emptyByte = new byte[0];
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            responseMessage.Content = new ByteArrayContent(emptyByte);

            var apiServiceMock = new Moq.Mock<IAPIService>();
            apiServiceMock.Setup(data => data.CallAPI(It.IsAny<string>()))
                .Returns(Task.FromResult(responseMessage));

            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("http://sample-cat-url.com/");

            Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(x => x.GetSection(It.Is<string>(k => k == "Cataas:RandonCatAPI"))).Returns(mockSection.Object);

            //Mock<ILogger> mockLogging = new Mock<ILogger>();
            Mock<IImageParsing> imageParsingMock = new Mock<IImageParsing>();

            // when
            var catController = new CatController(mockConfig.Object, mocklogger, imageParsingMock.Object, apiServiceMock.Object);
            var httpResponse = catController.Get().Result;

            // then
            Assert.IsInstanceOfType(httpResponse, typeof(FileContentResult));
            var fileContentResult = (FileContentResult) httpResponse;
            Assert.AreEqual(fileContentResult.ContentType, "image/jpeg");
            CollectionAssert.Equals(fileContentResult.FileContents, emptyByte);
         }
    }
}
