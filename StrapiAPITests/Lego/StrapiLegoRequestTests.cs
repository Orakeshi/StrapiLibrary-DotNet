using System.ComponentModel.DataAnnotations;
using Solarflare.StrapiAPI;
using Solarflare.StrapiAPI.StrapiRequests;
using Xunit;
using Xunit.Abstractions;

namespace StrapiAPITests.Lego
{
    public class StrapiLegoRequestTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public StrapiLegoRequestTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }
        
        private readonly StrapiService StrapiService = new ("https://api.findkevinstatues.com/");

        FormData formData = new()
        {
            email = "hey",
            fullName = "Tommy",
            age = 22,
            phone = "02042042704",
            address = "23 nfnfnf lane"
        };
            
        [Fact(Skip = "Skip Entry")]
        public void TestPrizeDrawEntry()
        {
            
            //testOutputHelper.WriteLine(strapiInterface.Create("prize-draw-entries", formData));
            //StrapiService.Login("frontEndApp", "Leg0p@ss");
            testOutputHelper.WriteLine(StrapiService.GetFile("1102").Result);
        }
    }

    public class FormData
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public int age { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }
}