<tpl:section name="View">
	<div class="hero-unit">
		<h2>Welcome to Premotion Mansion CMS</h2>
		<p>Use the menu on the left or the actions below to start managing content for your websites.</p>
	</div>
	<ul class="thumbnails">
		{HomeBlock}
	</ul>
</tpl:section>

<tpl:section name="HomeBlock">
	<li class="span4">
		<div class="clearfix thumbnail">
			<h3>
				<i class="{Section.iconClass}"></i> {Section.label}
			</h3>
			<p>{Section.description}</p>
			<a href="{Section.url}"class="btn btn-primary pull-right">
				Go <i class="icon-chevron-right"></i>
			</a>
		</div>
	</li>
</tpl:section>