//Dropzone Configuration
if (typeof (Dropzone) != "undefined") {
    Dropzone.autoDiscover = false;
}

$(document).ready(function () {
    $(".btn-exp-coll").parent().addClass("section-collapsed");
    
    $(".btn-exp-coll").click(function () {
        var iconElement = $(this).children().first();
        $(iconElement).toggleClass("fa-plus");
        $(iconElement).toggleClass("fa-minus");
        $(this).parent().toggleClass("section-collapsed");
    });

    $(".btn-edit-auto").click(function () {
        var section = $(this).data("section");
        $("." + section).data("content", $("." + section).html());
        $("." + section).attr("contenteditable", true);
        $(this).hide();
        $(this).prev().show();
    });

    $(".btn-edit").click(function () {
        var section = $(this).data("section");

        $("." + section).each(function (index, element) {
            $(element).data("content", $(element).html());
            $(element).summernote({ focus: true });
        });

        $(this).hide();
        $(this).prev().show();
    });

    $(".btn-edit-list").click(function () {
        var section = $(this).data("section");

        $("." + section + " .category-item").each(function (index, element) {
            $(element).find("span").toggleClass("hide");
            $(element).find("select").toggleClass("hide");
            $(element).find("span").data("value", $(element).find("select").val());
        });

        $(this).hide();
        $(this).prev().show();
    });

    $(".btn-cancel-html").click(function () {
        var section = $(this).data("section");
        
        $("." + section).each(function (index, element) {
            $(element).summernote('destroy');
            $(element).html($(element).data("content"));
        });

        $(this).parent().hide();
        $(this).parent().next().show();
    });

    $(".btn-cancel").click(function () {
        var section = $(this).data("section");
        $("." + section).html($("." + section).data("content"));
        $("." + section).attr("contenteditable", false);
        $(this).parent().hide();
        $(this).parent().next().show();
    });

    $(".btn-cancel-list").click(function () {
        var section = $(this).data("section");

        $("." + section + " .category-item").each(function (index, element) {
            var selecteVal = $(element).find("span").data("value");
            $(element).find("select").val(selecteVal);

            $(element).find("span").toggleClass("hide");
            $(element).find("select").toggleClass("hide");
        });

        $(this).parent().hide();
        $(this).parent().next().show();
    });

    $(".btn-save").click(function () {
        var rfp_key = $("#rfp_key").val();
        var section = $(this).data("section");
        var element = $(this).parent().parent().find("input").first();

        if (element.length == 0) {
            var data = { rfp_key: rfp_key, key: $("." + section).data("field"), value: $("." + section).html() };
            $.post("/rfp/update", data, function (result) {
                $("." + section).attr("contenteditable", false);
                ShowResult(result);
            });
        }
        else {
            if ($(element).data("type") == "date") {
                var date_val = new Date($(element).val());
                var date_str = date_val.getFullYear() + "-" + (date_val.getMonth() + 1) + "-" + (date_val.getDate());
                var data = { rfp_key: rfp_key, key: $(element).data("field"), value: date_str };
                $.post("/rfp/update", data, function (result) {
                    ShowResult(result);
                });
            }
        }

        $(this).parent().hide();
        $(this).parent().next().show();
    });

    $(".btn-save-html").click(function () {
        var section = $(this).data("section");
        var rfp_key = $("#rfp_key").val();

        $("." + section).each(function (element, index) {
            var markup = $(this).summernote('code');
            var data = { rfp_key: rfp_key, key: $(this).data("field"), value: markup };
            $(this).summernote('destroy');
            $(this).data("content", markup);
            $(this).parent().children().first().hide();
            $(this).parent().children().eq(1).show();
            $(this).parent().next().show();

            $.post("/rfp/update", data, function (result) {
                ShowResult(result);
            });
        });
    });

    $(".btn-save-list").click(function () {
        var section = $(this).data("section");
        var rfp_key = $("#rfp_key").val();

        $("." + section + " .category-item").each(function (index, element) {
            var data = { rfp_key: rfp_key, key: $(element).find("select").attr("id"), value: $(element).find("select").val() };
            $.post("/rfp/update", data, function (result) {
                ShowResult(result);
            });

            $(element).find("span").text($(element).find("select option:selected").text().toUpperCase());
            $(element).find("span").toggleClass("hide");
            $(element).find("select").toggleClass("hide");
        });

        $(this).parent().hide();
        $(this).parent().next().show();
    });

    // Binds the hidden input to be used as datepicker.
    if ($('.datepicker-input').length > 0) {
        $('.datepicker-input').datepicker({
            format: 'MM d, yyyy', autoclose: true
        }).on('change', function (e) {
            $("." + $(this).data('section')).html($(this).val());
        });
    }

    // Shows the datepicker when clicking on the content editable div
    $('.date').click(function () {
        // Triggering the focus event of the hidden input, the datepicker will come up.
        $(this).parent().find('.datepicker-input').focus();
    });

    if ($('.summernote').length > 0) {
        $('.summernote').summernote({
            fontNames: ['ibm_plex_sansregular', 'Arial', 'Arial Black', 'Assistant', 'Comic Sans MS', 'Courier New', 'Helvetica Neue', 'Helvetica', 'Impact', 'Lucida Grande', 'Tahoma', 'Times New Roman', 'Verdana'],
            fontNamesIgnoreCheck: ['Assistant'],
            defaultFontName: 'Assistant',
            toolbar: [
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['fontsize', ['fontname', 'fontsize']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']]
            ],
            height: 100,
        });
    }

    $("#btn-add-team").click(function () {
        var isValid = true;
        $(this).parent().parent().find("input").each(function (i, e) {
            if ($(this).val().trim().length == 0) {
                isValid = false;
                $(this).css('border-color', 'red');
            }
        });

        if (isValid) {

            var contact_info = { full_name: $("#raci-fullname").val(), email: $("#raci-email").val(), title: $("#raci-title").val(), role_id: $("#raci-role").val(), role: $("#raci-role option:selected").text() };
            var member_item = '<div class="row" style="margin:5px">' +
                                '<input name="ContactList" id="ContactList" type="hidden" value="' +  + '" />' + 
                                '<div class="col-md-12 "><a style="float:right" class="btn-remove"><i class="fa fa-times"></i></a></div>'+
                                    '<div class="col-md-6 sec-raci-team padding-zero" contenteditable = "false" >'+
                                        $("#raci-fullname").val()+
                                    '</div >'+
                                    '<div class="col-md-6 sec-raci-team padding-zero" contenteditable="false">'+
                                        $("#raci-email").val() +
                                    '</div>'+
                                    '<div class="col-md-6 sec-raci-team padding-zero" contenteditable="false">'+
                                        $("#raci-title").val() +
                                    '</div>'+
                                    '<div class="col-md-6 sec-raci-team padding-zero" contenteditable="false">'+
                                        $("#raci-role option:selected").text() +
                                     '</div>'+
                '</div >';
            $("#raci-list").append(member_item);
            $("#raci-list").find("input[type=hidden]").last().val(JSON.stringify(contact_info));
            $(this).parent().parent().find("input").val("");
        }
    });

    $(".btn-del-rfp").click(function () {
        var element = $(this);

        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to access this RFP!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        }).then((willDelete) => {
            if (willDelete) {
                var rfp_key = $(this).data("key");

                $.getJSON('/rfp/delete?id=' + rfp_key, null, function (result) {
                    if (result.IsSuccess) {
                        
                        if ($("#current-action").val() == "index") {
                            $(element).parent().parent().remove();
                            ShowResult(result);
                        }
                        else {
                            ShowResult(result);
                            window.location = "/rfp";
                        }
                    }
                    else {
                        ShowResult(result);
                    }
                });
            } else {
                swal("Your RFP is safe!", "", "success");
            }
        });
    });

    $(document).on('click', '.btn-remove', function () {
        $(this).parent().parent().remove();
    });
});
$(function () {
    var d = new Date(),
        h = d.getHours(),
        m = d.getMinutes();
    if (h < 10) h = '0' + h;
    if (m < 10) m = '0' + m;
    $('input[type="time"]').each(function () {
        $(this).attr({ 'value': h + ':' + m });
    });
});