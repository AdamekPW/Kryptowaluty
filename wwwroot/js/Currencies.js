"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/CurrenciesHub").build();
let table = document.getElementById('CurrenciesTable');
let tbody = table.getElementsByTagName('tbody')[0];


connection.start();

connection.on("ReceiveCurrencies", function (currencies) {
    tbody.innerHTML = '';
    console.log(currencies); // Wyświetlenie całego obiektu currencies w konsoli
    currencies.forEach(function (currency) {
        let row = tbody.insertRow();
        let idCurrency = row.insertCell(0);
        let idValue = row.insertCell(1);
        let id24Low = row.insertCell(2);
        let id24High = row.insertCell(3);
        let idChange = row.insertCell(4);
        idCurrency.textContent = currency.code;
        idValue.textContent = currency.value.toFixed(3);
        id24Low.textContent = currency.low.toFixed(3);
        id24High.textContent = currency.high.toFixed(3);
        idChange.textContent = currency.change;
    });

});



