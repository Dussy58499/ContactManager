using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Repository.Interfaces;
using Repository.Models.Domain;
using Service.Interfaces;

namespace Service.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;

        public ContactService(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task UploadCsvAsync(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                throw new ArgumentException("File is required");
            }

            else
            {
                using var reader = new StreamReader(csvFile.OpenReadStream());
                string headerLine = await reader.ReadLineAsync(); //Use it when you have HeaderLine or commit it if not!!!!(Name,DateofBirth.. e.g)

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(';'); //My csv file with , was with ; so if your have , just change ";" to ","

                    var name = values[0];
                    var dateOfBirth = DateTime.Parse(values[1]);
                    var married = bool.Parse(values[2]);
                    var phone = values[3];
                    var salary = decimal.Parse(values[4]);

                    var existingContact = await _repository.FindByConditionAsync(c =>
                        c.Name == name &&
                        c.DateOfBirth == dateOfBirth &&
                        c.Married == married &&
                        c.Phone == phone &&
                        c.Salary == salary);

                    if (existingContact != null)
                    {
                        continue;
                    }

                    var contact = new Contact
                    {
                        Name = name,
                        DateOfBirth = dateOfBirth,
                        Married = married,
                        Phone = phone,
                        Salary = salary
                    };

                    await _repository.AddAsync(contact);
                }
            }

        }

        public async Task UpdateContactAsync(Guid id, Contact updatedContact)
        {
            var contact = await _repository.GetByIdAsync(id);

            if (contact == null)
            {
                throw new Exception("Contact not found");
            }

            else
            {
                contact.Name = updatedContact.Name;
                contact.DateOfBirth = updatedContact.DateOfBirth;
                contact.Married = updatedContact.Married;
                contact.Phone = updatedContact.Phone;
                contact.Salary = updatedContact.Salary;

                await _repository.UpdateAsync(contact);
            }
        }
        public async Task DeleteContactAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
