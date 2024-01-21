const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

searchInput.addEventListener("input", function () {
    if (searchInput.value.trim() === "") {
        searchButton.disabled = true;
    } else {
        searchButton.disabled = false;
    }
});

function navigateTo(action, ProductTypeId) {
    window.location.href = `/Admin/ProductType/${action}?ProductTypeId=${ProductTypeId}`;
}

function deleteStaff(ProductTypeId) {
    if (confirm('Are you sure you want to delete this ProductType?')) {
        navigateTo('Delete', ProductTypeId);
    }
}