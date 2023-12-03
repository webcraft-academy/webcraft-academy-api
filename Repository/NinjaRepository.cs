namespace webcnAPI.Repository;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using webcnAPI.Configurations;
using webcnAPI.Domain;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

public class NinjaRepository : INinjaRepository
{
    private readonly IMongoCollection<Ninja> _ninjaCollection = default!;
    public NinjaRepository(IOptions<WebcraftDBSettings> productDatabaseSetting)
    {
        var mongoClient = new MongoClient(
                productDatabaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(
                productDatabaseSetting.Value.DatabaseName);
            _ninjaCollection = mongoDatabase.GetCollection<Ninja>(
                productDatabaseSetting.Value.ProductCollectionName);
    }
    public  async Task<Ninja> CreateNinja(Ninja ninja)
    {
        Console.Write($"INSIDE REPO: {JsonConvert.SerializeObject(ninja)}");
        await  _ninjaCollection.InsertOneAsync(ninja);
        return ninja;
         
    }

    public Task<Ninja> GetNinjaById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Ninja> GetUserByUsername(string username, string password)
    {
         var user = await _ninjaCollection.Find(u => u.UserName == username && u.PasswordHash == HashPassword(password)).FirstOrDefaultAsync();
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
