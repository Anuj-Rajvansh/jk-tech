using DocManagementSystem.Reposetories.Interface;
using DocManagementSystem.Services.Implementation;
using DocManagementSystem.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocManagementSystem.Test.Services
{
    public class DocumentServiceTest
    {
        private readonly Mock<IDocumentRepository> _mockRepo;
        private readonly DocumentService _service;

        public DocumentServiceTest()
        {
            _mockRepo = new Mock<IDocumentRepository>();
            var logger = new LoggerFactory().CreateLogger<DocumentService>();
            _service = new DocumentService(_mockRepo.Object, logger);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDocuments()
        {
            var docs = new List<Document>
            {
                new Document { Id = 1, Name = "Doc1" },
                new Document { Id = 2, Name = "Doc2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(docs);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, ((List<Document>)result).Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsDocument_WhenExists()
        {
            var doc = new Document { Id = 10, Name = "Doc10" };

            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(doc);

            var result = await _service.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal("Doc10", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Document)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_CreatesAndReturnsDocument()
        {
            var input = new Document { Name = "New Doc", Content = new byte[10] };
            var created = new Document { Id = 5, Name = "New Doc", Content = new byte[10] };

            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Document>())).ReturnsAsync(created);

            var result = await _service.CreateAsync(input);

            Assert.Equal("New Doc", result.Name);
            Assert.Equal(new byte[10], result.Content);
            Assert.Equal(5, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesDocument_WhenExists()
        {
            var existing = new Document { Id = 3, Name = "Old", Content = new byte[10] };
            var updateDto = new Document { Name = "Updated", Content = new byte[10] };

            _mockRepo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Document>())).Returns(Task.CompletedTask);

            await _service.UpdateAsync(3, updateDto);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<Document>(d =>
                d.Id == 3 &&
                d.Name == "Updated" &&
                d.Content == new byte[10]
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsKeyNotFoundException_WhenDocumentNotFound()
        {
            var updateDto = new Document { Name = "Updated", Content = new byte[10] };
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Document)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99, updateDto));
        }

        [Fact]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            _mockRepo.Setup(r => r.DeleteAsync(8)).Returns(Task.CompletedTask);

            await _service.DeleteAsync(8);

            _mockRepo.Verify(r => r.DeleteAsync(8), Times.Once);
        }
    }
}
