<tpl:section name="PageTitle">
	<h1>{ContentProperties.name}</h1>
</tpl:section>

<tpl:section name="PageDescription" requires="{Not( IsEmpty( PageProperties.description ) )}"><div class="description">{PageProperties.description}</div></tpl:section>
<tpl:section name="PageDescriptionTrusted" requires="{Not( IsEmpty( PageProperties.description ) )}"><div class="description">{#PageProperties.description}</div></tpl:section>

<tpl:section name="PageBody" requires="{Not( IsEmpty( PageProperties.body ) )}"><div class="body">{PageProperties.body}</div></tpl:section>
<tpl:section name="PageBodyTrusted" requires="{Not( IsEmpty( PageProperties.body ) )}"><div class="body">{#PageProperties.body}</div></tpl:section>