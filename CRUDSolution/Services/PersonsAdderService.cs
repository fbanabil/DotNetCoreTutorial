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
    public class PersonsAdderService : IPersonsAdderService
    {

        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Initialize _persons in the constructor to avoid null reference
        public PersonsAdderService(IPersonsRepository personsRepository,ILogger<PersonsAdderService> logger,IDiagnosticContext diagnosticContext)
        {
            _diagnosticContext = diagnosticContext;
            _logger = logger;
            _personsRepository = personsRepository;
        }
    
        //Person to person resoponse method

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //Null argument
            if(personAddRequest==null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Null Person name
            if(string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("Person can't be blank");
            }

            //Model Validation
            ValidationHelper.ModelValidate(personAddRequest);


            // Create person id
            Person person = personAddRequest.ToPerson();

            //Assign Guid
            person.PersonId = Guid.NewGuid();

            //Add person
            await _personsRepository.AddPerson(person);
            //await _db.SaveChangesAsync();

            //_db.sp_InsertPerson(person);

            //Return person
            return person.ToPersonResponse();
        }

      
    }
}
