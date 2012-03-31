{IncludeStaticResource( '/Shared/Js/Libs/jquery.js' )}
{IncludeStaticResource( '/Shared/Js/Libs/jquery-ui.start.js' )}
{IncludeStaticResource( '/Shared/Js/Common.js' )}
{IncludeStaticResource( '/Dialog/Js/Dialog.js' )}
{IncludeDynamicResource( '/Controls/Js/Controls.js' )}

/* ==|== CMS script ======================================================
Author: Premotion Software Solutions
========================================================================== */

;(function ($, undefined) {
	// register event handlers
	if (top.document === document) {
		var topDocument = $(document);
		var nodeTreeFrame = topDocument.find("#cms-tree-frame");
		var nodeBrowserFrame = topDocument.find("#cms-browser-frame");
		var previewFrame = topDocument.find("#cms-preview-frame");
		var tabControl = $("#cms-tabs").tabs();
		
		// navigates to tree frame to a particular node
		topDocument.bind("cms.tree.navigate", function(event, url) {
			window.frames[nodeTreeFrame.attr("name")].location = url;
		});
		
		// navigates the browser frame to a particular page
		topDocument.bind("cms.browser.navigate", function(event, url) {
			window.frames[nodeBrowserFrame.attr("name")].location = url;
		});
		
		// navigates the preview fraem to a particular page
		topDocument.bind("cms.preview.navigate", function(event, url) {
			window.frames[previewFrame.attr("name")].location = url;
		});
		
		// refresh all frames
		topDocument.bind("cms.refresh.frames", function() {
			window.frames[nodeTreeFrame.attr("name")].location.reload();
			window.frames[nodeBrowserFrame.attr("name")].location.reload();
		});
		
		// refresh the comlete cms
		topDocument.bind("cms.refresh", function() {
			location.reload();
		});
		
		// close the dialog
		topDocument.bind("cms.dialog.close", function() {
			$.fancybox.close();
		});
		
		// navigates the preview fraem to a particular page
		topDocument.bind("cms.activate-preview-tab", function() {
			tabControl.tabs("select", 1);
		});
	}
	
	// initialize controls
	$(".node-tree-display").treeview({	
	});
	$("a.cms-navigation").click(function() {
		var treeUrl = $(this).attr("data-tree-url");
		if (treeUrl != undefined)
			top.jQuery(top.document).trigger("cms.tree.navigate", [ treeUrl ]);
		var browserUrl = $(this).attr("data-browser-url");
		if (browserUrl != undefined)
			top.jQuery(top.document).trigger("cms.browser.navigate", [ browserUrl ]);
		var previewUrl = $(this).attr("data-preview-url");
		if (previewUrl != undefined)
			top.jQuery(top.document).trigger("cms.preview.navigate", [ previewUrl ]);
	});
	$("a.cms-preview").click(function(event) {
		event.preventDefault();
		var previewUrl = $(this).attr("href");
		if (previewUrl != undefined) {
			top.jQuery(top.document).trigger("cms.preview.navigate", [previewUrl]);
			top.jQuery(top.document).trigger("cms.activate-preview-tab");
		}
	});
	$(".cms-dialog").click(function(event) {
		event.preventDefault();
		var link = $(this).attr("href");
		top.jQuery.fancybox({
			modal: true,
			titleShow: false,
			type: "iframe",
			padding: 0,
			scrolling: "no",
			width: "75%",
			height: "75%",
			transitionIn: "none",
			transitionOut: "none",
			href: link
		});
	});
})(window.jQuery);