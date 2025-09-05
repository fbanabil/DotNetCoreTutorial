using FluentAssertions;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PersonsControllerIntegrationTest(CustomWebApplicationFactory customWebApplicationFactory)
        {
            _client = customWebApplicationFactory.CreateClient();
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            //Act
            HttpResponseMessage response =await _client.GetAsync("/Persons/Index");

            //Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseBody);

            var document = htmlDocument.DocumentNode;
            document.QuerySelectorAll("table.persons").Should().NotBeNull();
        }


        #endregion
    }
}
