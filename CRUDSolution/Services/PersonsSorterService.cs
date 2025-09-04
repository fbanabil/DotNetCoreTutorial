using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsSorterService : IPersonsSorterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsSorterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsSorterService(IPersonsRepository personsRepository, ILogger<PersonsSorterService> logger, IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> personResponses, string sortBy, SortOrderOptions sortOrder)
        {
            _logger.LogInformation("GetSortedPersons method of PersonsService called");

            if (string.IsNullOrEmpty(sortBy) || personResponses == null || personResponses.Count == 0)
            {
                return personResponses;
            }

            // Sort the personResponses based on the sortBy parameter

            List<PersonResponse> sortedResponse = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.PersonName,StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) 
                    => personResponses.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),


                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) 
                    => personResponses.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.ASC)
                    => personResponses.OrderBy(p => p.RecieveNewsLetters).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.DESC)
                    => personResponses.OrderByDescending(p => p.RecieveNewsLetters).ToList(),

                _=> personResponses
            };

            return sortedResponse;
        }

    }
}
