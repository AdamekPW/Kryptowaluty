"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/CurrenciesHub").build();
let table = document.getElementById('CurrenciesTable');
let tbody = table.getElementsByTagName('tbody')[0];


connection.start();
function FixValue(value) {
    if (value > 10000) {
        return value.toFixed(0);
    } else if (value > 1000) {
        return value.toFixed(1);
    } else if (value > 100) {
        return value.toFixed(2);
    } else if (value > 10) {
        return value.toFixed(3);
    } else
        return value.toFixed(4);
}

connection.on("ReceiveCurrencies", function (currencies) {
    tbody.innerHTML = '';
    //console.log(currencies); // Wyświetlenie całego obiektu currencies w konsoli
    currencies.forEach(function (currency) {
        let row = tbody.insertRow();
        let idCurrency = row.insertCell(0);
        let idValue = row.insertCell(1);
        let id24Low = row.insertCell(2);
        let id24High = row.insertCell(3);
        let idChange = row.insertCell(4);

        var linkElement = document.createElement('a');
        linkElement.className = 'tradeLink';
        linkElement.href = '/Trade/' + currency.id;
        var img = document.createElement('img');
        img.src = '/images/Icons/' + currency.id + '.svg';
        img.width = 26;
        img.style.marginRight = '8px';
        linkElement.appendChild(img);
        linkElement.appendChild(document.createTextNode(currency.code));
        idCurrency.appendChild(linkElement);
        
        idValue.textContent = FixValue(currency.value);
        id24Low.textContent = FixValue(currency.low);
        id24High.textContent = FixValue(currency.high);

        if (currency.change > 0) {
            idChange.textContent = '+' + currency.change.toFixed(2);
            idChange.style.color = 'green';
        } else if (currency.change < 0) {
            idChange.textContent = currency.change.toFixed(2);
            idChange.style.color = 'red';
        } else {
            idChange.textContent = currency.change.toFixed(2);
        }
        idChange.style.fontWeight = 500;
    });

});



