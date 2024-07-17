using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ASP_.NET_nauka.Models;

public partial class WalletCurrencyValue
{
	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	public decimal Value { get; set; }

	[Required]
	public int UserId { get; set; }

	[Required]
	[StringLength(30)]
	public string CurrencyId { get; set; } = null!;

	//nav properties
	[JsonIgnore]
	public Wallet Wallet { get; set; } = null!;

	[JsonIgnore]
	public Currency Currency { get; set; } = null!;


	public WalletCurrencyValue() { }
	public WalletCurrencyValue(string currencyId)
	{
		Value = 0;
		UserId = -1;
		CurrencyId = currencyId;
	}
}
