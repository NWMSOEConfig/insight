namespace Insight.Models;
public class NewWorldSetting{

    public string Name { get; private set; }
    public List<Parameter>? Parameters  { get; set; }
    public Category? Category { get; set; }
    public Tenant? Tenant{ get; set; }

    public NewWorldSetting(string name){
        Name=name;

    }
}