namespace QuickStart.JsonSerialization;

public class JsonSerializerTest
{
    [Fact]
    public void Serialize()
    {
        User user = new(1, "test");

        string json = user.AsJson();

        User? deserializedUser = json.FromJson<User>();

        Assert.NotNull(deserializedUser);
        Assert.Equal(1, deserializedUser.Id);
        Assert.Equal("test", deserializedUser.Name);
    }

    [Fact]
    public void Deserialize()
    {
        string userJosn = "userJson";

        User? user = userJosn.FromJson<User>();

        Assert.Null(user);
    }
}
