const searchInput = document.getElementById("searchInput");
const searchButton = document.getElementById("searchButton");

searchInput.addEventListener("input", function () {
    if (searchInput.value.trim() === "") {
        searchButton.disabled = true;
    } else {
        searchButton.disabled = false;
    }
});

function navigateTo(action, ProductId) {
    window.location.href = `/Admin/Product/${action}?ProductId=${ProductId}`;
}

function deleteStaff(ProductId) {
    if (confirm('Are you sure you want to delete this Product?')) {
        navigateTo('Delete', ProductId);
    }
}