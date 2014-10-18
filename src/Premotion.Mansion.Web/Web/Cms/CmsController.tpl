<!-- User menu section -->
<tpl:section name="UserMenu">
	<li class="dropdown">
		<a class="dropdown-toggle" data-toggle="dropdown" href="#">
			<i class="icon-user"></i> Welcome {User.name}
			<span class="caret"></span>
		</a>
		<ul class="dropdown-menu" role="menu">
			<li>
				<a href="{CmsRouteUrl( 'Authentication', 'Logoff' )}" title="Sign Out">
					<i class="icon-signout"></i>Sign Out
				</a>
			</li>
		</ul>
	</li>
</tpl:section>



<!-- layout section -->
<tpl:section name="Layout" field="Content">
	<div class="row">
		<div class="col-md-3">
			{Navigation}
		</div>
		<div class="col-md-9">
			{View}
		</div>
	</div>
</tpl:section>



<!-- Navigation menu sections -->
<tpl:section name="Navigation">
	<ul class="nav nav-pills nav-stacked">
		{NavigationItem}
	</ul>
</tpl:section>