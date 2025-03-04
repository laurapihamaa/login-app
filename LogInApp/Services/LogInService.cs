using System.Text.Json;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.Http.Headers;


public class LogInService{

    public async Task<bool> loginUser (string username, string password){

        try {
            string jsonFile = "resources/users.json";

            var users = await ParseJsonFile(jsonFile);

            var user = users
                        .FirstOrDefault(e=>e.username.Equals(username));
            
            return user.password.Equals(password);

        }catch (Exception e){
            throw new Exception("Failed to verify user. Please try again");
        }
    }


    public async Task<List<User>> ParseJsonFile(string json)
    {
        try{
            string jsonFile = await File.ReadAllTextAsync(json);

            var users = JsonSerializer.Deserialize<List<User>>(jsonFile);

            return users ?? new List<User>();

        }catch(FileNotFoundException e){
            throw new FileNotFoundException("The file was not found.");
        }

    }
}