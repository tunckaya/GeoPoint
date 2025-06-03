using GenericRepositoryApp.Data;
using GenericRepositoryApp.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class PointServiceTests : System.IDisposable
{
    private readonly AppDbContext _context;
    private readonly PointService _service;

    public PointServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        var unitOfWork = new UnitOfWork(_context);
        _service = new PointService(unitOfWork);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetPointByIdAsync_ReturnsPoint_WhenExists()
    {
        var point = new Point { PointX = 1, PointY = 2, Name = "Test" };
        await _service.AddPointAsync(point);

        var result = await _service.GetPointByIdAsync(point.Id);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.Name);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddPointAsync_PersistsPoint()
    {
        var point = new Point { PointX = 3, PointY = 4, Name = "Add" };

        await _service.AddPointAsync(point);

        var exists = await _context.Points.AnyAsync(p => p.Id == point.Id);
        Assert.True(exists);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeletePointAsync_RemovesPoint()
    {
        var point = new Point { PointX = 5, PointY = 6, Name = "Delete" };
        await _service.AddPointAsync(point);

        await _service.DeletePointAsync(point.Id);

        var removed = await _context.Points.FindAsync(point.Id);
        Assert.Null(removed);
    }
}
