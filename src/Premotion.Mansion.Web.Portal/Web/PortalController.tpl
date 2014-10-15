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

<tpl:section name="GoogleAnalytics" requires="{And( Not( IsEmpty( SiteNode.googleAnalyticsTrackingCode ) ), IsTrue( Application.APPLICATION_IS_LIVE ) )}">
<script>
	var _gaq=[['_setAccount','{SiteNode.googleAnalyticsTrackingCode}'],['_trackPageview']];
	(function(d,t){ var g=d.createElement(t),s=d.getElementsByTagName(t)[0];
	g.src=('https:'==location.protocol?'//ssl':'//www')+'.google-analytics.com/ga.js';
	s.parentNode.insertBefore(g,s)}(document,'script'));
</script>
</tpl:section>


<!-- admin sections -->
<tpl:section name="PortalAdminScript" requires="{HasPortalAdminPermission()}">
	<div class="modal fade" id="portal-modal-popup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
			</div>
		</div>
	</div>
	<script src="{MergeResourceUrl( '/Js/PortalAdmin.js' )}"></script>
</tpl:section>

<tpl:section name="PortalAdminStyles" requires="{HasPortalAdminPermission()}">
	<link rel="stylesheet" href="{DynamicResourceUrl( '/Css/Portal.css' )}">
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
		{#RenderSection( If( And( CrumbProperties.IsLinked, Not( IsLast() ) ), 'CrumbTrailItemLinked', 'CrumbTrailItemLabel' ) )}
		{@CrumbTrailDivider}
	</li>
</tpl:section>

	<tpl:section name="CrumbTrailItemLinked"><a href="{NodeUrl( CrumbProperties.Node )}">{RenderSection( 'CrumbTrailItemLabel' )}</a></tpl:section>

	<tpl:section name="CrumbTrailItemLabel">{CrumbProperties.name}</tpl:section>

	<tpl:section name="CrumbTrailDivider" requires="{Not( IsLast() )}"><span class="divider">/</span></tpl:section>




<!-- portal admin dialog sections -->
<tpl:section name="PortalAdminDialog"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8" />
		<title>Premotion Mansion Portal</title>
	</head>

	<body class="dialog">
		<div class="container-fluid">
			{Content}
		</div>
		{Footer}
	</body>
</html></tpl:section>