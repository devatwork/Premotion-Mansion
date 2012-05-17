<tpl:section name="Siblings" field="Content">
	<ol>
		{Sibling}
	</ol>
</tpl:section>

	<tpl:section name="Sibling">
		<li>
			<strong>{SiblingNode.order}</strong> {SiblingNode.name}
		</li>
	</tpl:section>