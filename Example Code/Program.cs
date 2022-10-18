using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

HttpClient client = new HttpClient();

 HttpResponseMessage response = await client.GetAsync(
                "https://pauat.newworldnow.com/api/applicationsettings");

 response.EnsureSuccessStatusCode();
