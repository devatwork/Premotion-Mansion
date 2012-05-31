<tpl:section name="TwoColumnLayout" field="Layout">
	<div class="row">
		<div class="column span6">
			{RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
		<div class="column span6">
			{RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>