<tpl:section name="ContentTitle"><h1>{ContentProperties.name}</h1></tpl:section>

<tpl:section name="ContentMeta"><span class="meta">{FormatDate( ContentProperties.publicationDate, 'dd-MM-yyyy' )}</span></tpl:section>

<tpl:section name="ContentDescription" requires="{Not( IsEmpty( ContentProperties.description ) )}"><div class="description">{ContentProperties.description}</div></tpl:section>
<tpl:section name="ContentDescriptionTrusted" requires="{Not( IsEmpty( ContentProperties.description ) )}"><div class="description">{#ContentProperties.description}</div></tpl:section>

<tpl:section name="ContentBody" requires="{Not( IsEmpty( ContentProperties.body ) )}"><div class="body">{ContentProperties.body}</div></tpl:section>
<tpl:section name="ContentBodyTrusted" requires="{Not( IsEmpty( ContentProperties.body ) )}"><div class="body">{#ContentProperties.body}</div></tpl:section>



<!-- Default list item appearance sections -->
<tpl:section name="ListItemAppearanceCompact" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{Row.name}">
		<h4>{Row.name}</h4>
	</a>
</tpl:section>

<tpl:section name="ListItemAppearanceNormal" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{Row.name}">
		<h4>{Row.name}</h4>
		<div class="description">{Clip( StripHTML( Row.description ), '150' )}</div>
	</a>
</tpl:section>

<tpl:section name="ListItemAppearanceExtensive" field="ListContent">
	<a href="{NodeUrl( $Row )}" title="{Row.name}">
		<h4>{Row.name}</h4>
		<span class="meta">{FormatDate( Row.publicationDate, 'dd-MM-yyyy' )}</span>
		<div class="description">{StripHTML( Row.description )}</div>
	</a>
</tpl:section>