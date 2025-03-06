public class User{
    public string? username {get; set;}
    public byte[] password {get; set;}
    public byte[] salt {get; set;}
}
public class UserList
    {

        public List<User> users {get; set;} = new List<User>();
    }