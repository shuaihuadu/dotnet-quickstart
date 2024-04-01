namespace QuickStart.JsonSerialization;

public abstract class UserBase
{
    public UserBase()
    {

    }

    public UserBase(int id)
    {
        this.Id = id;
    }
    public int Id { get; set; }
}

public class User : UserBase
{
    public User() { }

    public User(int id, string name) : base(id)
    {
        this.Name = name;
    }

    public string Name { get; set; } = string.Empty;
}
