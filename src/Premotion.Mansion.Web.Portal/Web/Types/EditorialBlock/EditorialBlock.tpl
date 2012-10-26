<tpl:section name="EditorialBlock" field="Block">
	<article class="block {BlockProperties.blockStyle} {BlockProperties.borderStyle} editorial"{@BlockId}>
		{@BlockToolbar}
		{@BlockTitle}
		<div class="content">
			{@BlockDescriptionTrusted}
			{@BlockBodyTrusted}
		</div>
	</article>
</tpl:section>