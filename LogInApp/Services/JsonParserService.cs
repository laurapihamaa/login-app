using System.Text.Json;
using System.Threading.Tasks;

public class JsonParserService {

    string _jsonFile = "resources/users.json";

    public async Task<List<User>> ParseJsonFile()
    {
        try{
            string jsonFile = await File.ReadAllTextAsync(_jsonFile);

            var users = JsonSerializer.Deserialize<List<User>>(jsonFile);

            return users ?? new List<User>();

        }catch(FileNotFoundException e){
            throw new FileNotFoundException("The file was not found.");
        }

    }

    public async Task<bool> WriteToJson(string username, byte[] password, byte[] salt)
    {
        try{

            var userObj = new User{username = username, password = password, salt=salt};

            string jsonFile = await File.ReadAllTextAsync(_jsonFile);

            var users = JsonSerializer.Deserialize<List<User>>(jsonFile);

            users.Add(userObj);

            var jsonObj = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_jsonFile, jsonObj);

            return true;

        }catch(Exception e){
            Console.WriteLine(e.Message);
            throw new FileNotFoundException("The file was not found.");
        }

    }
}