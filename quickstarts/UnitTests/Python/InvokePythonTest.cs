using Python.Runtime;

namespace UnitTests.Python;

public class InvokePythonTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task RunAsync()
    {
        Runtime.PythonDLL = @"C:\edb\languagepack\v4\Python-3.11\python311.dll";

        PythonEngine.Initialize();

        using (Py.GIL())
        {
            // 获取 sys 模块  
            dynamic sys = Py.Import("sys");

            sys.path.append(@"D:\Github\dotnet-quickstart\quickstarts\UnitTests\bin\Debug\net8.0\Python");

            dynamic script = Py.Import("script");

            dynamic resultAdd = script.add(1, 2);
            dynamic resultSubtract = script.subtract(6, 2);

            this.WriteLine(resultAdd);
            this.WriteLine(resultSubtract);
        }

        //启用BinaryFormatter
        //如果你确实需要使用BinaryFormatter，可以通过在应用程序配置文件中启用它。不过，这种方法不推荐，因为BinaryFormatter存在安全风险。
        //<PropertyGroup>  
        //<EnableUnsafeBinaryFormatterSerialization>true</EnableUnsafeBinaryFormatterSerialization>  
        //</PropertyGroup>  
        PythonEngine.Shutdown();
    }
}
