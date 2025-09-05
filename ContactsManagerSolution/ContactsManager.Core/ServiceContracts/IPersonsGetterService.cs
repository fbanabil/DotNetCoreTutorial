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
    public interface IPersonsGetterService
    {
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
