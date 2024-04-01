namespace SourceCodeGeneraterApp;

partial class Program
{
    static void Main(string[] args)
    {
        HelloFrom("Generated code");
    }

    static partial void HelloFrom(string name);
}