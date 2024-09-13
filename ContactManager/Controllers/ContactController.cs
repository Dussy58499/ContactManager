using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models.Domain;
using Service.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ContactManager.Controllers
{
    [Route("Contacts")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return View(contacts);
        }

        [HttpPost("UploadCsv")]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {
            await _contactService.UploadCsvAsync(csvFile);
            return RedirectToAction(nameof(Index));
        }

        [HttpPut("Save/{id}")]
        public async Task<IActionResult> Save(Guid id, [FromBody] Contact updatedContact)
        {
            try
            {
                if (updatedContact == null)
                {
                    return BadRequest("Contact data is null.");
                }

                await _contactService.UpdateContactAsync(id, updatedContact);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _contactService.DeleteContactAsync(id);
            return Ok();
        }

        [HttpGet("Error")]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }
    }
}
