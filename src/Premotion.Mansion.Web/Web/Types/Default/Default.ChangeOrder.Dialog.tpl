<tpl:section name="Siblings" field="Content">
	<ol class="unstyled">
		{Sibling}
	</ol>
</tpl:section>

	<tpl:section name="Sibling">
		<li>
			<strong>{SiblingNode.order}</strong> {SiblingNode.name}
		</li>
	</tpl:section>