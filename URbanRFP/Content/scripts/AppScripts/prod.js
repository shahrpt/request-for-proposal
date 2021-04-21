$(document).ready(function () {
    $("#prd_type").change(function () {
        let dropdown = $('#prd_subtype');
        dropdown.empty();
        dropdown.append('<option selected="true" disabled>Choose sub-type</option>');
        dropdown.prop('selectedIndex', 0);

        $.getJSON('/lookup/GetSubTypes?id=' + $(this).val(), null, function (result) {
            var subtypeId = $("#tmpSubTypeId").val();
            if (result.IsSuccess && result.DataCollection.length > 0) {
                $.each(result.DataCollection, function (key, entry) {
                    if (entry.Id == subtypeId)
                        dropdown.append($('<option></option>').attr('value', entry.Id).attr('selected', 'selected').text(entry.Name));
                    else
                        dropdown.append($('<option></option>').attr('value', entry.Id).text(entry.Name));
                });
            }
        });
    });

    $(".btn-del-product").click(function () {
        var element = $(this);

        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to access this product!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                var prd_key = $(this).data("key");

                $.getJSON('/product/delete?id=' + prd_key, null, function (result) {
                    if (result.IsSuccess) {
                        debugger;
                        if ($("#current-action").val() == "index") {
                            $(element).parent().parent().remove();
                            ShowResult(result);
                        }
                        else {
                            ShowResult(result);
                            window.location = "/product";
                        }
                    }
                    else {
                        ShowResult(result);
                    }
                });
            } else {
                swal("Your product is safe!", "", "success");
            }
        });
    });
});