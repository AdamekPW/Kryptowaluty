namespace ASP_.NET_nauka.Models;

public partial class TradeChartPackage
{
	public Currency Currency { get; set; } = null!;
	public List<CurrencyHistory> CurrencyHistory { get; set; } = null!;

}
