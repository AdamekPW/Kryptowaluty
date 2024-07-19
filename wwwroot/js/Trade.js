"use strict";
var CurrencyConnection = new signalR.HubConnectionBuilder().withUrl("/CurrenciesHub").build();

const TradeUpbarContainer = document.getElementById("TradeUpbarContainer");


CurrencyConnection.start();
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

function AddHeader(text) {
    const header = document.createElement('div');
    header.innerText = text;
    header.className = 'TradeUpbarContainerBox';
    TradeUpbarContainer.appendChild(header);
}

function AddChange(base) {
    const headerBox = document.createElement('div');
    headerBox.className = 'TradeUpbarContainerBox';

    const Change = document.createElement('div');
    Change.className = 'TradeUpbarContainerBox';
    Change.innerHTML = 'Change:&nbsp';

    const header = document.createElement('div');
    header.className = 'TradeUpbarContainerBox';
    headerBox.appendChild(Change);
    if (Cur.Currency.Change > 0) {
        header.innerText = ' +' + base.toFixed(2);
        header.style.color = 'green';
    } else if (Cur.Currency.Change < 0) {
        header.innerText = ' ' + base.toFixed(2);
        header.style.color = 'red';
    } else {
        header.innerText = '  ' + base.toFixed(2);
    }
    header.innerText += '%';
    headerBox.appendChild(header);
    TradeUpbarContainer.appendChild(headerBox);
}


AddHeader(Cur.Currency.Name + ' [USD]');
AddHeader('Value: ' + Cur.Currency.Value);
AddHeader('Low: ' + Cur.Currency.Low);
AddHeader('High: ' + Cur.Currency.High);
AddChange(Cur.Currency.Change);




CurrencyConnection.on("ReceiveCurrencies", function (currencies) {
    TradeUpbarContainer.innerHTML = '';
    currencies.forEach(function (currency) {
        if (currency.id === Cur.Currency.Id) {
            AddHeader(currency.name + ' [USD]');
            AddHeader('Value: ' + currency.value);
            AddHeader('Low: ' + currency.low);
            AddHeader('High: ' + currency.high);
            AddChange(currency.change);
            return;
        }
    });

});










var LineData = [
    /*{
        x: new Date(1538778600000),
        y: 6604
    }, {
        x: new Date(1538782200000),
        y: 6602
    }, {
        x: new Date(1538814600000),
        y: 6607
    }, {
        x: new Date(1538884800000),
        y: 6620
    }*/
]


 
var options = {
    series: [{
        name: 'Candle',
        type: 'candlestick',
        data: CandleData
    }],
    chart: {
        height: "100%",
        width: "100%",
        type: 'candlestick',
    },
    
    tooltip: {
        shared: true,
        custom: function ({ seriesIndex, dataPointIndex, w }) {
            var o = w.globals.seriesCandleO[seriesIndex][dataPointIndex];
            var h = w.globals.seriesCandleH[seriesIndex][dataPointIndex];
            var l = w.globals.seriesCandleL[seriesIndex][dataPointIndex];
            var c = w.globals.seriesCandleC[seriesIndex][dataPointIndex];
            return (
                '<div>' +
                '<div>Open: <strong>' + o + '</strong></div>' +
                '<div>High: <strong>' + h + '</strong></div>' +
                '<div>Low: <strong>' + l + '</strong></div>' +
                '<div>Close: <strong>' + c + '</strong></div>' +
                '</div>'
            );
        }
    },
    xaxis: {
        type: 'datetime',
        labels: {
            formatter: function (value) {
                return new Date(value).toLocaleDateString('pl-PL', {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric'
                });
            }
        }
    }
};


var chart = new ApexCharts(document.querySelector("#chart"), options);
chart.render();

