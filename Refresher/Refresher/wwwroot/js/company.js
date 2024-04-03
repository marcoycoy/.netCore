
$(document).ready(function () {
    console.log("Company Test Data");
    loadDatatableCompany();
});

function loadDatatableCompany() {
    console.log("Company Test Data");
    datatable = $('#myTable').DataTable({
        "ajax": { url: '/Admin/Company/GetAll' },
        "columns": [
            { data: "name" },
            { data: "streetAddress" },
            { data: "city" },
            { data: "state" },
            { data: "postalCode" },
            { data: "phoneNumber" },
            {
                    data: 'id',
                "render": function (data) {
                    
                    return `<div class="w-75 btn-group" role="group">
                        <a href="Company/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit</a>
                        <a onclick="confirmationDelete('Company/DeleteCompany?id=${data}')"  class="btn btn-danger mx-2"><i class="bi bi-trash"></i> Delete</a>
                    </div>`
                }
            }
        ]
    });
}

function confirmationDelete(URL) {
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
            $.ajax({
                url: URL,
                type: 'DELETE',
                success: function (data) {
                    //Swal.fire({
                    //    title: "Deleted!",
                    //    text: data,
                    //    icon: "success"
                    //});
                    datatable.ajax.reload();
                    toastr.success(data.message)
                }
            });
          
        }
    });
}