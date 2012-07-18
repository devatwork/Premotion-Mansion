/* CMS init functions */
;(function($, undefined) {
	$(document).ready(function () {
		/* init tooltips */
		$('[rel=tooltip]').tooltip();
		
		/* initialize modal forms */
		$('#modal-popup').bind('show', function() {
			// create the frame
			var frame = $('<iframe class="seamless" seamless />');
			frame.load(function() {
				this.style.height = this.contentWindow.document.body.offsetHeight + 'px';
			});

			// navigate
			var href = $(this).attr('data-href');
			frame.attr("src", href);

			// assemble dialog
			$(this).html(frame);
		});
		$('.btn-popup').click(function (e){
			e.preventDefault();
			$('#modal-popup').attr('data-href', $(this).attr('href'));
			$('#modal-popup').modal("show");
		});

	});
})(jQuery);