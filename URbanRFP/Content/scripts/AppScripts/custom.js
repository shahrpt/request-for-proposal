$("#sidebarCollapse").click(function () {
    $("#sidebar").toggleClass("closed");
    $("#content").toggleClass("wide");
});

$(document).ready(function () {
    $(".ad_menu").mouseenter(function () {
        $('.ad_submenu').show();
    });

    $(".ad_submenu").mouseleave(function () {
        $('.ad_submenu').hide();
    });

    $(document).on('click', '.btn-hidAdSubmenu', function () {
        $('.ad_submenu').hide();
    });

    $("body").click(function () {
        $('.ad_submenu').hide();
    });

    $('.txtSearch').keyup(function (e) {

        if (e.keyCode == 13) {
            $(this).parent().find("input[type=button]").trigger("click");
        }
    });
});

$(document).on('click', '.clear', function () {
    $('.chk_mp.product').prop('checked', false);
});
$(document).mouseup(function (e) {
    var container = $("aside");

    // if the target of the click isn't the container nor a descendant of the container
    if (!container.is(e.target) && container.has(e.target).length === 0) {
        $(container).addClass('aside-closed').removeClass('aside-opened');
    }
});

$('.aside-closed').click(function (event) {
    //event.stopPropagation();
});
$(document).on('click', 'li.opened a', function (e) {
    //$('li.opened a').click(function (e) {
    e.preventDefault();
    $('aside').removeClass('aside-closed');
    $('aside').addClass('aside-opened');
});

$('a.close-menu').click(function (e) {
    e.preventDefault();
    $('aside').removeClass('aside-opened');
    $('aside').addClass('aside-closed');
});

$(document).on('click', '.chk_mp', function () {
    var search_type = $(this).hasClass('product') ? 1 : 2;
    var search_name = (search_type == 1) ? "prod_" : "rfp_";
    var types = new Array();
    var sub_types = new Array();

    if ($(this).data("level") == 1 && !$(this).is(':checked')) {
        $(this).parent().parent().parent().parent().find(".chk_mp.type").removeProp('checked');

        if (this.id.startsWith("filter")) {
            $("#" + this.id.substr(7)).removeProp('checked');
        }
        else {
            $("#filter-" + this.id).removeProp('checked');
        }
    }
    else if ($(this).data("level") == 1 && $(this).is(':checked')) {
        if ($(this).parent().parent().parent().find(".chk_mp").length == $(this).parent().parent().parent().find(".chk_mp:checked").length) {
            $(this).parent().parent().parent().parent().find(".chk_mp.type").prop('checked', true);
        }

        if (this.id.startsWith("filter")) {
            $("#" + this.id.substr(7)).prop('checked', true);
        }
        else {
            $("#filter-" + this.id).prop('checked', true);
        }
    }
    else if ($(this).data("level") == 0 && !$(this).is(':checked')) {
        if (this.id.startsWith("filter")) {
            $("#" + this.id.substr(7)).removeProp('checked');

            $(this).parent().parent().find(".sub-type.chk_mp" + (search_type == 1 ? ".product" : ".rfp")).each(function (si, se) {
                $(se).removeProp('checked');
            });
        }
        else {
            $("#filter-" + this.id).removeProp('checked');

            $(this).parent().parent().find(".sub-type.chk_mp" + (search_type == 1 ? ".product" : ".rfp")).each(function (si, se) {
                $(se).removeProp('checked');
            });
        }
    }
    else if ($(this).data("level") == 0 && $(this).is(':checked')) {
        if (this.id.startsWith("filter")) {
            $("#" + this.id.substr(7)).prop('checked', true);

            $(this).parent().parent().find(".sub-type.chk_mp" + (search_type == 1 ? ".product" : ".rfp")).each(function (si, se) {
                $(se).prop('checked', true);
            });
        }
        else {
            $("#filter-" + this.id).prop('checked', true);

            $(this).parent().parent().find(".sub-type.chk_mp" + (search_type == 1 ? ".product" : ".rfp")).each(function (si, se) {
                $(se).prop('checked', true);
            });
        }
    }

    $(".chk_mp" + (search_type == 1 ? ".product" : ".rfp") + ":checked").each(function (i, e) {
        if ($(this).data("level") == 1) {
            if (!sub_types.includes($(e).val())) sub_types.push($(e).val());
        }
        else if ($(this).data("level") == 0) {
            $(this).parent().parent().find(".sub-type.chk_mp" + (search_type == 1 ? ".product" : ".rfp")).each(function (si, se) {
                $(se).prop('checked', true);
                if (!sub_types.includes($(se).val())) sub_types.push($(se).val());
            });

            if (!types.includes($(e).val())) types.push($(e).val());
        }
    });

    localStorage.setItem(search_name + "types", types);
    localStorage.setItem(search_name + "sub_types", sub_types);

    var allow_refresh = $("#type_id").val() != search_type;
    get_search_results('', allow_refresh, search_type);
});

/// Menu from database 

$.ajax({
    type: "GET",
    url: "/general/GetMenuItems",
    dataType: "json",
    contentType: 'application/json',
    success: function (response) {
        if (response.IsSuccess === true) {
            var data = response.DataCollection;

            var menudiv = $('#app_menu');
            menudiv.html('');
            //menudiv.append('<li class="opened"><a href=""><img id="menu-img" src="/Content/images/menu.svg" alt=""></a></li>');

            var li;

            if (data !== null) {
                if (data.length > 0) {
                    $.each(data, function (i, item) {
                        var alt = item.MenuIconAlt === null ? '' : item.MenuIconAlt;
                        var icon = item.MenuIcon;
                        var img = "";

                        if (alt === 'RFP') {
                            img = 'home1.png';
                        }
                        if (icon === "fas fa-exchange-alt") {
                            img = 'directions.svg';
                        }
                        else if (icon === "fab fa-medapps") {
                            img = 'idea.svg';
                        }
                        else if (icon === "feather icon-grid") {
                            img = 'layout.svg';
                        }
                        else if (icon === "fas fa-briefcase") {
                            img = 'plug.svg';
                        }

                        if (img !== "") {
                            li = '<li data-id="' + item.MenuID + '" class="main-menu"> <a href="' + item.MenuLink + '"><img src="/Content/images/' + img + '" alt="' + alt + '" /> <span>' + item.MenuName + '</span></a>';
                        }
                        else {
                            li = '<li data-id="' + item.MenuID + '" class="main-menu"> <a href="' + item.MenuLink + '"><span>' + item.MenuName + '</span></a>';
                        }

                        //if (item.SubMenu.length > 0) {
                        //    var subLi = "<ul class='MP_submenu' data-show='0'>";
                        //    subLi += "<li><button class='btn btn-filter' onclick='hide_filter();' style='cursor: pointer; margin: 0px;margin-bottom: 10px;'>Hide Filters <div style='float:right'><i class='fa fa-angle-left'></i></div></button></li>";
                        //    subLi += "<li class='text-center'><input class='text-blue' type='button' onclick='clear_filter(1);' style='width: 100px;margin-top:10px;border-radius:14.5px;background-color:#fff;border: none;border: 1px solid #3A97D0;cursor: pointer; margin-bottom: 10px;' value='Clear all'></li>";
                        //    subLi += "</ul></li>";
                        //    li += subLi;
                        //}
                        //else {
                        li += "</li>";
                        //}
                        menudiv.append(li);
                    });

                }
            }

            //load_marketplace();
        }
    }
});

/// Menu from xml 

//$.ajax({
//    type: "GET",
//    url: "/menu/menu.xml",
//    dataType: "xml",
//    success: function (xml) {
//        xmlDoc = xml;
//        xmlNode = xmlDoc.getElementsByTagName('marketPlaceItems');
//        var txt = "";

//        for (var i = 0; i < xmlNode.length; i++) {
//            if (xmlNode[i].getAttribute("type") === "button") {
//                txt += "<li><input type='button' class='clear' value='" + xmlNode[i].getAttribute("name") + "'></li>"; //<button class='mp_close'></button>
//            }
//            if (xmlNode[i].getAttribute("type") === "checkbox") {
//                txt += "<li style='color:#3A97D0'><a href=''><input class='chk_mp' type='checkbox' value='" + xmlNode[i].getAttribute("name") + "'><label for='" + xmlNode[i].getAttribute("name") + "'>" + xmlNode[i].getAttribute("name") + "</label></a></li>";
//            }
//            if (xmlNode[i].getAttribute("type") === "text") {
//                txt += "<li><a href=''>" + xmlNode[i].getAttribute("name") + "</a></li>";
//            }
//        }
//        document.getElementById("MP_submenu").innerHTML = txt;
//    }
//});

$('#btnSendRfq').click(function () {

    window.location.href = "/rfq";
});

$('.prodchk').click(function () {

    window.location.href = "/search/IndividualResult";
});

function load_marketplace() {
    $.getJSON('/lookup/GetAllTypes', null, function (result) {
        if (result.IsSuccess && result.DataCollection.length > 0) {

            var selected_types = localStorage.getItem('prod_types');
            if (selected_types == undefined || selected_types == null) {
                selected_types = new Array();
            }

            $.each(result.DataCollection, function (index, item) {
                $(".MP_submenu").append("<li class='prod-type' data-id='" + item.Id + "' style='color:#3A97D0'><a href=''><input id='type" + item.Id + "' data-level='0' class='type hide product' style=' margin-right: 10px;width: 16px; height: 16px;border: solid 1px #d8dde6;background-color: #fff;' type='checkbox' value='" + item.Id + "' " + (selected_types.includes(item.Id) ? "checked='checked'" : "") + "><label for='type" + item.Id + "'>" + item.Name + "</label></a></li>");
            });
        }

        load_prod_subtype();
        $(".MP_submenu").mouseenter(function () {
            $(this).parent().children().first().addClass('active');
        });
        $(".MP_submenu").mouseleave(function () {
            if ($(this).data('show') == '0') {
                $(this).parent().children().first().removeClass('active');
                $(this).hide();
            }
        });

        $(".MP_submenu").parent().children().first().mouseover(function () {
            $(".MP_submenu").css('display', 'inline-block');
        });

        $(".main-menu").mouseenter(function () {
            if ($(this).children().length < 2 && $(".MP_submenu").data('show') == 0) {
                $(".MP_submenu").hide();
                $(".MP_submenu").parent().children().first().removeClass('active');
            }
        });
        $(".main-menu-setting--1").mouseenter(function () {
            if ($(this).children().length < 2 && $(".MP_submenu_setting").data('show') == 0) {
                $(".MP_submenu_setting").hide();
                $(".MP_submenu_setting").parent().children().first().removeClass('active');
            }
        });
    });
}

function load_prod_subtype() {
    $(".MP_submenu li.prod-type").each(function (index, element) {
        var typeId = $(this).data("id");

        $.getJSON('/lookup/GetSubTypes?id=' + typeId, null, function (result) {
            if (result.IsSuccess && result.DataCollection.length > 0) {
                var selected_subtypes = localStorage.getItem('prod_sub_types');
                if (selected_subtypes == undefined || selected_subtypes == null) {
                    selected_subtypes = new Array();
                }

                var subtypeList = "";
                $.each(result.DataCollection, function (index, item) {
                    subtypeList += "<div data-id='" + item.Id + "' style='color:#3A97D0'><a class='subtype-item'><input id='subtype" + item.Id + "' data-level='1' class='sub-type chk_mp product' style=' margin-right: 10px;width: 16px; height: 16px;border: solid 1px #d8dde6;background-color: #fff;' type='checkbox' value='" + item.Id + "' " + (selected_subtypes.includes(item.Id) ? "checked='checked'" : "") + "><label for='subtype" + item.Id + "'>" + item.Name + "</label></a></div>";
                });

                $(element).append("<div>" + subtypeList + "</div>");
            }
        });
    });
}

function clear_filter() {
    var search_type = $("#type_id").val();
    var search_name = (search_type == 1) ? "prod_" : "rfp_";

    if (search_type == 1)
        $(".chk_mp.product").removeProp('checked');
    else
        $(".chk_mp.rfp").removeProp('checked');

    localStorage.removeItem(search_name + "types");
    localStorage.removeItem(search_name + "sub_types");

    var allow_refresh = $("#type_id").val() != search_type;

    get_search_results('', allow_refresh, search_type);
}

function hide_filter() {
    $(".MP_submenu").data('show', 0);
    $("aside ul.up li ul").css("display", "");
    $(".MP_submenu").parent().children().first().removeClass('active');
}

function show_filter() {
    $(".MP_submenu").data('show', 1);
    $("aside ul.up li ul").css("display", "inline-block");
}

function get_search_results(org_key, refresh = false, search_type = 1) {
    var search_name = (search_type == 1) ? "prod_" : "rfp_";
    var searchText = "";

    if ($('.txtSearch').length > 0)
        searchText = $('.txtSearch').val().trim();

    var inputSearchText = searchText;

    localStorage.setItem(search_name + "searchText", searchText);

    /*if (searchText === "") {
        searchText = 'all';
    }*/

    $.ajax({
        type: "POST",
        url: '/search/' + (search_type == 1 ? 'SetProductFilter' : 'SetRfpFilter'),
        data: { 'types': localStorage.getItem(search_name + 'types'), 'sub_types': localStorage.getItem(search_name + 'sub_types'), 'text': inputSearchText, 'OrgKey_search': org_key },
        success: function (response, textStatus, jqXHR) {
            if ($("#current-action").val() == 'result') {
                if (!refresh)
                    search_results();
                else {
                    if (search_type == 1)
                        window.location.href = '/search/result' + (searchText.length > 0 ? '?searchText=' + searchText : '');
                    else
                        window.location.href = '/search/result' + (searchText.length > 0 ? '?searchText=' + searchText + '&type=rfp' : '?type=rfp');
                }
            }
            else {
                if (search_type == 1)
                    window.location.href = '/search/result' + (searchText.length > 0 ? '?searchText=' + searchText : '');
                else
                    window.location.href = '/search/result' + (searchText.length > 0 ? '?searchText=' + searchText + '&type=rfp' : '?type=rfp');
            }
        }
    });
}
function mark_favorite(item) {
    var key = $(item).data("key");
    var type = $(item).data("type");
    var status = true;

    if ($(item).hasClass('fa-star')) {
        $(item).removeClass('fa-star').addClass('fa-star-o');
        status = false;
    }
    else {
        $(item).removeClass('fa-star-o').addClass('fa-star');
        status = true;
    }

    $.ajax({
        type: "POST",
        url: '/base/AddToFavorite',
        data: { 'key': key, 'type': type, 'status': status },
        success: function (response, textStatus, jqXHR) {
            //alert(response.Message);
        }
    });
}