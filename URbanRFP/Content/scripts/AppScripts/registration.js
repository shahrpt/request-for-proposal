$("#Phone, #Fax").mask("(000)-000-0000", { placeholder: "(000)-000-0000" });

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
}

// jQuery code to show the working of this method 
$(document).ready(function () {
    $("#form-register").submit(function (e) {

    });

    $(".registration #Email").blur(function () {
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
            
            var email_parts = $(".registration #Email").val().trim().split("@");
            if (email_parts.length == 2) {
                $("#OrgDomain").val(email_parts[1]);

                $.getJSON('/account/CheckOrgExists?domain=' + email_parts[1], null, function (org_result) {
                    if (org_result.IsSuccess == false) {
                        $("div#section-org").show();
                    }
                    else {
                        $("div#section-org").hide();
                    }
                });
            }
        });
    });
}); 