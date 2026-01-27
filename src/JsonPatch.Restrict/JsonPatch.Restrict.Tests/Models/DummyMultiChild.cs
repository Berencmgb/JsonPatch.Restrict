namespace JsonPatch.Restrict.Tests.Models;

public class DummyMultiChild
{
    public int Id { get; set; }
    public Child1 Child1 { get; set; }
}

public class Child1
{
    public Child2 Child2 { get; set; }
}

public class Child2
{
    public string Value { get; set; }
}