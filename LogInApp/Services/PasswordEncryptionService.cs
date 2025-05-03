using System.Security.Cryptography;
using System.Text;

public class PasswordEncryptionService{

    public static byte[] CreateKey (string password, byte[] salt){
        
    var iterations = 1000;
    var desiredKeyLength = 16; 
    var hashMethod = HashAlgorithmName.SHA384;
    //the key uses a PBKDF2-library for hashing
    return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                     salt,
                                     iterations,
                                     hashMethod,
                                     desiredKeyLength);
    }

    public byte[] CreateSalt (int length){
        //salt is generated using random bytes
        using var rng = new RNGCryptoServiceProvider();
        byte [] randomBytes = new byte[length];

        rng.GetBytes(randomBytes);
        return randomBytes;
    }

    /*
    Password are encrypted before storing
    */
    public async Task<byte[]> EncryptPassword (string password, byte[] salt){
        using Aes aes = Aes.Create();

        aes.Key = CreateKey(password, salt);
        aes.IV = CreateSalt(16);

        using MemoryStream output = new();
        using (CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write)){

        await cryptoStream.WriteAsync(salt);
        await cryptoStream.WriteAsync(aes.IV);
        
        byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
        await cryptoStream.WriteAsync(passwordBytes, 0, passwordBytes.Length);
        await cryptoStream.FlushFinalBlockAsync();}

        return output.ToArray();
    }

    public async Task<string> DecryptPassword (byte[] encryptedPassword, byte[] salt, string password){
        using Aes aes = Aes.Create();

        aes.Key = CreateKey(password, salt);

        byte[] iv = new byte[16];

        Array.Copy(encryptedPassword, 16, iv, 0, 16);

        aes.IV = iv;

        using MemoryStream input = new(encryptedPassword[32..]);
        using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);

        using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);

        return Encoding.Unicode.GetString(output.ToArray());
    }

}