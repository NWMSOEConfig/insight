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

    public async Task<List<Setting>>  populateGetRequest(string url){
        if(validPopulateUrl(url)){
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());
           return await jsonToSetting(response);
        }
        else{
            throw new ArgumentException("Url is Invalid");
        }
    }

    public async Task<List<Setting>> jsonToSetting(HttpResponseMessage response){
         List<Setting> list = new List<Setting>();
        if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json"){
            var contentStream = await response.Content.ReadAsStreamAsync();
            using var streamReader = new StreamReader(contentStream);
           using var jsonReader = new JsonTextReader(streamReader);
            JsonSerializer serializer = new JsonSerializer();
            try{
                list=serializer.Deserialize<List<Setting>>(jsonReader);
                Console.WriteLine(list.First());
            }
            catch(JsonReaderException){
                Console.WriteLine("Invalid JSON.");
            } 
        }
        else
        {
            Console.WriteLine("HTTP Response was invalid and cannot be deserialized.");
        }
        if(list!=null){
             return list;
        }else{
            throw new NullReferenceException("New World Site did not return valid data.");
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
            valid= true;
        }else{
            valid = false;
    }
    return valid;
    }

}

