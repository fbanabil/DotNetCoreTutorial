using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PersonsRepository : IPersonsRepository
    {

        private readonly ApplicationDbContext _db;

        public PersonsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {
            await _db.Persons.AddAsync(person);
            await _db.SaveChangesAsync();
            return person; 
        }

        public async Task<bool> DeletePersonByPersonId(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp => temp.PersonId == personID));
            int rows = await _db.SaveChangesAsync();
            return rows > 0;

        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include(temp=>temp.Country).ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include(temp=>temp.Country).Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonID(Guid personID)
        {
            return await _db.Persons.Include(temp=>temp.Country).FirstOrDefaultAsync(temp => temp.PersonId == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? existingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonId == person.PersonId);

            if (existingPerson == null)
            {
                return person;
            }

            existingPerson.PersonName = person.PersonName;
            //existingPerson.TIN = person.TIN;
            existingPerson.DateOfBirth = person.DateOfBirth;
            existingPerson.CountryID = person.CountryID;
            existingPerson.Email = person.Email;
            person.Address = person.Address;
            existingPerson.RecieveNewsLetters = person.RecieveNewsLetters;
            existingPerson.Gender = person.Gender;
            
            await _db.SaveChangesAsync();

            return existingPerson;
        }
    }
}
