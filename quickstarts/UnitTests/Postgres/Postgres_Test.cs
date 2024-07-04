using Npgsql;

namespace UnitTests.Postgres;

public class Postgres_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task TestConnection()
    {
        var connectionString = "Host=localhost;UserName=postgres;Password=p@ssword;Database=Test";

        // 创建连接对象  
        using (var conn = new NpgsqlConnection(connectionString))
        {
            // 打开连接  
            conn.Open();

            // 执行查询  
            using (var cmd = new NpgsqlCommand("SELECT * FROM items", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }
        }
    }
}
