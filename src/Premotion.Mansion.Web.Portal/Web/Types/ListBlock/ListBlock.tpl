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

<tpl:section name="NotResultsMessage" field="Results">{NotEmpty( BlockProperties.noResultsMessage, 'No results found' )}</tpl:section>