﻿<tpl:section name="NavigationItem">
	<li class="{If( NavigationItemProperties.isActive, 'active' )}">
		<a href="{NavigationItemNode.externalUrl}" title="{NavigationItemNode.name}" class="external" target="_blank">{NavigationItemNode.name}</a>
		{SubNavigationItem}
	</li>
</tpl:section>