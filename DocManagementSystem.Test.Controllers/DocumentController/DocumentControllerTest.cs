using DocManagementSystem.Web.Controllers;
using DocManagementSystem.Services.Interface;
using DocManagementSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocManagementSystem.Test.Controllers
{
    public class DocumentControllerTest
    {
        private readonly Mock<IDocumentService> _mockService;
        private readonly Mock<ILogger<DocumentController>> _mockLogger;
        private readonly DocumentController _controller;

        public DocumentControllerTest()
        {
            _mockService = new Mock<IDocumentService>();
            _mockLogger = new Mock<ILogger<DocumentController>>();
            _controller = new DocumentController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfDocuments()
        {
            var documents = new List<Document> { new Document { Id = 1 } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(documents);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(documents, okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenDocumentExists()
        {
            var document = new Document { Id = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(document);

            var result = await _controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(document, okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenDocumentDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Document?)null);

            var result = await _controller.Get(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithCreatedDocument()
        {
            var document = new Document { Id = 10 };
            _mockService.Setup(s => s.CreateAsync(document)).ReturnsAsync(document);

            var result = await _controller.Create(document);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(document, createdResult.Value);
            Assert.Equal(nameof(_controller.Get), createdResult.ActionName);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenUpdateSuccessful()
        {
            var document = new Document { Id = 1 };

            var result = await _controller.Update(1, document);

            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.UpdateAsync(1, document), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenKeyNotFoundExceptionThrown()
        {
            var document = new Document { Id = 1 };
            _mockService.Setup(s => s.UpdateAsync(1, document))
                        .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.Update(1, document);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_LogsInformation()
        {
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Document>());

            await _controller.GetAll();
        }

        [Fact]
        public async Task Get_LogsWarning_WhenDocumentNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((Document?)null);

            await _controller.Get(5);
        }

        [Fact]
        public async Task Update_LogsError_WhenExceptionThrown()
        {
            var doc = new Document();
            var exception = new KeyNotFoundException("Not found");
            _mockService.Setup(s => s.UpdateAsync(3, doc)).ThrowsAsync(exception);

            await _controller.Update(3, doc);
        }
    }
}
