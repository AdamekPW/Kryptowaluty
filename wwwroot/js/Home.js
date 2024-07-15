var connection = new signalR.HubConnectionBuilder().withUrl("/CurrenciesHub").build();
const currTable = document.getElementById("CurrTable");
const tbody = currTable.getElementsByTagName('tbody')[0];
connection.start();


connection.on("ReceiveCurrencies", function (currencies) {
    tbody.innerHTML = '';
    currencies.sort((a, b) => b.change - a.change);
    if (currencies.length < 5) return;
    for (var i = 0; i < 5; i++) {
        let currency = currencies[i];
        let row = tbody.insertRow();
        let idCurrency = row.insertCell(0);
        let idValue = row.insertCell(1);
        let idChange = row.insertCell(2);
        var img = document.createElement("img");
        img.src = "/images/Icons/" + currency.id + ".svg";
        img.width = 25;
        img.style.marginRight = "8px";

        idCurrency.appendChild(img);
        idCurrency.appendChild(document.createTextNode(currency.code));

        if (currency.value > 10000) {
            idValue.textContent = currency.value.toFixed(0);
        } else if (currency.value > 1000) {
            idValue.textContent = currency.value.toFixed(1);
        } else if (currency.value > 100) {
            idValue.textContent = currency.value.toFixed(2);
        } else if (currency.value > 10) {
            idValue.textContent = currency.value.toFixed(3);
        } else 
            idValue.textContent = currency.value.toFixed(4);
        
        
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
    }


});


