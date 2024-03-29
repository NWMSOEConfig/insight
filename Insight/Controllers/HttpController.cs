using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Insight.Models;
using System.Text;

class HttpController
{
    public record ParameterWithName(string Name, string Value);
    private HttpClient httpClient;

    public HttpController(HttpMessageHandler? handler = null)
    {
        if (handler is null)
            httpClient = new HttpClient();
        else
            httpClient = new HttpClient(handler); // for mocks
    }

    /// <summary>
    /// PopulateGetRequest uses a url to get all the settings from as JSON, then converts to a List<NewWorldSetting>
    /// </summary>
    /// <param name="url"> The url from which we get our settings </param>
    public async Task<List<NewWorldSetting>> PopulateGetRequest(string url)
    {
        if (ValidPopulateUrl(url))
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            List<ParameterWithName>? parameters = await response.Content.ReadFromJsonAsync<List<ParameterWithName>>();
            List<NewWorldSetting>? settings = parameters?.Select(parameter => new NewWorldSetting(parameter.Name)
            {
                Parameters = new() { new Parameter(parameter.Value) },
            }).ToList();

            if (settings != null)
            {
                return settings;
            }
            else
            {
                throw new NullReferenceException("HTTP Response was invalid and cannot be deserialized.");
            }
        }
        else
        {
            throw new ArgumentException("Url is Invalid");
        }
    }

    public void MakePostRequest(QueuedChange changes, string url)
    {

        string jsonString = JsonConvert.SerializeObject(changes.Settings.ToString());
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        Console.WriteLine(jsonString);

        //var request = await httpClient.PostAsync(url, content);

    }

    /// <summary>
    /// ValidPopulateUrl determines if a url matches our regex standards
    /// </summary>
    /// <param name="url"> The url which we test </param>     
    public bool ValidPopulateUrl(string url)
    {
        bool valid = false;
        //Removing any extraneous white space
        url=url.Replace(" ", "");
        //Replace annoying characters with spaces so we can use a simpler pattern
        url=url.Replace(".", " ").Replace("/", " ");

        //pattern to match against
        string pattern = "(?:https:  )?[a-zA-Z] newworldnow com v7 api applicationsettings ";  
        //Create a Regex  
        Regex rg = new Regex(pattern);  

        //Match pattern against url    
        Match m = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
        {
            valid = true;
        }
        else
        {
            valid = false;
        }
    return valid;
    }

}

