using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ASP_.NET_nauka.Models;

public partial class Currency
{
	[Key]
	[StringLength(30)]
	public string Id { get; set; } = null!;

	[Required]
	[StringLength(40)]
	public string Name { get; set; } = null!;

	[Required]
	[StringLength(10)]
	public string Code { get; set; } = null!;

	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "Value must be non-negative.")]
	public decimal Value { get; set; }

	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "Low must be non-negative.")]
	public decimal Low { get; set; }

	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "High must be non-negative.")]
	public decimal High { get; set; }

	[Required]
	[Column(TypeName = "decimal(4, 2)")]
	public decimal Change { get; set; }

	[Required]
	public DateTime Date { get; set; }


	//nav properties
	public IEnumerable<WalletCurrencyValue> WalletCurrencyValues { get; set; } = null!;
	public IEnumerable<CurrencyHistory> CurrenciesHistories { get; set; } = null!;
}
