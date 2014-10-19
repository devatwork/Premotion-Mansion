<tpl:section name="View">
	<div id="alert-cms-welcome" class="alert alert-success alert-dismissible" role="alert">
		<button type="button" class="close" data-dismiss="alert">
			<span aria-hidden="true">&times;</span>
			<span class="sr-only">Close</span>
		</button>
		<div class="title">
			<strong>
				Welcome to Premotion Mansion CMS
			</strong>
		</div>
		<div class="message">
			<p>Use the menu on the left or the actions below to start managing content for your websites.</p>
		</div>
	</div>

	<div class="thumbnails row">
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