namespace IdentityByController.Models;

public interface IStoreRepository
{
    IQueryable<Product> Products { get; }
}