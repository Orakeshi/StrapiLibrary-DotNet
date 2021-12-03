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
        
        CompetitionEntry compEntry = new CompetitionEntry
        {
            Email = "tommygeorgewebb@googlemail.com",
            FullName = "Tommy Webb",
            Address = "test",
            PhoneNumber = "0303949493939"
        };
        
        PrizeDrawEntry prizeDrawEntry = new PrizeDrawEntry
        {
            Email = "eheh@googlhttemail.com",
            FullName = "hey",
            Age = "26",
            PhoneNumber = "0303949493939",
            Address = "tegrgrhrrhrhrst"
        };
        
        LoginEntry loginInfo = new LoginEntry
        {
            Identifier = "frontEndApp",
            Password = "Leg0p@ss"
        };

        [Fact]
        public void TestLogin()
        {
            Assert.True(StrapiService.Login(loginInfo).Result);
        }
        
        [Fact(Skip = "Manually run, remove skip")]
        public void TestGet()
        {

            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            var test = StrapiService.Get<CompetitionEntry>("34");
            testOutputHelper.WriteLine(StrapiService.Get<CompetitionEntry>("").Result);
            //testOutputHelper.WriteLine(StrapiService.Get("prize-draw-entries", "").Result);
        }
        
        [Fact(Skip = "Remove skip to run")]
        public void TestCreate()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Create(prizeDrawEntry).Result);
        }
        
        /*[Fact(Skip = "Manually run, remove skip")]
        public void TestUpdate()
        {
            Assert.True(StrapiService.Update("Email", new RequestData() {id = 1}).Result);

        }*/

        [Fact] public void TestGetFileUrl()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            string url = JObject.Parse(StrapiService.GetFile("1102").Result)["url"]?.ToString() ?? string.Empty;
            
            testOutputHelper.WriteLine(url);
        }
        
        [Fact]
        public void OutputJwt()
        {
            bool isLoggedIn = StrapiService.Login(loginInfo).Result;
            testOutputHelper.WriteLine(StrapiService.Jwt);
        }

    }
}