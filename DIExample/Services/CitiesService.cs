using ServiceContracts;
namespace Services
{

    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;
        private Guid _seviceInstanceId;
        public Guid ServiceInstanceId { 
            get
            {
                return _seviceInstanceId;
            }
        } 
        public CitiesService()
        {
            _cities = new List<string>()
            {
                "New York",
                "Los Angeles",
                "Chicago",
                "Houston",
                "Phoenix",
                "Philadelphia",
                "San Antonio",
                "San Diego",
                "Dallas",
                "San Jose"
            };
            _seviceInstanceId = Guid.NewGuid();
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
