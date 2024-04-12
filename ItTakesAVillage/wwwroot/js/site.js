//#region handeAccordionFunction
function handleAccordionClick(notificationId) {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Notification?handler=HandleAccordionClick", true);
    xhr.setRequestHeader("Content-Type", "application/json");

    let antiForgeryToken = document.getElementsByName("__RequestVerificationToken")[0].value;
    xhr.setRequestHeader("RequestVerificationToken", antiForgeryToken);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                let response = JSON.parse(xhr.responseText);
                if (response.success) {

                    let accordionButton = document.querySelector(`button[data-bs-target="#collapse_${notificationId}"]`);

                    accordionButton.classList.remove("fw-bold");

                    updateNotificationLink(response.unreadCount);
                } else {
                    console.error("Server responded with an error:", response);
                }
            } else {
                console.error("AJAX request failed with status:", xhr.status);
            }
        }
    };

    let data = JSON.stringify(notificationId);
    xhr.send(data);
}
//#endregion

//#region UpdateNotificationCount
function updateNotificationLink(unreadCount) {
    let notificationLink = document.getElementById("notification-link");
    if (notificationLink) {
        notificationLink.innerHTML = `
            <i class="bi bi-bell"></i>
            ${unreadCount > 0 ? `<span class="badge bg-danger">${unreadCount}</span>` : ''}
        `;
    }
}
//#endregion

//#region TooltipFunction
let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
let tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})
//#endregion

//#region ValidateSelectList    
    function validateSelection() {
        let selectedValue1 = document.getElementById("selectList1").value;
        let selectedValue2 = document.getElementById("selectList2").value;

        if (selectedValue1 === "" || selectedValue2 === "") {
            alert("Vänligen välj ett alternativ från varje lista.");
        } else {
            document.forms[0].submit();
        }
    }
//#endregion ValidateSelectList
