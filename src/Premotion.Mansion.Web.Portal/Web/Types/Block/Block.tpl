<tpl:section name="BlockTitle" requires="{Not( IsTrue( BlockProperties.hideTitle ) )}">
	<header>
		<h1>{BlockProperties.name}</h1>
	</header>
</tpl:section>

<tpl:section name="BlockDescription" requires="{Not( IsEmpty( BlockProperties.description ) )}"><div class="description">{BlockProperties.description}</div></tpl:section>
<tpl:section name="BlockBody" requires="{Not( IsEmpty( BlockProperties.body ) )}"><div class="body">{BlockProperties.body}</div></tpl:section>



<!-- maintenance toolbars -->
<tpl:section name="BlockToolbar" requires="{HasPortalAdminPermission()}">
	<div class="clearfix block-toolbar">
		<a href="{RouteUrl( 'Dialog', 'Dialog', 'Configure', BlockProperties.id )}" class="ir command configure dialog" title="Configure this block">Configure</a>
		<span class="ir command move" title="Move this block">Move</span>
		<a href="{RouteUrl( 'Dialog', 'Dialog', 'Remove', BlockProperties.id )}" class="ir command remove dialog" title="Remove this block">Remove</a>
	</div>
</tpl:section>

<tpl:section name="BlockId" requires="{HasPortalAdminPermission()}"> data-id="{BlockProperties.id}"</tpl:section>