<!-- shared control sections -->
<tpl:section name="ControlId">{ControlProperties.id}</tpl:section>

<tpl:section name="ControlContainer" field="Control">
	{Control}
</tpl:section>


<!-- form control sections -->
<tpl:section name="FormControlName">{FormProperties.prefix}</tpl:section>
	
<tpl:section name="FormControl" field="Control">
	<form method="post" action="{NotEmpty( FormProperties.submitUrl, Request.url )}" id="{@ControlId}" name="{@FormControlName}" class="form {NotEmpty( ControlProperties.cssClass, 'form-horizontal' )}" accept-charset="utf-8" enctype="multipart/form-data" target="_self">
		{Control}
		{Hidden}
		<input type="hidden" name="{FormProperties.prefix}current-step" value="{FormProperties.currentStepId}">
		<input type="hidden" name="{FormProperties.prefix}state" value="{$FieldProperties}">
		<input type="hidden" name="{FormProperties.actionPrefix}" class="action" value="">
	</form>
</tpl:section>



<!-- step control sections -->
<tpl:section name="StepControl" field="Control">
	<fieldset id="{@ControlId}" class="step {ControlProperties.cssClass}">
		{@StepControlLegend}
		<div class="{ControlProperties.bodyCssClasses}">
			{Control}
		</div>
		{CommandBar}
	</fieldset>
</tpl:section>

	<tpl:section name="StepControlLegend" requires="{Not( IsEmpty( StepProperties.label ) )}">
		<legend>
			<h3 class="step-title {ControlProperties.headerCssClasses}">{StepProperties.label}</h3>
		</legend>
	</tpl:section>



<!-- render script control -->
<tpl:section name="RenderScriptControl" field="Control">
	<div class="{ControlProperties.cssClass}">
		{Content}
	</div>
</tpl:section>



<!-- field control container sections -->
<tpl:section name="FieldName">{FormProperties.fieldPrefix}{ControlProperties.name}</tpl:section>

<tpl:section name="FieldContainer" field="Control">
	<div class="control-group {If( IsInValidationError( $FormControl, $FieldControl ), 'error' )}">
		{@FieldLabel}
		<div class="controls">
			{Field}
			{@FieldExplanation}
		</div>
	</div>
</tpl:section>

<tpl:section name="FieldLabel" requires="{Not( IsEmpty( ControlProperties.label ) )}">
	<label class="control-label" for="{@ControlId}">{ControlProperties.label}{@FieldRequired}:</label>
</tpl:section>

<tpl:section name="FieldRequired" requires="{IsTrue( ControlProperties.isRequired )}">
	<em>*</em>
</tpl:section>

<tpl:section name="FieldExplanation" requires="{Not( IsEmpty( ControlProperties.explanation ) )}">
	<div class="help-block">{ControlProperties.explanation}</div>
</tpl:section>

<tpl:section name="FieldReadonlyAttribute">{If( IsTrue( ControlProperties.readonly ), 'disabled' )}</tpl:section>


<!-- Field controls sections -->

<tpl:section name="HiddenFieldControl" field="Hidden">
	<input type="hidden" id="{@ControlId}" name="{@FieldName}" class="field hidden" value="{ControlProperties.Value}">
</tpl:section>

<tpl:section name="ReadonlyFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge readonly" value="{ControlProperties.Value}" disabled>
</tpl:section>

<tpl:section name="TextboxFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="PasswordFieldControl" field="Field">
	<input type="password" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text password {ControlProperties.cssClass}" value="{ControlProperties.value}">
</tpl:section>

<tpl:section name="TextareaFieldControl" field="Field">
	<textarea id="{@ControlId}" name="{@FieldName}" class="field input-xlarge textarea {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>{ControlProperties.Value}</textarea>
</tpl:section>

<tpl:section name="EmailFieldControl" field="Field">
	<input type="email" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text email {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="UrlFieldControl" field="Field">
	<input type="url" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text url {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="RichTextAreaFieldControl" field="Field">
	<textarea id="{@ControlId}" name="{@FieldName}" class="field input-xlarge rich-text {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>{ControlProperties.Value}</textarea>
</tpl:section>


<tpl:section name="TagTextboxFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text tags {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute} data-autocomplete-url="{RouteUrlWithArea( 'Controls', 'Async', 'AutoComplete', TagIndexNode.id )}">
</tpl:section>

<tpl:section name="CheckboxFieldControl" field="Field">
	<input type="checkbox" id="{@ControlId}" name="{@FieldName}" class="input-small checkbox {ControlProperties.cssClass}" {If( IsTrue( ControlProperties.value ), 'checked' )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="SelectboxFieldControl" field="Field">
	<select id="{@ControlId}" name="{@FieldName}" class="field input-xlarge selectbox {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>
		{@SelectboxFieldControlDefaultOption}
		{SelectboxFieldControlOption}
	</select>
</tpl:section>

	<tpl:section name="SelectboxFieldControlDefaultOption" requires="{Not( IsTrue( ControlProperties.isRequired ) )}">
		<option value="" {If( IsEmpty( GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}></option>
	</tpl:section>

	<tpl:section name="SelectboxFieldControlOption">
		<option value="{OptionProperties.value}" {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}>{OptionProperties.label}</option>
	</tpl:section>

<tpl:section name="MultiSelectFieldControl" field="Field">
	<select id="{@ControlId}" name="{@FieldName}" class="field input-xlarge multiselect {ControlProperties.cssClass}" {@FieldReadonlyAttribute} multiple="multiple">
		{MultiSelectFieldControlOption}
	</select>
</tpl:section>

	<tpl:section name="MultiSelectFieldControlOption">
		<option value="{OptionProperties.value}" {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}>{OptionProperties.label}</option>
	</tpl:section>

<tpl:section name="CheckboxListFieldControl" field="Field">
	<div id="{@ControlId}" class="field input-xlarge checkbox-list {ControlProperties.cssClass}">
		{CheckboxListFieldControlOption}
	</div>
</tpl:section>

	<tpl:section name="CheckboxListFieldControlOption">
		<label for="{@ControlId}-{Loop.current}">
			<input type="checkbox" id="{@ControlId}-{Loop.current}" name="{@FieldName}" value="{OptionProperties.value}" {@FieldReadonlyAttribute} {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'checked')}>
			{OptionProperties.label}
		</label>
	</tpl:section>

<tpl:section name="DateFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge date {ControlProperties.cssClass}" value="{FormatDate( ControlProperties.Value, 'dd MMMM yyyy' )}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="DateTimeFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge datetime {ControlProperties.cssClass}" value="{FormatDate( ControlProperties.Value, 'dd MMMM yyyy HH:mm' )}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="UploadFieldControl" field="Field">
	<div id="{@ControlId}" class="field input-small upload {ControlProperties.cssClass}">
		{@UploadFieldControlPreview}
		<input type="file" id="{@ControlId}-upload" name="{@FieldName}-upload" {@FieldReadonlyAttribute}>
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
	</div>
</tpl:section>

	<tpl:section name="UploadFieldControlPreview" requires="{Not( IsEmpty( ControlProperties.value ) )}">
		<a href="{StaticContentUrl( ControlProperties.value )}" id="{@ControlId}-preview">{ControlProperties.value}</a>
		<a href="#" id="{@ControlId}-clear" name="{@FieldName}-clear">Remove</a>
	</tpl:section>
	


<tpl:section name="ImageUploadFieldControl" field="Field">
	<div id="{@ControlId}" class="field input-small image-upload {ControlProperties.cssClass}">
		{@ImageUploadFieldControlPreview}
		<input type="file" id="{@ControlId}-upload" name="{@FieldName}-upload" {@FieldReadonlyAttribute} value="{ControlProperties.value}">
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
	</div>
</tpl:section>

	<tpl:section name="ImageUploadFieldControlPreview" requires="{Not( IsEmpty( ControlProperties.value ) )}">
		<img id="{@ControlId}-preview" src="{StaticContentUrl( ControlProperties.value )}">
		<a href="#" id="{@ControlId}-clear" name="{@FieldName}-clear">Remove</a>
	</tpl:section>

<tpl:section name="NumberFieldControl" field="Field">
	<input type="number" id="{@ControlId}" name="{@FieldName}" class="field input-small number {ControlProperties.cssClass}" value="{ControlProperties.value}" {If( Not( IsEmpty( ControlProperties.min ) ), Concat( ' min="', ControlProperties.min, '"'  ) )} {If( Not( IsEmpty( ControlProperties.max ) ), Concat( ' max="', ControlProperties.max, '"'  ) )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="DecimalFieldControl" field="Field">
	<input type="number" id="{@ControlId}" name="{@FieldName}" class="field input-small number decimal {ControlProperties.cssClass}" value="{ControlProperties.value}" {If( Not( IsEmpty( ControlProperties.min ) ), Concat( ' min="', ControlProperties.min, '"'  ) )} {If( Not( IsEmpty( ControlProperties.max ) ), Concat( ' max="', ControlProperties.max, '"'  ) )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="RangeFieldControl" field="Field">
	<input type="range" id="{@ControlId}" name="{@FieldName}" class="field input-small range {ControlProperties.cssClass}" value="{ControlProperties.value}" {If( Not( IsEmpty( ControlProperties.min ) ), Concat( ' min="', ControlProperties.min, '"'  ) )} {If( Not( IsEmpty( ControlProperties.max ) ), Concat( ' max="', ControlProperties.max, '"'  ) )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="FieldsetControl" field="Control">
	<fieldset id="{@ControlId}" class="group {ControlProperties.cssClass}">
		<legend>
			<h4 class="{ControlProperties.headerCssClasses}">{ControlProperties.label}</h4>
		</legend>
		<div class="{ControlProperties.bodyCssClasses}">
			{Control}
		</div>
	</fieldset>
</tpl:section>

<tpl:section name="CollapsibleFieldsetControl" field="Control">
	<fieldset id="{@ControlId}" class="group collapsible {ControlProperties.cssClass}">
		<legend>
			<h4 class="{ControlProperties.headerCssClasses}" data-toggle="collapse" data-target="#{@ControlId}-toggle"><i class="icon-resize-vertical"></i>{ControlProperties.label}</h4>
		</legend>
		<div id="{@ControlId}-toggle" class="collapse {ControlProperties.bodyCssClasses}">
			{Control}
		</div>
	</fieldset>
</tpl:section>

<tpl:section name="FieldContainerControl" field="Control">
	<div class="control-group">
		{@FieldLabel}
		<div class="controls">
			{Control}
			{@FieldExplanation}
		</div>
	</div>
</tpl:section>



/* ==|== Nodeselector Section ============================================
Author: Premotion Software Solutions
========================================================================== */

<tpl:section name="SingleNodeSelectorFieldControl" field="Field">
	<div id="{@ControlId}" class="field input-xxlarge" data-behavior="single-node-selector" data-service-endpoint="{RouteUrlWithArea( 'Controls', 'Async', 'NodeSelector', '0' )}">
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
		<input type="hidden" id="{@ControlId}-settings" name="_{@FieldName}-settings" value="{$NodeSelectorProperties}">
		<div class="selected-item">
		</div>
		<div class="clearfix">
			<input type="search" class="pull-right input-medium search-query" data-behavior="filter-node-select" placeholder="Filter">
		</div>
		<div class="well well-small" tabindex="0">
			<ul class="breadcrumb">
			</ul>
			<ul class="nav nav-list">
			</ul>
		</div>
	</div>
</tpl:section>

<tpl:section name="MultiNodeSelectorFieldControl" field="Field">
	<div id="{@ControlId}" class="field input-xxlarge" data-behavior="multi-node-selector" data-service-endpoint="{RouteUrlWithArea( 'Controls', 'Async', 'NodeSelector', '0' )}">
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
		<input type="hidden" id="{@ControlId}-settings" name="_{@FieldName}-settings" value="{$NodeSelectorProperties}">
		<div class="row-fluid">
			<div class="span6">
				<div class="clearfix">
					<input type="search" class="pull-right input-medium search-query" data-behavior="filter-node-select" placeholder="Filter">
				</div>
				<div class="well well-small" tabindex="0">
					<ul class="breadcrumb">
					</ul>
					<ul class="nav nav-list">
					</ul>
				</div>
			</div>
			<div class="span6">
				<div class="well well-small">
					<ul class="nav nav-pills nav-stacked">
					</ul>
				</div>
			</div>
		</div>
	</div>
</tpl:section>



<!-- buttons -->
<tpl:section name="ButtonName">{FormProperties.actionPrefix}{ControlProperties.action}</tpl:section>

<tpl:section name="ButtonBarControl" field="CommandBar">
	<div class="clearfix form-actions {ControlProperties.cssClass}">
		{Control}
	</div>
</tpl:section>

<tpl:section name="ButtonGroupControl" field="Control">
	<div class="clearfix btn-group {ControlProperties.cssClass}">
		{Control}
	</div>
</tpl:section>

<tpl:section name="ButtonControl" field="Control">
	<button href="#" id="{@ControlId}" class="btn {If( IsTrue( ControlProperties.isDefault ), 'default' )} {ControlProperties.cssClass}" data-action="{ControlProperties.action}" {@ButtonControlTooltip}>{@ButtonControlIcon}{ControlProperties.label}</button>&nbsp;
</tpl:section>	

<tpl:section name="LinkButtonControl" field="Control">
	<a href="{ControlProperties.action}" id="{@ControlId}" class="btn {If( IsTrue( ControlProperties.isDefault ), 'default' )} {ControlProperties.cssClass}" {@ButtonControlTooltip}>{@ButtonControlIcon}{ControlProperties.label}</a>&nbsp;
</tpl:section>

	<tpl:section name="ButtonControlIcon" requires="{Not( IsEmpty( ControlProperties.iconClass ) )}"><i class="{ControlProperties.iconClass}"></i>&nbsp;</tpl:section>
	<tpl:section name="ButtonControlTooltip" requires="{Not( IsEmpty( ControlProperties.tooltip ) )}">rel="tooltip" data-original-title="{ControlProperties.tooltip}"</tpl:section>



<!-- validation -->
<tpl:section name="ValidationSummaryControl" field="Control">
	<div class="controls alert alert-block alert-error">
		<h4 class="alert-heading">Error</h4>
		<ul>
			{ValidationResult}
		</ul>
	</div>
</tpl:section>

<tpl:section name="ValidationResult">
	<li>
		<strong>{ValidationResult.controlLabel}:</strong> {ValidationResult.message}
	</li>
</tpl:section>



<!-- messages -->
<tpl:section name="InfoMessageControl" field="Control">
	<div class="alert alert-info">{ControlProperties.message}</div>
</tpl:section>

<tpl:section name="InstructionMessageControl" field="Control">
	<div class="alert alert-info">{ControlProperties.message}</div>
</tpl:section>

<tpl:section name="WarningMessageControl" field="Control">
	<div class="alert alert-warning">{ControlProperties.message}</div>
</tpl:section>

<tpl:section name="SuccessMessageControl" field="Control">
	<div class="alert alert-success">{ControlProperties.message}</div>
</tpl:section>

<tpl:section name="ErrorMessageControl" field="Control">
	<div class="alert alert-error">{ControlProperties.message}</div>
</tpl:section>



<!-- alerts-->
<tpl:section name="AlertDismissCaret"><a class="close" data-dismiss="alert" href="#"><i class="icon-remove-sign"></i></a></tpl:section>

<tpl:section name="InfoAlertControl" field="Control">
	<div class="alert alert-info fade in">
		{@AlertDismissCaret}
		{ControlProperties.message}
	</div>
</tpl:section>

<tpl:section name="InstructionAlertControl" field="Control">
	<div class="alert alert-info fade in">
		{@AlertDismissCaret}
		{ControlProperties.message}
	</div>
</tpl:section>

<tpl:section name="WarningAlertControl" field="Control">
	<div class="alert alert-warning fade in">
		{@AlertDismissCaret}
		{ControlProperties.message}
	</div>
</tpl:section>

<tpl:section name="SuccessAlertControl" field="Control">
	<div class="alert alert-success fade in">
		{@AlertDismissCaret}
		{ControlProperties.message}
	</div>
</tpl:section>

<tpl:section name="ErrorAlertControl" field="Control">
	<div class="alert alert-error fade in">
		{@AlertDismissCaret}
		{ControlProperties.message}
	</div>
</tpl:section>



<!-- paging controls -->
<tpl:section name="PagingControl" field="Control">
	<div class="pagination {PagingProperties.cssClasses}">
		<ul>
			<li class="{If( IsEqual( PagingProperties.currentPage, '1' ), 'disabled' )}">
				<a href="{ChangeQueryString( Request.url, GetControlPropertyName( PagingProperties.id, 'page-number' ), Max( Subtract( PagingProperties.currentPage, '1' ), '1' ) )}"><i class="icon-caret-left"></i></a>
			</li>
			{PagingControlPageOption}
			<li class="{If( IsEqual( PagingProperties.currentPage, PagingProperties.pageCount ), 'disabled' )}">
				<a href="{ChangeQueryString( Request.url, GetControlPropertyName( PagingProperties.id, 'page-number' ), Min( Add( PagingProperties.currentPage, '1' ), PagingProperties.pageCount ) )}"><i class="icon-caret-right"></i></a>
			</li>
		</ul>
	</div>
</tpl:section>

	<tpl:section name="PagingControlPageOption">
		<li class="{If( Equal( PageOption.number, PagingProperties.currentPage ), 'active' )}">
			<a href="{ChangeQueryString( Request.url, GetControlPropertyName( PagingProperties.id, 'page-number' ), PageOption.number )}">{PageOption.number}</a>
		</li>
	</tpl:section>



<!-- dialog control sections -->
<tpl:section name="InvokeDialogParentTrigger" field="Header">
	<script>parent.jQuery(parent.document).trigger("{TriggerProperties.action}");</script>
</tpl:section>
<tpl:section name="InvokeDialogParentTriggerWithParameters" field="Header">
	<script>parent.jQuery(parent.document).trigger("{TriggerProperties.action}", {#DataspaceToJSonArray( $TriggerArguments )});</script>
</tpl:section>



<!-- group sections -->
<tpl:section name="GroupControl" field="Control">
	<fieldset id="{@ControlId}" class="clearfix group {ControlProperties.cssClass}">
		<legend>{ControlProperties.label}</legend>
		{Control}
	</fieldset>
</tpl:section>



<!-- Grid control sections -->
<tpl:section name="FormGridControl" field="Control">
	<form method="get" action="{ChangeQueryString( Request.url, GetControlPropertyName( 'page-number' ), '1' )}">
		<div id="{@ControlId}">
			<table class="table table-striped table-bordered table-condensed">
				{Content}
			</table>
			<div class="pull-right">
				{#RenderPagingControl( $Dataset, GridProperties.id )}
			</div>
		</div>
		<input type="hidden" name="{GetControlPropertyName( 'sort' )}" value="{GetControlPropertyValue( 'sort' )}">
	</form>
</tpl:section>

	<tpl:section name="GridControlHeader" field="Content">
		<thead>
			{Row}
		</thead>
	</tpl:section>

	<tpl:section name="GridControlFilterRow" field="Row">
		<tr class="filters">{Cell}</tr>
	</tpl:section>

		<tpl:section name="GridControlNoFilter" field="Cell"><td>&nbsp;</td></tpl:section>

		<tpl:section name="GridControlTextboxColumnFilter" field="Cell"><td><input type="text" name="{GetControlPropertyName( FilterProperties.on )}" value="{GetControlPropertyValue( FilterProperties.on )}"></td></tpl:section>
		
		<tpl:section name="GridControlSelectboxColumnFilter" field="Cell"><td><select name="{GetControlPropertyName( FilterProperties.on )}" value="{GetControlPropertyValue( FilterProperties.on )}" onchange="this.form.submit();">{GridControlSelectboxColumnFilterOption}</select></td></tpl:section>
			<tpl:section name="GridControlSelectboxColumnFilterOption"><option value="{OptionProperties.value}" {If( InList( OptionProperties.value, GetControlPropertyValue( FilterProperties.on ) ), 'selected')}>{OptionProperties.label}</option></tpl:section>

	<tpl:section name="GridControlHeaderRow" field="Row">
		<tr class="headings">{Cell}</tr>
	</tpl:section>
	
		<tpl:section name="GridControlPropertyColumnHeader" field="Cell"><th>{ColumnProperties.heading}</th></tpl:section>

		<tpl:section name="GridControlColumnSortHeader" field="Cell"><th><a href="{ChangeQueryString( Request.url, GetControlPropertyName( 'sort' ), ColumnSortProperties.sortParameter )}">{ColumnProperties.heading} <i class="pull-right icon-{If( IsTrue( ColumnSortProperties.active ), If( IsTrue( ColumnSortProperties.direction ), 'sort-up', 'sort-down' ), 'sort' )}"></i></a></th></tpl:section>

	<tpl:section name="GridControlBody" field="Content">
		<tbody>
			{Row}
		</tbody>
	</tpl:section>

	<tpl:section name="GridControlRowNoResults" field="Row"><tr><td colspan="{GridProperties.columnCount}">{NotEmpty( GridProperties.notFoundMessage, 'No results found' )}</td></tr></tpl:section>

	<tpl:section name="GridControlBodyRow" field="Row">
		<tr>{Cell}</tr>
	</tpl:section>

		<tpl:section name="GridControlCell" field="Cell"><td>{CellContent}</td></tpl:section>

			<tpl:section name="GridControlPropertyColumnContent" field="CellContent">{CellProperties.value}</tpl:section>

			<tpl:section name="GridControlExpressionColumnContent" field="CellContent">{#CellProperties.value}</tpl:section>