using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
Waluty: (Currencies)
nazwa, skrót, obecna wartość, 24h change (%), 24h low, 24h high
*/

namespace ASP_.NET_nauka.Models
{
	public class Currency
	{
		[Key]
		public string Id { get; set; } = null!;

		[Required]
		public string Name { get; set; } = null!;

		[Required]
		public string Code { get; set; } = null!;

		public double Value { get; set; }

		public double Change {  get; set; }

		public double Low { get; set; }

		public double High { get; set; }


	}
}
