namespace webcnAPI.Repository;
using webcnAPI.Domain;
public interface INinjaRepository
{
    Task<Ninja> CreateNinja(Ninja user);
    Task<Ninja> GetNinjaById(string id);
    Task<Ninja> GetUserByUsername(string username, string password);
}