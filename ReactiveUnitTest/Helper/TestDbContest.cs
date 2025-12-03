using System;
using Microsoft.EntityFrameworkCore;
using Persistence;



namespace ReactiveUnitTest.Helper;

public class TestDbContest : IDisposable
{
    private readonly AppDbContext _context;
    public TestDbContest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDatabase")
        .Options;
        _context = new AppDbContext(options);

    }

    public AppDbContext GetContext() => _context;
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
