const FirstnameInput = document.getElementById("firstname");
const LastnameInput = document.getElementById("lastname");
const BirthdateContainer = document.getElementById("BirthdateContainer");
const BirthdateInput = document.getElementById("datePicker");
const PasswordInput1 = document.getElementById("password1");
const PasswordInput2 = document.getElementById("password2");
const Button = document.getElementById("submitButton");
const Info = document.getElementById("PasswordMessage");
const strengthIndicator = document.getElementById("strengthIndicator");
const strengthMessage = document.getElementById("strengthMessage");
const emailInput = document.getElementById("email");

function CheckFirstname() {
    if (FirstnameInput.value === "") {
        FirstnameInput.style.border = "1px solid #ccc";
    } else {
        FirstnameInput.style.border = "1px solid green";
    }
}

function CheckLastname() {
    if (LastnameInput.value === "") {
        LastnameInput.style.border = "1px solid #ccc";
    } else {
        LastnameInput.style.border = "1px solid green";
    }
}

function CheckBirthdate() {
    if (BirthdateInput.value !== "") {
        BirthdateContainer.style.border = "1px solid green";
    } else {
        BirthdateContainer.style.border = "1px solid #ccc";
    }
}

function CheckPasswords() {
    var PInput1 = PasswordInput1.value;
    var PInput2 = PasswordInput2.value;

    if (PInput1 === "" || PInput2 === "") {
        Info.textContent = "";
        Button.disabled = true;
        PasswordInput1.style.border = "1px solid #ccc";
        PasswordInput2.style.border = "1px solid #ccc"
    } else if (PInput1 === PInput2 && PInput2 !== "") {
        Info.textContent = "Passwords are the same";
        Info.style.color = "green";
        Button.disabled = false;
        PasswordInput1.style.border = "1px solid green"
        PasswordInput2.style.border = "1px solid green"
    } else {
        Info.textContent = "Passwords are different";
        Info.style.color = "red";
        Button.disabled = true;
        PasswordInput1.style.border = "1px solid red"
        PasswordInput2.style.border = "1px solid red"
    }
}

function checkPasswordStrength() {
    var password = PasswordInput1.value;
    var strength = 0;

    if (password.length >= 8) strength += 1;               // Długość hasła
    if (/[A-Z]/.test(password)) strength += 1;             // Wielkie litery
    if (/[a-z]/.test(password)) strength += 1;             // Małe litery
    if (/[0-9]/.test(password)) strength += 1;             // Cyfry
    if (/[\W]/.test(password)) strength += 1;              // Znaki specjalne

 
    switch (strength) {
        case 0:
            strengthIndicator.style.width = "0%";
            strengthIndicator.style.backgroundColor = "white";
            strengthMessage.textContent = "Strength:";
            break;
        case 1:
            strengthIndicator.style.width = "20%";
            strengthIndicator.style.backgroundColor = "red";
            strengthMessage.textContent = "Strength: very week";
            break;
        case 2:
            strengthIndicator.style.width = "40%";
            strengthIndicator.style.backgroundColor = "orange";
            strengthMessage.textContent = "Strength: week";
            break;
        case 3:
            strengthIndicator.style.width = "60%";
            strengthIndicator.style.backgroundColor = "yellow";
            strengthMessage.textContent = "Strength: midium";
            break;
        case 4:
            strengthIndicator.style.width = "80%";
            strengthIndicator.style.backgroundColor = "blue";
            strengthMessage.textContent = "Strength: strong";
            break;
        case 5:
            strengthIndicator.style.width = "100%";
            strengthIndicator.style.backgroundColor = "green";
            strengthMessage.textContent = "Strength: very strong";
            break;
    }
}

function validateEmail() {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (emailInput.value === "") {
        emailInput.style.border = "1px solid #ccc";
    } else if (!re.test(emailInput.value)) {
        emailInput.style.border = "1px solid red";
    } else {
        emailInput.style.border = "1px solid green";
    }
}

FirstnameInput.addEventListener("input", CheckFirstname);
LastnameInput.addEventListener("input", CheckLastname);
BirthdateInput.addEventListener("input", CheckBirthdate);
emailInput.addEventListener("input", validateEmail)
PasswordInput1.addEventListener("input", CheckPasswords);
PasswordInput2.addEventListener("input", CheckPasswords);
PasswordInput1.addEventListener("input", checkPasswordStrength);