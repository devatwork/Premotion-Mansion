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
	var _gaq=[['_setAccount','{SiteNode.googleAnalyticsTrackingCode}'],['_trackPageview']];
	(function(d,t){ var g=d.createElement(t),s=d.getElementsByTagName(t)[0];
	g.src=('https:'==location.protocol?'//ssl':'//www')+'.google-analytics.com/ga.js';
	s.parentNode.insertBefore(g,s)}(document,'script'));
</script>
</tpl:section>


<!-- admin sections -->
<tpl:section name="PortalAdminScript" requires="{HasPortalAdminPermission()}">
	<script defer src="{MergeResourceUrl( '/Js/PortalAdmin.js' )}"></script>
</tpl:section>

<tpl:section name="ColumnAdminParameters" requires="{HasPortalAdminPermission()}"> data-column-name="{BlockColumn.name}" data-column-owner-id="{OwnerNode.id}"</tpl:section>



<!-- crumb trails -->
<tpl:section name="CrumbTrail">
	<ul class="breadcrumb">
		{CrumbTrailItem}
	</ul>
</tpl:section>

<tpl:section name="CrumbTrailItem">
	<li {If( IsLast(), 'class="active"' )}>
		{@CrumbTrailHome}
		{RenderSection( If( And( CrumbProperties.IsLinked, Not( IsLast() ) ), 'CrumbTrailItemLinked', 'CrumbTrailItemLabel' ) )}
		{@CrumbTrailDivider}
	</li>
</tpl:section>

	<tpl:section name="CrumbTrailItemLinked"><a href="{NodeUrl( CrumbProperties.Node )}">{RenderSection( 'CrumbTrailItemLabel' )}</a></tpl:section>

	<tpl:section name="CrumbTrailItemLabel">{CrumbProperties.name}</tpl:section>

	<tpl:section name="CrumbTrailHome" requires="{IsFirst()}"><i class="icon-home"></i></tpl:section>

	<tpl:section name="CrumbTrailDivider" requires="{Not( IsLast() )}"><span class="divider">&raquo;</span></tpl:section>