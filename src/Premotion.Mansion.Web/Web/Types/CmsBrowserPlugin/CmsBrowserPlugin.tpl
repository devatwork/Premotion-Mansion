<!-- View: Browse -->
<tpl:section name="Browse" field="View">
	<div class="page-slider">
		{CrumbPath}
		{Content}
		{ActionButtons}
	</div>
</tpl:section>



<tpl:section name="CrumbPath">
	<ul class="breadcrumb">
		{Crumb}
	</ul>
</tpl:section>

	<tpl:section name="ParentCrumb" field="Crumb">
		<li>
			<a href="{CmsNodeBrowserUrl( 'false', ParentNode.id )}" data-href="{CmsNodeBrowserUrl( 'true', ParentNode.id )}" title="Navigate to {ParentNode.name}">{GetTypeDefinitionIcon( ParentNode.type )} {ParentNode.name}</a>
			<span class="divider">/</span>
		</li>
	</tpl:section>

	<tpl:section name="ActiveCrumb" field="Crumb">
		<li class="active">{GetTypeDefinitionIcon( CurrentNode.type )} {CurrentNode.name}</li>
	</tpl:section>



<tpl:section name="NodeBrowser" field="Content">
	<table class="table table-striped table-bordered browser">
		<tr>
			<th>Name</th>
			<th>Type</th>
		</tr>
		{Child}
	</table>
	<div class="btn-toolbar">
		<div class="btn-group">
			<button href="#" class="btn btn-primary" rel="tooltip" title="Add a new child to this folder">
				<i class="icon-plus-sign"></i> Add child
			</button>
			<button href="#" class="btn btn-primary" rel="tooltip" title="Reorder the position of this node">
				<i class="icon-reorder"></i> Change order
			</button>
			<button href="#" class="btn btn-primary" rel="tooltip" title="Move this node to another folder">
				<i class="icon-move"></i> Move
			</button>
			<button href="#" class="btn btn-primary" rel="tooltip" title="Move this to another folder">
				<i class="icon-copy"></i> Copy
			</button>
			<button href="#" class="btn btn-danger" rel="tooltip" title="Delete this node">
				<i class="icon-remove-sign"></i> Delete
			</button>
		</div>
	</div>
</tpl:section>

	<tpl:section name="NodeBrowserChild" field="Child">
		<tr>
			<td>
				<a href="{CmsNodeBrowserUrl( 'false', ChildNode.id )}" data-href="{CmsNodeBrowserUrl( 'true', ChildNode.id )}" title="Navigate to {ChildNode.name}">{GetTypeDefinitionIcon( ChildNode.type )} {ChildNode.name}</a>
			</td>
			<td>{GetTypeDefinitionLabel( ChildNode.type )}</td>
		</tr>
	</tpl:section>



<tpl:section name="NodeProperties" field="Content">
	<hr>
	{Control}
</tpl:section>



<!-- View: Find -->
<tpl:section name="Find" field="View">
	<div class="page-slider">
		<div class="row-fluid">
			<div class="span8">
				<h2>2.213 Results</h2>
				<table class="table table-striped table-bordered browser">
					<tr>
						<th>Name</th>
						<th>Type</th>
						<th>Context</th>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 1
							</a>
						</td>
						<td>Folder</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 2
							</a>
						</td>
						<td>Folder</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 3
							</a>
						</td>
						<td>Folder</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 1
							</a>
						</td>
						<td>Article</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 2
							</a>
						</td>
						<td>Article</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 3
							</a>
						</td>
						<td>Article</td>
						<td>
							...Lorem ipsum dolor sit amet, consectetur adipiscing elit. <strong>Mauris</strong> vitae libero nec turpis eleifend volutpat..
						</td>
					</tr>
				</table>
				<div class="pagination">
					<ul>
						<li class="disabled">
							<a href="#">Prev</a>
						</li>
						<li class="active">
							<a href="#">1</a>
						</li>
						<li>
							<a href="browse-slice.htm" data-href="browse-slice.htm">2</a>
						</li>
						<li>
							<a href="browse-slice.htm" data-href="browse-slice.htm">3</a>
						</li>
						<li>
							<a href="browse-slice.htm" data-href="browse-slice.htm">4</a>
						</li>
						<li>
							<a href="browse-slice.htm" data-href="browse-slice.htm">Next</a>
						</li>
					</ul>
				</div>
			</div>
			<div class="span4">
				<form class="well form-horizontal">
					<fieldset>
						<div class="control-group">
							<label class="control-label" for="input01">Keywords:</label>
							<div class="controls">
								<input type="search" class="input-large" id="input01">
										</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="input02">Types:</label>
							<div class="controls">
								<ul class="nav nav-list">
									<li>
										<a href="#" rel="tooltip" title="Filter on articles">Article (1)</a>
									</li>
									<li>
										<a href="#" rel="tooltip" title="Filter on pages">Page (1)</a>
									</li>
								</ul>
							</div>
						</div>
						<div class="form-actions">
							<button type="submit" class="btn btn-primary">
								<i class="icon-search"></i> Search
							</button>
						</div>
					</fieldset>
				</form>
			</div>
		</div>
	</div>
</tpl:section>