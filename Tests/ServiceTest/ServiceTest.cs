using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using Repository.Models.Domain;
using Service.Interfaces;
using Service.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace ContactManager.Tests
{
    [TestClass]
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _repositoryMock;
        private readonly IContactService _service;

        public ContactServiceTests()
        {
            _repositoryMock = new Mock<IContactRepository>();
            _service = new ContactService(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task UploadCsvAsync_BAD()
        {
            // Arrange
            IFormFile nullFile = null;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _service.UploadCsvAsync(nullFile));
        }

        [TestMethod]
        public async Task UploadCsvAsync_OK()
        {
            // Arrange
            var csvFileMock = new Mock<IFormFile>();
            var fileContent = "Name;DateOfBirth;Married;Phone;Salary\nJohn Doe;1990-01-01;true;1234567890;1000,00";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
            csvFileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            csvFileMock.Setup(f => f.Length).Returns(stream.Length);

            // Act
            await _service.UploadCsvAsync(csvFileMock.Object);

            // Assert
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateContactAsync_OK()
        {
            // Arrange
            var id = Guid.NewGuid();
            var contact = new Contact { Name = "Updated Name" };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Contact)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _service.UpdateContactAsync(id, contact));
        }

        [TestMethod]
        public async Task UpdateContactAsync_OldData_OK()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existingContact = new Contact { Name = "Old Name" };
            var updatedContact = new Contact { Name = "Updated Name" };

            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingContact);

            // Act
            await _service.UpdateContactAsync(id, updatedContact);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Contact>(c => c.Name == "Updated Name")), Times.Once);
        }

        [TestMethod]
        public async Task DeleteContactAsync_OK()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _service.DeleteContactAsync(id);

            // Assert
            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [TestMethod]
        public async Task GetAllContactsAsync_OK()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Name = "Contact 1" },
                new Contact { Name = "Contact 2" }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(contacts);

            // Act
            var result = await _service.GetAllContactsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}
