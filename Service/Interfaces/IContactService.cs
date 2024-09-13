using Microsoft.AspNetCore.Http;
using Repository.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IContactService
    {
        Task UploadCsvAsync(IFormFile csvFile);
        Task UpdateContactAsync(Guid id, Contact updatedContact);
        Task DeleteContactAsync(Guid id);
        Task<IEnumerable<Contact>> GetAllContactsAsync();
    }
}
