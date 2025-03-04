public class User{
    public string? username {get; set;}
    public string? password {get; set;}
}
public class UserList
    {

        public List<User> users {get; set;} = new List<User>();
    }