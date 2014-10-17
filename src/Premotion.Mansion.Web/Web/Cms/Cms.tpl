<tpl:section name="CmsPage"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8">
		<title>Premotion Mansion CMS</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">

		<!-- Le styles -->
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap.min.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Controls/Css/Controls.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/jquery/jquery-ui.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/fontawesome/font-awesome.css' )}">
		<!--[if lt IE 8]>
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/fontawesome/font-awesome-ie7.css' )}">
		<![endif]-->
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Cms/css/cms.css' )}">
		{@IncludeModernizrJS}
		{Header}
	</head>
	<body>

		{Container}
		
		<!-- Le javascript
		================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
		{@IncludeSelectivizrJS}
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery-ui.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/lodash/lodash.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/bootstrap/bootstrap.min.js' )}"></script>
		<script src="{StaticResourceUrl( '/Cms/js/cms.js' )}"></script>
		{Footer}
	</body>
</html></tpl:section>



<tpl:section name="PageContainer" field="Container">
	<div class="navbar navbar-default navbar-fixed-top">
		<div class="container-fluid">
			<div class="navbar-header">
				<button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#cms-navbar-collapse-1">
					<span class="sr-only">Toggle navigation</span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
				</button>
				<a class="navbar-brand" href="{CmsNodeUrl( RootNode.id, 'CmsHomePlugin', 'Default' )}">Premotion Mansion CMS</a>
			</div>
			<div class="navbar-collapse collapse" id="cms-navbar-collapse-1">
				<ul class="nav navbar-nav navbar-right">
					{UserMenu}
				</ul>
			</div>
		</div>
	</div>

	<div class="container-fluid cms-page">
		{Content}
		<hr />
		<footer>
			<p>&copy; Premotion Software Solutions</p>
		</footer>
	</div>
	
	<!-- Le modal -->
	<div class="modal fade" id="modal-popup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
			</div>
		</div>
	</div>
</tpl:section>