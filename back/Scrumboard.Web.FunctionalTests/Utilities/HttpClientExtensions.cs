using System.Text;
using Scrumboard.Shared.TestHelpers.Serializations;

namespace Scrumboard.Web.FunctionalTests.Utilities;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(
        this HttpClient client, 
        string requestUrl, 
        T payload) where T : notnull
    {
        var stringContent = new StringContent(
            SerializationHelper.Serialize(payload),
            Encoding.UTF8, "application/json");
        
        return await client.PostAsync(requestUrl, stringContent);
    }
    
    public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(
        this HttpClient client, 
        string requestUrl, 
        T payload) where T : notnull
    {
        var stringContent = new StringContent(
            SerializationHelper.Serialize(payload),
            Encoding.UTF8, "application/json");
        
        return await client.PutAsync(requestUrl, stringContent);
    }
}
