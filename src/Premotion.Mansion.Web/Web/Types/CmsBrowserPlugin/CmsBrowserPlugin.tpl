<!-- View: Browse -->
<tpl:section name="Browse" field="View">
	<div class="page-slider">
		<ul class="breadcrumb">
			<li>
				<a href="browse-slice.htm" data-href="browse-slice.htm">Home</a>
				<span class="divider">/</span>
			</li>
			<li>
				<a href="browse-slice.htm" data-href="browse-slice.htm">Folder</a>
				<span class="divider">/</span>
			</li>
			<li class="active">Data</li>
		</ul>

		<ul class="nav nav-tabs">
			<li class="active">
				<a href="#browse-tab" data-toggle="tab">
					<i class="icon-list"></i> Browse
				</a>
			</li>
			<li>
				<a href="#properties-tab" data-toggle="tab">
					<i class="icon-edit"></i> Properties
				</a>
			</li>
		</ul>
		<div class="tab-content">
			<div class="tab-pane active" id="browse-tab">
				<!-- tree slider -->
				<table class="table table-striped table-bordered browser">
					<tr>
						<th>Name</th>
						<th>Type</th>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 1
							</a>
						</td>
						<td>Folder</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 2
							</a>
						</td>
						<td>Folder</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-folder-close"></i>
								Content folder 3
							</a>
						</td>
						<td>Folder</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 1
							</a>
						</td>
						<td>Article</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 2
							</a>
						</td>
						<td>Article</td>
					</tr>
					<tr>
						<td>
							<a href="browse-slice.htm" data-href="browse-slice.htm">
								<i class="icon-file"></i>
								File 3
							</a>
						</td>
						<td>Article</td>
					</tr>
				</table>
				<!-- /tree slider -->
			</div>

			<div class="tab-pane" id="properties-tab">
				<form class="form-horizontal">
					<fieldset>
						<div class="control-group">
							<label class="control-label" for="input01">Text input</label>
							<div class="controls">
								<input type="text" class="input-xlarge" id="input01">
									<p class="help-block">In addition to freeform text, any HTML5 text-based input appears like so.</p>
								</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="optionsCheckbox">Checkbox</label>
							<div class="controls">
								<label class="checkbox">
									<input type="checkbox" id="optionsCheckbox" value="option1">
												Option one is this and that—be sure to include why it's great
											</label>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="select01">Select list</label>
							<div class="controls">
								<select id="select01">
									<option>something</option>
									<option>2</option>
									<option>3</option>
									<option>4</option>
									<option>5</option>
								</select>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="multiSelect">Multicon-select</label>
							<div class="controls">
								<select multiple="multiple" id="multiSelect">
									<option>1</option>
									<option>2</option>
									<option>3</option>
									<option>4</option>
									<option>5</option>
								</select>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="fileInput">File input</label>
							<div class="controls">
								<input class="input-file" id="fileInput" type="file">
										</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="textarea">Textarea</label>
							<div class="controls">
								<textarea class="input-xlarge" id="textarea" rows="3"></textarea>
							</div>
						</div>
					</fieldset>
				</form>
			</div>
		</div>
		<div class="btn-toolbar">
			<div class="btn-group">
				<button type="submit" class="btn btn-success" rel="tooltip" title="Save any changes made to this node">
					<i class="icon-save"></i> Save
				</button>
				<button class="btn btn-info"rel="tooltip" title="Reverts any changes made to this node">
					<i class="icon-undo"></i> Cancel
				</button>
			</div>
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
	</div>
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