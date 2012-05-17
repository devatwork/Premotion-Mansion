<tpl:section name="BlockColumn">
	<div class="column {BlockColumn.name}">
		<div class="column-content"{@ColumnAdminParameters}>
			{Block}
		</div>
	</div>
</tpl:section>


<tpl:section name="BlockContainer">{Block}</tpl:section>

<!-- Google Applications -->
<tpl:section name="GoogleSiteVerification" requires="{Not( IsEmpty( SiteNode.googleSiteVerification ) )}"><meta name="google-site-verification" content="{SiteNode.googleSiteVerification}"></tpl:section>

<tpl:section name="GoogleAnalytics" requires="{And( Not( IsEmpty( SiteNode.googleAnalyticsTrackingCode ) ), IsTrue( Application.live ) )}">
<script>
	window._gaq = [['_setAccount','{SiteNode.googleAnalyticsTrackingCode}'],['_trackPageview'],['_trackPageLoadTime']];
	Modernizr.load({
		load: ('https:' == location.protocol ? '//ssl' : '//www') + '.google-analytics.com/ga.js'
	});
</script>
</tpl:section>


<!-- admin sections -->
<tpl:section name="PortalAdminScript" requires="{HasPortalAdminPermission()}">
	<script defer src="{MergeResourceUrl( '/Js/PortalAdmin.js' )}"></script>
</tpl:section>

<tpl:section name="ColumnAdminParameters" requires="{HasPortalAdminPermission()}"> data-column-name="{BlockColumn.name}" data-column-owner-id="{OwnerNode.id}"</tpl:section>