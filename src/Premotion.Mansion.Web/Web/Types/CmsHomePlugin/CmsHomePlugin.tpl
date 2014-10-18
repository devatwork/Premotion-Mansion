<tpl:section name="View">
	<div class="jumbotron">
		<h2>Welcome to Premotion Mansion CMS</h2>
		<p>Use the menu on the left or the actions below to start managing content for your websites.</p>
	</div>
	<div class="row">
		{HomeBlock}
	</div>
</tpl:section>

<tpl:section name="HomeBlock">
	<div class="col-md-4">
		<div class="thumbnail">
			<div class="caption">
				<h3>
					<i class="{Section.iconClass}"></i> {Section.label}
				</h3>
				<p>{Section.description}</p>
				<p>
					<a href="{Section.url}" class="btn btn-primary" role="button">
						Go <i class="icon-chevron-right"></i>
					</a>
				</p>
			</div>
		</div>
	</div>
</tpl:section>