// Function to handle the WhatsApp button click
$("#whatsapp-button").click(function () {
    // Get user inputs
    const carModel = $("#car-model").val();
    const pickupDate = $("#pickup-date").val();
    const pickupTime = $("#pickup-time").val();
    const pickupLocation = $("#pickup-location").val();
    const dropoffLocation = $("#dropoff-location").val();

    // Create the WhatsApp message starting with carModel
    let message = `Hi, I would like to rent ${carModel}, is it available?`;

    // Check if other fields are filled and add them to the message
    if (pickupDate) {
        message += `%0A
      Pickup Date: ${pickupDate}`;
    }

    if (pickupTime) {
        message += `%0A
      Pickup Time: ${pickupTime}`;
    }

    if (pickupLocation) {
        message += `%0A
      Pickup Location: ${pickupLocation}`;
    }

    if (dropoffLocation) {
        message += `%0A
      Drop-off Location: ${dropoffLocation}`;
    }

    // Construct the WhatsApp URL
    const whatsappURL = `https://wa.me/+905388782103/?text=${message}`;

    // Open WhatsApp in a new window with the message
    window.open(whatsappURL);
});
