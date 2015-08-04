using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using ContactManagement.Models;
using ContactManagement.ViewModel;

namespace ContactManagement.Services
{
    public interface IContactService
    {
        Task<int> AddContactAsyn(Contact contact);
        Task<IEnumerable<ContactListViewModel>> GetAllContactsAsyn();
        Task<IEnumerable<ContactListViewModel>> GetAllContactsAsyn(string name, string mobile);
        Task<Contact> GetContactAsyn(int id);
        Task<int> EditContactAsyn(Contact contact);
        Task<int> DeleteContactAsyn(Contact contact);
    }

    public class ContactService : IContactService,IDisposable
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ContactService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<int> AddContactAsyn(Contact contact)
        {
            contact.CreateDate = DateTime.UtcNow;
            contact.UpdateDate = DateTime.UtcNow;
            _applicationDbContext.Contacts.Add(contact);
            return await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ContactListViewModel>> GetAllContactsAsyn()
        {
            var listOfContacts = await _applicationDbContext.Contacts.Project().To<ContactListViewModel>().ToListAsync();
            return listOfContacts;
        }

        public async Task<IEnumerable<ContactListViewModel>> GetAllContactsAsyn(string name, string mobile)
        {
            
            var listOfContacts =
                await _applicationDbContext.Contacts
                .Where(c =>
                        (string.IsNullOrEmpty(name)||c.FirstName.Contains(name)) 
                    && (string.IsNullOrEmpty(mobile) || c.Mobile.Contains(mobile) ))
                .Project()
                .To<ContactListViewModel>()
                .ToListAsync();
            return listOfContacts;

        }

        public async Task<int> EditContactAsyn(Contact contact)
        {
          _applicationDbContext.Entry(contact).State=EntityState.Modified;
           return await _applicationDbContext.SaveChangesAsync();
            
        }

        public async Task<Contact> GetContactAsyn(int id)
        {
            var contact = await _applicationDbContext.Contacts.FindAsync(id);
            return contact;
        }

        public async Task<int> DeleteContactAsyn(Contact contact)
        {
            
            _applicationDbContext.Contacts.Remove(contact);
            return await _applicationDbContext.SaveChangesAsync();
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if (!_disposed)
            {
                _applicationDbContext.Dispose();
                _disposed = true;
            }
        }
    }
}