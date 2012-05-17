<tpl:section name="TwoColumnRightLayout" field="Layout">
	<div class="layout layout-twocolumns-right" role="main">
		<div class="clearfix column-row">
			{RenderColumn( 'secondary-column', $PageNode, $PageBlockNodeset )}
			{RenderColumn( 'primary-column', $PageNode, $PageBlockNodeset )}
		</div>
	</div>
</tpl:section>