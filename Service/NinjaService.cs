namespace webcnAPI.Service;
using webcnAPI.Domain;
using webcnAPI.Repository;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;


public class NinjaService
{
    private readonly INinjaRepository _ninjaRepository;

    public NinjaService(INinjaRepository ninjaRepository)
    {
        _ninjaRepository = ninjaRepository;
    }

    public  async Task<Ninja> RegisterUser(Ninja ninja)
    {
        ninja.PasswordHash = HashPassword(ninja.PasswordHash);
        await _ninjaRepository.CreateNinja(ninja);
        return ninja;
    }

    public async Task<Ninja> LoginUser(string username, string password)
    {
        var user = await _ninjaRepository.GetUserByUsername(username, password);
        return user;
    }



    private string HashPassword(string password)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }

}
public class UserCredentials
{
    public string Username { get; set; } 
    public string Password { get; set; } 
}