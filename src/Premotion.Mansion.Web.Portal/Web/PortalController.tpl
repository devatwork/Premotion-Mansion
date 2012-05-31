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
	<div id="portal-modal-popup" class="modal hide fade"></div>
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
		{@CrumbTrailHome}
		{RenderSection( If( And( CrumbProperties.IsLinked, Not( IsLast() ) ), 'CrumbTrailItemLinked', 'CrumbTrailItemLabel' ) )}
		{@CrumbTrailDivider}
	</li>
</tpl:section>

	<tpl:section name="CrumbTrailItemLinked"><a href="{NodeUrl( CrumbProperties.Node )}">{RenderSection( 'CrumbTrailItemLabel' )}</a></tpl:section>

	<tpl:section name="CrumbTrailItemLabel">{CrumbProperties.name}</tpl:section>

	<tpl:section name="CrumbTrailHome" requires="{IsFirst()}"><i class="icon-home"></i></tpl:section>

	<tpl:section name="CrumbTrailDivider" requires="{Not( IsLast() )}"><span class="divider">&raquo;</span></tpl:section>




<!-- portal admin dialog sections -->
<tpl:section name="PortalAdminDialog"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8">
		<title>Premotion Mansion Portal</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">

		<!-- Le styles -->
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap.min.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Controls/Css/Controls.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/jquery/jquery-ui.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Cms/css/cms.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap-responsive.min.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Css/Portal.css' )}">
		{Header}
		<!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
		<!--[if lt IE 9]>
		<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
		<![endif]-->
	</head>

	<body class="dialog">
		<div class="container-fluid">
			{Content}
		</div>
		<!-- Le javascript
		================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery-ui.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/bootstrap/bootstrap.min.js' )}"></script>
		<script src="{StaticResourceUrl( '/Cms/js/cms.js' )}"></script>
		<script src="{StaticResourceUrl( '/Js/PortalAdmin.js' )}"></script>
		{Footer}
	</body>
</html></tpl:section>