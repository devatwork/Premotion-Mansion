/*
* Treeview 1.4 - jQuery plugin to hide and show branches of a tree
* 
* http://bassistance.de/jquery-plugins/jquery-plugin-treeview/
* http://docs.jquery.com/Plugins/Treeview
*
* Copyright (c) 2007 Jörn Zaefferer
*
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Revision: $Id: jquery.treeview.js 4684 2008-02-07 19:08:06Z joern.zaefferer $
*
*/
; (function (a) {  a.extend(a.fn, {  swapClass: function (e, d) {  var c = this.filter("." + e); this.filter("." + d).removeClass(d).addClass(e); c.removeClass(e).addClass(d); return this }, replaceClass: function (d, c) {  return this.filter("." + d).removeClass(d).addClass(c).end() }, hoverClass: function (c) {  c = c || "hover"; return this.hover(function () {  a(this).addClass(c) }, function () {  a(this).removeClass(c) }) }, heightToggle: function (c, d) {  c ? this.animate({  height: "toggle" }, c, d) : this.each(function () {  jQuery(this)[jQuery(this).is(":hidden") ? "show" : "hide"](); if (d) {  d.apply(this, arguments) } }) }, heightHide: function (c, d) {  if (c) {  this.animate({  height: "hide" }, c, d) } else {  this.hide(); if (d) {  this.each(d) } } }, prepareBranches: function (c) {  if (!c.prerendered) {  this.filter(":last-child:not(ul)").addClass(b.last); this.filter((c.collapsed ? "" : "." + b.closed) + ":not(." + b.open + ")").find(">ul").hide() } return this.filter(":has(>ul)") }, applyClasses: function (c, d) {  this.filter(":has(>ul):not(:has(>a))").find(">span").click(function (e) {  d.apply(a(this).next()) }).add(a("a", this)).hoverClass(); if (!c.prerendered) {  this.filter(":has(>ul:hidden)").addClass(b.expandable).replaceClass(b.last, b.lastExpandable); this.not(":has(>ul:hidden)").addClass(b.collapsable).replaceClass(b.last, b.lastCollapsable); this.prepend('<div class="' + b.hitarea + '"/>').find("div." + b.hitarea).each(function () {  var e = ""; a.each(a(this).parent().attr("class").split(" "), function () {  e += this + "-hitarea " }); a(this).addClass(e) }) } this.find("div." + b.hitarea).click(d) }, treeview: function (d) {  d = a.extend({  cookieId: "treeview" }, d); if (d.add) {  return this.trigger("add", [d.add]) } if (d.toggle) {  var i = d.toggle; d.toggle = function () {  return i.apply(a(this).parent()[0], arguments) } } function c(l, n) {  function m(o) {  return function () {  f.apply(a("div." + b.hitarea, l).filter(function () {  return o ? a(this).parent("." + o).length : true })); return false } } a("a:eq(0)", n).click(m(b.collapsable)); a("a:eq(1)", n).click(m(b.expandable)); a("a:eq(2)", n).click(m()) } function f() {  a(this).parent().find(">.hitarea").swapClass(b.collapsableHitarea, b.expandableHitarea).swapClass(b.lastCollapsableHitarea, b.lastExpandableHitarea).end().swapClass(b.collapsable, b.expandable).swapClass(b.lastCollapsable, b.lastExpandable).find(">ul").heightToggle(d.animated, d.toggle); if (d.unique) {  a(this).parent().siblings().find(">.hitarea").replaceClass(b.collapsableHitarea, b.expandableHitarea).replaceClass(b.lastCollapsableHitarea, b.lastExpandableHitarea).end().replaceClass(b.collapsable, b.expandable).replaceClass(b.lastCollapsable, b.lastExpandable).find(">ul").heightHide(d.animated, d.toggle) } } function k() {  function m(n) {  return n ? 1 : 0 } var l = []; j.each(function (n, o) {  l[n] = a(o).is(":has(>ul:visible)") ? 1 : 0 }); a.cookie(d.cookieId, l.join("")) } function e() {  var l = a.cookie(d.cookieId); if (l) {  var m = l.split(""); j.each(function (n, o) {  a(o).find(">ul")[parseInt(m[n]) ? "show" : "hide"]() }) } } this.addClass("treeview"); var j = this.find("li").prepareBranches(d); switch (d.persist) {  case "cookie": var h = d.toggle; d.toggle = function () {  k(); if (h) {  h.apply(this, arguments) } }; e(); break; case "location": var g = this.find("a").filter(function () {  return this.href.toLowerCase() == location.href.toLowerCase() }); if (g.length) {  g.addClass("selected").parents("ul, li").add(g.next()).show() } break } j.applyClasses(d, f); if (d.control) {  c(this, d.control); a(d.control).show() } return this.bind("add", function (m, l) {  a(l).prev().removeClass(b.last).removeClass(b.lastCollapsable).removeClass(b.lastExpandable).find(">.hitarea").removeClass(b.lastCollapsableHitarea).removeClass(b.lastExpandableHitarea); a(l).find("li").andSelf().prepareBranches(d).applyClasses(d, f) }) } }); var b = a.fn.treeview.classes = {  open: "open", closed: "closed", expandable: "expandable", expandableHitarea: "expandable-hitarea", lastExpandableHitarea: "lastExpandable-hitarea", collapsable: "collapsable", collapsableHitarea: "collapsable-hitarea", lastCollapsableHitarea: "lastCollapsable-hitarea", lastCollapsable: "lastCollapsable", lastExpandable: "lastExpandable", last: "last", hitarea: "hitarea" }; a.fn.Treeview = a.fn.treeview })(jQuery);
; (function (c) {  function b(f, e, g, d) {  c.getJSON(f.url, {  root: e }, function (h) {  function i(k) {  var l = c("<li/>").attr("id", this.id || "").html("<span>" + this.text + "</span>").appendTo(k); if (this.classes) {  l.children("span").addClass(this.classes) } if (this.expanded) {  l.addClass("open") } if (this.hasChildren || this.children && this.children.length) {  var j = c("<ul/>").appendTo(l); if (this.hasChildren) {  l.addClass("hasChildren"); i.call({  text: "placeholder", id: "placeholder", children: [] }, j) } if (this.children && this.children.length) {  c.each(this.children, i, [j]) } } } c.each(h, i, [g]); c(d).treeview({  add: g }) }) } var a = c.fn.treeview; c.fn.treeview = function (f) {  if (!f.url) {  return a.apply(this, arguments) } var d = this; b(f, "source", this, d); var e = f.toggle; return a.call(this, c.extend({ }, f, {  collapsed: true, toggle: function () {  var h = c(this); if (h.hasClass("hasChildren")) {  var g = h.removeClass("hasChildren").find("ul"); g.empty(); b(f, this.id, g, d) } if (e) {  e.apply(this, arguments) } } })) } })(jQuery);

/*
* jQuery UI Date and Time Picker Widget
* 
* http://plugins.jquery.com/project/datetime
*
*/
; (function (a, b) {  a.widget("ui.datetime", {  ready: false, widgetEventPrefix: "datetime", options: {  value: null, format: "yy-mm-dd hh:ii", altField: null, altFormat: null, inline: "auto", withDate: true, minDate: null, maxDate: null, showWeek: false, numMonths: 1, withTime: true, stepHours: 1, stepMins: 5, chainTo: null, chainFrom: null }, _create: function () {  this._insert(); this._position(); this._prepare(); this._update(); this._generate(); this._events(); this._chain() }, _prepare: function () {  var c = this.options; this.today = this._clean(new Date()); if (!c.altFormat) {  this.options.altFormat = c.format } value = (!c.value) ? (c.altField) ? a(c.altField).val() : this.element.val() : c.value; format = (!value || value == c.value || value == this.element.val()) ? c.format : c.altFormat; this.date = (!value) ? new Date() : this._parse(value, format); this._limits(); if (value) {  this.options.value = this.date.format(c.format) } }, _limits: function () {  var d = this.options, c = null; this.minDate = (d.minDate) ? this._parse(d.minDate, d.format) : new Date(1970, 0, 1, 0, 0, 0, 0); this.maxDate = (d.maxDate) ? this._parse(d.maxDate, d.format) : new Date(9999, 11, 31, 23, 59, 59, 0); c = (this.current) ? this._clean(this.current) : this._clean(this.date); c.setTime(Math.max(Math.max(c.getTime(), this.minDate.getTime()), Math.min(c.getTime(), this.maxDate.getTime()))); c.setHours(this.date.getHours()); c.setMinutes(this.date.getMinutes()); if (!this.current || this.current != c.getTime()) {  this.date.setTime(c.getTime()) } this.current = c.getTime() }, _insert: function () {  this.tag = this.element.get(0).tagName.toLowerCase(); this.inline = (this.options.inline != "auto") ? this.options.inline : (a.inArray(this.tag, ["input"]) > -1) ? false : true; this.container = (this.tag == "div") ? this.element.addClass("ui-datetime") : a("<div>").addClass("ui-datetime").insertAfter(this.element); if (this.inline) {  this.container.addClass("ui-datetime-inline ui-helper-clearfix") } else {  this.container.hide() } }, _position: function () {  var c = 0; if (a.browser.opera || a.browser.msie) {  } else {  c = this.element.position().left - this.container.position().left; this.container.css("left", c) } }, _generate: function () {  this.calendar = a("<div>").addClass("ui-datetime-calendar ui-widget ui-widget-content ui-corner-all").appendTo(this.container); this.calendar.css("width", (17 * this.options.numMonths).toString() + "em"); withDate = (this.options.withDate) ? this._calendar() : this.calendar.hide(); this.clock = a("<div>").addClass("ui-datetime-clock ui-widget ui-widget-content ui-corner-all").appendTo(this.container); withTime = (this.options.withTime) ? this._clock() : this.clock.hide() }, _chain: function () {  var d = a.extend({ }, this.options, {  chainTo: null, chainFrom: this.element, altField: null, minDate: this.options.value }), c = (this.options.chainTo) ? (this.options.chainTo == "self") ? this.element : a(this.options.chainTo) : null; if (c && c.is(this.element)) {  this.options.chainTo = a("<div>").appendTo(this.container); d.inline = true } this.chainedTo = (c) ? a(this.options.chainTo).datetime(d) : null; this.chainedFrom = (this.options.chainFrom) ? a(this.options.chainFrom).datetime("widget") : null; if (this.chainedFrom) {  this.container.addClass("ui-datetime-to") } }, _calendar: function () {  var c = this.options, e = null, k = "", m = null, j = null, h = 0, l = 0, f = this._clean(new Date(this.current)).getTime(), g = this._clean(this.minDate).getTime(), n = this._clean(this.maxDate).getTime(), p = new Date(this.date.getFullYear(), this.date.getMonth(), 0); if (p.getTime() < this.minDate.getTime()) {  this.date.setMonth(this.minDate.getMonth()) } m = this.date.getFullYear(), month = this.date.getMonth(); for (h = 0; h < c.numMonths; h++) {  e = new Date(m, month); e.setDate(e.getDate() - e.getDay()); className = ((c.numMonths == 1) ? "all" : (h == 0) ? "first" : (h == c.numMonths - 1) ? "last" : "middle"); corners = "ui-corner-" + ((c.numMonths == 1) ? "all" : (h == 0) ? "left" : (h == c.numMonths - 1) ? "right" : "none"); k += '<div class="ui-datetime-calendar-' + className + '"><div class="ui-datetime-header ui-widget-header ' + corners + '"><div class="ui-datetime-title">'; k += (a.inArray(className, ["last", "middle"]) > -1) ? "" : '<a title="Prev" class="ui-datetime-prev ui-corner-all' + ((g > e.getTime()) ? " ui-state-disabled" : "") + '"><span class="ui-icon ui-icon-circle-triangle-w">Prev</span></a>'; k += (a.inArray(className, ["first", "middle"]) > -1) ? "" : '<a title="Next" class="ui-datetime-next ui-corner-all' + ((n < e.getTime()) ? " ui-state-disabled" : "") + '"><span class="ui-icon ui-icon-circle-triangle-e">Next</span></a>'; k += '<span class="ui-datetime-month">' + Date.monthNames[month] + '</span>&nbsp;<span class="ui-datetime-year">' + m + "</span></div></div>"; k += "<table><thead><tr>"; if (c.showWeek) {  k += "<th><span>Wk</span></th>" } a.each(Date.dayNamesMin, function (d, i) {  k += "<th><span>" + i + "</span></th>" }); k += "</tr></thead><tbody>"; for (l = 0; l < 42; l++) {  other = (e.getMonth() != month || e.getTime() < g || e.getTime() > n); k += ((l % 7 == 0) ? "<tr>" + ((c.showWeek) ? '<td class="ui-datetime-week">' + e.getWeek() + "</td>" : "") : ""); k += '<td class="' + ((other) ? "ui-datetime-unselectable ui-state-disabled" : "") + '">'; j = (e.getTime() == this.today.getTime()) ? " ui-state-highlight" : (e.getTime() == f) ? " ui-state-active" : ""; if (other) {  k += '<span class="ui-state-default">' + e.getDate() + "</span>" } else {  k += '<a class="ui-state-default' + j + '" day="' + e.getDate() + '" month="' + e.getMonth() + '">' + e.getDate() + "</a>" } k += "</td>" + ((l % 7 == 6) ? "</tr>" : ""); e.setDate(e.getDate() + 1) } k += "</tbody></table></div>"; if (month == 11) {  m += 1 } month = (month == 11) ? 0 : month + 1 } this.calendar.html(k); this._calendarEvents(); return this }, _calendarEvents: function () {  var c = this; this.calendar.find(".ui-datetime-prev:not('.ui-state-disabled'), .ui-datetime-next:not('.ui-state-disabled'), td a").bind("mouseout", function () {  a(this).removeClass("ui-state-hover") }).bind("mouseover", function () {  a(this).addClass("ui-state-hover") }); this.calendar.find(".ui-datetime-prev:not('.ui-state-disabled')").click(function () {  c._calendarUpdate("months", -1) }); this.calendar.find(".ui-datetime-next:not('.ui-state-disabled')").click(function () {  c._calendarUpdate("months", 1) }); this.calendar.find("td a").click(function (d) {  d.preventDefault(); c.date.setDate(a(this).attr("day")); c.date.setMonth(a(this).attr("month")); c._value(c.date) }) }, _calendarUpdate: function (c, d) {  switch (c) {  case "months": this.date.setMonth(this.date.getMonth() + d); break; case "years": this.date.setFullYear(this.date.getFullYear() + d); break } this._calendar() }, _clock: function () {  var d = this.options, c = '<div class="ui-datetime-header ui-widget-header ui-corner-all"><div class="ui-datetime-title ui-datetime-time">' + this.date.format("'<span class=\"ui-datetime-time-hour\">'hh'</span>:<span class=\"ui-datetime-time-mins\">'ii'</span>'") + '</div></div><table><thead><tr><th><span>Hr</span></th><th><span>Mn</span></th></tr></thead><tbody><tr><td class="ui-datetime-slider-hour"></td><td class="ui-datetime-slider-mins"></td></tr></tbody></table>'; this.clock.html(c).height(this.options.withDate ? this.calendar.height() : "14.2em"); if (this.options.withDate) {  this.clock.css("marginLeft", ".2em") } this._clockSlider(this.date.getHours(), 0, (24 - d.stepHours), d.stepHours, "hour"); this._clockSlider(this.date.getMinutes(), 0, (60 - d.stepMins), d.stepMins, "mins"); return this }, _clockSlider: function (j, f, h, e, i) {  var k = this, c = null, d = a("<div>").addClass("ui-datetime-slider ui-datetime-slider-vertical ui-widget ui-widget-content ui-corner-all"), g = a("<a>").addClass("ui-datetime-slider-handle ui-state-default ui-corner-all").css("top", "0").appendTo(d); this.clock.find(".ui-datetime-slider-" + i).html(d); d.height(this.clock.height() - (d.parent().offset().top - this.clock.offset().top) - 6); increment = (d.height() - g.height()) / ((h - f) / e); value = (j > h) ? h : (j < f) ? f : j; g.css("top", Math.round((value - f) / e) * increment); g.mousedown(function (l) {  l.preventDefault(); a("body").css("cursor", "move"); increment = (d.height() - g.height()) / ((h - f) / e); c = l.pageY - d.offset().top - (g.height() / 2) - l.pageY; a(document).bind("mousemove.datetimeclock", function (n) {  value = Math.round((c + n.pageY) / increment) * e + f; value = (value > h) ? h : (value < f) ? f : value; g.css("top", (value - f) / e * increment); k._clockUpdate(i, value) }).bind("mouseup.datetimeclock", function (m) {  a("body").css("cursor", "auto"); a(document).unbind(".datetimeclock"); k._timeUpdate(i, value) }) }); return d }, _clockUpdate: function (d, c) {  c = ("0" + c).substrOffset(-2); this.clock.find(".ui-datetime-time-" + d).html(c) }, _timeUpdate: function (c, d) {  if (c == "hour") {  this.date.setHours(d) } else {  if (c == "mins") {  this.date.setMinutes(d) } } date = new Date(this.current); date.setHours(this.date.getHours()); date.setMinutes(this.date.getMinutes()); this._value(date) }, _events: function () {  var c = this; if (!this.inline) {  this.element.bind("click", function () {  c.show() }) } }, _update: function () {  if (!this.options.value) {  return true } var c = new Date(this.current); if (this.options.altField) {  a(this.options.altField).val(c.format(this.options.altFormat)) } if (this.tag == "input") {  this.element.val(c.format(this.options.format)) } if (this.chainedTo) {  this.chainedTo.datetime("option", {  minDate: this.options.value }) } if (this.chainedFrom) {  this.chainedFrom.datetime("option", {  maxDate: this.options.value }) } this._trigger("change", null, {  value: this.options.value }); return this }, _value: function (c) {  this.current = c.getTime(); this.options.value = c.format(this.options.format); this._update()._calendar()._clock(); return this }, _clean: function (c) {  if (typeof c != "object") {  c = new Date(c) } return new Date(c.getFullYear(), c.getMonth(), c.getDate(), 0, 0, 0, 0) }, _parse: function (c, d) {  if (c.constructor == Date) {  return c } return Date.strtodate(c, d) }, value: function (c) {  if (c !== b) {  this.date = this._parse(c, this.options.format); this._limits(); return this._value(this.date) } return new Date(this.current).format(this.options.format) }, timestamp: function () {  return this.current }, show: function () {  var c = this; if (this.option("disabled")) {  return } this.container.fadeIn(); if ((!this.inline && !this.ready) || (this.chainedFrom && !this.ready)) {  this._clock() } a(document).bind("mousedown.datetimecalendar", function (d) {  if (a(d.target).parents(".ui-datetime").length == 0) {  c.hide() } }).bind("keydown.datetimecalendar", function (d) {  if (d.which == 27) {  c.hide() } }); this._calendar()._clock(); this._trigger("show", null, {  value: this.options.value }); this.ready = true }, hide: function () {  a(document).unbind(".datetimecalendar"); this.container.fadeOut("fast"); this._trigger("hide", null, {  value: this.options.value }) }, disable: function () {  a.Widget.prototype.disable.apply(this, arguments); this.element.attr("disabled", "disabled"); this.container.hide() }, enable: function () {  a.Widget.prototype.enable.apply(this, arguments); this.element.attr("disabled", ""); if (this.inline) {  this.container.show() } }, destroy: function () {  a.Widget.prototype.destroy.apply(this, arguments); if (this.chainedTo && arguments[0]["unchain"] !== false) {  this.chainedTo.datetime("destroy") } if (this.chainedFrom && arguments[0]["unchain"] !== false) {  this.chainedFrom.datetime("destroy") } this.container.remove() }, _setOption: function (c, d) {  a.Widget.prototype._setOption.apply(this, arguments); switch (c) {  case "value": this.value(d); break; case "format": this._update(); break; case "showWeek": this._calendar(); break; case "minDate": case "maxDate": this._limits(); this._calendar(); break } return this } }); a.extend(Date, {  W3CDTF: "yy-mm-dd hh:ii", ISO8601: "yy-mm-dd hh:ii O", RFC822: "D, d M yy hh:ii", RFC1123: "D, d M yy hh:ii", RFC2822: "D, d M yy hh:ii", RFC1036: "D, d M y hh:ii", RFC850: "DD, dd-M-y hh:ii", USASCII: "mm/dd/yy g:ii A", dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], dayNamesShort: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], dayNamesMin: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"], monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], strtodate: function (e, f) {  var d = new Date(), c = {  yyyy: [4, "FullYear"], MM: [Date.monthNames, "Month"], M: [Date.monthNamesShort, "Month"], DD: [Date.dayNames, "Day"], D: [Date.dayNamesShort, "Day"], mm: [2, "Month"], dd: [2, "Date"], hh: [2, "Hours"], ii: [2, "Minutes"], m: [1, "Month", 12], d: [1, "Date", 31], h: [1, "Hours", 59], i: [1, "Minutes", 59] }; f = f.replace(new RegExp("('[^']*)?(yy|')", "g"), function (h, g) {  return g ? h : "yyyy" }); a.each(c, function (k, g) {  var n = 0, m = 0, h = 0, j = false, l = 0; while ((n = f.indexOf(k, ((n) ? n + 1 : 0))) > -1) {  j = false, l = 0; f = f.replace(new RegExp("('[^']*)?(" + k + "|')", "g"), function (o, i) {  if (i && (hide = o.replace(k, k.replace(/./g, "?"))) != o) {  j = true } return i ? hide : (!j && (l += 1) == 1) ? o.replace(/./g, "?") : o }); if (j) {  continue } m = n - f.substrCount("'", 0, n); if (typeof g[0] === "object") {  a.each(g[0], function (p, o) {  if (e.indexOf(o) > -1) {  e = e.replace("/" + o + "/g", k); d["set" + g[1]](p) } }) } else {  if (typeof g[0] === "number") {  if (g[0] == 1) {  h = parseInt(e.substr(m, 2).replace(/[^0-9]/g, ""), 10); if (h > g[2]) {  h = parseInt(e.substr(m, 1).replace(/[^0-9]/g, ""), 10) } } else {  h = parseInt(e.substr(m, g[0]).replace(/[^0-9]/g, ""), 10) } e = e.splice(m, Math.max(h.toString().length, g[0]), k); if (!isNaN(h)) {  d["set" + g[1]]((g[1] == "Month") ? h - 1 : h) } else {  d["set" + g[1]](0) } } } } }); d.setSeconds(0, 0); return d }, strtotime: function (c, d) {  return Date.strtodate(c, d).getTime() } }); a.extend(Date.prototype, {  format: function (d) {  var c = this; d = d.replace(new RegExp("('[^']*)?((yy|y|mm|m|MM|M|dd|d|DD|D|hh|h|gg|g|ii|i|a|A|O)|')", "g"), function (f, e) {  if (!e) {  switch (f) {  case "yy": return c.getFullYear(); case "y": return c.getShortYear(); case "mm": return ("0" + (c.getMonth() + 1)).substrOffset(-2); case "m": return c.getMonth() + 1; case "MM": return Date.monthNames[c.getMonth()]; case "M": return Date.monthNamesShort[c.getMonth()]; case "dd": return ("0" + c.getDate()).substrOffset(-2); case "d": return c.getDate(); case "DD": return Date.dayNames[c.getDay()]; case "D": return Date.dayNamesShort[c.getDay()]; case "hh": return ("0" + c.getHours()).substrOffset(-2); case "h": return c.getHours(); case "gg": return ("0" + c.get12Hours()).substrOffset(-2); case "g": return c.get12Hours(); case "ii": return ("0" + c.getMinutes()).substrOffset(-2); case "i": return c.getMinutes(); case "a": return c.getMeridiem(); case "A": return c.getMeridiem().toUpperCase(); case "O": return c.getUTCTimezone() } } return f }); return d.replace(/'/g, "") }, setDay: function (c) {  if (c < 7 && this.getDay() != c) {  diff = c - this.getDay(); if (diff > 0) {  return this.setDate(this.getDate() + diff) } return this.setDate(this.getDate() + diff) } }, setShortYear: function (d, c) {  var e = new Date().getFullYear().toString().substr(0, 2); e = (parseInt(d) <= parseInt(c)) ? e : parseInt(e) - 1; this.setFullYear(e + d) }, setMeridiem: function (c) {  }, get12Hours: function () {  return (this.getHours() == 0) ? 12 : (this.getHours() > 12) ? this.getHours() - 12 : this.getHours() }, getMeridiem: function () {  return (this.getHours() >= 12) ? "pm" : "am" }, getShortYear: function () {  return this.getFullYear().toString().substrOffset(-2) }, getWeek: function () {  var c = new Date(this.getFullYear(), 0, 1); first = (c.getDay() == 0 || c.getDay() > 4) ? 0 : 1; return Math.ceil(this.getOrdinal() / 7) + first }, getOrdinal: function () {  var c = new Date(this.getFullYear(), 0, 1); return Math.ceil((this.getTime() - c.getTime()) / 86400000) }, getUTCTimezone: function () {  offset = this.getTimezoneOffset(); mins = ("0" + (offset % 60 * -1)).substrOffset(-2); hours = ("0" + ((offset - mins) / 60 * -1)).substrOffset(-2); return "+" + hours + mins } }); a.extend(String.prototype, {  splice: function (c, e, d) {  return this.substr(0, c) + d + this.substr(c + e) }, substrCount: function (g, h, f) {  var c = 0, e = 0, d = this.substr(h, f); while ((c = d.indexOf(g, c) + 1) > 0) {  e++ } return e }, substrOffset: function (c) {  return this.substr(this.length + c, this.length) } }) })(jQuery);

/* CKeditor */
var CKEDITOR_BASEPATH = "{Request.baseUrl}/static-resources/Shared/js/ckeditor/";
{IncludeStaticResource( '/Shared/js/ckeditor/ckeditor.js' )}
{IncludeStaticResource( '/Shared/js/ckeditor/adapters/jquery.js' )}
{IncludeStaticResource( '/Shared/js/ckfinder/ckfinder.js' )}

/*

	jQuery Tags Input Plugin 1.3.3
	
	Copyright (c) 2011 XOXCO, Inc
	
	Documentation for this plugin lives here:
	http://xoxco.com/clickable/jquery-tags-input
	
	Licensed under the MIT license:
	http://www.opensource.org/licenses/mit-license.php

	ben@xoxco.com

*/
; (function(a){ var b=new Array;var c=new Array;a.fn.doAutosize=function(b){ var c=a(this).data("minwidth"),d=a(this).data("maxwidth"),e="",f=a(this),g=a("#"+a(this).data("tester_id"));if(e===(e=f.val())){ return}var h=e.replace(/&/g,"&").replace(/\s/g," ").replace(/</g,"<").replace(/>/g,">");g.html(h);var i=g.width(),j=i+b.comfortZone>=c?i+b.comfortZone:c,k=f.width(),l=j<k&&j>=c||j>c&&j<d;if(l){ f.width(j)}};a.fn.resetAutosize=function(b){ var c=a(this).data("minwidth")||b.minInputWidth||a(this).width(),d=a(this).data("maxwidth")||b.maxInputWidth||a(this).closest(".tagsinput").width()-b.inputPadding,e="",f=a(this),g=a("<tester/>").css({ position:"absolute",top:-9999,left:-9999,width:"auto",fontSize:f.css("fontSize"),fontFamily:f.css("fontFamily"),fontWeight:f.css("fontWeight"),letterSpacing:f.css("letterSpacing"),whiteSpace:"nowrap"}),h=a(this).attr("id")+"_autosize_tester";if(!a("#"+h).length>0){ g.attr("id",h);g.appendTo("body")}f.data("minwidth",c);f.data("maxwidth",d);f.data("tester_id",h);f.css("width",c)};a.fn.addTag=function(d,e){ e=jQuery.extend({ focus:false,callback:true},e);this.each(function(){ var f=a(this).attr("id");var g=a(this).val().split(b[f]);if(g[0]==""){ g=new Array}d=jQuery.trim(d);if(e.unique){ var h=a(g).tagExist(d);if(h==true){ a("#"+f+"_tag").addClass("not_valid")}}else{ var h=false}if(d!=""&&h!=true){ a("<span>").addClass("tag").append(a("<span>").text(d).append("  "),a("<a>",{ href:"#",title:"Removing tag",text:"x"}).click(function(){ return a("#"+f).removeTag(escape(d))})).insertBefore("#"+f+"_addTag");g.push(d);a("#"+f+"_tag").val("");if(e.focus){ a("#"+f+"_tag").focus()}else{ a("#"+f+"_tag").blur()}a.fn.tagsInput.updateTagsField(this,g);if(e.callback&&c[f]&&c[f]["onAddTag"]){ var i=c[f]["onAddTag"];i.call(this,d)}if(c[f]&&c[f]["onChange"]){ var j=g.length;var i=c[f]["onChange"];i.call(this,a(this),g[j-1])}}});return false};a.fn.removeTag=function(d){ d=unescape(d);this.each(function(){ var e=a(this).attr("id");var f=a(this).val().split(b[e]);a("#"+e+"_tagsinput .tag").remove();str="";for(i=0;i<f.length;i++){ if(f[i]!=d){ str=str+b[e]+f[i]}}a.fn.tagsInput.importTags(this,str);if(c[e]&&c[e]["onRemoveTag"]){ var g=c[e]["onRemoveTag"];g.call(this,d)}});return false};a.fn.tagExist=function(b){ return jQuery.inArray(b,a(this))>=0};a.fn.importTags=function(b){ id=a(this).attr("id");a("#"+id+"_tagsinput .tag").remove();a.fn.tagsInput.importTags(this,b)};a.fn.tagsInput=function(d){ var e=jQuery.extend({ interactive:true,defaultText:"add a tag",minChars:0,width:"300px",height:"100px",autocomplete:{ selectFirst:false},hide:true,delimiter:",",unique:true,removeWithBackspace:true,placeholderColor:"#666666",autosize:true,comfortZone:20,inputPadding:6*2},d);this.each(function(){ if(e.hide){ a(this).hide()}var d=a(this).attr("id");if(!d||b[a(this).attr("id")]){ d=a(this).attr("id","tags"+(new Date).getTime()).attr("id")}var f=jQuery.extend({ pid:d,real_input:"#"+d,holder:"#"+d+"_tagsinput",input_wrapper:"#"+d+"_addTag",fake_input:"#"+d+"_tag"},e);b[d]=f.delimiter;if(e.onAddTag||e.onRemoveTag||e.onChange){ c[d]=new Array;c[d]["onAddTag"]=e.onAddTag;c[d]["onRemoveTag"]=e.onRemoveTag;c[d]["onChange"]=e.onChange}var g='<div id="'+d+'_tagsinput" class="tagsinput"><div id="'+d+'_addTag">';if(e.interactive){ g=g+'<input id="'+d+'_tag" value="" data-default="'+e.defaultText+'" />'}g=g+'</div><div class="tags_clear"></div></div>';a(g).insertAfter(this);a(f.holder).css("width",e.width);a(f.holder).css("height",e.height);if(a(f.real_input).val()!=""){ a.fn.tagsInput.importTags(a(f.real_input),a(f.real_input).val())}if(e.interactive){ a(f.fake_input).val(a(f.fake_input).attr("data-default"));a(f.fake_input).css("color",e.placeholderColor);a(f.fake_input).resetAutosize(e);a(f.holder).bind("click",f,function(b){ a(b.data.fake_input).focus()});a(f.fake_input).bind("focus",f,function(b){ if(a(b.data.fake_input).val()==a(b.data.fake_input).attr("data-default")){ a(b.data.fake_input).val("")}a(b.data.fake_input).css("color","#000000")});if(e.autocomplete_url!=undefined){ autocomplete_options={ source:e.autocomplete_url};for(attrname in e.autocomplete){ autocomplete_options[attrname]=e.autocomplete[attrname]}if(jQuery.Autocompleter!==undefined){ a(f.fake_input).autocomplete(e.autocomplete_url,e.autocomplete);a(f.fake_input).bind("result",f,function(b,c,f){ if(c){ a("#"+d).addTag(c[0]+"",{ focus:true,unique:e.unique})}})}else if(jQuery.ui.autocomplete!==undefined){ a(f.fake_input).autocomplete(autocomplete_options);a(f.fake_input).bind("autocompleteselect",f,function(b,c){ a(b.data.real_input).addTag(c.item.value,{ focus:true,unique:e.unique});return false})}}else{ a(f.fake_input).bind("blur",f,function(b){ var c=a(this).attr("data-default");if(a(b.data.fake_input).val()!=""&&a(b.data.fake_input).val()!=c){ if(b.data.minChars<=a(b.data.fake_input).val().length&&(!b.data.maxChars||b.data.maxChars>=a(b.data.fake_input).val().length))a(b.data.real_input).addTag(a(b.data.fake_input).val(),{ focus:true,unique:e.unique})}else{ a(b.data.fake_input).val(a(b.data.fake_input).attr("data-default"));a(b.data.fake_input).css("color",e.placeholderColor)}return false})}a(f.fake_input).bind("keypress",f,function(b){ if(b.which==b.data.delimiter.charCodeAt(0)||b.which==13){ b.preventDefault();if(b.data.minChars<=a(b.data.fake_input).val().length&&(!b.data.maxChars||b.data.maxChars>=a(b.data.fake_input).val().length))a(b.data.real_input).addTag(a(b.data.fake_input).val(),{ focus:true,unique:e.unique});a(b.data.fake_input).resetAutosize(e);return false}else if(b.data.autosize){ a(b.data.fake_input).doAutosize(e)}});f.removeWithBackspace&&a(f.fake_input).bind("keydown",function(b){ if(b.keyCode==8&&a(this).val()==""){ b.preventDefault();var c=a(this).closest(".tagsinput").find(".tag:last").text();var d=a(this).attr("id").replace(/_tag$/,"");c=c.replace(/[\s]+x$/,"");a("#"+d).removeTag(escape(c));a(this).trigger("focus")}});a(f.fake_input).blur();if(f.unique){ a(f.fake_input).keydown(function(b){ if(b.keyCode==8||String.fromCharCode(b.which).match(/\w+|[áéíóúÁÉÍÓÚñÑ,/]+/)){ a(this).removeClass("not_valid")}})}}});return this};a.fn.tagsInput.updateTagsField=function(c,d){ var e=a(c).attr("id");a(c).val(d.join(b[e]))};a.fn.tagsInput.importTags=function(d,e){ a(d).val("");var f=a(d).attr("id");var g=e.split(b[f]);for(i=0;i<g.length;i++){ a(d).addTag(g[i],{ focus:false,callback:false})}if(c[f]&&c[f]["onChange"]){ var h=c[f]["onChange"];h.call(d,d,g[i])}}})(jQuery);

/* initialize controls */
;(function ($, undefined) {
	
	/* button fields */
	$(".btn").click(function(event) {
		event.preventDefault();
		var action = $(this).attr("data-action");
		var form = $(this).closest("form");
		var actionField = form.find(".action");
		actionField.val(action);
		form.submit();
	});
	
	// initialize the date selectors
	$("input.date").datepicker({
		dateFormat: "d MM yy",
		changeMonth: true,
		changeYear: true,
		howOtherMonths: true,
		selectOtherMonths: true,
		showWeek: true,
		numberOfMonths: 3,
		showButtonPanel: true,
		yearRange: 'c-100:c+20'
	});
	
	// initialize the date time selectors
	$("input.datetime").datetime();

	// initialize the node tree
	$(".node-tree").treeview({
	});
	
	// initialize tags
	$(".tags").each(function() {
		var params = { };
		params.height = "auto";
		params.width = "auto";
		params.autocomplete_url = $(this).attr("data-autocomplete-url");
		
		$(this).tagsInput(params);
	});
	
	/* initialize upload */
	$(".upload").each(function() {
		var containerElement = $(this);
		var containerId = containerElement.attr("id");
		var valueElement = containerElement.find("#" + containerId + "-value");
		var uploadFieldElement = containerElement.find("#" + containerId + "-upload");
		var clearButtonElement = containerElement.find("#" + containerId + "-clear");
		var previewElement = containerElement.find("#" + containerId + "-preview");
		// attach click event handler
		clearButtonElement.click(function(event) {
			valueElement.val("");
			uploadFieldElement.val("");
			previewElement.attr("href", "#");
			previewElement.addClass("hidden");
			clearButtonElement.addClass("hidden");
			event.preventDefault();
		});
	});

	/* initialize image upload */
	$(".image-upload").each(function() {
		var containerElement = $(this);
		var containerId = containerElement.attr("id");
		var valueElement = containerElement.find("#" + containerId + "-value");
		var uploadFieldElement = containerElement.find("#" + containerId + "-upload");
		var clearButtonElement = containerElement.find("#" + containerId + "-clear");
		var previewElement = containerElement.find("#" + containerId + "-preview");
		// attach click event handler
		clearButtonElement.click(function(event) {
			valueElement.val("");
			uploadFieldElement.val("");
			previewElement.attr("width", 0);
			previewElement.attr("height", 0);
			previewElement.addClass("hidden");
			clearButtonElement.addClass("hidden");
			event.preventDefault();
		});
	});
	
	/* initialize node selectors */
	$(".node-selector").each(function() {
		var containerElement = $(this);
		var containerId = containerElement.attr("id");
		var updateListener = "nodetree" + containerId + ".update";
		var valueElement = containerElement.find("#" + containerId + "-value");
		var labelElement = containerElement.find("#" + containerId + "-label");
		var clearButtonElement = containerElement.find("#" + containerId + "-clear");
		$(document).bind(updateListener, function(event, values, labels) {
			$.fancybox.close();
			valueElement.val(values);
			labelElement.text(labels);
		});
		clearButtonElement.click(function(event) {
			valueElement.val("");
			labelElement.text("");
			event.preventDefault();
		});
	});
	
	/* initialize html editor fields */
	$("textarea.rich-text").ckeditor(function () { }, {
		toolbar: "Full",
		toolbar_Full: [
			{ name: "clipboard", items : [ "Cut","Copy","Paste","PasteText","PasteFromWord","-","Undo","Redo" ] },
			{ name: "editing", items : [ "Find","Replace","-","SelectAll","-","SpellChecker", "Scayt" ] },
			{ name: "basicstyles", items : [ "Bold","Italic","Underline","Strike","Subscript","Superscript","-","RemoveFormat" ] },
			{ name: "paragraph", items : [ "NumberedList","BulletedList","-","Outdent","Indent","-","Blockquote","CreateDiv","-","JustifyLeft","JustifyCenter","JustifyRight","JustifyBlock","-","BidiLtr","BidiRtl" ] },
			{ name: "links", items : [ "Link","Unlink","Anchor" ] },
			{ name: "insert", items : [ "Image","Flash","Table","HorizontalRule","Smiley","SpecialChar","PageBreak","Iframe" ] },
			{ name: "colors", items : [ "TextColor","BGColor" ] },
			{ name: "tools", items : [ "Maximize", "ShowBlocks" ] }
		],
		extraPlugins: "internpage",
		internpage: {
			pageArrayUrl: "{RouteUrlWithArea( 'Controls', 'Async', 'GetPages', '1' )}",
			baseUrl: "{Request.baseUrl}/"
		}
	});
	CKFinder.setupCKEditor( null, {
		basePath: "{StaticResourcePathUrl( 'Shared/Js/Libs/ckfinder' )}"
	});
	
})(window.jQuery);