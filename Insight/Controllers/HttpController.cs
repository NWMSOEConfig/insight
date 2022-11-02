using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Newtonsoft.Json; 
using Insight.Models;


class HttpController{

    private HttpClient httpClient;

    public HttpController(){
        httpClient=new HttpClient();
    }

    /// <summary>
    /// populateGetRequest uses a url to get all the settings from as JSON, then converts to a List<NewWorldSetting>
    /// </summary>
    /// <param name="url"> The url from which we get our settings </param>
    public async Task<List<NewWorldSetting>>  populateGetRequest(string url){
        if(validPopulateUrl(url)){
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

           List<NewWorldSetting>? settings= await response.Content.ReadFromJsonAsync<List<NewWorldSetting>>();
           if(settings!=null){
               Console.WriteLine(settings.First().Name);
                return settings;
           }else{
                throw new NullReferenceException("HTTP Response was invalid and cannot be deserialized.");
           }
        }
        else{
            throw new ArgumentException("Url is Invalid");
        }
    }

    /// <summary>
    /// validPopulateUrl determines if a url matches our regex standards
    /// </summary>
    /// <param name="url"> The url which we test </param>     
    public bool validPopulateUrl(string url){
        bool valid= false;
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
        if(m.Success){
            valid= true;
        }else{
            valid = false;
    }
    return valid;
    }

}

