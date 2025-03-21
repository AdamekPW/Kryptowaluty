﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_.NET_nauka.Models;

public partial class Wallet
{
	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	public decimal TotalValue { get; set; }

	[Required]	
	public DateTime latestUpdate { get; set; }

	[Required]
	public int UserId { get; set; }

	//nav properties
	public User User { get; set; } = null!;
	public IEnumerable<WalletCurrencyValue> WalletCurrencyValue { get; set; } = null!;
}
