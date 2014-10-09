<tpl:section name="Dialog"><!DOCTYPE html>
<html>
	<head>
		<meta charset="utf-8" />
		<title>Premotion Mansion CMS</title>
	</head>
	<body>


		{Control}

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
</html>
</tpl:section>