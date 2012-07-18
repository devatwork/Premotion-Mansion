<!-- User menu section -->
<tpl:section name="UserMenu">
	<div class="btn-group pull-right">
		<a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
			<i class="icon-user"></i> Welcome {User.name}
			<span class="caret"></span>
		</a>
		<ul class="dropdown-menu">
			<li>
				<a href="{CmsRouteUrl( 'Authentication', 'Logoff' )}" title="Sign Out">
					<i class="icon-signout"></i>Sign Out
				</a>
			</li>
		</ul>
	</div>
</tpl:section>



<!-- layout section -->
<tpl:section name="Layout" field="Content">
	<div class="row-fluid">
		<div class="span3">
			{Navigation}
		</div>
		<div class="span9">
			{View}
		</div>
	</div>
</tpl:section>



<!-- Navigation menu sections -->
<tpl:section name="Navigation">
	<div class="well sidebar-nav">
		<ul class="nav nav-list">
			{NavigationItem}
		</ul>
	</div>
</tpl:section>



<tpl:section name="OldNav">
	<li class="active">
		<a href="index.htm">
			<i class="icon-home"></i> Home
		</a>
	</li>
	<li class="nav-header">Content</li>
	<li>
		<a href="browse.htm">
			<i class="icon-list"></i> Browse
		</a>
	</li>
	<li>
		<a href="find.htm">
			<i class="icon-search"></i> Find
		</a>
	</li>

	<li class="nav-header">Websites</li>
	<li>
		<a href="website.htm">
			<i class="icon-globe"></i> Website A
		</a>
	</li>
	<li>
		<a href="website.htm">
			<i class="icon-globe"></i> Website B
		</a>
	</li>

	<li class="nav-header">Assets</li>
	<li>
		<a href="#">
			<i class="icon-picture"></i> Images
		</a>
	</li>
	<li>
		<a href="#">
			<i class="icon-file"></i> Files
		</a>
	</li>

	<li class="nav-header">Tags</li>
	<li>
		<a href="#">
			<i class="icon-tags"></i> Manage
		</a>
	</li>

	<li class="nav-header">Configuration</li>
	<li>
		<a href="#">
			<i class="icon-calendar"></i> Scheduled tasks
		</a>
	</li>

	<li class="nav-header">Security</li>
	<li>
		<a href="#">
			<i class="icon-key"></i> Roles
		</a>
	</li>
	<li>
		<a href="#">
			<i class="icon-group"></i> Groups
		</a>
	</li>
	<li>
		<a href="#">
			<i class="icon-user"></i> Users
		</a>
	</li>
</tpl:section>