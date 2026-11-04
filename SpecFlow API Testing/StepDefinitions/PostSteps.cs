using System.Text;
using System.Text.Json;
using Reqnroll;
using SpecFlow_API_Testing.Support;

namespace SpecFlow_API_Testing.StepDefinitions;

// [Binding] tells Reqnroll that this class contains step definitions for the .feature files
[Binding]
public class PostsSteps
{
    // Stores the server's reply so later steps (Then...) can check it
    private HttpResponseMessage _response = null!;
    // Stores the reply's content (the JSON text)
    private string _body = "";

    // Matches the feature step text; (.*) captures the quoted value into 'path'
    [When(@"I send a GET request to ""(.*)""")]
    public async Task WhenISendAGetRequestTo(string path)
    {
        // Send a GET request to the API (e.g. /posts/1) and save the response
        _response = await ApiClient.Client.GetAsync(path);
        // Read the response content as text
        _body = await _response.Content.ReadAsStringAsync();
    }

    [When(@"I send a POST request to ""(.*)"" with title ""(.*)""")]
    public async Task WhenISendAPostRequest(string path, string title)
    {
        // Build the data to send and convert it to JSON text
        var json = JsonSerializer.Serialize(new { title, body = "test", userId = 1 });
        // Wrap the JSON so it can be sent as the request body
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        // Send a POST request (creates a new item) and save the response
        _response = await ApiClient.Client.PostAsync(path, content);
        _body = await _response.Content.ReadAsStringAsync();
    }

    [Then(@"the response status should be (\d+)")]
    public void ThenTheResponseStatusShouldBe(int expected)
    {
        // Check the HTTP status code (e.g. 200 OK, 201 Created) matches what the feature expects
        Assert.That((int)_response.StatusCode, Is.EqualTo(expected));
    }

    [Then(@"the response body should contain ""(.*)"" with value ""(.*)""")]
    public void ThenTheBodyShouldContain(string property, string expected)
    {
        // Parse the JSON text so we can look up individual fields
        using var doc = JsonDocument.Parse(_body);
        // Get the value of the named field (e.g. "title")
        var actual = doc.RootElement.GetProperty(property).ToString();        // Fail the test if the field's value isn't what the feature expects
        Assert.That(actual, Is.EqualTo(expected));
    }

    [When(@"I send a PUT request to ""(.*)"" with title ""(.*)""")]
    public async Task WhenISendAPUTRequestToWithTitle(string path, string title)
    {
        var json = JsonSerializer.Serialize(new { title, body = "test", userId = 1 });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _response = await ApiClient.Client.PutAsync(path, content);
        _body = await _response.Content.ReadAsStringAsync();
    }

    [When("I send a DELETE request to {string}")]
    public async Task WhenISendADELETERequestTo(string path)
    {
        _response = await ApiClient.Client.DeleteAsync(path);
        _body = await _response.Content.ReadAsStringAsync();
    }

    [Then(@"the response body should be blank")]
    public void ThenTheResponseBodyShouldBeBlank()
    {
        // JSONPlaceholder returns an empty JSON object "{}" for a DELETE, so treat that as blank
        var isBlank = string.IsNullOrWhiteSpace(_body) || _body.Trim() == "{}";
        Assert.That(isBlank, Is.True);
    }
}
