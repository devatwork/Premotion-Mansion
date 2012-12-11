<tpl:section name="ThreeColumnLayout" field="Layout">
	<div class="row-fluid">
		<div class="column span4">
			{#RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
		<div class="column span4">
			{#RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
		</div>
		<div class="column span4">
			{#RenderColumn( 'tertiary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>