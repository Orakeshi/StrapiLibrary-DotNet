using Newtonsoft.Json.Linq;
using Solarflare.StrapiAPI;
using Solarflare.StrapiAPI.StrapiRequests;
using Xunit;
using Xunit.Abstractions;

namespace StrapiAPITests
{
    public class StrapiApiServiceTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public StrapiApiServiceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        private readonly StrapiService StrapiService = new ("https://api.findkevinstatues.com/");

        private CompetitionEntry compEntry = new CompetitionEntry
        {
            Email = "tommygeorgewebb@googlemail.com",
            FullName = "Tommy Webb",
            Address = "test",
            PhoneNumber = "0303949493939"
        };

        private readonly PrizeDrawEntry prizeDrawEntry = new PrizeDrawEntry
        {
            Email = "Update@Update.com",
            FullName = "Update Record",
            Age = "131",
            PhoneNumber = "00099292929292",
            Address = "UpdateStrt",
        };

        private readonly LoginEntry loginInfo = new LoginEntry
        {
            Identifier = "frontEndApp",
            Password = "Leg0p@ss"
        };

        // Passed -> True is returned
        [Fact(Skip = "Remove skip to run")]
        public void TestLogin()
        {
            Assert.True(StrapiService.Login(loginInfo).Result);
        }
        
        // Passed -> True is returned
        [Fact(Skip = "Remove skip to run")]
        public void TestLoginNew()
        {
            Assert.True(StrapiService.Login("frontEndApp", "Leg0p@ss").Result);
        }
        
        // Passed -> Strapi Disabled for getting entry with id
        [Fact/*(Skip = "Remove skip to run")*/]
        public void TestGet()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Get(prizeDrawEntry).Result);
        }
        
        // Passed -> Record is created in strapi library
        [Fact(Skip = "Remove skip to run")]
        public void TestCreate()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Create(prizeDrawEntry).Result);
        }

        // Passed -> Strapi Disabled for updating entries
        [Fact(Skip = "Skip, remove to run")]
        public void TestUpdate()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Update(prizeDrawEntry).Result);
        }

        // Passed -> Strapi Disabled for deleting entries
        [Fact(Skip = "Skip, remove to run")]
        public void TestDelete()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Del("3326").Result);
        }
        
        // Passed -> Data returned
        [Fact(Skip = "Skip, remove to run")]
        public void TestGetFile()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;

            testOutputHelper.WriteLine(StrapiService.GetFile("1102").Result);
        }
        
        // Passed -> Jwt is written to console {JWT currently made public, will need to be made private}
        [Fact(Skip = "Skip, remove to run")]
        public void OutputJwt()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            //testOutputHelper.WriteLine(StrapiService.Jwt);
        }

    }
}