using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_.NET_nauka.Models;

public partial class WalletCurrencyValue
{
	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	public decimal Value;

	[Required]
	public int UserId { get; set; }

	[Required]
	[StringLength(30)]
	public string CurrencyId { get; set; } = null!;

	//nav properties
	public Wallet Wallet { get; set; } = null!;
	public Currency Currency { get; set; } = null!;
}
