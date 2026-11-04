using System.Text;
using System.Text.Json;
using NUnit.Framework;
using Reqnroll;
using SpecFlow_API_Testing.Support;

namespace SpecFlow_API_Testing.StepDefinitions;

[Binding]
public class PostsSteps
{
    

    private HttpResponseMessage _response = null!;
    private string _body = "";

    //private static readonly HttpClient Client = new()
    //{
    //    BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
    //};

    [When(@"I send a GET request to ""(.*)""")]
    public async Task WhenISendAGetRequestTo(string path)
    {
        _response = await ApiClient.Client.GetAsync(path);        _body = await _response.Content.ReadAsStringAsync();
    }

    [When(@"I send a POST request to ""(.*)"" with title ""(.*)""")]
    public async Task WhenISendAPostRequest(string path, string title)
    {
        var json = JsonSerializer.Serialize(new { title, body = "test", userId = 1 });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _response = await ApiClient.Client.PostAsync(path, content);        _body = await _response.Content.ReadAsStringAsync();
    }

    [Then(@"the response status should be (\d+)")]
    public void ThenTheResponseStatusShouldBe(int expected)
    {
        Assert.That((int)_response.StatusCode, Is.EqualTo(expected));
    }

    [Then(@"the response body should contain ""(.*)"" with value ""(.*)""")]
    public void ThenTheBodyShouldContain(string property, string expected)
    {
        using var doc = JsonDocument.Parse(_body);
        var actual = doc.RootElement.GetProperty(property).ToString();
        Assert.That(actual, Is.EqualTo(expected));
    }
}