using WebApi.Models;

namespace WebApi.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Create(string name, string description, int price);
        bool Update(int id, string name, string description, int price);
    }
}
