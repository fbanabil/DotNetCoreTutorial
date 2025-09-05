using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
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
    public class PersonsUpdaterService : IPersonsUpdaterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsUpdaterService(IPersonsRepository personsRepository,ILogger<PersonsUpdaterService> logger,IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }
    

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest==null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            if(personUpdateRequest.PersonName==null)
            {
                throw new ArgumentException(nameof(personUpdateRequest.PersonName));
            }

            ValidationHelper.ModelValidate(personUpdateRequest);

            Person? personResponse = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonId);

            if (personResponse==null)
            {
                throw new InvalidPersonIDException("Given PersonID doesn't exist");
            }


            personResponse.PersonName = personUpdateRequest.PersonName;
            personResponse.Email = personUpdateRequest.Email;
            personResponse.DateOfBirth = personUpdateRequest.DateOfBirth;
            personResponse.Gender = personUpdateRequest.Gender.ToString();
            personResponse.CountryID = personUpdateRequest.CountryID;
            personResponse.Address = personUpdateRequest.Address;
            personResponse.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;

            await _personsRepository.UpdatePerson(personResponse);

            return personResponse.ToPersonResponse();
        }

    }
}
