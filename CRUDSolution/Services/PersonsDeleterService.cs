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
    public class PersonsDeleterService : IPersonsDeleterService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsDeleterService(IPersonsRepository personsRepository,ILogger<PersonsDeleterService> logger,IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _personsRepository.GetPersonByPersonID(personId.Value);

            if (person == null)
            {
                return false;
            }

            return await _personsRepository.DeletePersonByPersonId(personId.Value);
        }

    }
}
