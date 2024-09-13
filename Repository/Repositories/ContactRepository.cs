using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interfaces;
using Repository.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactDbContext _repository;

        public ContactRepository(ContactDbContext repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _repository.Contacts.ToListAsync();
        }

        public async Task<Contact> GetByIdAsync(Guid Id)
        {
            return await _repository.Contacts.FindAsync(Id);
        }

        public async Task<Contact> FindByConditionAsync(Expression<Func<Contact, bool>> expression)
        {
            return await _repository.Contacts.FirstOrDefaultAsync(expression);
        }

        public async Task AddAsync(Contact contact)
        {
            _repository.Contacts.Add(contact);

            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _repository.Contacts.Update(contact);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contact = await _repository.Contacts.FindAsync(id);

            if (contact != null)
            {
                _repository.Contacts.Remove(contact);

                await _repository.SaveChangesAsync();
            }
        }
    }
}