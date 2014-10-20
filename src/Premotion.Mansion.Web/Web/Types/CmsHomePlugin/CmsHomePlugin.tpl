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

	<div class="panels row">
		{HomeBlock}
	</div>
</tpl:section>

<tpl:section name="HomeBlock">
	<div class="col-sm-6 col-md-4">
		<div class="panel panel-default">
			<div class="panel-heading">
				<h3 class="panel-title">
					<i class="{Section.iconClass}"></i> {Section.label}
				</h3>
			</div>
			<div class="panel-body">
				<p>{Section.description}</p>
				<a href="{Section.url}" class="btn btn-primary pull-right" role="button">
					Go <i class="icon-chevron-right"></i>
				</a>
			</div>
		</div>
	</div>
</tpl:section>