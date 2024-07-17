



function AllowOnlyDecimal(e) {
    e.target.value = e.target.value.replace(/[^0-9.]/g, '');
    var parts = e.target.value.split('.');
    if (parts.length > 2) {
        e.target.value = parts[0] + '.' + parts[1].slice(0, 2);
    }

    if (parts[1] && parts[1].length > 2) {
        e.target.value = parts[0] + '.' + parts[1].slice(0, 2);
    }
}

function CheckIfLessThanBalance() {
    const CurrentValue = QtyInput.value;
    if (CurrentValue > UserBalance) {
        QtyInput.value = UserBalance;
    }
}


