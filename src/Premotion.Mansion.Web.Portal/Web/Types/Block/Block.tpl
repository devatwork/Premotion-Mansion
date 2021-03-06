﻿<tpl:section name="BlockTitle" requires="{Not( IsTrue( BlockProperties.hideTitle ) )}">
	<header>
		<h2>{BlockProperties.name}</h2>
	</header>
</tpl:section>

<tpl:section name="BlockDescription" requires="{Not( IsEmpty( BlockProperties.description ) )}"><div class="description">{BlockProperties.description}</div></tpl:section>
<tpl:section name="BlockDescriptionTrusted" requires="{Not( IsEmpty( BlockProperties.description ) )}"><div class="description">{#BlockProperties.description}</div></tpl:section>
<tpl:section name="BlockBody" requires="{Not( IsEmpty( BlockProperties.body ) )}"><div class="body">{BlockProperties.body}</div></tpl:section>
<tpl:section name="BlockBodyTrusted" requires="{Not( IsEmpty( BlockProperties.body ) )}"><div class="body">{#BlockProperties.body}</div></tpl:section>

<tpl:section name="BlockReadMoreLink" requires="{Not( IsEmpty( BlockProperties.readMoreTargetGuid ) )}">
	<div class="read-more">
		<a href="{NodeUrl( $TargetNode )}" title="{BlockProperties.readMoreLabel}">{BlockProperties.readMoreLabel}</a>
	</div>
</tpl:section>

<!-- maintenance toolbars -->
<tpl:section name="BlockToolbar" requires="{And( HasPortalAdminPermission(), Not( IsTrue( BlockProperties._readonly ) ) )}">
	<div class="block-toolbar">
		<a href="{RouteUrl( 'Portal', 'ConfigureBlock', BlockProperties.id )}" data-toggle="modal" data-target="#portal-modal-popup" class="hide-text command dialog configure" title="Configure this block">Configure</a>
		<span class="hide-text command move" title="Move this block">Move</span>
		<a href="{RouteUrl( 'Portal', 'RemoveBlock', BlockProperties.id )}" data-toggle="modal" data-target="#portal-modal-popup" class="hide-text command dialog remove" title="Remove this block">Remove</a>
	</div>
</tpl:section>

<tpl:section name="BlockId" requires="{HasPortalAdminPermission()}"> data-id="{BlockProperties.id}"</tpl:section>