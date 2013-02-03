<tpl:section name="Page"><!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8">
		<title>Premotion Mansion CMS</title>
		<meta name="viewport" content="width=device-width, initial-scale=1.0">
		<!-- Le styles -->
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap.min.css' )}">
		<link rel="stylesheet" href="{DynamicResourceUrl( '/Controls/Css/Controls.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/jquery/jquery-ui.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/bootstrap/bootstrap-responsive.min.css' )}">
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/fontawesome/font-awesome.css' )}">
		<!--[if lt IE 8]>
		<link rel="stylesheet" href="{StaticResourceUrl( '/Shared/css/fontawesome/font-awesome-ie7.css' )}">
		<![endif]-->
		<!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
		<!--[if lt IE 9]>
		<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
		<![endif]-->
	</head>
	<body>

		{Controls}

		<!-- Le javascript
		================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery-ui.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/lodash/lodash.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/bootstrap/bootstrap.min.js' )}"></script>
		{Footer}
	</body>
</html></tpl:section>