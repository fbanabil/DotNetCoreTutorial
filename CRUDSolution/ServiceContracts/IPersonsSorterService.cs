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
    public interface IPersonsSorterService
    {
        /// <summary>
        /// Sorts the list of persons based on the given attribute and order
        /// </summary>
        /// <param name="personResponses">All Persons</param>
        /// <param name="sortBy">Attribute to sort</param>
        /// <param name="sortOrder">Sort Order</param>
        /// <returns>List of sorted Person Response</returns>
        public Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses,string sortBy, SortOrderOptions sortOrder);
        
    }
}
