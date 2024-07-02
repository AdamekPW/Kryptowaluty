﻿using Microsoft.AspNetCore.SignalR;

namespace ASP_.NET_nauka.Hubs;

public class CurrenciesHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}