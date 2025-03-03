document.getElementById("waitlistForm").addEventListener("submit", function (event) {
    event.preventDefault();

    let formData = {
        fullname: document.getElementById("fullname").value,
        email: document.getElementById("email").value
    };

    fetch("https://dayn-apis-ckfde8a4fxbcbwet.uksouth-01.azurewebsites.net/v1/join-waitlist", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(formData)
    })
    .then(response => response.json())
    .then(data => {
        console.log("Success:", data);

        let mainImage = document.getElementById("mainImage");
        if (mainImage) {
            mainImage.style.display = "none";
        }

        let verifiedImage = document.getElementById("verifiedImage");
        if (verifiedImage) {
            verifiedImage.style.display = "block";
        }

        let subtextMessage = document.getElementById("subtextMessage");
        if (subtextMessage) {
            subtextMessage.textContent = "You’re in! Welcome to the future of Shari’ah-compliant, secure, and ethical financial documentation. Stay tuned for exclusive updates and next steps!"
        }

        let waitlistFormInput = document.getElementById("waitlistFormInput");
        if (waitlistFormInput) {
            waitlistFormInput.style.display = "none";
        }

        let heading = document.getElementById("headingMessage");
        if (heading) {
            heading.textContent = "🎉 You have successfully joined the Waitlist!";
        }
    })
    .catch(error => console.error("Error:", error));
});