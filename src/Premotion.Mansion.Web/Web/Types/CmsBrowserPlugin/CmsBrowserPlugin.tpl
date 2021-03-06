﻿<!-- View: Browse -->
<tpl:section name="Browse" field="View">
	<div class="page-slider">
		{AlertMessage}
		{CrumbPath}
		{Content}
	</div>
</tpl:section>



<tpl:section name="AlertMessage">{Control}</tpl:section>



<tpl:section name="CrumbPath">
	<ul class="breadcrumb">
		{Crumb}
	</ul>
</tpl:section>

	<tpl:section name="ParentCrumb" field="Crumb">
		<li>
			<a href="{CmsNodeUrl( ParentNode.id, 'CmsBrowserPlugin', 'Browse' )}" title="Navigate to {ParentNode.name}">{#GetTypeDefinitionIcon( ParentNode.type )} {ParentNode.name}</a>
			<span class="divider"></span>
		</li>
	</tpl:section>

	<tpl:section name="ActiveCrumb" field="Crumb">
		<li class="active">{#GetTypeDefinitionIcon( CurrentNode.type )} {CurrentNode.name}</li>
	</tpl:section>



<tpl:section name="NodeBrowser" field="Content">
	{NodeBrowserContent}
	<div class="btn-toolbar">
		<div class="btn-group">
			{@AddChildButton}
			{@ChangeOrderButton}
			{@MoveNodeButton}
			{@CopyNodeButton}
			{@DeleteNodeButton}
		</div>
	</div>
</tpl:section>

	<tpl:section name="AddChildButton" requires="{HasChildTypes( $CurrentNode )}">
		<a href="{CmsRouteUrl( 'Dialog', 'AddChildToNode', CurrentNode.id )}" class="btn btn-primary" rel="tooltip" data-toggle="modal" data-target="#modal-popup" data-container="body" title="Add a new child to this node">
			<i class="icon-plus-sign"></i> Add child
		</a>
	</tpl:section>

	<tpl:section name="ChangeOrderButton" requires="{IsTrue( GetTypeDefinitionProperty( CurrentNode.type, 'reorderable', 'true' ) )}">
		<a href="{CmsRouteUrl( 'Dialog', 'ChangeOrder', CurrentNode.id )}" class="btn btn-primary" rel="tooltip" data-toggle="modal" data-target="#modal-popup" data-container="body" title="Reorder the position of this node">
			<i class="icon-reorder"></i> Change order
		</a>
	</tpl:section>

	<tpl:section name="MoveNodeButton" requires="{IsTrue( GetTypeDefinitionProperty( CurrentNode.type, 'movable', 'true' ) )}">
		<a href="{CmsRouteUrl( 'Dialog', 'MoveNode', CurrentNode.id )}" class="btn btn-primary" rel="tooltip" data-toggle="modal" data-target="#modal-popup" data-container="body" title="Move this node to another folder">
			<i class="icon-move"></i> Move
		</a>
	</tpl:section>

	<tpl:section name="CopyNodeButton" requires="{IsTrue( GetTypeDefinitionProperty( CurrentNode.type, 'copyable', 'true' ) )}">
		<a href="{CmsRouteUrl( 'Dialog', 'CopyNode', CurrentNode.id )}" class="btn btn-primary" rel="tooltip" data-toggle="modal" data-target="#modal-popup" data-container="body" title="Create a copy of this node">
			<i class="icon-copy"></i> Copy
		</a>
	</tpl:section>

	<tpl:section name="DeleteNodeButton" requires="{IsTrue( GetTypeDefinitionProperty( CurrentNode.type, 'deletable', 'true' ) )}">
		<a href="{CmsRouteUrl( 'Dialog', 'DeleteNode', CurrentNode.id )}" class="btn btn-danger" rel="tooltip" data-toggle="modal" data-target="#modal-popup" data-container="body" title="Delete this node">
			<i class="icon-remove-sign"></i> Delete
		</a>
	</tpl:section>

	<tpl:section name="NodeBrowserNodeLink">
		<a href="{CmsNodeUrl( Row.id, 'CmsBrowserPlugin', 'Browse' )}" title="Navigate to {Row.name}">{#GetTypeDefinitionIcon( Row.type )} {Row.name}</a>
	</tpl:section>



<tpl:section name="NodeProperties" field="Content">
	{Control}
</tpl:section>



<!-- View: Find -->
<tpl:section name="Find" field="View">
	<div class="find page-slider">
		{NumberOfResults}
		<div class="row">
			<div class="col-md-8">
				{Results}
			</div>
			<div class="search-form col-md-4">
				{FindSearchForm}
			</div>
		</div>
	</div>
</tpl:section>

<tpl:section name="NumberOfResults">
	<h2>{ResultSet.totalCount} Result{If( IsEqual( ResultSet.totalCount, '1' ), '', 's' )}</h2>
</tpl:section>

<tpl:section name="FindSearchForm">
	<form class="well form-vertical" action="{WithoutQueryString( Request.url )}" method="get">
		<fieldset>
			<div class="keywords control-group">
				<label class="control-label" for="q">Keywords:</label>
				<div class="controls">
					<input type="search" class="form-control" id="q" name="q" value="{Get.q}">
				</div>
			</div>
			<div class="facets control-group">
				<ul class="nav nav-list">
					{Facet}
				</ul>
			</div>
			<div class="form-actions">
				<button type="submit" class="btn btn-primary">
					<i class="icon-search"></i> Search
				</button>
			</div>
		</fieldset>
		<input type="hidden" name="{GetControlPropertyName( 'node-browser', 'page-number' )}" value="1" />
	</form>
</tpl:section>

<tpl:section name="SearchFacets">
	<div class="well">
		<ul class="nav">
			{Facet}
		</ul>
	</div>
</tpl:section>

<tpl:section name="Facet">
	<li class="nav-header">{FacetRow.friendlyName}</li>
	{FacetValue}
	{@FacetSeparator}
</tpl:section>

<tpl:section name="FacetSeparator" requires="{Not( IsLast() )}">
	<li class="divider"></li>
</tpl:section>

<tpl:section name="FacetValue">
	<li>
		<a href="{ChangeQueryString( Request.url, FacetRow.propertyName, FacetValueRow.value )}" title="Filter op {FacetValueRow.displayValue}">{FacetValueRow.displayValue} ({FacetValueRow.count})</a>
	</li>
</tpl:section>

<tpl:section name="ActiveFacetValue" field="FacetValue">
	<li>
		<a href="{ChangeQueryString( Request.url, FacetRow.propertyName, '' )}" title="Stop filter op {FacetValueRow.displayValue}">
			<input type="hidden" name="{FacetRow.propertyName}" value="{FacetValueRow.value}" />
			<i class="icon-remove-sign"></i> {FacetValueRow.displayValue} ({FacetValueRow.count})
		</a>
	</li>
</tpl:section>