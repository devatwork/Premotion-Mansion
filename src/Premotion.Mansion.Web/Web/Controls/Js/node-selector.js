/* ==|== Autocomlete =====================================================
Author: Premotion Software Solutions
========================================================================== */
;
(function($, undefined) {
    /* NODESELECTOR CLASS DEFINITION
	* =========================== */
    var NodeSelector = function($element, options) {
        this.options = options;
        this.$element = $element;
        this.$candidateList = this.$element.find('.nav-list');
        this.$selectedValuesList = this.$element.find('.nav-pills');
        this.$searchInput = this.$element.find('[type="search"]');
        this.listen();
    };
    NodeSelector.prototype = {
        constructor: NodeSelector,
        listen: function () {
            var that = this;
            // attach autocomplete handler to search input list
            that.$searchInput
                .on('keydown', _.debounce(function (e) {
                    that.onkeydown(e);
                }, 200))
                .on('keypress', _.debounce(function (e) {
                    that.onkeypress(e);
                }, 200))
                .on('keyup', _.debounce(function (e) {
                    that.onkeyup(e);
                }, 200));
        },
        //  result list methods
        resultListPrev: function () {
            console.log('selecting previous item');
        },
        resultListNext: function () {
            console.log('selecting next item');
        },
        resultListSelect: function () {
            console.log('selecting current item');
        },
        // autocomplete methods
        retrieveAutcompleteResults: function () {
            var that = this;
            that.query = that.$searchInput.val();
            if (!that.query || that.query.length < that.options.minLength) {
                return that.autocompleteShown ? that.hideAutocomplete() : that;
            }
            console.log('retrieve autocomplete results for: ' + that.query);
        },
        hideAutocomplete: function(e) {
            console.log('hiding autocomplete results');
            this.autocompleteShown = false;
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
                case 38: // up arrow
                    e.preventDefault();
                    this.resultListPrev();
                    break;
                case 40: // down arrow
                    e.preventDefault();
                    this.resultListNext();
                    break;
            }

            e.stopPropagation();
        },
        onkeydown: function (e) {
            this.suppressKeyPressRepeat = ~$.inArray(e.keyCode, [40, 38, 9, 13, 27]);
            this.autocompleteMove(e);
        },
        onkeypress: function (e) {
            if (this.suppressKeyPressRepeat)
                return;
            this.autocompleteMove(e);
        },
        onkeyup: function (e) {
            switch (e.keyCode) {
                case 40: // down arrow
                case 38: // up arrow
                case 16: // shift
                case 17: // ctrl
                case 18: // alt
                    break;

                case 9: // tab
                case 13: // enter
                    if (!this.autocompleteShown)
                        return;
                    this.resultListSelect();
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
        NodeSelector.apply(this, arguments);
    };
    SingleNodeSelector.prototype = Object.create(NodeSelector.prototype);
    SingleNodeSelector.prototype.parent = NodeSelector.prototype;
    SingleNodeSelector.prototype.constructor = SingleNodeSelector;
    //SingleNodeSelector.prototype.init = function () {
    //    this.parent.init.call(this, arguments);
    //};

    /* MULTINODESELECTOR CLASS DEFINITION
    * ================================ */
    var MultiNodeSelector = function($element, options) {
        NodeSelector.apply(this, arguments);
    };
    MultiNodeSelector.prototype = Object.create(NodeSelector.prototype);
    MultiNodeSelector.prototype.parent = NodeSelector.prototype;
    MultiNodeSelector.prototype.constructor = MultiNodeSelector;
    //MultiNodeSelector.prototype.init = function () {
    //    this.parent.init.call(this, arguments);
    //};

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