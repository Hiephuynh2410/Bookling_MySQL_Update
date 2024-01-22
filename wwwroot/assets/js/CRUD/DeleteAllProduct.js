    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('selectAll').addEventListener('click', function () {
            var checkboxes = document.querySelectorAll('.productCheckbox');
            checkboxes.forEach(function (checkbox) {
                checkbox.checked = this.checked;
            }, this);
        });

        var checkboxes = document.querySelectorAll('.productCheckbox');
        checkboxes.forEach(function (checkbox) {
            checkbox.addEventListener('click', function () {
                var anyChecked = document.querySelectorAll('.productCheckbox:checked').length > 0;
                document.getElementById('deleteSelected').disabled = !anyChecked;
            });
        });

        document.getElementById('deleteSelected').addEventListener('click', function () {
            var selectedProductIds = Array.from(document.querySelectorAll('.productCheckbox:checked')).map(function (checkbox) {
                return checkbox.getAttribute('data-product-id');
            });

            if (selectedProductIds.length > 0) {
                console.log('Selected Product IDs:', selectedProductIds);
                deleteSelectedProducts(selectedProductIds);
            } else {
                console.log('No products selected.');
            }
        });

        function deleteSelectedProducts(productIds) {
            var apiUrl = '/api/ProductApi/deleteAll';

            fetch(apiUrl, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(productIds),
            })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('');
                }
            })
            .then(result => {
                if (result.success) {
                    alert(result.message);
                    location.reload();
                } else {
                    alert('Failed to delete products: ' + result.message);
                }
            })
            .catch(error => {
                console.error('Error deleting products:', error);
                alert('An error occurred while deleting products.');
            });
        }
    });
