using System;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as a return type for most of
    /// countries service methods.
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }

        // Overriding the Equals method to ensure object value matvhing
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if(obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse? countryResponse = (CountryResponse)obj;

            return this.CountryID == countryResponse.CountryID;
        }


        // Overriding the GetHashCode method to ensure object value matching
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
