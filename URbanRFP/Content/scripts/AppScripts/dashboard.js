$(document).ready(function () {
    $("#change-list-view").click(function () {
        if ($(this).data('view') == 'list') {
            $(this).attr('src', '/Content/images/tile-view.png');
            $(this).data('view', 'tile');
            $(".rfp-list").hide();
            $(".rfp-tile").show();
            $(".bordered-section").addClass("bordered-section-tile");
        }
        else {
            $(this).attr('src', '/Content/images/list-view.png');
            $(this).data('view', 'list');
            $(".rfp-list").show();
            $(".rfp-tile").hide();
            $(".bordered-section").removeClass("bordered-section-tile");
        }
    });

    $(".rfp-status-filter").change(function () {
        $(".status-none").hide();

        if ($(this).val() == 0) {
            $(".status-done,.status-inprogress").show();
        }
        else if ($(this).val() == 1) {
            $(".status-done").hide();
            $(".status-inprogress").show();

            if ($(".status-inprogress").length == 0) {
                $(".status-none").show();
            }
        }
        else if ($(this).val() == 2) {
            $(".status-done").show();
            $(".status-inprogress").hide();

            if ($(".status-done").length == 0) {
                $(".status-none").show();
            }
        }
    });

    if ($(".tbl-dash-rfp-list").length > 0) {
        $(".tbl-dash-rfp-list").tablesorter({ headers: { 0: { sorter: false }, 8: { sorter: false } } });
    }

    if ($(".tbl-dash-rfq-list").length > 0) {
        $(".tbl-dash-rfq-list").tablesorter({ headers: { 0: { sorter: false }, 8: { sorter: false } } });
    }

    if ($(".table-favorite").length > 0) {
        $(".table-favorite").tablesorter({ headers: { 0: { sorter: false }, 6: { sorter: false } } });
    }

    if ($(".comment").length > 0) {
        $(".comment").shorten();
    }

    if ($("#tbl-response").length > 0) {
        $("#tbl-response").tablesorter({ headers: { 0: { sorter: false }, 1: { sorter: false }, 2: { sorter: false }, 3: { sorter: false }, 4: { sorter: false }, 5: { sorter: false }, 6: { sorter: false }, 7: { sorter: false } } });
    }
});