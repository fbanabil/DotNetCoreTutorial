

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPersonStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                CREATE PROCEDURE [dbo].[InserPerson]
                (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(100), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryID uniqueidentifier, @Address nvarchar(200), @RevieveNewsLetters bit)
                AS BEGIN
                   INSERT INTO [dbo].[Persons](PersonId, PersonName, Email,DateOfBirth, Gender,CountryID,Address,RecieveNewsLetters) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth , @Gender, @CountryID, @Address, @RevieveNewsLetters)
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                DROP PROCEDURE [dbo].[InserPerson]
                (@PersonId uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(100), @DateOfBirth datetime2(7), @Gender varchar(10), @CountryID uniqueidentifier, @Address nvarchar(200), @RevieveNewsLetters bit)
                AS BEGIN
                   INSERT INTO [dbo].[Persons](PersonId, PersonName, Email,DateOfBirth, Gender,CountryID,Address,RecieveNewsLetters) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth , @Gender, @CountryID, @Address, @RevieveNewsLetters)
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
