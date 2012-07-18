﻿<tpl:section name="NavigationHeader" field="NavigationItem">
	<li class="nav-header">{Section.label}</li>
</tpl:section>

<tpl:section name="NavigationItem">
	<li class="{If( And( IsEqual( Section.pluginType, ActivePluginProperties.pluginType ), IsEqual( Section.viewName, ActivePluginProperties.viewName ) ), 'active' )}">
		<a href="index.htm">
			<i class="{Section.iconClass}"></i> {Section.label}
		</a>
	</li>
</tpl:section>