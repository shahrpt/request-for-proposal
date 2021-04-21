var total_score = 100;

$(document).ready(function () {
    var row_counter = 1;
    create_score_chart();

    $(document).on("click", ".btn-edit-rule", function () {
        var row = $(this).data("section");

        $("#" + row + " div.edit").each(function (index, element) {
            $(element).data("value", $(element).html());
        });

        $("#" + row + " div.edit").attr("contenteditable", true);
        $(this).toggleClass("hide");
        $("#" + row + " a.btn-save-rule").toggleClass("hide");
        $("#" + row + " a.btn-cancel-rule").toggleClass("hide");
    });

    $(document).on("click", ".btn-cancel-rule", function () {
        var row = $(this).data("section");

        $("#" + row + " div.edit").each(function (index, element) {
            $(element).html($(element).data("value"));
        });

        $("#" + row + " div.edit").attr("contenteditable", false);
        $(this).toggleClass("hide");
        $("#" + row + " a.btn-save-rule").toggleClass("hide");
        $("#" + row + " a.btn-edit-rule").toggleClass("hide");
    });

    $(document).on("click", ".btn-delete-rule", function () {
        var row = $(this).data("section");

        if ($(this).data("key") == "") {
            $(this).parent().parent().remove();

            swal("Score rule deleted successfully!", {
                icon: "success",
            });
        }
        else {
            swal({
                title: "Are you sure?",
                text: "Once deleted, you will not be able to access this RFP score rule!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            }).then((willDelete) => {
                if (willDelete) {
                    $.getJSON('/rfp/RemoveScoreRule?id=' + $(this).data("key"), null, function (result) {
                        if (result.IsSuccess) {
                            $(this).parent().parent().remove();
                            ShowResult(result);
                            setTimeout(function () { location.reload(); }, 1000);
                        }
                        else {
                            ShowResult(result);
                        }
                    });
                } else {
                    swal("Your RFP score rule is safe!", "" , "success");
                }
            });
        }
    });

    $(document).on("click", ".btn-save-rule", function () {
        var row = $(this).data("section");
        var criteria = { eva_key: $(this).data("key"), eva_rfp_key: $("#rfp_key").val() };

        $("#" + row + " div.edit").each(function (index, element) {
            $(element).data("value", $(element).html());
            criteria[$(element).data("field")] = $(element).html().trim();
        });

        //if (validate_score(criteria)) {
            $.post("/rfp/SaveCriteria", criteria, function (result) {
                ShowResult(result);

                if (result.IsSuccess) {
                    $("#" + row + " div.edit").attr("contenteditable", false);
                    $(this).toggleClass("hide");
                    $("#" + row + " a.btn-edit-rule").toggleClass("hide");
                    $("#" + row + " a.btn-cancel-rule").toggleClass("hide");
                    setTimeout(function () { location.reload(); }, 1000);
                }
            });
        //}
        //else {
        //    swal("Validation failed!", "Total weight of all rules can't be over 100%!,", "error");
        //}
    });

    $(".btn-add-criteria").click(function () {
        $("#tbl-score-rules tbody").append('<tr id="rule-' + row_counter+'">'+
            '<td></td>'+
            '<td class="fzpd"><a><div class="edit" data-field="eva_criteria_name" contenteditable="true" style="display:inline-block"></div></a></td>'+
            '<td class="fzpd"><div class="edit" data-field="eva_criteria_description" contenteditable="true" style="display:inline-block"></div></td>'+
            '<td class="fzpd text-center"><div data-field="eva_point_max" class="edit" contenteditable="true" style="display:inline-block">10</div></td>'+
            '<td class="fzpd text-center"><div data-field="eva_weight" class="edit" contenteditable="true" style="display:inline-block"></div></td>'+
            '<td class="txtcenter">'+
            '    <a class="btn-edit-rule hide" data-key="" data-section="rule-' + row_counter + '"><img src="/Content/images/fa-edit.png"></a>' +
            '    <a class="btn-delete-rule" data-key="" data-section="rule-' + row_counter + '"><img src="/Content/images/delete.png"></a>' +
            '    <a class="btn btn-primary btn-auto btn-save-rule" data-key="" data-section="rule-' + row_counter + '">OK</a>' +
            '    <a class="btn btn-primary btn-auto btn-cancel-rule" data-key="" data-section="rule-' + row_counter + '">Cancel</a>' +
            '</td>'+
            '</tr>');

        row_counter++;
    });
});

function create_score_chart() {
    total_score = 100;
    var chart_data = [];
    
    $(score_rules).each(function (i, e) {
        chart_data.push({ y: parseFloat(e.eva_weight), label: e.eva_criteria_name });
        //total_score -= parseFloat(e.eva_weight);
    });

    //chart_data.push({ y: total_score, label: "", color: "white" });

    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        title: {
            text: "",
            horizontalAlign: "left"
        },
        legend: {
            fontFamily: "ibm_plex_sansregular",
            fontWeight: "normal",
            fontSize: 14,
            fontColor: "#403f2d",
            fontStyle: "normal"
        },
        data: [{
            type: "doughnut",
            startAngle: -90,
            innerRadius: 50,
            indexLabelFontSize: 14,
            indexLabelFontFamily: "ibm_plex_sansregular",
            indexLabel: "{label} ({y})",
            toolTipContent: "{label} ({y})",
            dataPoints: chart_data
        }]
    });

    chart.render();
}

function validate_score(criteria) {
    var index = find_array(criteria);
    var due_score = parseFloat(criteria.eva_weight);

    $(score_rules).each(function (i, e) {
        if (i != index) {
            due_score += parseFloat(e.eva_weight);
        }
    });

    return due_score <= 100;
}

function find_array(criteria) {
    var index = -1;

    for (var i = 0; i < score_rules.length; i++) {
        if (score_rules[i].eva_key == criteria.eva_key) {
            index = i;
            break;
        }
    }

    return index;
}