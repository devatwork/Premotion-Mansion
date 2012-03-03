/* ==|== Column script ===================================================
Author: Premotion Software Solutions
========================================================================== */
// portal functions
(function ($, undefined) {
	$().ready(function() {
		/* make cells the same height */
		$(".column-row").each(function() {
			var maxHeight = 0;
			$(this).children().each(function() {
				var columnHeight = $(this).outerHeight();
				maxHeight = (columnHeight > maxHeight ) ? columnHeight : maxHeight;
			});
			$(this).children().height( maxHeight );
		});
	});
})(window.jQuery);