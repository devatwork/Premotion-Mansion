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
	
	/* Block actions */
	$("#portal-modal-popup").on("show", function() {
		// create the frame
		var frame = $('<iframe class="seamless" seamless />');
		frame.load(function() {
			this.style.height = this.contentWindow.document.body.offsetHeight + "px";
		});
		
		// navigate
		var href = $(this).attr("data-href");
		frame.attr("src", href);
		
		// assemble dialog
		$(this).html(frame);
	});
	$(".command.configure").click( function(e) {
		e.preventDefault();
		$("#portal-modal-popup").attr("data-href", $(this).attr("href"));
		$("#portal-modal-popup").modal('show');
	});
	$(".command.remove").click( function(e) {
		e.preventDefault();
		$("#portal-modal-popup").attr("data-href", $(this).attr("href"));
		$("#portal-modal-popup").modal('show');
	});
	
	// register handlers for top level document
	if (top.document === document) {
		var topDocument = $(document);
		
		// handle dialog close
		topDocument.bind("portal.dialog.close", function() {
			$('#portal-modal-popup').modal("hide");
		});
		
		// handle navigate
		topDocument.bind("portal.navigate", function(event, url) {
			$('#modal-popup').modal("hide");
			document.location = url;
		});
	}

})(window.jQuery);