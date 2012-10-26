<tpl:section name="NavigationItem">
	<li class="{If( NavigationItemProperties.isActive, 'active' )}">
		<a href="{NodeUrl( NavigationItemProperties.targetNode )}" title="{NavigationItemNode.name}" class="internal">{NavigationItemNode.name}</a>
		{SubNavigationItem}
	</li>
</tpl:section>