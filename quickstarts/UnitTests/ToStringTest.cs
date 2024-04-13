using QuickStart;
using System.Globalization;

namespace UnitTests;

public class ToStringTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void ToStringWithNumberFormatInfoTest()
    {
        WriteLine(3.ToString(new NumberFormatInfo()));
    }
}
