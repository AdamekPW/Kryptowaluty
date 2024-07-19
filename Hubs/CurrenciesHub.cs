using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.SignalR;

namespace ASP_.NET_nauka.Hubs;

public class CurrenciesHub : Hub
{ 
    public async Task SendCurrencies(IEnumerable<Currency> Currencies)
    {
        await Clients.All.SendAsync("ReceiveCurrencies", Currencies);
    }
}