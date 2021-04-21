$(document).ready(function () {
    $("#form-add-contact").submit(function (e) {
        e.preventDefault();
        var email_parts = $(".new-contact .email").val().trim().split("@");
        if (email_parts.length == 2) {
            if ($("#OrgDomain").val() != email_parts[1]) {
                $("#ErrorMsg span").html("Please enter an email with your organization domain.");
                $("#ErrorMsg").show();
                return false;
            }
            else {
                $("#ErrorMsg").hide();
            }
        }

        var form = $(this);
        $.validator.unobtrusive.parse(form);
        form.validate();

        if (form.valid()) {
            var url = form.attr("action");
            var formData = $(form).serializeArray();
            var selected_contacts = new Array();
            $(".raci-contact").each(function (i, e) {
                selected_contacts.push($(this).data('ct-key'));
            });

            formData.push({ name: 'contact_keys', value: selected_contacts });

            $.post(url, formData).done(function (data) {
                $(".new-contact input[type='text']").val("");
                $('#partial').html(data);
            });
        }
        else {
            $.each(form.validate().errorList, function (key, value) {
                $errorSpan = $("span[data-valmsg-for='" + value.element.id + "']");
                $errorSpan.html("<span style='color:red'>" + value.message + "</span>");
                $errorSpan.show();
            });
        }
    });

    $(".new-contact .email").blur(function () {
        $.getJSON('/account/CheckUserExists?email=' + $(this).val().trim(), null, function (result) {
            if (result.IsSuccess) {
                $("span.check-user-exists").html(result.Message);
                $("span.check-user-exists").fadeTo(3000, 500).slideUp(500, function () {
                    $("span.check-user-exists").slideUp(500);
                });
            }
            else {
                $("#ErrorMsg").hide();
            }

            var email_parts = $(".new-contact .email").val().trim().split("@");
            if (email_parts.length == 2) {
                if ($("#OrgDomain").val() != email_parts[1]) {
                    $("#ErrorMsg span").html("Please enter an email with your organization domain.");
                    $("#ErrorMsg").show();
                }
                else {
                    $("#ErrorMsg").hide();
                }
            }
        });
    });

    $(".add-team").click(function () {
        var counter = $(".raci-contact").length;
        
        $(".chk-contact:checked").each(function (e, i) {
            var contact = $(this).data('contact');
            var contact_html = '<div class="row raci-contact" data-ct-key="' + contact.ct_key + '" data-raci-role="' + $(this).parent().parent().find("select").val() +'" style="margin:5px">' +
                    '<input name="RACIContactList[' + counter + '].ct_key" id="RACIContactList[' + counter +'].ct_key" type="hidden" value="'+contact.ct_key+'" />' +
                    '<input name="RACIContactList[' + counter + '].lk5_key" id="RACIContactList[' + counter +'].lk5_key" type="hidden" value="' + $(this).parent().parent().find("select").val() +'" />' +
                    '<input name="RACIContactList[' + counter + '].ct_first_name" id="RACIContactList[' + counter + '].ct_first_name" type="hidden" value="' + contact.ct_first_name + '" />' +
                    '<input name="RACIContactList[' + counter + '].ct_last_name" id="RACIContactList[' + counter + '].ct_last_name" type="hidden" value="' + contact.ct_last_name + '" />' +
                    '<input name="RACIContactList[' + counter + '].ct_email" id="RACIContactList[' + counter + '].ct_email" type="hidden" value="' + contact.ct_email + '" />' +
                    '<input name="RACIContactList[' + counter + '].cxo_title" id="RACIContactList[' + counter + '].cxo_title" type="hidden" value="' + (contact.cxo_title == null ? "" : contact.cxo_title) + '" />' +
                '<input name="RACIContactList[' + counter + '].lk5_value" id="RACIContactList[' + counter + '].lk5_value" type="hidden" value="' + $(this).parent().parent().find("select option:selected").text() + '" />' +
                    '<div class="col-md-12 "><a style="float:right" class="btn-remove"><i class="fa fa-times"></i></a></div>' +
                    '<div class="col-md-6 sec-raci-team padding-zero">' +
                        contact.ct_first_name + " " + contact.ct_last_name +
                    '</div>' +
                    '<div class="col-md-6 sec-raci-team padding-zero">' +
                        contact.ct_email +
                    '</div>' +
                    '<div class="col-md-6 sec-raci-team padding-zero">' +
                        (contact.cxo_title == null ? "" : contact.cxo_title) +
                    '</div>' +
                    '<div class="col-md-6 sec-raci-team padding-zero">' +
                        $(this).parent().parent().find("select option:selected").text() +
                    '</div>' +
                '</div >';
            $("#raci-list").append(contact_html);

            if (typeof (add_raci) === "function") {
                add_raci(contact.ct_key, $(this).parent().parent().find("select").val());
            }

            counter++;
        });
    });
});