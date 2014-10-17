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
	<div class="well">
		<ul class="nav">
			{NavigationItem}
		</ul>
	</div>
</tpl:section>