namespace ASP_.NET_nauka.Models;

public partial class TradeChartPackage
{
	public Currency Currency { get; set; } = null!;
	public User User { get; set; } = null!;
	public List<CurrencyHistory> CurrencyHistory { get; set; } = null!;
	public WalletCurrencyValue WalletCurrencyValue { get; set; } = null!;
	public IEnumerable<CompletedOrder> CompletedBuyOrders { get; set; }
	public IEnumerable<CompletedOrder> CompletedSellOrders { get; set; }
	public TradeChartPackage()
	{
		User = new();
		User.Id = -1;
		User.USDT_balance = 0;
        CompletedBuyOrders = new List<CompletedOrder>();
		CompletedSellOrders = new List<CompletedOrder>();
	}

}
