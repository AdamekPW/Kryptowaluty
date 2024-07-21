using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace ASP_.NET_nauka.Models;

public partial class CompletedOrder
{
    [Key]
    public long id { get; set; }

    [Required]
    [Column(TypeName = "decimal(20, 10)")]
    [Range(0, double.MaxValue, ErrorMessage = "Qty must be non-negative.")]
    public decimal Qty { get; set; }

    [Required]
    [Column(TypeName = "decimal(20, 10)")]
    [Range(0, double.MaxValue, ErrorMessage = "QtyUSDT must be non-negative.")]
    public decimal QtyUSDT { get; set; }

    [Required]
    public DateTime DateOfIssue { get; set; }

    [Required]
    public DateTime EndDate {  get; set; }

    [Required]
    public bool IsBuyer { get; set; }


    //Foreign keys
    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(30)]
    public string CurrencyId { get; set; } = null!;


    //nav properties
    [JsonIgnore]
    public Currency Currency { get; set; } = null!;

    [JsonIgnore]
    public User User { get; set; } = null!;


    public CompletedOrder() { }
    public CompletedOrder(ActiveOrder activeOrder, DateTime endDate)
    {
        Qty = Math.Round(activeOrder.Qty, 7);
        QtyUSDT = Math.Round(activeOrder.QtyUSDT, 3);
        DateOfIssue = activeOrder.DateOfIssue;
        EndDate = endDate;
        UserId = activeOrder.UserId;
        CurrencyId = activeOrder.CurrencyId;
        IsBuyer = activeOrder.IsBuyer;

    }
}
