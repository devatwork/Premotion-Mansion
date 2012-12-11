<tpl:section name="OneColumnLayout" field="Layout">
	<div class="row-fluid">
		<div class="column span12">
			{#RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>