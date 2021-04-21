$(document).ready(function () {
    $("#tbl-org").DataTable();
    $(".btn-del-org").click(function () {
        if (confirm('Are you sure, you want to delete?') == true) {
            var prd_key = $(this).data("orgkey");
            $.getJSON('/organization/delete?id=' + prd_key, null, function (result) {
                if (result.IsSuccess) {
                    window.location = '/organization';
                }
                else {
                    alert(result.Message);
                }
            })
        }
    });
});