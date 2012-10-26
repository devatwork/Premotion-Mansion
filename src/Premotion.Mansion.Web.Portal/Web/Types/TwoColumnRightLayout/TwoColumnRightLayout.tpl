<tpl:section name="TwoColumnRightLayout" field="Layout">
	<div class="row-fluid">
		<div class="column span4">
			{#RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
		</div>
		<div class="column span8">
			{#RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>