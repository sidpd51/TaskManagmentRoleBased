$(document).ready(function () {


    $(document).on('click', '.addtask', function () {
        var id = $(this).data('id');
        $('.modal-body').html('');
        console.log(id)
        if (id == undefined) {
            id = 0;
        }
        $('#myModal').modal('show');
        $.ajax({
            method: "GET",
            url: "/Home/GetTaskData/" + id,
            contentType: false,
            success: function (response) {
                console.log(response);
                $('.modal-body').append(response);
                $('#myModal').modal('show');
                $.validator.unobtrusive.parse($("#formData"));

            }

        })
    })



    $(document).on('click', '.approve', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/Home/approve/" + id,
            contentType: false,
            success: function (response) {
                window.location.href = "/Home/EmployeeTask"
            }

        })

    })
    $(document).on('click', '.approveDirector', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/Home/Approve/" + id,
            contentType: false,
            success: function (response) {
                window.location.href = "/Home/ManageManagerTask"
            }

        })

    })
    $(document).on('click', '.reject', function () {
        var id = $(this).data('id');
        
        $.ajax({
            method: "POST",
            url: "/Home/Reject/" + id,
            contentType: false,
            success: function (response) {
                window.location.href = "/Home/EmployeeTask"
            }
        })
    })

    $(document).on('click', '.rejectDirector', function () {
        var id = $(this).data('id');

        $.ajax({
            method: "POST",
            url: "/Home/reject/" + id,
            contentType: false,
            success: function (response) {
                window.location.href = "/Home/ManageManagerTask"
            }
        })

    })

    $(document).on('click', '.edit', function () {
        var id = $(this).data('id');
        $('.modal-body').html('');
        console.log(id)
        if (id == undefined) {
            id = 0;
        }
        $.ajax({
            method: "GET",
            url: "/Home/GetData/" + id,
            contentType: false,
            success: function (response) {
                console.log(response);
                $('.modal-body').append(response);
                $('#myModal').modal('show');
                $.validator.unobtrusive.parse($("#formData"));
                $(document).on("change", "#department", function () {
                    let id = $(this).val();
                    let data = response.data;
                    console.log(id)
                    $.ajax({
                        method: 'GET',
                        url: "/Home/GetReportingPerson/" + id,
                        success: function (response) {
                            console.log(response.data);
                            $("#reportingPerson").empty();
                            let data = response.data;
                            for (let i = 0; i <= data.length; i++) {
                                console.log(data[i]);
                                let option = `<option value=${data[i].Value}>${data[i].Text}</option>`;
                                $('#reportingPerson').append(option);
                                if (data.length == 0) {
                                    $('#reportingPerson').empty(); $('#myButton').attr('data-action')
                                    $('#reportingPerson').prop("disable", true);
                                    let option = `<option value=${data[i].Value}>not assigned</option>`;
                                    $('#reportingPerson').append(option);
                                }
                            }
                        }
                    })
                });

            }

        })
    })

})

$('table tbody').on('click', '.delete', function () {

    var id = $(this).data('id');
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
            window.location = "/Home/Delete/" + id;
        }
    })
})
$('table tbody').on('click', '.deleteTask', function () {
    var id = $(this).data('id');
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
            window.location = "/Home/DeleteTask/" + id;
        }
    })
})