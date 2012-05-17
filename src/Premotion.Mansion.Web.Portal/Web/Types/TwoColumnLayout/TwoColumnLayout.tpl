<tpl:section name="TwoColumnLayout" field="Layout">
	<div class="layout layout-twocolumns" role="main">
		<div class="clearfix column-row">
			{RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
			{RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>