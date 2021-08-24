using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bazinga.AspNetCore.Authentication.Basic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RealWorld_One.Interfaces;

namespace RealWorld_One.Controllers
{
    [Route("api/v1/cat")]
    public class CatController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CatController> _logger;
        private readonly IImageParsing _imageParsing;
        private readonly IAPIService _apiService;

        public CatController(IConfiguration configuration, ILogger<CatController> logger, IImageParsing imageParsing, IAPIService apiService)
        {
            _configuration = configuration;
            _logger = logger;
            _imageParsing = imageParsing;
            _apiService = apiService;

        }


        /// <summary>
        /// Rotate the cat received from cataas.com
        /// </summary>
        /// <param name="filter">Will return a random cat with image filtered by :filter (blur, mono, sepia, negative, paint, pixel)</param>
        /// <param name="width">Will return a random cat with :width</param>
        /// <param name="height">Will return a random cat with height</param>
        /// <param name="type">Will return a random cat with image :type (small or sm, medium or md, square or sq, original or or)</param>

        /// <returns>rotated cat image</returns>
        [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme + ", " + JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string filter = null,
            [FromQuery] string width  = null,
            [FromQuery] string height = null,
            [FromQuery] string type   = null
        )
        {
            _logger.LogInformation("Inside Get api call");
            string finalURL = _configuration["Cataas:RandonCatAPI"];

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(filter))
                queryString.Add("filter", filter);
            if (!string.IsNullOrEmpty(width))
                queryString.Add("width", width);
            if (!string.IsNullOrEmpty(height))
                queryString.Add("height", height);
            if (!string.IsNullOrEmpty(type))
                queryString.Add("type", type);

            if (queryString.Count > 0)
                finalURL= _configuration["Cataas:RandonCatAPI"]+"?" + string.Join("&", queryString);

            _logger.LogInformation(string.Format("Final URL  build is {0}", finalURL));

            HttpResponseMessage response = await _apiService.CallAPI(finalURL);
            _logger.LogInformation(string.Format(" HttpResponseMessage response StatusCode is {0} ", response.StatusCode));
            
            Stream imgStream = response.Content.ReadAsStream();
            Byte[] imageBytes = _imageParsing.ReadImageStreamToByte(imgStream);
            Byte[] RotateByteArray = _imageParsing.RotateImageAndConvertToByteArray(imageBytes);

            _logger.LogInformation("Serving rotated cat image");
            return File(RotateByteArray, "image/jpeg");

        }
    }
}
