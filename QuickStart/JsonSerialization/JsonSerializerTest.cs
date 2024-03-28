namespace QuickStart.JsonSerialization;

public class JsonSerializerTest
{
    [Fact]
    public void Serialize()
    {
        User user = new(1, "test");
        //{
        //    Id = 1,
        //    Name = "test"
        //};

        string json = user.AsJson();

        User? deserializedUser = json.FromJson<User>();

        Assert.NotNull(deserializedUser);
        Assert.Equal(1, deserializedUser.Id);
        Assert.Equal("test", deserializedUser.Name);
    }
}
