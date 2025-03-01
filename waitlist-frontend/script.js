// Declaring variables for the form, input fields and submit button
const fullNameInput = document.getElementById('fullname');
const emailInput = document.getElementById('email');
const submitButton = document.getElementById('submitButton');
const waitlistForm = document.getElementById('waitlistForm');
const successImage = document.getElementById('successImage');
const mainImage = document.getElementById('mainImag');
const headingMessage = document.getElementById('headingMessage')

// Disabling button when the form is not filled with necessary data
function checkInputs() {
    if (fullNameInput.value.trim() !== '' && emailInput.value.trim() !== '') {
        submitButton.removeAttribute('disabled');
    } else {
        submitButton.setAttribute('disabled', 'true');
    }
}

fullNameInput.addEventListener('input', checkInputs);
emailInput.addEventListener('input', checkInputs);

// Creating custom validation messages for input fields
fullNameInput.addEventListener('invalid', function(event) {
    event.target.setCustomValidity('Please enter your full name.');
});
fullNameInput.addEventListener('input', function(event) {
    event.target.setCustomValidity('');
});

emailInput.addEventListener('invalid', function(event) {
    event.target.setCustomValidity('Valid email address needed. Jazakumullahu khaeran');
});
emailInput.addEventListener('input', function(event) {
    event.target.setCustomValidity('');
});

// Handling data validation, submission and response.
waitlistForm.addEventListener('submit', async function(event) {
    event.preventDefault();

    fullNameInput.value.trim();
    emailInput.value.trim();

    if (!fullNameInput || !emailInput) {
        alert("Please fill in all required fields.");
        return;
    }

    const response = await fetch('https://dayn-apis-ckfde8a4fxbcbwet.uksouth-01.azurewebsites.net/v1/join-waitlist', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ fullNameInput, emailInput })
    });

    if (response.ok) {
        successImage.style.display = "block";

        mainImage.style.display = "none";

        headingMessage.innerHTML = "🎉 You are on the Waitlist!";
        
        waitlistForm.reset();
        checkInputs();
    } else if (response.status === 409) {
        headingMessage.innerHTML = "✅ This email has already been registered!";

        submitButton.removeAttribute('disabled');
    }else {
        headingMessage.innerHTML = "⚠️ Failed to join the waitlist. Please try again.";
        headingMessage.style.color = "red";
        
        submitButton.removeAttribute('disabled');
    }
});