<!-- header section -->
<tpl:section name="ProfileButton">
	<div class="btn-group pull-right">
		<a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
			<i class="icon-user"></i> Welcome {User.name}
			<span class="caret"></span>
		</a>
		<ul class="dropdown-menu">
			<li>
				<a href="{CmsRouteUrl( 'Authentication', 'Logoff' )}">Sign out</a>
			</li>
		</ul>
	</div>
</tpl:section>



<!-- content sections -->
<tpl:section name="Content">
	<div class="row-fluid">
		<div class="span3">
         {Tree}
		</div>
		
		<div class="span9">
			{Browser}
		</div>
	</div>
</tpl:section>



<!-- node tree sections -->
<tpl:section name="NodeNavigationTree" field="Tree">
	<div class="well sidebar-nav">
		<ul class="nav nav-list">
			<li class="nav-header">Browse</li>
			{NodeNavigationTreeLeaf}
		</ul>
	</div>
	<!--/.well -->
</tpl:section>

	<tpl:section name="NodeNavigationTreeLeaf">
		<li>
			<a href="{CmsRouteUrl( 'Cms', 'View', LeafNode.id )}">{GetTypeDefinitionIcon( LeafNode.type )} {LeafNode.name}</a>
			{NodeNavigationTreeBranch}
		</li>
	</tpl:section>

	<tpl:section name="NodeNavigationTreeBranch">
		<ul class="unstyled">
			{NodeNavigationTreeLeaf}
		</ul>
	</tpl:section>



<!-- browser sections -->
<tpl:section name="NodeViewer" field="Browser">
	{NodeCrumbPath}
	<ul class="nav nav-tabs">
		<li class="active">
			<a href="#edit-node" data-toggle="tab"><i class="icon-edit"></i> Edit</a>
		</li>
		<li>
			<a href="#preview-node" data-toggle="tab"><i class=" icon-zoom-in"></i> Preview</a>
		</li>
	</ul>
	<div class="tab-content">
		<div id="edit-node" class="tab-pane fade active in">
			{Control}
		</div>
		<div id="preview-node" class="tab-pane fade">
			
		</div>
	</div>
</tpl:section>

	<tpl:section name="NodeCrumbPath">
		<ul class="breadcrumb">
			{NodeCrumb}
		</ul>
	</tpl:section>

	<tpl:section name="NodeParentCrumb" field="NodeCrumb">
		<li>
			<a	href="{CmsRouteUrl( 'Cms', 'View', ParentNode.id )}">{GetTypeDefinitionIcon( ParentNode.type )} {ParentNode.name}</a>
			<span class="divider">/</span>
		</li>
	</tpl:section>

	<tpl:section name="NodeActiveCrumb" field="NodeCrumb">
		<li class="active">{GetTypeDefinitionIcon( CurrentNode.type )} {CurrentNode.name}</li>
	</tpl:section>