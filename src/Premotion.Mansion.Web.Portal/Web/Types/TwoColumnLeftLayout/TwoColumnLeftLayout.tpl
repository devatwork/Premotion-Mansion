<tpl:section name="TwoColumnLeftLayout" field="Layout">
	<div class="row">
		<div class="column span8">
			{RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
		<div class="column span4">
			{RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>