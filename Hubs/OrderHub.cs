using ASP_.NET_nauka.Data;
using ASP_.NET_nauka.Models;
using Microsoft.AspNetCore.SignalR;

namespace ASP_.NET_nauka.Hubs;

public class OrderHub : Hub
{
    public async Task SendBuyers(IEnumerable<CompletedOrder> orders)
    {
        await Clients.All.SendAsync("ReceiveBuyers", orders);
    }
}
