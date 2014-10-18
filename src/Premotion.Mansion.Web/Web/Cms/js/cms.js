/* CMS init functions */
(function($, undefined) {
	$(document).ready(function () {
		/* init tooltips */
		$('[rel=tooltip]').tooltip();
		dismissibleAlerts();
	});


	/* 
		Dismissable alerts are alerts that can be closed.
		This code will read and write a cookie called 'dismissed-alerts' to keep track of the (by the user) closed alerts.
		This way we can prevent bothering the user with an alert he already closed.
	*/
	function dismissibleAlerts() {
		/* Get currently dismissed alerts from cookie */
		var dismissedAlertIds = new Array();
		var dismissedAlertIdsCookie = $.cookie('dismissed-alerts');
		if (typeof dismissedAlertIdsCookie != 'undefined')
			dismissedAlertIds = JSON.parse($.cookie('dismissed-alerts'));

		/* Show alerts that haven't been dismissed yet */
		var allDismissableAlerts = $('.alert.alert-dismissible');
		var dismissedAlerts = $.map(dismissedAlertIds, function (i) { return document.getElementById(i) });
		allDismissableAlerts.each(function () {
			if (jQuery.inArray(this.id, dismissedAlertIds) == -1)
				$(this).fadeIn();
		});

		/* Add dismissed alerts to cookie */
		$('.alert.alert-dismissible button.close[data-dismiss="alert"]').click(function () {
			var aid = $(this).parent('.alert').attr('id');
			if (typeof aid != 'undefined') {
				if (jQuery.inArray(aid, dismissedAlertIds) == -1) {
					dismissedAlertIds.push(aid);
					$.cookie('dismissed-alerts', JSON.stringify(dismissedAlertIds), { expires: 365, path: '/' });
				}
			}
		});
	}
})(jQuery);