<!-- shared control sections -->
<tpl:section name="ControlId">{ControlProperties.id}</tpl:section>

<tpl:section name="ControlContainer" field="Control">
	{Control}
</tpl:section>


<!-- form control sections -->
<tpl:section name="FormControlName">{FormProperties.prefix}</tpl:section>
	
<tpl:section name="FormControl" field="Control">
	<form method="post" action="{Request.url}" id="{@ControlId}" name="{@FormControlName}" class="form form-horizontal {ControlProperties.cssClass}" accept-charset="utf-8" enctype="multipart/form-data">
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
		<legend>
			<h3 class="step-title {ControlProperties.headerCssClasses}">{StepProperties.label}</h3>
		</legend>
		<div class="{ControlProperties.bodyCssClasses}">
			{Control}
		</div>
		{CommandBar}
	</fieldset>
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
	<div class="control-group">
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

<tpl:section name="HiddenControl" field="Hidden">
	<input type="hidden" id="{@ControlId}" name="{@FieldName}" class="field hidden" value="{ControlProperties.Value}">
</tpl:section>

<tpl:section name="ReadonlyControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge readonly" value="{FieldProperties.Value}" disabled>
</tpl:section>

<tpl:section name="TextboxControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="PasswordControl" field="Field">
	<input type="password" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text password {ControlProperties.cssClass}" value="{ControlProperties.value}">
</tpl:section>

<tpl:section name="TextareaControl" field="Field">
	<textarea id="{@ControlId}" name="{@FieldName}" class="field input-xlarge textarea {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>{ControlProperties.Value}</textarea>
</tpl:section>

<tpl:section name="EmailControl" field="Field">
	<input type="email" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text email {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="UrlControl" field="Field">
	<input type="url" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text url {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="RichTextAreaControl" field="Field">
	<textarea id="{@ControlId}" name="{@FieldName}" class="field input-xlarge rich-text {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>{ControlProperties.Value}</textarea>
</tpl:section>


<tpl:section name="TagTextboxControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge text tags {ControlProperties.cssClass}" value="{ControlProperties.value}" {@FieldReadonlyAttribute} data-autocomplete-url="{RouteUrlWithArea( 'Controls', 'Async', 'AutoComplete', TagIndexNode.id )}">
</tpl:section>

<tpl:section name="CheckboxControl" field="Field">
	<input type="checkbox" id="{@ControlId}" name="{@FieldName}" class="input-small checkbox {ControlProperties.cssClass}" {If( IsTrue( ControlProperties.value ), 'checked' )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="SelectboxControl" field="Field">
	<select id="{@ControlId}" name="{@FieldName}" class="field input-xlarge selectbox {ControlProperties.cssClass}" {@FieldReadonlyAttribute}>
		{@SelectboxControlDefaultOption}
		{SelectboxControlOption}
	</select>
</tpl:section>

	<tpl:section name="SelectboxControlDefaultOption" requires="{Not( IsTrue( ControlProperties.isRequired ) )}">
		<option value="" {If( IsEmpty( GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}></option>
	</tpl:section>

	<tpl:section name="SelectboxControlOption">
		<option value="{HtmlEncode( OptionProperties.value )}" {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}>{OptionProperties.label}</option>
	</tpl:section>

<tpl:section name="MultiSelectControl" field="Field">
	<select id="{@ControlId}" name="{@FieldName}" class="field input-xlarge multiselect {ControlProperties.cssClass}" {@FieldReadonlyAttribute} multiple="multiple">
		{MultiSelectControlOption}
	</select>
</tpl:section>

	<tpl:section name="MultiSelectControlOption">
		<option value="{HtmlEncode( OptionProperties.value )}" {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'selected')}>{OptionProperties.label}</option>
	</tpl:section>

<tpl:section name="CheckboxListControl" field="Field">
	<div id="{@ControlId}" class="field input-xlarge checkbox-list {ControlProperties.cssClass}">
		{CheckboxListControlOption}
	</div>
</tpl:section>

	<tpl:section name="CheckboxListControlOption">
		<label for="{@ControlId}-{Loop.current}">
			<input type="checkbox" id="{@ControlId}-{Loop.current}" name="{@FieldName}" value="{HtmlEncode( OptionProperties.value )}" {@FieldReadonlyAttribute} {If( InList( OptionProperties.value, GetStackValue( 'FieldProperties', ControlProperties.name, '' ) ), 'checked')}>
			{OptionProperties.label}
		</label>
	</tpl:section>

<tpl:section name="SingleNodeSelectorControl" field="Field">
	<div id="{@ControlId}" class="field input-xlarge node-selector single-node-selector">
		<div id="{@ControlId}-label" class="label">{ControlProperties.displayValue}</div>
		<a id="{@ControlId}-select" class="button dialog select-button" href="{DataspaceToQueryString( RouteUrlWithArea( 'Dialog', 'Dialog', 'NodeSelector', '1', 'single' ), $SelectorProperties )}">Select</a>
		<a id="{@ControlId}-clear" class="button clear-button" href="#">Clear</a>
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" class="field hidden" value="{ControlProperties.value}">
	</div>
</tpl:section>

<tpl:section name="MultiNodeSelectorControl" field="Field">
	<div id="{@ControlId}" class="field input-xlarge node-selector multi-node-selector">
		<div id="{@ControlId}-label" class="label">{ControlProperties.displayValue}</div>
		<a id="{@ControlId}-select" class="button dialog select-button" href="{DataspaceToQueryString( RouteUrlWithArea( 'Dialog', 'Dialog', 'NodeSelector', '1', 'multi' ), $SelectorProperties )}">Select</a>
		<a id="{@ControlId}-clear" class="button clear-button" href="#">Clear</a>
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" class="field hidden" value="{ControlProperties.value}">
	</div>
</tpl:section>

<tpl:section name="NodeTreeSelectControl" field="Field">
	<ul class="field tree-select node-tree">
		{Leaf}
	</ul>
</tpl:section>
	
	<tpl:section name="NodeTreeSelectControlLeaf" field="Leaf">
		<li>
			<label for="{@FieldName}-{LeafProperties.id}">
				{@NodeTreeSelectControlLeafButton}
				{LeafProperties.label}
			</label>
			{Children}
		</li>
	</tpl:section>

		<tpl:section name="NodeTreeSelectControlLeafButton" requires="{Not( LeafProperties.disabled )}"><input type="{If( ControlProperties.allowMultiple, 'checkbox', 'radio' )}" name="{@FieldName}" id="{@FieldName}-{LeafProperties.id}" value="{LeafProperties.value}" {If( InList( LeafProperties.value, ControlProperties.value ), 'checked')} {If( LeafProperties.disabled, 'disabled' )}></tpl:section>
	
	<tpl:section name="NodeTreeSelectControlWithChildren" field="Children">
		<ul>
			{Leaf}
		</ul>
	</tpl:section>

<tpl:section name="DateControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge date {ControlProperties.cssClass}" value="{FormatDate( ControlProperties.Value, 'dd MMMM yyyy' )}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="DateTimeFieldControl" field="Field">
	<input type="text" id="{@ControlId}" name="{@FieldName}" class="field input-xlarge datetime {ControlProperties.cssClass}" value="{FormatDate( ControlProperties.Value, 'dd MMMM yyyy HH:mm' )}" {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="UploadControl" field="Field">
	<div id="{@ControlId}" class="field input-small upload {ControlProperties.cssClass}">
		{@UploadControlPreview}
		<input type="file" id="{@ControlId}-upload" name="{@FieldName}-upload" {@FieldReadonlyAttribute}>
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
	</div>
</tpl:section>

	<tpl:section name="UploadControlPreview" requires="{Not( IsEmpty( ControlProperties.value ) )}">
		<a href="{StaticContentUrl( ControlProperties.value )}" id="{@ControlId}-preview">{ControlProperties.value}</a>
		<a href="#" id="{@ControlId}-clear" name="{@FieldName}-clear">Remove</a>
	</tpl:section>
	


<tpl:section name="ImageUploadControl" field="Field">
	<div id="{@ControlId}" class="field input-small image-upload {ControlProperties.cssClass}">
		{@ImageUploadControlPreview}
		<input type="file" id="{@ControlId}-upload" name="{@FieldName}-upload" {@FieldReadonlyAttribute} value="{ControlProperties.value}">
		<input type="hidden" id="{@ControlId}-value" name="{@FieldName}" value="{ControlProperties.value}">
	</div>
</tpl:section>

	<tpl:section name="ImageUploadControlPreview" requires="{Not( IsEmpty( ControlProperties.value ) )}">
		<img id="{@ControlId}-preview" src="{StaticContentUrl( ControlProperties.value )}">
		<a href="#" id="{@ControlId}-clear" name="{@FieldName}-clear">Remove</a>
	</tpl:section>

<tpl:section name="NumberControl" field="Field">
	<input type="number" id="{@ControlId}" name="{@FieldName}" class="field input-small number {ControlProperties.cssClass}" value="{ControlProperties.value}" {If( Not( IsEmpty( ControlProperties.min ) ), Concat( ' min="', ControlProperties.min, '"'  ) )} {If( Not( IsEmpty( ControlProperties.max ) ), Concat( ' max="', ControlProperties.max, '"'  ) )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="DecimalNumberControl" field="Field">
	<input type="number" id="{@ControlId}" name="{@FieldName}" class="field input-small number decimal {ControlProperties.cssClass}" value="{ControlProperties.value}" {If( Not( IsEmpty( ControlProperties.min ) ), Concat( ' min="', ControlProperties.min, '"'  ) )} {If( Not( IsEmpty( ControlProperties.max ) ), Concat( ' max="', ControlProperties.max, '"'  ) )} {@FieldReadonlyAttribute}>
</tpl:section>

<tpl:section name="RangeControl" field="Field">
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



<!-- buttons -->
<tpl:section name="ButtonName">{FormProperties.actionPrefix}{ControlProperties.action}</tpl:section>

<tpl:section name="ButtonBarControl" field="CommandBar">
	<div class="clearfix form-actions {ControlProperties.cssClass}">
		{Control}
	</div>
</tpl:section>

<tpl:section name="ButtonControl" field="Control">
	<a href="#" id="{@ControlId}" class="btn {If( IsTrue( ControlProperties.isDefault ), 'default' )} {ControlProperties.cssClass}" data-action="{ControlProperties.action}">{ControlProperties.label}</a>&nbsp;
</tpl:section>

<tpl:section name="LinkButtonControl" field="Control">
	<a href="{ControlProperties.action}" id="{@ControlId}" class="btn {If( IsTrue( ControlProperties.isDefault ), 'default' )} {ControlProperties.cssClass}">{ControlProperties.label}</a>&nbsp;
</tpl:section>



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
		<strong>{NotEmpty( ValidatedControlProperties.label, ValidatedControlProperties.name )}:</strong> {MessageProperties.message}
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



<!-- list control sections -->
<tpl:section name="FormListControl" field="Control">
	<li class="clearfix">
		<form id="{GetControlPropertyName( 'form' )}" action="{HtmlEncode( Request.url )}" method="get">
			<ul id="{@ControlId}" class="list">
				{Row}
			</ul>
			{ListControlPaging}
			<input type="submit" value="updateui" class="updateui visuallyhidden">
		</form>
	</li>
</tpl:section>

<tpl:section name="EmbeddedListControl" field="Control">
	<li class="clearfix">
		<ul id="{@ControlId}" class="list">
			{Row}
		</ul>
		{ListControlPaging}
	</li>
</tpl:section>

	<tpl:section name="ListControlRow" field="Row">
		<li class="row {RowClasses} clickable">{RowProperties.content}</li>
	</tpl:section>

	<tpl:section name="ListControlRowNoResults" field="Row"><li class="row">{NotEmpty( ListProperties.notFoundMessage, 'No results found' )}</li></tpl:section>

	<tpl:section name="ListControlPaging">{RenderEmbeddedPagingControl( $dataset, ListProperties.id )}</tpl:section>



<!-- grid control sections -->
<tpl:section name="FormGridControl" field="Control">
	<li class="clearfix">
		<form id="{GetControlPropertyName( 'form' )}" action="{HtmlEncode( Request.url )}" method="get">
			<table id="{@ControlId}" class="grid">
				{Content}
			</table>
			{GridControlPaging}
			<input type="submit" value="updateui" class="updateui visuallyhidden">
		</form>
	</li>
</tpl:section>

<tpl:section name="EmbeddedGridControl" field="Control">
	<li class="clearfix">
		<table id="{@ControlId}" class="grid">
			{Content}
		</table>
		{GridControlPaging}
	</li>
</tpl:section>

	<tpl:section name="GridControlHeader" field="Content">
		<thead>
			{Row}
		</thead>
	</tpl:section>

		<tpl:section name="GridControlHeaderRow" field="Row">
			<tr class="row {RowClasses}">{Cell}</tr>
		</tpl:section>

	<tpl:section name="GridControlBody" field="Content">
		<tbody>
			{Row}
		</tbody>
	</tpl:section>

		<tpl:section name="GridControlBodyRow" field="Row">
			<tr class="row {RowClasses} clickable">{Cell}</tr>
		</tpl:section>

	<tpl:section name="GridControlFilterRow" field="Row">
		<tr class="row filters {RowClasses}">{Cell}</tr>
	</tpl:section>

	<tpl:section name="GridControlRowNoResults" field="Row"><tr><td colspan="{GridProperties.columnCount}">{NotEmpty( GridProperties.notFoundMessage, 'No results found' )}</td></tr></tpl:section>

	<tpl:section name="GridControlHeaderCell" field="Cell"><th style="{@GridControlHeaderCellWidth}{CellClasses}">{ColumnProperties.heading}</th></tpl:section>

	<tpl:section name="GridControlSortableHeaderCell" field="Cell"><th style="{@GridControlHeaderCellWidth}{CellClasses}"><a href="{HtmlEncode( ChangeQueryString( Request.url, GetControlPropertyName( 'sort' ), HeaderProperties.sortParameter ) )}" class="sortable {HeaderProperties.direction}">{ColumnProperties.heading}</a></th></tpl:section>

		<tpl:section name="GridControlHeaderCellWidth" field="CellClasses" requires="{Not( IsEmpty( ColumnProperties.width ) )}">width:{ColumnProperties.width};</tpl:section>

	<tpl:section name="GridControlCell" field="Cell"><td>{CellContent}</td></tpl:section>

		<tpl:section name="GridControlPropertyColumnContent" field="CellContent">{CellProperties.value}</tpl:section>

	<tpl:section name="GridControlPaging">{RenderEmbeddedPagingControl( $dataset, GridProperties.id )}</tpl:section>

	<tpl:section name="NoFilterColumnFilter" field="Cell"><td>&nbsp;</td></tpl:section>

	<tpl:section name="TextboxColumnFilter" field="Cell"><td><input type="text" name="{GetControlPropertyName( ColumnFilterProperties.property )}" value="{GetControlPropertyValue( ColumnFilterProperties.property )}" class="filter-textbox"></td></tpl:section>

	<tpl:section name="SelectboxColumnFilter" field="Cell">
		<td>
			<select name="{GetControlPropertyName( ColumnFilterProperties.property )}" class="filter-selectbox">
				{Option}
			</select>
		</td>
	</tpl:section>

		<tpl:section name="SelectboxColumnFilterOption" field="Option"><option value="{HtmlEncode( OptionProperties.value )}" {If( Equal( OptionProperties.value, GetControlPropertyValue( ColumnFilterProperties.property ) ), 'selected')}>{OptionProperties.label}</option></tpl:section>



<!-- paging controls -->
<tpl:section name="EmbeddedPagingControl" field="Control">
	{Control}
</tpl:section>

<tpl:section name="StandAlonePagingControl" field="Control">
	<form method="get" action="{Request.url}" class="form helper-form" accept-charset="utf-8">
		{Control}
		<input type="submit" value="updateui" class="updateui visuallyhidden">
	</form>
</tpl:section>

<tpl:section name="PagingControl" field="Control">
	<nav id="{GetControlPropertyName( ControlProperties.id, 'paging' )}" class="paging-control clearfix">
		<div class="summary">
			Showing {PagingProperties.rowStart} - {PagingProperties.rowEnd} of {PagingProperties.rowTotal}
		</div>
		<div class="navigation">
			<label for="{GetControlPropertyName( ControlProperties.id, 'paging-page-size' )}">Items:</label>
			<select name="{GetControlPropertyName( ControlProperties.id, 'page-size' )}" id="{GetControlPropertyName( ControlProperties.id, 'paging-page-size' )}">
				<option value="1" {If( Equal( PagingProperties.pageSize, '1' ), 'selected' )}>1</option>
				<option value="5" {If( Equal( PagingProperties.pageSize, '5' ), 'selected' )}>5</option>
				<option value="25" {If( Equal( PagingProperties.pageSize, '25' ), 'selected' )}>25</option>
				<option value="50" {If( Equal( PagingProperties.pageSize, '50' ), 'selected' )}>50</option>
				<option value="500" {If( Equal( PagingProperties.pageSize, '500' ), 'selected' )}>500</option>
				<option value="9999" {If( Equal( PagingProperties.pageSize, '9999' ), 'selected' )}>All</option>
			</select>
			<label for="{GetControlPropertyName( ControlProperties.id, 'page-number' )}">Page:</label>
			<select name="{GetControlPropertyName( ControlProperties.id, 'page-number' )}" id="{GetControlPropertyName( ControlProperties.id, 'paging-page-number' )}">
				{PagingControlPageOption}
			</select>
			of <span id="{GetControlPropertyName( ControlProperties.id, 'paging-page-total' )}">{PagingProperties.pageCount}</span>
			<div>
				{Controls}
			</div>
		</div>
	</nav>
</tpl:section>

	<tpl:section name="PagingControlPageOption"><option {If( Equal( PageOption.number, PagingProperties.currentPage ), 'selected' )}>{PageOption.number}</option></tpl:section>

	<tpl:section name="PreviousControlsEnabled" field="Controls">
		<a href="#" class="first-page" title="Go to first page">First</a>
		<a href="#" class="previous-page" title="Go to previous page">Previous</a>
	</tpl:section>
	<tpl:section name="PreviousControlsDisabled" field="Controls">
		<span class="first-page">First</span>
		<span class="previous-page">Previous</span>
	</tpl:section>

	<tpl:section name="NextControlsEnabled" field="Controls">
		<a href="#" class="next-page" title="Go to next page">Next</a>
		<a href="#" class="last-page" title="Go to last page">Last</a>
	</tpl:section>
	<tpl:section name="NextControlsDisabled" field="Controls">
		<span class="next-page">Next</span>
		<span class="last-page">Last</span>
	</tpl:section>



<!-- dialog control sections -->
<tpl:section name="InvokeDialogParentTrigger" field="Header">
	<script>top.jQuery(top.document).trigger("{TriggerProperties.action}");</script>
</tpl:section>
<tpl:section name="InvokeDialogParentTriggerWithParameters" field="Header">
	<script>top.jQuery(top.document).trigger("{TriggerProperties.action}", {DataspaceToJSonArray( $TriggerArguments )});</script>
</tpl:section>



<!-- group sections -->
<tpl:section name="GroupControl" field="Control">
	<fieldset id="{@ControlId}" class="clearfix group {ControlProperties.cssClass}">
		<legend>{ControlProperties.label}</legend>
		{Control}
	</fieldset>
</tpl:section>