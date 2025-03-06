using System.Text;
using System.Security.Cryptography;


public class LogInService{

    private readonly JsonParserService _jsonParserService;
    private readonly PasswordEncryptionService _passwordEncryptionService;

    public LogInService(JsonParserService jsonParserService, PasswordEncryptionService passwordEncryptionService)
    {
        _jsonParserService = jsonParserService;
        _passwordEncryptionService=passwordEncryptionService;
    }

    public async Task<bool> loginUser (string username, string password){

        try {
            var users = await _jsonParserService.ParseJsonFile();

            var user = users
                        .FirstOrDefault(e=>e.username.Equals(username));
            
            var decryptedPassword = await _passwordEncryptionService.DecryptPassword(user.password, user.salt, password);         
            return decryptedPassword.Equals(password);

        }catch (Exception e){
            Console.WriteLine(e.Message);
            throw new Exception("Failed to verify user. Please try again");
        }
    }

    public async Task<bool> registerUser (string username, string password){

        try{
            byte[] salt = _passwordEncryptionService.CreateSalt(16);
            var encryptedPassword = await _passwordEncryptionService.EncryptPassword(password, salt);
            await _jsonParserService.WriteToJson(username, encryptedPassword, salt);
            
            return true;

        }catch (Exception){
            throw new Exception("Failed to create user. Please try again.");
        }
    }


}