

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.add-to-fav-button').forEach(button => {
        button.addEventListener('click', async function () {
            const productId = this.getAttribute('data-product-id');

            try {
                const response = await fetch('/Wishlist/Add', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-Requested-With': 'XMLHttpRequest'
                    },
                    body: JSON.stringify({ productId: parseInt(productId) }) // Wrap productId in an object
                });

                const result = await response.json();

                if (response.ok) {
                    Swal.fire({
                        title: 'Success!',
                        text: result.message,
                        icon: 'success',
                        timer: 1200,
                        showConfirmButton: false
                    });

                    // Optionally call updateWishlistUI() if you need to update other parts of the UI
                    updateWishlistUI();
                } else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Failed to add product to wishlist. ' + result.message,
                        icon: 'error',
                        timer: 1200,
                        showConfirmButton: false
                    });
                }
            } catch (error) {
                console.error('Network error:', error);
                Swal.fire({
                    title: 'Network Error!',
                    text: 'An error occurred. Please try again.',
                    icon: 'error',
                    timer: 1200,
                    showConfirmButton: false
                });
            }
        });
    });

    function updateWishlistUI() {
        // Implement this function if you need to refresh the wishlist display or other UI elements
    }
});




document.querySelectorAll('.add-to-basket-button').forEach(button => {
    button.addEventListener('click', async function () {
        const productId = this.getAttribute('data-product-id');

        try {
            const response = await fetch('/Basket/Add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(productId)
            });

            const result = await response.json();

            if (response.ok) {
                Swal.fire({
                    title: 'Success!',
                    text: result.message,
                    icon: 'success',
                    timer: 1200,
                    showConfirmButton: false
                });

                document.querySelector('.count-basket').textContent = result.uniqueProductCount;

                updateBasketUI();
            } else {
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to add product to basket. ' + result.message,
                    icon: 'error',
                    timer: 1200,
                    showConfirmButton: false
                });
            }
        } catch (error) {
            console.error('Network error:', error);
            Swal.fire({
                title: 'Network Error!',
                text: 'An error occurred. Please try again.',
                icon: 'error',
                timer: 1200,
                showConfirmButton: false
            });
        }
    });
});

function updateBasketUI() {
    // Implement this function to refresh the basket display, count items, etc.
}

// search
function openSearch() {
    document.getElementById("searchOverlay").style.display = "block";
    document.body.classList.add("no-scroll"); // Prevent scrolling
}

function closeSearch() {
    document.getElementById("searchOverlay").style.display = "none";
    document.body.classList.remove("no-scroll"); // Allow scrolling again
}

function checkEnter(event) {
    if (event.key === "Enter") {
        performSearch();
    }
}

function performSearch() {
    var searchQuery = document.getElementById("searchInput").value;
    alert("Search performed for: " + searchQuery);
    closeSearch(); // Optionally close the search after performing it
}



document.addEventListener('DOMContentLoaded', function () {
    // Get all filter buttons
    const filterButtons = document.querySelectorAll('.filter-btn');

    const showAllButton = document.querySelector('.show-all-btn');

    const productCards = document.querySelectorAll('.featured-products-card');

    function filterProducts(brandName) {
        productCards.forEach(card => {
            if (card.getAttribute('data') === brandName || brandName === 'all') {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        });
    }
    filterButtons.forEach(button => {
        button.addEventListener('click', function () {
            const brandName = this.getAttribute('data');
            filterProducts(brandName);
        });
    });
    showAllButton.addEventListener('click', function () {
        filterProducts('all');
    });
});