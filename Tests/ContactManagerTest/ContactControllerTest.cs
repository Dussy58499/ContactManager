using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Models.Domain;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactManager.Controllers;
using System.Linq;

namespace ContactManager.Tests
{
    [TestClass]
    public class ContactControllerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _controller = new ContactController(_contactServiceMock.Object);
        }

        [TestMethod]
        public async Task Index_ReturnsViewResult_WithListOfContacts()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact { Name = "John Doe" },
                new Contact { Name = "Jane Doe" }
            };
            _contactServiceMock.Setup(service => service.GetAllContactsAsync()).ReturnsAsync(contacts);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as IEnumerable<Contact>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public async Task Delete_ReturnsOk_WhenContactIsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            _contactServiceMock.Setup(service => service.DeleteContactAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id) as OkResult;

            // Assert
            Assert.IsNotNull(result);
            _contactServiceMock.Verify(service => service.DeleteContactAsync(id), Times.Once);
        }
    }
}
