<tpl:section name="PresentationPageBlock" field="Block">
	<article class="block {BlockProperties.blockStyle} {BlockProperties.borderStyle} detail page {ToLower( PageProperties.type )}"{@BlockId}>
		{@BlockToolbar}
		{Content}
	</article>
</tpl:section>