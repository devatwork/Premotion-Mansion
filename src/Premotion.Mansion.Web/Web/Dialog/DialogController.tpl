<tpl:section name="Dialog"><!doctype html>
<!--[if lt IE 7]> <html class="no-js ie6 oldie popup" lang="en"> <![endif]-->
<!--[if IE 7]>    <html class="no-js ie7 oldie popup" lang="en"> <![endif]-->
<!--[if IE 8]>    <html class="no-js ie8 oldie popup" lang="en"> <![endif]-->
<!--[if gt IE 8]><!-->
<html class="no-js popup" lang="en">
<!--<![endif]-->
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<base href="{Request.baseUrl}/">
	<title></title>
	<meta name="description" content="">
	<meta name="author" content="">
	<meta name="viewport" content="width=device-width,initial-scale=1">
	<link rel="stylesheet" href="{MergeResourceUrl( '/Dialog/Css/Style.css' )}">
	<script src="{MergeResourceUrl( '/Dialog/Js/Header.js' )}"></script>
	{Header}
</head>
<body class="popup">

	<header>
		<h2>{DialogProperties.title}</h2>
	</header>

	<div class="content">
		{Control}
	</div>

	<footer>
	</footer>

	<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
	<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.15/jquery-ui.min.js"></script>
	<!--[if (gte IE 6)&(lte IE 8)]>
	<script type="text/javascript" src="{StaticResourceUrl( '/Shared/Js/Libs/selectivizr.js' )}"></script>
	<![endif]-->
	<script defer src="{MergeResourceUrl( '/Dialog/Js/Footer.js' )}"></script>
</body>
</html></tpl:section>