using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_nauka.Models;

public partial class CurrencyHistory 
{

	public DateTime Date { get; set; }


	[StringLength(30)]
	public string CurrencyId { get; set; } = null!;


	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "Low value must be non-negative.")]
	public decimal Low { get; set; }

	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "High value must be non-negative.")]
	public decimal High { get; set; }


	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "Open value must be non-negative.")]
	public decimal Open { get; set; }

	[Required]
	[Column(TypeName = "decimal(20, 10)")]
	[Range(0, double.MaxValue, ErrorMessage = "Close value must be non-negative.")]
	public decimal Close { get; set; }


	//nav properties
	public Currency Currency { get; set; } = null!;

}

