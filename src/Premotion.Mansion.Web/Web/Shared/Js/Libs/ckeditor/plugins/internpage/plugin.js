( function()
{
	// Get a CKEDITOR.dialog.contentDefinition object by its ID.
	var getById = function(array, id, recurse) {
		for (var i = 0, item; (item = array[i]); i++) {
			if (item.id == id)
				return item;
			if (recurse && item[recurse]) {
				var retval = getById(item[recurse], id, recurse);
				if (retval)
					return retval;
				}
		}
		return null;
	};
	
	var extractPath = function(value, baseUrl) {
		value = CKEDITOR.tools.trim(value);
		if (value.indexOf(baseUrl) != 0) {
			return false;
		}
		return value;
	};
	
	var revertPath = function(value, baseUrl) {
		var path = extractPath(value, baseUrl);
		if (!path) {
			return false;
		}
		return path;
	};
	
	// initialize the plugin
	CKEDITOR.plugins.add("internpage", {
		lang: ['en'],
		init: function(editor) {
			var baseUrl = editor.config.internpage.baseUrl;
			CKEDITOR.on('dialogDefinition', function(e) {
				//  only interested in link selector
				if ((e.editor != editor) || (e.data.name != 'link'))
					return;
				
				// overrides definition
				var definition = e.data.definition;
				definition.onOk = CKEDITOR.tools.override(definition.onOk, function(original) {
					return function() {
						var process = false;
						if ((this.getValueOf('info', 'linkType') == 'mansion') && !this._.selectedElement) {
							var ranges = editor.getSelection().getRanges(true);
							if ((ranges.length == 1) && ranges[0].collapsed) {
								process = true;
							}
						}
						original.call(this);
						if (process) {
							var value = this.getValueOf('info', 'mansion_path');
							var index = value.lastIndexOf('(');
							if (index != -1) {
								var text = CKEDITOR.tools.trim(value.substr(0, index));
								if (text) {
									CKEDITOR.plugins.link.getSelectedLink(editor).setText(text);
								}
							}
						}
					};
				});
				
				// overrides linkType definition
				var infoTab = definition.getContents('info');
				var content = getById(infoTab.elements, 'linkType');
				content.items.unshift([editor.lang.internpage.link_type_name, 'mansion']);
				infoTab.elements.push({
					type: 'vbox',
					id: 'mansionOptions',
					children: [{
						type: 'select',
						items: [],
						id: 'mansion_path',
						label: editor.lang.link.title,
						required: true,
						onLoad: function() {
						},
						setup: function(data) {
							this.setValue(data.mansion_path || '');
						},
						validate: function() {
							var dialog = this.getDialog();
							if (dialog.getValueOf('info', 'linkType') != 'mansion') {
								return true;
							}
							var func = CKEDITOR.dialog.validate.notEmpty(editor.lang.link.noUrl);
							if (!func.apply(this)) {
								return false;
							}
							if (!extractPath(this.getValue(), baseUrl)) {
								alert(editor.lang.internpage.msg_invalid_path);
								this.focus();
								return false;
							}
							return true;
						}
					}]
				});
				content.onChange = CKEDITOR.tools.override(content.onChange, function(original) {
					return function() {
						original.call(this);
						var dialog = this.getDialog();
						var element = dialog.getContentElement('info', 'mansionOptions').getElement().getParent().getParent();
						if (this.getValue() == 'mansion') {
							element.show();
							if (editor.config.linkShowTargetTab) {
								dialog.showPage('target');
							}
							var uploadTab = dialog.definition.getContents('upload');
							if (uploadTab && !uploadTab.hidden) {
								dialog.hidePage('upload');
							}
						}
						else {
							element.hide();
						}
					};
				});
				content.setup = function(data) {
					if (!data.type || (data.type == 'url') && !data.url) {
						// select the internal page link by default
						data.type = 'mansion';
					}
					else if (data.url && !data.url.protocol && data.url.url) {
						var path = revertPath(data.url.url, baseUrl);
						if (path) {
							data.type = 'mansion';
							data.mansion_path = path;
							delete data.url;
						}
					}
					this.setValue(data.type);
				};
				content.commit = function(data) {
					data.type = this.getValue();
					if (data.type == 'mansion') {
						data.type = 'url';
						var dialog = this.getDialog();
						dialog.setValueOf('info', 'protocol', '');
						dialog.setValueOf('info', 'url', dialog.getValueOf('info', 'mansion_path'));
					}
				};
				
				// retrieve the content of the select box
				$.getJSON(e.editor.config.internpage.pageArrayUrl, function(data) {
					var dialog = CKEDITOR.dialog.getCurrent();
					var mansionPath = dialog.getContentElement('info', 'mansion_path');
					mansionPath.clear();
					$.each(data, function(i, obj) {
						mansionPath.add(obj.name, obj.url);
					});
				});
			});
		}
	});
	
})();