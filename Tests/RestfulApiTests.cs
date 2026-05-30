using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RestfulApiTests.Tests;

public class RestfulApiTests
{
    private static readonly HttpClient Client = CreateClient();

    private const string BaseUrl = "https://api.restful-api.dev/objects";

    private static readonly string LogFile =
        Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "Reports",
                "TestResultsOutput.txt"));

    private static HttpClient CreateClient()
    {
        var client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

        client.DefaultRequestHeaders.Add(
            "x-api-key",
            "16b4e814-4f66-4d40-acaf-2082aa682b74");

        return client;
    }

    private static void Log(string message)
    {
        var directory = Path.GetDirectoryName(LogFile);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        File.AppendAllText(
            LogFile,
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
    }

    private static async Task<string> CreateObject()
    {
        var payload = new
        {
            name = "MacBook Pro",
            data = new
            {
                year = 2025,
                price = 2500,
                cpu = "M3"
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await Client.PostAsync(BaseUrl, content);

        var body = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.IsSuccessStatusCode,
            $"CREATE FAILED\nStatus: {response.StatusCode}\nResponse: {body}");

        using var doc = JsonDocument.Parse(body);

        var id = doc.RootElement
            .GetProperty("id")
            .GetString()!;

        Log($"Object Created | ID: {id}");

        return id;
    }

    [Fact]
    public async Task TC01_Get_All_Objects()
    {
        var response = await Client.GetAsync(BaseUrl);

        var body = await response.Content.ReadAsStringAsync();

        Log($"TC01_Get_All_Objects | Status: {response.StatusCode}");

        Assert.True(
            response.IsSuccessStatusCode,
            $"GET ALL FAILED\n{body}");
    }

    [Fact]
    public async Task TC02_Create_Object()
    {
        var id = await CreateObject();

        Log($"TC02_Create_Object | Created ID: {id}");

        Assert.False(string.IsNullOrWhiteSpace(id));
    }

    [Fact]
    public async Task TC03_Get_Single_Object()
    {
        var id = await CreateObject();

        var response = await Client.GetAsync($"{BaseUrl}/{id}");

        var body = await response.Content.ReadAsStringAsync();

        Log($"TC03_Get_Single_Object | ID: {id} | Status: {response.StatusCode}");

        Assert.True(
            response.IsSuccessStatusCode,
            $"GET SINGLE FAILED\n{body}");
    }

    [Fact]
    public async Task TC04_Update_Object()
    {
        var id = await CreateObject();

        var payload = new
        {
            name = "Updated MacBook Pro",
            data = new
            {
                year = 2026,
                price = 3000
            }
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await Client.PutAsync(
            $"{BaseUrl}/{id}",
            content);

        var body = await response.Content.ReadAsStringAsync();

        Log($"TC04_Update_Object | ID: {id} | Status: {response.StatusCode}");

        Assert.True(
            response.IsSuccessStatusCode,
            $"UPDATE FAILED\n{body}");
    }

    [Fact]
    public async Task TC05_Validate_Object()
    {
        var id = await CreateObject();

        var response = await Client.GetAsync($"{BaseUrl}/{id}");

        var body = await response.Content.ReadAsStringAsync();

        Log($"TC05_Validate_Object | ID: {id} | Status: {response.StatusCode}");

        Assert.True(
            response.IsSuccessStatusCode,
            $"VALIDATE FAILED\n{body}");

        using var doc = JsonDocument.Parse(body);

        Assert.Equal(
            "MacBook Pro",
            doc.RootElement.GetProperty("name").GetString());
    }

    [Fact]
    public async Task TC06_Delete_Object()
    {
        var id = await CreateObject();

        var response = await Client.DeleteAsync($"{BaseUrl}/{id}");

        var body = await response.Content.ReadAsStringAsync();

        Log($"TC06_Delete_Object | ID: {id} | Status: {response.StatusCode}");

        Assert.True(
            response.IsSuccessStatusCode,
            $"DELETE FAILED\n{body}");
    }

    [Fact]
    public async Task TC07_Verify_Delete()
    {
        var id = await CreateObject();

        await Client.DeleteAsync($"{BaseUrl}/{id}");

        var response = await Client.GetAsync($"{BaseUrl}/{id}");

        Log($"TC07_Verify_Delete | ID: {id} | Status: {response.StatusCode}");

        Assert.False(response.IsSuccessStatusCode);
    }
}