(function ($) {

    // prettyPhoto
    jQuery(document).ready(function () {
        jQuery('a[data-gal]').each(function () {
            jQuery(this).attr('rel', jQuery(this).data('gal'));
        });
        jQuery("a[data-rel^='prettyPhoto']").prettyPhoto({ animationSpeed: 'slow', theme: 'light_square', slideshow: false, overlay_gallery: false, social_tools: false, deeplinking: false });
    });


})(jQuery);

/*$(document).ready(function () {
    $(function () {
        $(document).tooltip({
            position: {
                my: "center bottom-20",
                at: "center top",
                using: function (position, feedback) {
                    $(this).css(position);
                    $("<div>")
                      .addClass("arrow")
                      .addClass(feedback.vertical)
                      .addClass(feedback.horizontal)
                      .appendTo(this);
                }
            }
        });
    });
});*/

function scrollToAnchor(aid) {
    var aTag = $("a[name='" + aid + "']");
    $('html,body').animate({ scrollTop: aTag.offset().top - 60 }, 'slow');
    history.pushState({ state: 1 }, 'State 1', '#' + aid);
}