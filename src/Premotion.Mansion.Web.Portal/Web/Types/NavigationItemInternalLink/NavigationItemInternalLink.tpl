<tpl:section name="NavigationItem">
	<li class="{If( NavigationItemProperties.isActive, 'active' )}">
		<a href="{NodeUrl( NavigationItemProperties.targetNode )}" title="{HtmlEncode( NavigationItemNode.name )}" class="internal">{NavigationItemNode.name}</a>
		{SubNavigationItem}
	</li>
</tpl:section>