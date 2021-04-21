$(document).ready(function () {
    $("#calendar").simpleCalendar({
        // event dates
        events: ['2019-07-20'],
        // event info to show
        eventsInfo: ['Event 1', 'Event 2', 'Event 3', 'Event 4'],
    });

    $("#hide-contact").click(function () {
        if ($("#contact-details").is(":visible")) {
            $("#hide-contact").text("(show full details)");
        }
        else {
            $("#hide-contact").text("(hide full details)");
        }
        $("#contact-details").fadeToggle(300);
    });

    $("a.show-more,a.show-less").click(function (e) {
        if ($(this).hasClass("show-more")) {
            $("#more-text").show();
            $(".show-less").show();
            $(this).hide();
        }
        else {
            $("#more-text").hide();
            $(".show-more").show();
            $(this).hide();
        }
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

    // Binds the hidden input to be used as datepicker.
    if ($('.datepicker-input').length > 0) {
        $('.datepicker-input').datepicker({
            format: 'MM d, yyyy', autoclose: true
        }).on('change', function (e) {
            $("." + $(this).data('section')).html($(this).val());
        });
    }
});