<tpl:section name="CmsPage"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8">
		<title>Premotion Mansion CMS</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">

		<!-- Le styles -->
		<link rel="stylesheet" href="{StaticResourceUrl( '/Cms/css/bootstrap.min.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Controls/Css/Controls.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/libs/jquery/jquery-ui.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Cms/css/cms.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Cms/css/bootstrap-responsive.min.css' )}">
		{Header}
		<!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
		<!--[if lt IE 9]>
		<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
		<![endif]-->
	</head>

	<body class="{@PageBodyClasses}">
		{CmsHeader}
		<div class="container-fluid">
			{Content}
			{CmsFooter}
		</div>

		<!-- Le modal -->
		<div class="modal fade hide" id="modal-popup"></div>

		<!-- Le javascript
		================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
		<script src="{StaticResourceUrl( '/Shared/js/libs/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/libs/jquery-ui.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		<script src="{StaticResourceUrl( '/Cms/js/bootstrap.min.js' )}"></script>
		<script src="{StaticResourceUrl( '/Cms/js/cms.js' )}"></script>
		{Footer}
	</body>
</html></tpl:section>

<tpl:section name="PageBodyClasses">cms</tpl:section>

<tpl:section name="CmsHeader">
	<div class="navbar navbar-fixed-top">
		<div class="navbar-inner">
			<div class="container-fluid">
				<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
				</a>
				<span class="brand">Premotion Mansion CMS</span>
				{ProfileButton}
				{Navigation}
			</div>
		</div>
	</div>
</tpl:section>

<tpl:section name="CmsFooter">
	<hr>
	<footer>
		<p>
			&copy; <a href="http://www.premotion.nl/" title="Premotion Software Solutions">Premotion Software Solutions</a> 2010 - {FormatDate( Now(), 'yyyy' )}
		</p>
	</footer>
</tpl:section>