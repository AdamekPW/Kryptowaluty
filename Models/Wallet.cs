using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_.NET_nauka.Models
{
	public class Wallet
	{
		[Key]
		[Column(Order = 0)]
		public int Id { get; set; }

		[Column(Order = 1)]
		public double Bitcoin {  get; set; }

		[Column(Order = 2)]
		public double Solana { get; set; }




	}
}
