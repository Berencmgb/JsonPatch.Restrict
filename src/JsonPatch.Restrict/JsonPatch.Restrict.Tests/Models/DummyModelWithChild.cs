namespace JsonPatch.Restrict.Tests.Models;

public class DummyModelWithChild
{
    public int Id { get; set; }
    public Child Child { get; set; }
}

public class Child
{
    public string Value { get; set; }
}