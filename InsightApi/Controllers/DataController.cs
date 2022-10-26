using InsightApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace InsightApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    private static readonly IList<Parameter> _parameters = new List<Parameter>
    {
        new(0, "Enabled", "boolean", true),
        new(1, "Foo", "number", 123),
        new(2, "Bar", "text", "Text"),
        new(3, "Baz", "email", "a@b.com"),
    };

    private static readonly IList<Setting> _settings = new List<Setting>
    {
        new(0, "Foo", new List<int> { 0, 1 }),
        new(1, "Bar", new List<int> { 2 }),
        new(2, "Baz", new List<int> { 3 }),
    };

    private static readonly IList<Subcategory> _subcategories = new List<Subcategory>
    {
        new(0, "Subcategory 1", new List<int> { 0 }),
        new(1, "Subcategory 2", new List<int> { 1 }),
        new(2, "Subcategory 3", new List<int> { 2 })
    };

    private static readonly IList<Category> _categories = new List<Category>
    {
        new(0, "Category A", new List<int> { 0 }),
        new(1, "Category B", new List<int> { 1 }),
        new(2, "Category C",  new List<int> { 2 })
    };

    private static readonly IList<Tenant> _tenants = new List<Tenant>
    {
        new(0, "State", new List<int> { 0, 1, 2}),
    };

    private static readonly List<Parameter> _queue = new();

    [HttpGet]
    [Route("parameter")]
    public IActionResult GetParameter(int id)
    {
        return id < 0 || id >= _parameters.Count ? BadRequest() : Ok(_parameters[id]);
    }

    [HttpGet]
    [Route("setting")]
    public IActionResult GetSetting(int id)
    {
        return id < 0 || id >= _settings.Count ? BadRequest() : Ok(_settings[id]);
    }

    [HttpGet]
    [Route("subcategory")]
    public IActionResult GetSubcategory(int id)
    {
        return id < 0 || id >= _subcategories.Count ? BadRequest() : Ok(_subcategories[id]);
    }

    [HttpGet]
    [Route("category")]
    public IActionResult GetCategory(int id)
    {
        return id < 0 || id >= _categories.Count ? BadRequest() : Ok(_categories[id]);
    }

    [HttpGet]
    [Route("tenant")]
    public IActionResult GetTenant(int id)
    {
        return id < 0 || id >= _tenants.Count ? BadRequest() : Ok(_tenants[id]);
    }

    [HttpPost]
    [Route("queue")]
    public IActionResult PostQueue([FromBody] IList<Parameter> parameters)
    {
        _queue.AddRange(parameters);

        return Ok($"queued {parameters.Count} changes, now at {_queue.Count}");
    }

    [HttpPost]
    [Route("populate")]
    public IActionResult PostQueue([FromBody] string url)
    {
        //clean string
        url=url.Replace(" ", "");
        url=url.Replace(".", " ").Replace("/", " ");
        string pattern = "(?:https:  )?[a-zA-Z] newworldnow com api applicationsettings ";  
        //Create a Regex  
        Regex rg = new Regex(pattern);  
        
        Match m = Regex.Match(url, pattern, RegexOptions.IgnoreCase);

        if(m.Success){
            Console.WriteLine("Regex succesful"); 
        }

        return Ok($"Url {url} is valid");
    }
}
