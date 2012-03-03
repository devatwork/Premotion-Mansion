/* ==|== Best practices ==================================================
Author: Premotion Software Solutions
Requires: jQuery
========================================================================== */
(function ($, undefined) {
	/* Clickable */
	$(".clickable").click(function () {
		window.location = $(this).find("a").attr("href");
		return false;
	});
	$(".clickable a").click(function (e) { e.stopPropagation(); });

	/* Hoverable */
	$(".hoverable, .clickable").hover(
		function () { $(this).addClass("hover"); },
		function () { $(this).removeClass("hover"); }
	);
})(window.jQuery);