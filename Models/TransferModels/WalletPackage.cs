using ASP_.NET_nauka.Data;
using Microsoft.Identity.Client;

namespace ASP_.NET_nauka.Models;

public class WalletPackage
{
	public User User { get; set; }
	public Wallet Wallet { get; set; }
	public IEnumerable<(Currency currency, decimal Value)>? CurrenciesValues { get; set; }

	public WalletPackage(Wallet wallet, MyDbContext _db)
	{
		this.Wallet = wallet;
		var User = _db.Users.Where(x => x.Id == wallet.UserId).FirstOrDefault();
		if (User == null)
		{
			this.User = new User();
			this.User.Id = -1;
		} else
		{
			this.User = User;
		}
		Calculate(_db);
	}
	private void Calculate(MyDbContext _db)
	{
		IEnumerable<WalletCurrencyValue> WCV = _db.WalletCurrencyValues.Where(WCV => WCV.UserId == Wallet.UserId).ToList();
		CurrenciesValues = from wcv in WCV
						   join currency in _db.Currencies
						   on wcv.CurrencyId equals currency.Id
						   select (currency, wcv.Value);

	}

}
