const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

searchInput.addEventListener("input", function () {
    if (searchInput.value.trim() === "") {
        searchButton.disabled = true;
    } else {
        searchButton.disabled = false;
    }
});

function navigateTo(action, ServicesId) {
    window.location.href = `/Admin/Services/${action}?ServicesId=${ServicesId}`;
}

function deleteStaff(ServicesId) {
    if (confirm('Are you sure you want to delete this Services?')) {
        navigateTo('Delete', ServicesId);
    }
}