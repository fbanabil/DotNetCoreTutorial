using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
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

        /// <summary>
        /// Sorts the list of persons based on the given attribute and order
        /// </summary>
        /// <param name="personResponses">All Persons</param>
        /// <param name="sortBy">Attribute to sort</param>
        /// <param name="sortOrder">Sort Order</param>
        /// <returns>List of sorted Person Response</returns>
        public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses,string sortBy, SortOrderOptions sortOrder);

    
        /// <summary>
        /// Update details of person
        /// </summary>
        /// <param name="personUpdateRequest">Includes PersonId</param>
        /// <returns>PersonResponse</returns>
        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);


        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="personId">Guid of PersonId</param>
        /// <returns>True if deletion successfull else returns fale</returns>
        public Task<bool> DeletePerson(Guid? personId);


        /// <summary>
        /// Returns persons as csv
        /// </summary>
        /// <returns>csv memory stresm</returns>
        Task<MemoryStream> GetPersonCSV();

        /// <summary>
        /// Returens persons as excel
        /// </summary>
        /// <returns>Memory stream</returns>
        Task<MemoryStream> GetPersonsExcel();


        
    }
}
