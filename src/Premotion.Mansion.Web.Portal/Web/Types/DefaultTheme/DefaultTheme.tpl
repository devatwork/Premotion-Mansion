<tpl:section name="Theme"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8">
		<base href="{Request.baseUrl}/">
		<title>{RenderPageTitle()}</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">
		{@FavIcon}
		{RenderSeoMetaTags()}
		{@GoogleSiteVerification}

		<!-- Le styles -->
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap.min.css' )}">
		{@PortalAdminStyles}
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap-responsive.min.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Types/DefaultTheme/css/theme.css' )}">

		<!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
		<!--[if lt IE 9]>
		<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
		<![endif]-->
		{Header}
	</head>

	<body>
		<div class="navbar navbar-fixed-top">
			<div class="navbar-inner">
				<div class="container">
					<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-main">
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
					</a>
					<a class="brand" href="{NodeUrl( $SiteNode )}">{SiteNode.name}</a>
					<div class="nav-collapse nav-main">
						{@MainNavigation}
					</div>
					<!--/.nav-collapse -->
				</div>
			</div>
		</div>

		<div class="container">
			{RenderCrumbTrail()}

			{Layout}

			<footer>
				<div class="navbar ">
					<div class="navbar-inner">
						<div class="container">
							<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-footer">
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
								<span class="icon-bar"></span>
							</a>
							<div class="nav-collapse nav-footer">
								{@FooterNavigation}
							</div>
						</div>
					</div>
				</div>
				<p>Copyright &copy; 2011 - {FormatDate( Now(), 'yyyy' )} {SiteNode.name}, alle rechten voorbehouden.</p>
			</footer>
		</div>
		<!-- /container -->
		
		<!-- Le javascript
		================================================== -->
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery-ui.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		{@PortalAdminScript}
		<script src="{StaticResourceUrl( '/Shared/js/bootstrap/bootstrap.min.js' )}"></script>
		<script src="{StaticResourceUrl( '/Types/DefaultTheme/js/script.js' )}"></script>
		{Footer}
		{@GoogleAnalytics}
	</body>
</html></tpl:section>

<tpl:section name="MainNavigation" requires="{IsNode( $MainNavigationNode )}">{RenderNavigation( $MainNavigationNode )}</tpl:section>
<tpl:section name="FooterNavigation" requires="{IsNode( $FooterNavigationNode )}">{RenderNavigation( $FooterNavigationNode )}</tpl:section>