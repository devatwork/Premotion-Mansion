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

		<p>Message: {Post.message}</p>
		<form action="{Request.applicationUrl}" method="post">
			<textarea name="message">{Post.message}</textarea>
			<input type="submit" name="send">
		</form>

		<script>var coolScript; {Post.message}</script>

		{CrumbPath}
		{Layout}

		<!-- Le javascript
		================================================== -->
		<!-- Placed at the end of the document so the pages load faster -->
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/jquery/jquery-ui.js' )}"></script>
		<script src="{DynamicResourceUrl( '/Controls/Js/Controls.js' )}"></script>
		<script src="{StaticResourceUrl( '/Shared/js/bootstrap/bootstrap.min.js' )}"></script>
	</body>
</html></tpl:section>

<tpl:section name="Layout">
	<div class="row-fluid">
		<div class="span6">
			<h6>Left</h6>
			{Post.message}
			{Results}
		</div>
		<div class="span6">
			<h6>Right</h6>
			{Message}
		</div>
	</div>
</tpl:section>


<tpl:section name="Results">
	<div class="block highlight border list-block">
		<div class="content">
			<ol class="unstyled list content-list extensive">
				{ListItem}
			</ol>
		</div>
	</div>
	{#RenderPagingControl( $ResultSet, Concat( 'block-', BlockProperties.id ) )}
</tpl:section>


<tpl:section name="Message">
	<div>Het bericht is: {Post.message}</div>
</tpl:section>


<tpl:section name="CrumbPath">
	<ul class="breadcrumb">
		{Crumb}
	</ul>
</tpl:section>

<tpl:section name="ParentCrumb" field="Crumb">
	<li>
		<a href="{CmsNodeUrl( ParentNode.id, 'CmsBrowserPlugin', 'Browse' )}" title="Navigate to {ParentNode.name}">{#GetTypeDefinitionIcon( ParentNode.type )} {ParentNode.name}</a>
		<span class="divider">/</span>
	</li>
</tpl:section>

<tpl:section name="ActiveCrumb" field="Crumb">
	<li class="active">{#GetTypeDefinitionIcon( CurrentNode.type )} {CurrentNode.name}</li>
</tpl:section>