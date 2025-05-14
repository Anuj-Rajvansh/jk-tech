using DocManagementSystem.Reposetories.Implementation;
using DocManagementSystem.Shared.Data;
using DocManagementSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DocManagementSystem.Tests.Repositories.Implementation
{
    public class DocumentRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDocuments()
        {
            var context = GetDbContext();
            context.Documents.Add(new Document { Id = 1, Name = "Doc1" });
            context.Documents.Add(new Document { Id = 2, Name = "Doc2" });
            await context.SaveChangesAsync();

            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            var result = await repo.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDocument_WhenExists()
        {
            var context = GetDbContext();
            context.Documents.Add(new Document { Id = 10, Name = "Doc10" });
            await context.SaveChangesAsync();

            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            var result = await repo.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("Doc10", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            var context = GetDbContext();
            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            var result = await repo.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_AddsNewDocument()
        {
            var context = GetDbContext();
            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            var doc = new Document { Name = "NewDoc" };
            var created = await repo.CreateAsync(doc);

            Assert.NotNull(created);
            Assert.NotEqual(default, created.Id);
            Assert.Equal("NewDoc", created.Name);

            var saved = await context.Documents.FindAsync(created.Id);
            Assert.NotNull(saved);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesDocument()
        {
            var context = GetDbContext();
            var original = new Document { Id = 5, Name = "Old Name" };
            context.Documents.Add(original);
            await context.SaveChangesAsync();

            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            original.Name = "Updated Name";
            await repo.UpdateAsync(original);

            var updated = await context.Documents.FindAsync(5);
            Assert.Equal("Updated Name", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesDocument_WhenExists()
        {
            var context = GetDbContext();
            context.Documents.Add(new Document { Id = 8, Name = "ToDelete" });
            await context.SaveChangesAsync();

            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            await repo.DeleteAsync(8);

            var result = await context.Documents.FindAsync(8);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_DoesNothing_WhenNotExists()
        {
            var context = GetDbContext();
            var repo = new DocumentRepository(context, new NullLogger<DocumentRepository>());

            var countBefore = await context.Documents.CountAsync();
            await repo.DeleteAsync(999); // Non-existing
            var countAfter = await context.Documents.CountAsync();

            Assert.Equal(countBefore, countAfter);
        }
    }
}
