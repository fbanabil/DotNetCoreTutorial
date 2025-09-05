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
    public interface IPersonsDeleterService
    {

        /// <summary>
        /// Delete a person
        /// </summary>
        /// <param name="personId">Guid of PersonId</param>
        /// <returns>True if deletion successfull else returns fale</returns>
        public Task<bool> DeletePerson(Guid? personId);
        
    }
}
