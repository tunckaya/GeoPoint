using GenericRepositoryApp.Models;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Point> Points { get; }
    IGenericRepository<AnotherEntity> AnotherEntities { get; }
    Task<int> CompleteAsync(); // SaveChanges işlemleri buradan yönetilecek
}
