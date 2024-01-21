const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

searchInput.addEventListener("input", function () {
    if (searchInput.value.trim() === "") {
        searchButton.disabled = true;
    } else {
        searchButton.disabled = false;
    }
});

function navigateTo(action, ServiceTypeId) {
    window.location.href = `/Admin/ServiceType/${action}?ServiceTypeId=${ServiceTypeId}`;
}

function deleteStaff(ServiceTypeId) {
    if (confirm('Are you sure you want to delete this ServiceType?')) {
        navigateTo('Delete', ServiceTypeId);
    }
}