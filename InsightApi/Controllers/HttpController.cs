using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json; 
using InsightApi.Models;


class HttpController{

    private HttpClient httpClient;

    public HttpController(){
        httpClient=new HttpClient();
    }

    public async Task populateGetRequest(string url){
        if(validPopulateUrl(url)){
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            jsonToSetting(response);
        }
        else{
            throw new ArgumentException("Url is Invalid");
        }
    }

    public async void jsonToSetting(HttpResponseMessage response){
          if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json"){
                var contentStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);
                JsonSerializer serializer = new JsonSerializer();
                try{
                    serializer.Deserialize<Setting[]>(jsonReader);
                }
                catch(JsonReaderException){
                    Console.WriteLine("Invalid JSON.");
                } 
            }
            else
            {
                Console.WriteLine("HTTP Response was invalid and cannot be deserialized.");
            }
    }
        
    public bool validPopulateUrl(string url){
        bool valid= false;
        //Removing any extraneous white space
        url=url.Replace(" ", "");
        //Replace annoying characters with spaces so we can use a simpler pattern
        url=url.Replace(".", " ").Replace("/", " ");

        //pattern to match against
        string pattern = "(?:https:  )?[a-zA-Z] newworldnow com api applicationsettings ";  
        //Create a Regex  
        Regex rg = new Regex(pattern);  

        //Match pattern against url    
        Match m = Regex.Match(url, pattern, RegexOptions.IgnoreCase);
        if(m.Success){
            Console.WriteLine("Regex succesful"); 
            valid= true;
        }else{
            valid = false;
    }
    return valid;
    }



}

