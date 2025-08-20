using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Buisness logics for person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a Person
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns>Returns PersonResponse class</returns>
        public Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Gets All Persons
        /// </summary>
        /// <returns>List of PersonResponse class</returns>
        public Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Gets a person by person id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>Matching person object as person response</returns>
        public Task<PersonResponse?> GetPersonByPersonId(Guid? personId);


        /// <summary>
        /// Returns all matching response
        /// </summary>
        /// <param name="searchBy">Attribute on which filter applies</param>
        /// <param name="searchString">Matches with atrribute fields</param>
        /// <returns>List of personResponse</returns>
        public Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);
    }
}
