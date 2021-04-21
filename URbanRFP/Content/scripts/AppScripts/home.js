$(document).on('click', '.btnSearch', function () {
    var search_type = $("#type_id").val();
    get_search_results("", false, search_type);
});

$("#btnSearchProduct").click(function () {
    get_search_results("", true, 1);
});

$("#btnSearchRFP").click(function () {
    get_search_results("", true, 2);
});

$('.txtSearch').keypress(function (e) {
    var key = e.which;
    if (key === 13)  // the enter key code
    {
        $('input[name = btnSearch]').click();
        return false;
    }
});
function ShowResult(result) {
    if (typeof result === "object" && result.IsSuccess == true) {
        swal(result.Message, {
            icon: "success",
        });
    }
    else {
        swal(result.Message, {
            icon: "error",
        });
    }
}