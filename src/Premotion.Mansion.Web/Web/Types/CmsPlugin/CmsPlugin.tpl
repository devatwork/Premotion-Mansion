<tpl:section name="NavigationHeader" field="NavigationItem">
	<li class="disabled">
		<a href="#">
			{Section.label}
		</a>
	</li>
</tpl:section>

<tpl:section name="NavigationItem">
	<li class="{If( Section.active, 'active' )}">
		<a href="{Section.url}">
			<i class="{Section.iconClass}"></i> {Section.label}
		</a>
	</li>
</tpl:section>