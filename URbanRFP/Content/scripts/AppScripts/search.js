
$(document).ready(function () {
    load_filter_marketplace();
    search_results();
});

function search_results() {
    var searchText = $('.txtSearch').val();
    var search_type = $("#type_id").val();
    localStorage.setItem("searchText", searchText)

    if (searchText) {
        searchText = searchText.trim();
    }

    if (search_type == 1) {
        $.ajax({
            type: "Get",
            url: '/search/SearchRFPsVendors',
            data: { search: searchText },
            dataType: 'json',
            contentType: 'application/json',
            success: function (response) {
                if (response.IsSuccess === true) {

                    var data = response.DataCollection;

                    var table = $('#tbl-rfp');
                    table.html('');
                    table.append('<thead> ' +
                        '<tr class="throw">' +
                        '<th></th>' +
                        '<th class="mainth ht47">' +
                        '<b>PRODUCT</b>' +
                        '</th>' +
                        '<th class="mainth wd8">' +
                        '<b>CATEGORY 1</b> ' +
                        '</th>' +
                        '<th class="mainth wd19">' +
                        '<b> CATEGORY 2 </b>' +
                        '</th>' +
                        '<th class="mainth wd25">' +
                        '<b> DESCRIPTION </b>' +
                        '<th class="mainth orgndiv"><b>ORGANIZATION</b></th>' +
                        '<th class="mainth">' +
                        '</th>' +
                        '</tr>' +
                        '</thead>');

                    var tr;
                    if (response.DataCollection !== null) {
                        if (response.DataCollection.length > 0) {
                            table.append('<tbody>');
                            $.each(data, function (i, item) {
                                var is_public = (item.prd_key == "00000000-0000-0000-0000-000000000000");

                                tr = $('<tr/>');
                                tr.append('<td></td>');
                                tr.append('<td><a href="' + (is_public ? '#' : '/search/IndividualResult?prdkey=' + item.prd_key) + '">' + item.prd_name + '</a></td>');
                                tr.append('<td><a href="#" onclick="search_result(1,' + item.prd_type_id+',1)">' + (item.prd_type == null ? '' : item.prd_type) + '</a></td>');
                                tr.append('<td><a href="#" onclick="search_result(2,' + item.prd_subtype_id +',1)">' + (item.prd_subtype == null ? '' : item.prd_subtype) + '</a></td>');
                                tr.append('<td><p class="comment">' + item.prd_description + '</p></td>');
                                tr.append('<td><a href="#" data-key="' + item.org_key + '" onclick="search_result(3, this,1);">' + (item.org_legal_name == null ? '' : item.org_legal_name) + '</a><div style="margin-top: 33px;"><div style="float:left;margin-top: 5px;">' + ($("#per-type").val() != 2 ? '<i class="fa fa-star-o fa-custom"></i>' : '<i class="fa fa-star' + (item.favorite == 1 ? '' : '-o') + ' fa-custom" data-key="' + item.prd_key + '" data-type="prd" onclick="mark_favorite(this);"></i>') + '</div>' + item.actions + '</div></td>');
                                tr.append('<td class="clrtext" style="vertical-align:bottom"></td>');
                                table.append(tr);
                            });
                        }
                    }
                    else {
                        tr = $('<tr/>');
                        tr.append("<td colspan='7' style='color:#3A97D0; text-align:center; font-size:16px;'>No record found.</td>");
                        table.append(tr);
                    }

                    table.append('</tbody>');
                }

                $("#tbl-rfp").tablesorter({ headers: { 0: { sorter: false }, 6: { sorter: false } } });

                //$(".txtSearch").val(localStorage.getItem("searchText"));
                $(".comment").shorten();
                $(".comment-small").shorten({ showChars: 10 });
            }
        });
    }
    else {
        $.ajax({
            type: "Get",
            url: '/search/SearchRFP',
            data: { search: searchText },
            dataType: 'json',
            contentType: 'application/json',
            success: function (response) {
                if (response.IsSuccess === true) {

                    var data = response.DataCollection;

                    var table = $('#tbl-rfp');
                    table.html('');
                    table.append('<thead> ' +
                        '<tr class="throw">' +
                        '<th></th>' +
                        '<th class="mainth">' +
                        '<b>RFP TITLE</b>' +
                        '</th>' +
                        '<th class="mainth wd10">' +
                        '<b>CATEGORY</b> ' +
                        '</th>' +
                        '<th class="mainth wd19">' +
                        '<b>DESCRIPTION</b>' +
                        '</th>' +
                        '<th class="mainth wd10">' +
                        '<b> PUBLISH DATE </b>' +
                        '</th>' +
                        '<th class="mainth wd10">' +
                        '<b> CLOSE DATE </b>' +
                        '</th>' +
                        '<th class="mainth orgndiv"><b>LOCAL GOVERNMENTS</b></th>' +
                        '<th class="mainth">' +
                        '</th>' +
                        '</tr>' +
                        '</thead>');

                    var tr;
                    
                    if (response.DataCollection !== null) {
                        if (response.DataCollection.length > 0) {
                            table.append('<tbody>');
                            $.each(data, function (i, item) {
                                var is_public = (item.rfp_key == "00000000-0000-0000-0000-000000000000");
                                tr = $('<tr/>');
                                tr.append('<td></td>');
                                tr.append('<td><a href="' + (is_public ? '#' : '/rfp?rfp_key=' + item.rfp_key) + '">' + item.rfp_name + '</a></td>');
                                tr.append('<td><a href="#" onclick="search_result(1,' + item.rfp_type + ', 2)">' + (item.rfp_type_name == null ? "" : item.rfp_type_name) + '</a>'+
                                    (item.rfp_subtype_name == null ? "" : (', <a href="#" onclick="search_result(1,' + item.rfp_subtype + ', 2)">' + item.rfp_subtype_name + '</a>')) +
                                    '</td > ');
                                tr.append('<td><p class="comment">' + item.rfp_summary + '</p></td>');
                                tr.append('<td>' + (item.rfp_issue_date == null ? "" : formatDate(item.rfp_issue_date)) + '</td>');
                                tr.append('<td>' + (item.rfp_close_date == null ? "" : formatDate(item.rfp_close_date)) + '</td>');
                                tr.append('<td><a href="#" data-key="' + item.rfp_org_key + '" onclick="search_result(3, this, 2);">' + (item.org_legal_name == null ? '' : item.org_legal_name) + '</a><div style="margin-top: 33px;"><div style="float:left;margin-top: 5px;">' + ($("#per-type").val() != 1 ? '<i class="fa fa-star-o fa-custom"></i>' : '<i class="fa fa-star' + (item.favorite == 1 ? '' : '-o') + ' fa-custom" data-key="' + item.rfp_key + '" data-type="rfp" onclick="mark_favorite(this);"></i>') + '</div>' + item.actions + '</div></td>');
                                tr.append('<td class="clrtext" style="vertical-align:bottom"></td>');
                                table.append(tr);
                            });
                        }
                    }
                    else {
                        tr = $('<tr/>');
                        tr.append("<td colspan='7' style='color:#3A97D0; text-align:center; font-size:16px;'>No record found.</td>");
                        table.append(tr);
                    }

                    table.append('</tbody>');
                }

                $("#tbl-rfp").tablesorter({ headers: { 0: { sorter: false }, 6: { sorter: false } } });

                //$(".txtSearch").val(localStorage.getItem("searchText"));
                $(".comment").shorten();
                $(".comment-small").shorten({ showChars: 10 });
            }
        });
    }
}

$(document).on('click', '.allchlrpf', function () {
    if ($(this).is(':checked')) {
        $('.chk_rpf').prop('checked', true);
    }
    else {
        $('.chk_rpf').prop('checked', false);
    }
});

$(document).on('click', '.lbl-type-filter', function () {
    var filter_class = $(this).data("filter");

    if ($(this).find("i").hasClass("fa-plus")) {
        $(this).find("i").removeClass("fa-plus").addClass("fa-minus");
    }
    else {
        $(this).find("i").addClass("fa-plus").removeClass("fa-minus");
    }

    $("." + filter_class).toggleClass("hide");
});

function formatDate(date) {
    var d = new Date(parseInt(date.substr(6)));
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;

    return [month, day, year].join('/');
}

function load_filter_marketplace() {
    var search_type = $("#type_id").val();
    var search_name = (search_type == 1) ? "prod_" : "rfp_";

    $(".search-filter-list").html("");
    $.getJSON('/lookup/GetAllTypes', null, function (result) {
        if (result.IsSuccess && result.DataCollection.length > 0) {
            var selected_types = localStorage.getItem(search_name+'types');

            if (selected_types == undefined || selected_types == null) {
                selected_types = new Array();
            }

            $.each(result.DataCollection, function (index, item) {
                $(".main-filter-list").append("<div class='prod-type' data-id='" + item.Id + "' style='color:#3A97D0'><a><input id='type" + item.Id + "' data-level='0' class='chk_mp type" + (search_type == 1 ? " product" : "") + "' style=' margin-right: 10px;width: 16px; height: 16px;border: solid 1px #d8dde6;background-color: #fff;' type='checkbox' value='" + item.Id + "' " + (selected_types.includes(item.Id) ? "checked='checked'" : "") + "><label class='lbl-type-filter' data-filter='sub-filter-" + item.Id +"'><i class='fa fa-plus'></i>&nbsp;&nbsp;" + item.Name + "</label></a></div>");
            });
        }

        load_filter_prod_subtype();
    });
}

function load_filter_prod_subtype() {
    var search_type = $("#type_id").val();
    var search_name = (search_type == 1) ? "prod_" : "rfp_";

    $(".main-filter-list div.prod-type").each(function (index, element) {
        var typeId = $(this).data("id");

        $.getJSON('/lookup/GetSubTypes?id=' + typeId, null, function (result) {
            if (result.IsSuccess && result.DataCollection.length > 0) {
                var selected_subtypes = localStorage.getItem(search_name+'sub_types');
                if (selected_subtypes == undefined || selected_subtypes == null) {
                    selected_subtypes = new Array();
                }

                var subtypeList = "";
                $.each(result.DataCollection, function (index, item) {
                    subtypeList += "<div data-id='" + item.Id + "' style='color:#3A97D0'><a class='subtype-item'><input id='filter-subtype" + item.Id + "' data-level='1' class='sub-type chk_mp" + (search_type == 1 ? " product" : " rfp") +"' style=' margin-right: 10px;width: 16px; height: 16px;border: solid 1px #d8dde6;background-color: #fff;' type='checkbox' value='" + item.Id + "' " + (selected_subtypes.includes(item.Id) ? "checked='checked'" : "") + "><label for='filter-subtype" + item.Id + "'>" + item.Name + "</label></a></div>";
                });

                $(element).append("<div class='hide sub-filter-" + typeId+"'>" + subtypeList + "</div>");
            }
        });
    });
}

function toggle_filter(btn) {
    if ($(btn).text().trim() == "Show Filters") {
        $(btn).text("Hide Filters");
    }
    else {
        $(btn).text("Show Filters");
    }

    $("#div-search-filter").toggleClass("div-search-filter");
    $("#div-search-result").toggleClass("div-search-result");
}

function search_result(type, key, search_type) {
    var search_name = (search_type == 1) ? "prod_" : "rfp_";

    if (search_type == 1)
        $(".chk_mp.product").removeProp('checked');
    else
        $(".chk_mp.rfp").removeProp('checked');

    localStorage.removeItem(search_name+"types");
    localStorage.removeItem(search_name+"sub_types");

    if (type == 1) {
        localStorage.setItem(search_name+"types", key);
        get_search_results("", true, search_type);
    }
    else if (type == 2) {
        localStorage.setItem(search_name+"sub_types", key);
        get_search_results("", true, search_type);
    }
    else if (type == 3) {
        get_search_results($(key).data("key"), true, search_type);
    }
}