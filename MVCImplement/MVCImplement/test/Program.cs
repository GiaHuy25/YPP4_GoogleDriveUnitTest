using System;

public class TestStatic
{
    // Trường tĩnh
    public static int StaticField = 0;

    // Thuộc tính tĩnh
    public static string StaticProperty { get; set; } = "Initial";

    // Phương thức tĩnh
    public static void StaticMethod()
    {
        Console.WriteLine("Static method called!");
    }

    // Static constructor
    static TestStatic()
    {
        Console.WriteLine("Static constructor called!");
        StaticField = 42;
        StaticProperty = "Set by static constructor";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Main started!");

        // Trường hợp 1: Truy cập trường tĩnh
        Console.WriteLine("Accessing StaticField...");
        Console.WriteLine($"StaticField value: {TestStatic.StaticField}");

        // Trường hợp 2: Truy cập thuộc tính tĩnh
        Console.WriteLine("\nAccessing StaticProperty...");
        Console.WriteLine($"StaticProperty value: {TestStatic.StaticProperty}");

        // Trường hợp 3: Gọi phương thức tĩnh
        Console.WriteLine("\nCalling StaticMethod...");
        TestStatic.StaticMethod();
    }
}