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
    public interface IPersonsUpdaterService
    {
        /// <summary>
        /// Update details of person
        /// </summary>
        /// <param name="personUpdateRequest">Includes PersonId</param>
        /// <returns>PersonResponse</returns>
        public Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        
    }
}
