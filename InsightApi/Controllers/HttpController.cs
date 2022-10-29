using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
// See https://aka.ms/new-console-template for more information


class HttpController{

    private HttpClient httpClient;

    public HttpController(){
        httpClient=new HttpClient();
    }

    public void populateGetRequest(string url){
        if(validPopulateUrl(url)){
        HttpResponseMessage response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        Console.WriteLine(await response.Content.ReadAsStringAsync());
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

