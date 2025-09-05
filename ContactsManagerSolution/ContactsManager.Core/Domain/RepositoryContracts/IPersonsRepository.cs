using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    public interface IPersonsRepository
    {
        /// <summary>
        /// Adds new Person to the data source
        /// </summary>
        /// <param name="person">Person object</param>
        /// <returns>Added object</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Gets all Persons from the data source
        /// </summary>
        /// <returns>List of Persons</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// Gets Person by PersonID
        /// </summary>
        /// <param name="personID">GUID of person</param>
        /// <returns>Person object</returns>
        Task<Person?> GetPersonByPersonID(Guid personID);


        /// <summary>
        /// Gets filtered persons based on the given predicate
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching personws</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person,bool> > predicate);

        /// <summary>
        /// Deletes Person by PersonID
        /// </summary>
        /// <param name="personID">Guid</param>
        /// <returns>Boolean value</returns>
        Task<bool> DeletePersonByPersonId(Guid personID);


        /// <summary>
        /// Updates existing Person in the data source
        /// </summary>
        /// <param name="person">Person Object</param>
        /// <returns>Updatedc Person Object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
