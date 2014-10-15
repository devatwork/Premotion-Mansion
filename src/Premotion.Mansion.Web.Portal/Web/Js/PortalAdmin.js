/* ==|== Column script ===================================================
Author: Premotion Software Solutions
========================================================================== */
/* portal admin functions  */
(function ($, undefined) {
	/* Hide block toolbar menu */
	$(".block").hover(
		function () {
			$(this).find(".block-toolbar").show();
		},
		function () {
			$(this).find(".block-toolbar").hide();
		}
	);

	/* Sorting & Dragging blocks */
	$(".column-content").sortable({
		connectWith: ".column-content",
		placeholder: "blockghost",
		handle: ".block-toolbar .command.move",
		update: function(event, ui) {
			if (this === ui.item.parent()[0]) {
				var block = ui.item;
				var blockId = block.attr("data-id");
				var column = block.parent();
				var columnName = column.attr("data-column-name");
				var columnOwnerId = column.attr("data-column-owner-id");
				var order = column.children().index(block) + 1;
				var params = {
					blockId: blockId,
					columnOwnerId: columnOwnerId,
					columnName: columnName,
					order: order
				};
				
				// send the update request
				$.get("{RouteUrl( 'Portal', 'UpdateBlockPosition' )}?" + $.param(params));
				
				// refresh the page
				location.reload();
			}
		}
	});
	$(".column-content").droppable({
		hoverClass: "hover",
		activeClass: "droptarget"
	});
})(window.jQuery);