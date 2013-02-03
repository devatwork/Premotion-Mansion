/* ==|== NodeSelector ====================================================
Author: Premotion Software Solutions
========================================================================== */
;
(function($, undefined) {
    /* NODESELECTOR COMPILED TEMPLATES
	* ============================= */
    var breadcrumbsTemplate = _.template($('#node-selector-breadcrumbs-template').html());
    var resultsTemplate = _.template($('#node-selector-results-template').html());
    var selectedItemTemplate = _.template($('#node-selector-selected-item-tempate').html());
    var selectedItemsTemplate = _.template($('#node-selector-selected-items-tempate').html());
    
    /* NODESELECTOR CLASS DEFINITION
	* =========================== */
    var NodeSelector = function($element, options) {
        if (!arguments.length)
            return;
        this.options = options;
        this.$element = $element;
        this.controlId = $element.attr('id');
        this.$breadcrumbList = this.$element.find('.breadcrumb');
        this.$candidateList = this.$element.find('.nav-list');
        this.$selectedValuesList = this.$element.find('.nav-pills');
        this.candidateListCount = 0;
        this.candidateListIndex = 0;
        this.$searchInput = this.$element.find('[type="search"]');
        this.endpointUrl = this.$element.data('service-endpoint');
        this.$valueField = this.$element.find('#' + this.controlId + '-value');
        this.settings = this.$element.find('#' + this.controlId + '-settings').val();
        this.parentId = undefined;
        this.listen();
        this.retrieve();
    };
    NodeSelector.prototype = {
        constructor: NodeSelector,
        listen: function () {
            var that = this;
            // attach event handlers
            that.$element
                .on('click', '[data-behavior="remove"]', $.proxy(function (e) {
                    that.removeSelectedValue(e);
                }))
                .on('click', '[data-behavior="select"]', $.proxy(function (e) {
                    that.resultListSelect(e);
                }))
                .on('click', '[data-behavior="browse"]', $.proxy(function (e) {
                    that.resultListBrowseSelected(e);
                }))
                .on('keydown', '[type="search"]', _.debounce(function (e) {
                    that.autocompleteOnkeydown(e);
                }, 200))
                .on('keypress', '[type="search"]', _.debounce(function (e) {
                    that.autocompleteOnkeypress(e);
                }, 200))
                .on('keyup', '[type="search"]', _.debounce(function (e) {
                    that.autocompleteOnkeyup(e);
                }, 200))
                .on('keydown', _.throttle(function (e) {
                    that.resultListOnkeydown(e);
                }, 100));
        },
        // retrieve methods
        retrieve: function() {
            var that = this;
            $.ajax({
                data: {
                    settings: that.settings,
                    action: 'initiate',
                    selected: that.$valueField.val()
                },
                dataType: 'json',
                type: 'POST',
                url: that.endpointUrl
            }).done(function (data) {
                that.renderResults(data);
                that.$candidateList.focus();
            });
        },
        // render methods
        renderResults: function(data) {
            var that = this;
            
            // set the parent id
            that.parentId = data.parentId;
            
            // render the crumbpath
            that.$breadcrumbList.empty().append(breadcrumbsTemplate(data));
            
            // render the results
            that.$candidateList.empty().append(resultsTemplate(data));
            that.candidateListCount = that.$candidateList.children().length;
            that.candidateListIndex = 0;
            that.$candidateList.children().eq(that.candidateListIndex).addClass('active');
            
            // render the current value if any
            if (data.selected != undefined && data.selected.length > 0) {
                _.forEach(data.selected, function (item) {
                    that.addSelectedValue(item.id, item.label);
                });
            }
        },
        // value list methods
        removeSelectedValue: function (e) {
            e.preventDefault();
            var that = this;

            // render the breadcrumbs
            that.$breadcrumbList.empty().append(
                breadcrumbsTemplate(data)
            );

            // render the results
            that.$candidateList.empty().append(
                resultsTemplate(data)
            );
            that.candidateListCount = that.$candidateList.children().length;
            that.candidateListIndex = 0;
            that.$candidateList.children().eq(that.candidateListIndex).addClass('active');
            return false;
        },
        // result list methods
        resultListOnkeydown: function (e) {
            switch (e.keyCode) {
                case 9: // tab
                case 13: // enter
                    e.preventDefault();
                    this.resultListSelectCurrent();
                    break;
                case 38: // up arrow
                    e.preventDefault();
                    this.resultListPrev();
                    break;
                case 40: // down arrow
                    e.preventDefault();
                    this.resultListNext();
                    break;
                case 37: // left arrow
                    e.preventDefault();
                    var $crumbs = this.$breadcrumbList.children(),
                        length = $crumbs.length;
                    if (length <= 1)
                        return;
                    var $parent = $crumbs.eq(length - 2);
                    this.resultListBrowse($parent);
                    break;
                case 39: // right arrow
                    e.preventDefault();
                    var $child = this.$candidateList.children().eq(this.candidateListIndex);
                    this.resultListBrowse($child);
                    break;
            }
            if (e.keyCode >= 48 && e.keyCode <= 90) {
                this.$searchInput.focus();
            }
            e.stopPropagation();
        },
        resultListPrev: function () {
            this.$candidateList.children().eq(this.candidateListIndex).removeClass('active');
            this.candidateListIndex = this.candidateListIndex - 1 >= 0 ? this.candidateListIndex - 1 : this.candidateListCount - 1;
            this.$candidateList.children().eq(this.candidateListIndex).addClass('active');
        },
        resultListNext: function () {
            this.$candidateList.children().eq(this.candidateListIndex).removeClass('active');
            this.candidateListIndex = this.candidateListIndex + 1 <= this.candidateListCount - 1 ? this.candidateListIndex + 1 : 0;
            this.$candidateList.children().eq(this.candidateListIndex).addClass('active');
        },
        resultListSelectCurrent: function () {
            // find the selected item
            var $item = this.$candidateList.children().eq(this.candidateListIndex);

            // check if the item is assignable
            if ($item.data('is-assignable') != true) {
                return;
            }

            this.addSelectedValue($item.data('id'), $item.data('label'));
        },
        resultListSelect: function (e) {
            e.preventDefault();
            e.stopPropagation();
            this.$candidateList.children().eq(this.candidateListIndex).removeClass('active');
            this.candidateListIndex = this.$candidateList.children().index($(e.target).parents('li'));
            this.$candidateList.children().eq(this.candidateListIndex).addClass('active');
            this.resultListSelectCurrent();
            return false;
        },
        resultListBrowseSelected: function (e) {
            e.preventDefault();
            var $elem = $(e.target).parents('li');
            this.resultListBrowse($elem);
            return false;
        },
        resultListBrowse: function ($elem) {
            var hasAssignableChildren = !$elem.hasClass('disabled'),
                that = this;
            that.parentId = $elem.data('id');
            if (that.parentId === undefined || hasAssignableChildren !== true) {
                return that;
            }
            $.ajax({
                data: {
                    settings: that.settings,
                    selected: that.$valueField.val(),
                    action: 'browse',
                    parent: that.parentId
                },
                dataType: 'json',
                type: 'POST',
                url: that.endpointUrl
            }).done(function (data) {
                that.renderResults(data);
                that.$candidateList.focus();
            });
            return that;
        },
        // autocomplete methods
        retrieveAutcompleteResults: function () {
            var that = this;
            that.query = that.$searchInput.val();
            if (!that.query || that.query.length < that.options.minLength) {
                return that.autocompleteShown ? that.hideAutocomplete() : that;
            }
            $.ajax({
                data: {
                    settings: that.settings,
                    selected: that.$valueField.val(),
                    action: 'autocomplete',
                    fragment: that.query,
                    parent: that.parentId
                },
                dataType: 'json',
                type: 'POST',
                url: that.endpointUrl
            }).done(function (data) {
                that.renderResults(data);
                that.autocompleteShown = true;
            });
            return that;
        },
        hideAutocomplete: function (e) {
            this.autocompleteShown = false;
            var $crumbs = this.$breadcrumbList.children(),
                length = $crumbs.length;
            if (length <= 1)
                return this;
            var $parent = $crumbs.eq(length - 2);
            this.resultListBrowse($parent);
            return this;
        },
        autocompleteMove: function (e) {
            if (!this.autocompleteShown)
                return;

            switch (e.keyCode) {
                case 9: // tab
                case 13: // enter
                case 27: // escape
                    e.preventDefault();
                    break;
            }

            e.stopPropagation();
        },
        autocompleteOnkeydown: function (e) {
            this.suppressKeyPressRepeat = !$.inArray(e.keyCode, [40, 38, 37, 39, 9, 13, 27]);
            this.autocompleteMove(e);
        },
        autocompleteOnkeypress: function (e) {
            if (this.suppressKeyPressRepeat)
                return;
            this.autocompleteMove(e);
        },
        autocompleteOnkeyup: function (e) {
            switch (e.keyCode) {
                case 40: // down arrow
                case 38: // up arrow
                case 37: // left arrow
                case 39: // right arrow
                case 16: // shift
                case 17: // ctrl
                case 18: // alt
                    break;

                case 9: // tab
                case 13: // enter
                    if (!this.autocompleteShown)
                        return;
                    this.resultListSelectCurrent();
                    break;

                case 27: // escape
                    if (!this.autocompleteShown)
                        return;
                    this.hideAutocomplete();
                    break;

                default:
                    this.retrieveAutcompleteResults();
            }

            e.stopPropagation();
            e.preventDefault();
        }
    };

    /* SINGLENODESELECTOR CLASS DEFINITION
    * ================================= */
    var SingleNodeSelector = function($element, options) {
        NodeSelector.call(this, $element, options);
        this.$selectedItem = this.$element.find('.selected-item');
    };
    SingleNodeSelector.prototype = new NodeSelector();
    SingleNodeSelector.prototype.parent = NodeSelector.prototype;
    SingleNodeSelector.prototype.constructor = SingleNodeSelector;
    SingleNodeSelector.prototype.removeSelectedValue = function (e) {
        e.preventDefault();
        
        // update the ui
        this.$selectedItem.empty().append('Select a value');

        // update the value field
        this.$valueField.val('');
    };
    SingleNodeSelector.prototype.addSelectedValue = function (id, label) {
        // update the ui
        this.$selectedItem.empty().append(selectedItemTemplate({
            id: id,
            label: label
        }));

        // update the value field
        this.$valueField.val(id);
    };

    /* MULTINODESELECTOR CLASS DEFINITION
    * ================================ */
    var MultiNodeSelector = function($element, options) {
        NodeSelector.call(this, $element, options);
        this.$selectedValuesList = this.$element.find('.nav-pills');
        this.selectedValues = [];
    };
    MultiNodeSelector.prototype = new NodeSelector();
    MultiNodeSelector.prototype.parent = NodeSelector.prototype;
    MultiNodeSelector.prototype.constructor = MultiNodeSelector;
    MultiNodeSelector.prototype.removeSelectedValue = function (e) {
        e.preventDefault();
        
        // find the id
        var id = $(e.target).parents('li').data('id');
        
        // update the ui
        this.$selectedValuesList.children('[data-id="' + id + '"]').remove();
        
        // remove from the list of selected valued
        this.selectedValues.splice(this.selectedValues.indexOf(id), 1);
        
        // update the value field
        this.$valueField.val(this.selectedValues.join());
    };
    MultiNodeSelector.prototype.addSelectedValue = function (id, label) {
        // check if the item is not already selected
        if (~$.inArray(id, this.selectedValues)) {
            return;
        }
        this.selectedValues.push(id);

        // update the ui
        this.$selectedValuesList.append(selectedItemsTemplate({
            id: id,
            label: label
        }));

        // update the value field
        this.$valueField.val(this.selectedValues.join());
    };

    /* NODESELECTOR PLUGIN DEFINITION
	* ============================ */
    $.fn.singleNodeSelector = function(option) {
        var args = arguments;
        return this.each(function() {
            var $this = $(this), data = $this.data('single-node-selector'), options = $.extend({}, $.fn.singleNodeSelector.defaults, $this.data(), typeof option == 'object' && option);
            if (!data)
                $this.data('single-node-selector', (data = new SingleNodeSelector($this, options)));
            if (typeof option == 'string')
                data[option].apply(data, Array.prototype.slice.call(args, 1));
        });
    };
    $.fn.singleNodeSelector.defaults = {
        minLength: 3
    };
    $.fn.singleNodeSelector.Constructor = SingleNodeSelector;
    $.fn.multiNodeSelector = function(option) {
        var args = arguments;
        return this.each(function() {
            var $this = $(this), data = $this.data('multi-node-selector'), options = $.extend({}, $.fn.multiNodeSelector.defaults, $this.data(), typeof option == 'object' && option);
            if (!data)
                $this.data('multi-node-selector', (data = new MultiNodeSelector($this, options)));
            if (typeof option == 'string')
                data[option].apply(data, Array.prototype.slice.call(args, 1));
        });
    };
    $.fn.multiNodeSelector.defaults = {
        minLength: 3
    };
    $.fn.multiNodeSelector.Constructor = MultiNodeSelector;

    /* NODESELECTOR DATA-API
	* =================== */
    $(function() {
        // init all controls
        $('[data-behavior="single-node-selector"]').singleNodeSelector();
        $('[data-behavior="multi-node-selector"]').multiNodeSelector();
    });

})(window.jQuery);