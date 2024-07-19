"use strict";
var OrderConnection = new signalR.HubConnectionBuilder().withUrl("/OrdersHub").build();

OrderConnection.start();

function CreateOrdersTable(Orders) {
    console.log(Orders);
    Orders.forEach(order => {
        console.log(order);
    });
}

OrderConnection.on(Cur.Currency.Id, CreateOrdersTable);







