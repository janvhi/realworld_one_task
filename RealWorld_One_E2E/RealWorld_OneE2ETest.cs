using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RealWorld_One;
using RealWorld_One.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Net;
namespace RealWorld_One_E2E
{
    public class RealWorld_OneE2ETest
    {
        protected readonly HttpClient httpClient;
        private ApplicationDbContext applicationDbContext;
        string username = "test@integration.com";
        string password = "SomePass1234!";
        public RealWorld_OneE2ETest()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>()
                                            .WithWebHostBuilder(builder =>
                                            {
                                                builder.ConfigureServices(services =>
                                                {

                                                    services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "RealWorld_OneInMemoryDb"));

                                                    var sp = services.BuildServiceProvider();
                                                    var scope = sp.CreateScope();
                                                    var scopedServices = scope.ServiceProvider;
                                                    applicationDbContext = scopedServices.GetRequiredService<ApplicationDbContext>();

                                                });
                                            });
            httpClient = webApplicationFactory.CreateClient();
        }
        /// <summary>
        /// Registering new user.
        /// login using the registered user
        /// </summary>
        /// <returns></returns>
        public async Task<string> AuthenticateRegister_Login()
        {
            var registerJson = JsonConvert.SerializeObject(new RegisterModel
            {
                Username = username,
                Password = password
            });
            var registerResponse = await httpClient.PostAsync("/api/authenticate/register", new StringContent(registerJson, Encoding.UTF8, "application/json"));
            
            var LoginJson = JsonConvert.SerializeObject(new LoginModel
            {
                Username = username,
                Password = password
            });
            var loginResponse = await httpClient.PostAsync("/api/authenticate/login", new StringContent(LoginJson, Encoding.UTF8, "application/json"));
            JObject deserialized = JsonConvert.DeserializeObject<JObject>(loginResponse.Content.ReadAsStringAsync().Result);
            return deserialized.First.First.ToString();
        }
        /// <summary>
        /// Authenticating using the generated token
        /// </summary>
        /// <returns></returns>
        protected virtual async Task AuthenticateAsync()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await AuthenticateRegister_Login());
        }

        /// <summary>
        /// Unauthorized user should not be able to get registered user
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnAuth_GetUser_Should_ReturnUnauthorized()
        {
            //action
            var response = await httpClient.GetAsync("api/authenticate/users");

            // Assert
            // checking whether API returns ok status
            Assert.Equal(response.StatusCode.ToString(), HttpStatusCode.Unauthorized.ToString());

        }

        /// <summary>
        /// Authenticate, verify and assert get user after registeration
        /// </summary>
        /// <returns>The registered user</returns>
        [Fact]
        public async Task Auth_GetUserAfterRegistration_Should_ReturnOneRecords()
        {
            //Registering a new user ,login using registered user and generating token to access the post request
            await AuthenticateAsync();
            var response = await httpClient.GetAsync("api/authenticate/users");

            //verifying wheather user fetch is same as registered;
            Assert.Equal(username, JsonConvert.DeserializeObject<List<JObject>>(response.Content.ReadAsStringAsync().Result)[0].Root.First.First.ToString());

            // Assert
            // checking whether API returns ok status
            Assert.Equal(response.StatusCode.ToString(), HttpStatusCode.OK.ToString());

        }

        /// <summary>
        /// Verify and assert unauthorize user should not be allowed to call rotate cat api
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UnAuth_GetRotateCat_Should_ReturnAuthorized()
        {
            //action
            var response = await httpClient.GetAsync("api/v1/cat");

            // Assert
            // checking whether API returns ok status
            Assert.Equal(response.StatusCode.ToString(), HttpStatusCode.Unauthorized.ToString());

        }

        /// <summary>
        /// Authenticate, verify and assert get rotate cat
        /// </summary>
        /// <returns><success response and content type image/jpeg /returns>
        [Fact]
        public async Task Auth_GetRotatedCat_Should_ReturnRotateCat()
        {
            //Registering a new user ,login using registered user and generating token to access the post request
            await AuthenticateAsync();

            //calling catcatroller get api
            var response = await httpClient.GetAsync("api/v1/cat");
            
            // checking whether put API returns ok status
            Assert.Equal(response.StatusCode.ToString(), HttpStatusCode.OK.ToString());
            //verify response content-type as "image/jpeg"
            Assert.Equal("image/jpeg", response.Content.Headers.ContentType.MediaType);
        }
    }
}
