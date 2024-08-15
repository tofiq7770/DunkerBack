const dltBtns = document.querySelectorAll(".delete-color");

dltBtns.forEach(dltBtn => {
    dltBtn.addEventListener("click", function () {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: "Deleted!",
                    text: "Your file has been deleted.",
                    icon: "success"
                });

                const btn = this;
                const id = btn.getAttribute("color-id");
                $.ajax({
                    type: "POST",
                    url: `/Admin/Product/DeleteColor/${id}`,
                    success: function (result) {
                        btn.parentNode.remove();
                    }
                });
            }
        });
    });
});