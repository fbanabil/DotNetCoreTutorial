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
    public interface IPersonsAdderService
    {
        /// <summary>
        /// Adds a Person
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns>Returns PersonResponse class</returns>
        public Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        
    }
}
