/* CMS init functions */
;(function($, undefined) {
	$(document).ready(function () {
		/* init tooltips */
		$('[rel=tooltip]').tooltip();
		
		/* initialize modal forms */
		$('#modal-popup').bind('show', function() {
			// create the frame
			var frame = $('<iframe class="seamless" id="modal-frame" name="modal-frame" />');
			frame.load(function() {
				this.style.height = this.contentWindow.document.body.offsetHeight + 'px';
			});

			// navigate
			var href = $(this).attr('data-href');
			frame.attr("src", href);

			// assemble dialog
			$(this).html(frame);
		});

		// register handlers for top level document
		if (top.document === document) {
			var topDocument = $(document);

			// handle dialog close
			topDocument.bind("cms.dialog.close", function() {
				$('#modal-popup').modal("hide");
			});

			// handle navigate
			topDocument.bind("cms.navigate", function(event, url) {
				$('#modal-popup').modal("hide");
				document.location = url;
			});

			// refresh
			topDocument.bind("cms.refresh", function() {
				$('#modal-popup').modal("hide");
				document.location = document.location;
			});
		}

	});
})(jQuery);