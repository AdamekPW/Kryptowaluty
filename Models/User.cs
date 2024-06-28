using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
	Dla użytkownika: (User)
	ID, Imię, nazwisko, Data urodzenia, mail, hasło (hashowane), nr konta bankowego ? , telefon ? 
*/
namespace ASP_.NET_nauka.Models
{
	public class User
	{
		[Key]
		public int Id {  get; set; }

		[Required]
		public string FirstName { get; set; } = null!;

		[Required]
		public string SecondName { get; set; } = null!;
		
		[Required]
		public DateTime BirthDate { get; set; }

		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		[Required]
		public string Role {  get; set; } = null!;

	}
}
