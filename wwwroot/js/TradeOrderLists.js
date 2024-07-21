"use strict";

const Buyers = document.getElementById("Buyers");
const Sellers = document.getElementById("Sellers");

var OrderConnection = new signalR.HubConnectionBuilder().withUrl("/OrdersHub").build();
OrderConnection.start();

function CreateBuyOrdersTable(Orders) {
    Buyers.innerHTML = '';
    Orders.forEach(order => {
        var RowDiv = document.createElement('div');
        RowDiv.className = "BuyersRow";

        var QtyUSDT = document.createElement('div');
        QtyUSDT.className = "Box";
        if (order.QtyUSDT)
            QtyUSDT.innerText = order.QtyUSDT;
        else
            QtyUSDT.innerText = order.qtyUSDT;
        RowDiv.appendChild(QtyUSDT);

        var Qty = document.createElement('div');
        Qty.className = "Box";
        if (order.Qty)
            Qty.innerText = order.Qty;
        else 
            Qty.innerText = order.qty;
        RowDiv.appendChild(Qty);

        Buyers.appendChild(RowDiv);
      
    });
}

function CreateSellOrdersTable(Orders) {
    Sellers.innerHTML = '';
    Orders.forEach(order => {
        var RowDiv = document.createElement('div');
        RowDiv.className = "SellersRow";

        var Qty = document.createElement('div');
        Qty.className = "Box";
        if (order.Qty)
            Qty.innerText = order.Qty;
        else
            Qty.innerText = order.qty;
        RowDiv.appendChild(Qty);

        var QtyUSDT = document.createElement('div');
        QtyUSDT.className = "Box";
        if (order.QtyUSDT)
            QtyUSDT.innerText = order.QtyUSDT;
        else
            QtyUSDT.innerText = order.qtyUSDT;
        RowDiv.appendChild(QtyUSDT);

        Sellers.appendChild(RowDiv);
        
    });
}

CreateBuyOrdersTable(Cur.CompletedBuyOrders);
CreateSellOrdersTable(Cur.CompletedSellOrders);

OrderConnection.on(Cur.Currency.Id + "/Buyers", CreateBuyOrdersTable);
OrderConnection.on(Cur.Currency.Id + "/Sellers", CreateSellOrdersTable);






