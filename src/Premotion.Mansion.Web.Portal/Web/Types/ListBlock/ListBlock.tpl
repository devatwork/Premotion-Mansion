<tpl:section name="ListBlock" field="Block">
	<section class="block {BlockProperties.blockStyle} {BlockProperties.borderStyle} list-block"{@BlockId}>
		{@BlockToolbar}
		{@BlockTitle}
		{Results}
	</section>
</tpl:section>

<tpl:section name="Results">
	<ol class="unstyled list content-list {BlockProperties.listItemAppearance}">
		{ListItem}
	</ol>
	{RenderPagingControl( $ResultSet, Concat( 'block-', BlockProperties.id ) )}
</tpl:section>

	<tpl:section name="ListItem">
		<li class="clickable {ToLower( Row.type )}-item">
			{ListContent}
		</li>
	</tpl:section>

<tpl:section name="NotFoundMessage" field="Results">TODO: not found</tpl:section>




<!-- Default list item appearance sections, may be overridden in content templates -->
<tpl:section name="ListItemAppearanceCompact" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{HtmlEncode( Row.name )}"><h4>{Row.name}</h4></a>
</tpl:section>

<tpl:section name="ListItemAppearanceNormal" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{HtmlEncode( Row.name )}">
		<h4>{Row.name}</h4>
		<div class="description">{Clip( StripHTML( Row.description ), '150' )}</div>
	</a>
</tpl:section>

<tpl:section name="ListItemAppearanceExtensive" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{HtmlEncode( Row.name )}">
		<h4>{Row.name}</h4>
		<span class="meta">{FormatDate( Row.publicationDate, 'dd-MM-yyyy' )}</span>
		<div class="description">{Row.description}</div>
	</a>
</tpl:section>