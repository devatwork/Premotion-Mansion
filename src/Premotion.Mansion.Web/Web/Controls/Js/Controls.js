﻿/*
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
* jQuery timepicker addon
* By: Trent Richardson [http://trentrichardson.com]
* Version 1.0.0
* Last Modified: 02/05/2012
*
* Copyright 2012 Trent Richardson
* Dual licensed under the MIT and GPL licenses.
* http://trentrichardson.com/Impromptu/GPL-LICENSE.txt
* http://trentrichardson.com/Impromptu/MIT-LICENSE.txt
*/
; (function($){ $.ui.timepicker=$.ui.timepicker||{ };if($.ui.timepicker.version){ return}$.extend($.ui,{ timepicker:{ version:"1.0.0"}});function Timepicker(){ this.regional=[];this.regional['']={ currentText:'Now',closeText:'Done',ampm:false,amNames:['AM','A'],pmNames:['PM','P'],timeFormat:'hh:mm tt',timeSuffix:'',timeOnlyTitle:'Choose Time',timeText:'Time',hourText:'Hour',minuteText:'Minute',secondText:'Second',millisecText:'Millisecond',timezoneText:'Time Zone'};this._defaults={ showButtonPanel:true,timeOnly:false,showHour:true,showMinute:true,showSecond:false,showMillisec:false,showTimezone:false,showTime:true,stepHour:1,stepMinute:1,stepSecond:1,stepMillisec:1,hour:0,minute:0,second:0,millisec:0,timezone:'+0000',hourMin:0,minuteMin:0,secondMin:0,millisecMin:0,hourMax:23,minuteMax:59,secondMax:59,millisecMax:999,minDateTime:null,maxDateTime:null,onSelect:null,hourGrid:0,minuteGrid:0,secondGrid:0,millisecGrid:0,alwaysSetTime:true,separator:' ',altFieldTimeOnly:true,showTimepicker:true,timezoneIso8609:false,timezoneList:null,addSliderAccess:false,sliderAccessArgs:null};$.extend(this._defaults,this.regional[''])};$.extend(Timepicker.prototype,{ $input:null,$altInput:null,$timeObj:null,inst:null,hour_slider:null,minute_slider:null,second_slider:null,millisec_slider:null,timezone_select:null,hour:0,minute:0,second:0,millisec:0,timezone:'+0000',hourMinOriginal:null,minuteMinOriginal:null,secondMinOriginal:null,millisecMinOriginal:null,hourMaxOriginal:null,minuteMaxOriginal:null,secondMaxOriginal:null,millisecMaxOriginal:null,ampm:'',formattedDate:'',formattedTime:'',formattedDateTime:'',timezoneList:null,setDefaults:function(settings){ extendRemove(this._defaults,settings||{ });return this},_newInst:function($input,o){ var tp_inst=new Timepicker(),inlineSettings={ };for(var attrName in this._defaults){ var attrValue=$input.attr('time:'+attrName);if(attrValue){ try{ inlineSettings[attrName]=eval(attrValue)}catch(err){ inlineSettings[attrName]=attrValue}}}tp_inst._defaults=$.extend({ },this._defaults,inlineSettings,o,{ beforeShow:function(input,dp_inst){ if($.isFunction(o.beforeShow))return o.beforeShow(input,dp_inst,tp_inst)},onChangeMonthYear:function(year,month,dp_inst){ tp_inst._updateDateTime(dp_inst);if($.isFunction(o.onChangeMonthYear))o.onChangeMonthYear.call($input[0],year,month,dp_inst,tp_inst)},onClose:function(dateText,dp_inst){ if(tp_inst.timeDefined===true&&$input.val()!='')tp_inst._updateDateTime(dp_inst);if($.isFunction(o.onClose))o.onClose.call($input[0],dateText,dp_inst,tp_inst)},timepicker:tp_inst});tp_inst.amNames=$.map(tp_inst._defaults.amNames,function(val){ return val.toUpperCase()});tp_inst.pmNames=$.map(tp_inst._defaults.pmNames,function(val){ return val.toUpperCase()});if(tp_inst._defaults.timezoneList===null){ var timezoneList=[];for(var i=-11;i<=12;i++)timezoneList.push((i>=0?'+':'-')+('0'+Math.abs(i).toString()).slice(-2)+'00');if(tp_inst._defaults.timezoneIso8609)timezoneList=$.map(timezoneList,function(val){ return val=='+0000'?'Z':(val.substring(0,3)+':'+val.substring(3))});tp_inst._defaults.timezoneList=timezoneList}tp_inst.hour=tp_inst._defaults.hour;tp_inst.minute=tp_inst._defaults.minute;tp_inst.second=tp_inst._defaults.second;tp_inst.millisec=tp_inst._defaults.millisec;tp_inst.ampm='';tp_inst.$input=$input;if(o.altField)tp_inst.$altInput=$(o.altField).css({ cursor:'pointer'}).focus(function(){ $input.trigger("focus")});if(tp_inst._defaults.minDate==0||tp_inst._defaults.minDateTime==0){ tp_inst._defaults.minDate=new Date()}if(tp_inst._defaults.maxDate==0||tp_inst._defaults.maxDateTime==0){ tp_inst._defaults.maxDate=new Date()}if(tp_inst._defaults.minDate!==undefined&&tp_inst._defaults.minDate instanceof Date)tp_inst._defaults.minDateTime=new Date(tp_inst._defaults.minDate.getTime());if(tp_inst._defaults.minDateTime!==undefined&&tp_inst._defaults.minDateTime instanceof Date)tp_inst._defaults.minDate=new Date(tp_inst._defaults.minDateTime.getTime());if(tp_inst._defaults.maxDate!==undefined&&tp_inst._defaults.maxDate instanceof Date)tp_inst._defaults.maxDateTime=new Date(tp_inst._defaults.maxDate.getTime());if(tp_inst._defaults.maxDateTime!==undefined&&tp_inst._defaults.maxDateTime instanceof Date)tp_inst._defaults.maxDate=new Date(tp_inst._defaults.maxDateTime.getTime());return tp_inst},_addTimePicker:function(dp_inst){ var currDT=(this.$altInput&&this._defaults.altFieldTimeOnly)?this.$input.val()+' '+this.$altInput.val():this.$input.val();this.timeDefined=this._parseTime(currDT);this._limitMinMaxDateTime(dp_inst,false);this._injectTimePicker()},_parseTime:function(timeString,withDate){ var regstr=this._defaults.timeFormat.toString().replace(/h{ 1,2}/ig,'(\\d?\\d)').replace(/m{ 1,2}/ig,'(\\d?\\d)').replace(/s{ 1,2}/ig,'(\\d?\\d)').replace(/l{ 1}/ig,'(\\d?\\d?\\d)').replace(/t{ 1,2}/ig,this._getPatternAmpm()).replace(/z{ 1}/ig,'(z|[-+]\\d\\d:?\\d\\d)?').replace(/\s/g,'\\s?')+this._defaults.timeSuffix+'$',order=this._getFormatPositions(),ampm='',treg;if(!this.inst)this.inst=$.datepicker._getInst(this.$input[0]);if(withDate||!this._defaults.timeOnly){ var dp_dateFormat=$.datepicker._get(this.inst,'dateFormat');var specials=new RegExp("[.*+?|()\\[\\]{ }\\\\]","g");regstr='^.{ '+dp_dateFormat.length+',}?'+this._defaults.separator.replace(specials,"\\$&")+regstr}treg=timeString.match(new RegExp(regstr,'i'));if(treg){ if(order.t!==-1){ if(treg[order.t]===undefined||treg[order.t].length===0){ ampm='';this.ampm=''}else{ ampm=$.inArray(treg[order.t].toUpperCase(),this.amNames)!==-1?'AM':'PM';this.ampm=this._defaults[ampm=='AM'?'amNames':'pmNames'][0]}}if(order.h!==-1){ if(ampm=='AM'&&treg[order.h]=='12')this.hour=0;else if(ampm=='PM'&&treg[order.h]!='12')this.hour=(parseFloat(treg[order.h])+12).toFixed(0);else this.hour=Number(treg[order.h])}if(order.m!==-1)this.minute=Number(treg[order.m]);if(order.s!==-1)this.second=Number(treg[order.s]);if(order.l!==-1)this.millisec=Number(treg[order.l]);if(order.z!==-1&&treg[order.z]!==undefined){ var tz=treg[order.z].toUpperCase();switch(tz.length){ case 1:tz=this._defaults.timezoneIso8609?'Z':'+0000';break;case 5:if(this._defaults.timezoneIso8609)tz=tz.substring(1)=='0000'?'Z':tz.substring(0,3)+':'+tz.substring(3);break;case 6:if(!this._defaults.timezoneIso8609)tz=tz=='Z'||tz.substring(1)=='00:00'?'+0000':tz.replace(/:/,'');else if(tz.substring(1)=='00:00')tz='Z';break}this.timezone=tz}return true}return false},_getPatternAmpm:function(){ var markers=[],o=this._defaults;if(o.amNames)$.merge(markers,o.amNames);if(o.pmNames)$.merge(markers,o.pmNames);markers=$.map(markers,function(val){ return val.replace(/[.*+?|()\[\]{ }\\]/g,'\\$&')});return'('+markers.join('|')+')?'},_getFormatPositions:function(){ var finds=this._defaults.timeFormat.toLowerCase().match(/(h{ 1,2}|m{ 1,2}|s{ 1,2}|l{ 1}|t{ 1,2}|z)/g),orders={ h:-1,m:-1,s:-1,l:-1,t:-1,z:-1};if(finds)for(var i=0;i<finds.length;i++)if(orders[finds[i].toString().charAt(0)]==-1)orders[finds[i].toString().charAt(0)]=i+1;return orders},_injectTimePicker:function(){ var $dp=this.inst.dpDiv,o=this._defaults,tp_inst=this,hourMax=parseInt((o.hourMax-((o.hourMax-o.hourMin)%o.stepHour)),10),minMax=parseInt((o.minuteMax-((o.minuteMax-o.minuteMin)%o.stepMinute)),10),secMax=parseInt((o.secondMax-((o.secondMax-o.secondMin)%o.stepSecond)),10),millisecMax=parseInt((o.millisecMax-((o.millisecMax-o.millisecMin)%o.stepMillisec)),10),dp_id=this.inst.id.toString().replace(/([^A-Za-z0-9_])/g,'');if($dp.find("div#ui-timepicker-div-"+dp_id).length===0&&o.showTimepicker){ var noDisplay=' style="display:none;"',html='<div class="ui-timepicker-div" id="ui-timepicker-div-'+dp_id+'"><dl>'+'<dt class="ui_tpicker_time_label" id="ui_tpicker_time_label_'+dp_id+'"'+((o.showTime)?'':noDisplay)+'>'+o.timeText+'</dt>'+'<dd class="ui_tpicker_time" id="ui_tpicker_time_'+dp_id+'"'+((o.showTime)?'':noDisplay)+'></dd>'+'<dt class="ui_tpicker_hour_label" id="ui_tpicker_hour_label_'+dp_id+'"'+((o.showHour)?'':noDisplay)+'>'+o.hourText+'</dt>',hourGridSize=0,minuteGridSize=0,secondGridSize=0,millisecGridSize=0,size=null;html+='<dd class="ui_tpicker_hour"><div id="ui_tpicker_hour_'+dp_id+'"'+((o.showHour)?'':noDisplay)+'></div>';if(o.showHour&&o.hourGrid>0){ html+='<div style="padding-left: 1px"><table class="ui-tpicker-grid-label"><tr>';for(var h=o.hourMin;h<=hourMax;h+=parseInt(o.hourGrid,10)){ hourGridSize++;var tmph=(o.ampm&&h>12)?h-12:h;if(tmph<10)tmph='0'+tmph;if(o.ampm){ if(h==0)tmph=12+'a';else if(h<12)tmph+='a';else tmph+='p'}html+='<td>'+tmph+'</td>'}html+='</tr></table></div>'}html+='</dd>';html+='<dt class="ui_tpicker_minute_label" id="ui_tpicker_minute_label_'+dp_id+'"'+((o.showMinute)?'':noDisplay)+'>'+o.minuteText+'</dt>'+'<dd class="ui_tpicker_minute"><div id="ui_tpicker_minute_'+dp_id+'"'+((o.showMinute)?'':noDisplay)+'></div>';if(o.showMinute&&o.minuteGrid>0){ html+='<div style="padding-left: 1px"><table class="ui-tpicker-grid-label"><tr>';for(var m=o.minuteMin;m<=minMax;m+=parseInt(o.minuteGrid,10)){ minuteGridSize++;html+='<td>'+((m<10)?'0':'')+m+'</td>'}html+='</tr></table></div>'}html+='</dd>';html+='<dt class="ui_tpicker_second_label" id="ui_tpicker_second_label_'+dp_id+'"'+((o.showSecond)?'':noDisplay)+'>'+o.secondText+'</dt>'+'<dd class="ui_tpicker_second"><div id="ui_tpicker_second_'+dp_id+'"'+((o.showSecond)?'':noDisplay)+'></div>';if(o.showSecond&&o.secondGrid>0){ html+='<div style="padding-left: 1px"><table><tr>';for(var s=o.secondMin;s<=secMax;s+=parseInt(o.secondGrid,10)){ secondGridSize++;html+='<td>'+((s<10)?'0':'')+s+'</td>'}html+='</tr></table></div>'}html+='</dd>';html+='<dt class="ui_tpicker_millisec_label" id="ui_tpicker_millisec_label_'+dp_id+'"'+((o.showMillisec)?'':noDisplay)+'>'+o.millisecText+'</dt>'+'<dd class="ui_tpicker_millisec"><div id="ui_tpicker_millisec_'+dp_id+'"'+((o.showMillisec)?'':noDisplay)+'></div>';if(o.showMillisec&&o.millisecGrid>0){ html+='<div style="padding-left: 1px"><table><tr>';for(var l=o.millisecMin;l<=millisecMax;l+=parseInt(o.millisecGrid,10)){ millisecGridSize++;html+='<td>'+((l<10)?'0':'')+l+'</td>'}html+='</tr></table></div>'}html+='</dd>';html+='<dt class="ui_tpicker_timezone_label" id="ui_tpicker_timezone_label_'+dp_id+'"'+((o.showTimezone)?'':noDisplay)+'>'+o.timezoneText+'</dt>';html+='<dd class="ui_tpicker_timezone" id="ui_tpicker_timezone_'+dp_id+'"'+((o.showTimezone)?'':noDisplay)+'></dd>';html+='</dl></div>';$tp=$(html);if(o.timeOnly===true){ $tp.prepend('<div class="ui-widget-header ui-helper-clearfix ui-corner-all">'+'<div class="ui-datepicker-title">'+o.timeOnlyTitle+'</div>'+'</div>');$dp.find('.ui-datepicker-header, .ui-datepicker-calendar').hide()}this.hour_slider=$tp.find('#ui_tpicker_hour_'+dp_id).slider({ orientation:"horizontal",value:this.hour,min:o.hourMin,max:hourMax,step:o.stepHour,slide:function(event,ui){ tp_inst.hour_slider.slider("option","value",ui.value);tp_inst._onTimeChange()}});this.minute_slider=$tp.find('#ui_tpicker_minute_'+dp_id).slider({ orientation:"horizontal",value:this.minute,min:o.minuteMin,max:minMax,step:o.stepMinute,slide:function(event,ui){ tp_inst.minute_slider.slider("option","value",ui.value);tp_inst._onTimeChange()}});this.second_slider=$tp.find('#ui_tpicker_second_'+dp_id).slider({ orientation:"horizontal",value:this.second,min:o.secondMin,max:secMax,step:o.stepSecond,slide:function(event,ui){ tp_inst.second_slider.slider("option","value",ui.value);tp_inst._onTimeChange()}});this.millisec_slider=$tp.find('#ui_tpicker_millisec_'+dp_id).slider({ orientation:"horizontal",value:this.millisec,min:o.millisecMin,max:millisecMax,step:o.stepMillisec,slide:function(event,ui){ tp_inst.millisec_slider.slider("option","value",ui.value);tp_inst._onTimeChange()}});this.timezone_select=$tp.find('#ui_tpicker_timezone_'+dp_id).append('<select></select>').find("select");$.fn.append.apply(this.timezone_select,$.map(o.timezoneList,function(val,idx){ return $("<option />").val(typeof val=="object"?val.value:val).text(typeof val=="object"?val.label:val)}));this.timezone_select.val((typeof this.timezone!="undefined"&&this.timezone!=null&&this.timezone!="")?this.timezone:o.timezone);this.timezone_select.change(function(){ tp_inst._onTimeChange()});if(o.showHour&&o.hourGrid>0){ size=100*hourGridSize*o.hourGrid/(hourMax-o.hourMin);$tp.find(".ui_tpicker_hour table").css({ width:size+"%",marginLeft:(size/(-2*hourGridSize))+"%",borderCollapse:'collapse'}).find("td").each(function(index){ $(this).click(function(){ var h=$(this).html();if(o.ampm){ var ap=h.substring(2).toLowerCase(),aph=parseInt(h.substring(0,2),10);if(ap=='a'){ if(aph==12)h=0;else h=aph}else if(aph==12)h=12;else h=aph+12}tp_inst.hour_slider.slider("option","value",h);tp_inst._onTimeChange();tp_inst._onSelectHandler()}).css({ cursor:'pointer',width:(100/hourGridSize)+'%',textAlign:'center',overflow:'hidden'})})}if(o.showMinute&&o.minuteGrid>0){ size=100*minuteGridSize*o.minuteGrid/(minMax-o.minuteMin);$tp.find(".ui_tpicker_minute table").css({ width:size+"%",marginLeft:(size/(-2*minuteGridSize))+"%",borderCollapse:'collapse'}).find("td").each(function(index){ $(this).click(function(){ tp_inst.minute_slider.slider("option","value",$(this).html());tp_inst._onTimeChange();tp_inst._onSelectHandler()}).css({ cursor:'pointer',width:(100/minuteGridSize)+'%',textAlign:'center',overflow:'hidden'})})}if(o.showSecond&&o.secondGrid>0){ $tp.find(".ui_tpicker_second table").css({ width:size+"%",marginLeft:(size/(-2*secondGridSize))+"%",borderCollapse:'collapse'}).find("td").each(function(index){ $(this).click(function(){ tp_inst.second_slider.slider("option","value",$(this).html());tp_inst._onTimeChange();tp_inst._onSelectHandler()}).css({ cursor:'pointer',width:(100/secondGridSize)+'%',textAlign:'center',overflow:'hidden'})})}if(o.showMillisec&&o.millisecGrid>0){ $tp.find(".ui_tpicker_millisec table").css({ width:size+"%",marginLeft:(size/(-2*millisecGridSize))+"%",borderCollapse:'collapse'}).find("td").each(function(index){ $(this).click(function(){ tp_inst.millisec_slider.slider("option","value",$(this).html());tp_inst._onTimeChange();tp_inst._onSelectHandler()}).css({ cursor:'pointer',width:(100/millisecGridSize)+'%',textAlign:'center',overflow:'hidden'})})}var $buttonPanel=$dp.find('.ui-datepicker-buttonpane');if($buttonPanel.length)$buttonPanel.before($tp);else $dp.append($tp);this.$timeObj=$tp.find('#ui_tpicker_time_'+dp_id);if(this.inst!==null){ var timeDefined=this.timeDefined;this._onTimeChange();this.timeDefined=timeDefined}var onSelectDelegate=function(){ tp_inst._onSelectHandler()};this.hour_slider.bind('slidestop',onSelectDelegate);this.minute_slider.bind('slidestop',onSelectDelegate);this.second_slider.bind('slidestop',onSelectDelegate);this.millisec_slider.bind('slidestop',onSelectDelegate);if(this._defaults.addSliderAccess){ var sliderAccessArgs=this._defaults.sliderAccessArgs;setTimeout(function(){ if($tp.find('.ui-slider-access').length==0){ $tp.find('.ui-slider:visible').sliderAccess(sliderAccessArgs);var sliderAccessWidth=$tp.find('.ui-slider-access:eq(0)').outerWidth(true);if(sliderAccessWidth){ $tp.find('table:visible').each(function(){ var $g=$(this),oldWidth=$g.outerWidth(),oldMarginLeft=$g.css('marginLeft').toString().replace('%',''),newWidth=oldWidth-sliderAccessWidth,newMarginLeft=((oldMarginLeft*newWidth)/oldWidth)+'%';$g.css({ width:newWidth,marginLeft:newMarginLeft})})}}},0)}}},_limitMinMaxDateTime:function(dp_inst,adjustSliders){ var o=this._defaults,dp_date=new Date(dp_inst.selectedYear,dp_inst.selectedMonth,dp_inst.selectedDay);if(!this._defaults.showTimepicker)return;if($.datepicker._get(dp_inst,'minDateTime')!==null&&$.datepicker._get(dp_inst,'minDateTime')!==undefined&&dp_date){ var minDateTime=$.datepicker._get(dp_inst,'minDateTime'),minDateTimeDate=new Date(minDateTime.getFullYear(),minDateTime.getMonth(),minDateTime.getDate(),0,0,0,0);if(this.hourMinOriginal===null||this.minuteMinOriginal===null||this.secondMinOriginal===null||this.millisecMinOriginal===null){ this.hourMinOriginal=o.hourMin;this.minuteMinOriginal=o.minuteMin;this.secondMinOriginal=o.secondMin;this.millisecMinOriginal=o.millisecMin}if(dp_inst.settings.timeOnly||minDateTimeDate.getTime()==dp_date.getTime()){ this._defaults.hourMin=minDateTime.getHours();if(this.hour<=this._defaults.hourMin){ this.hour=this._defaults.hourMin;this._defaults.minuteMin=minDateTime.getMinutes();if(this.minute<=this._defaults.minuteMin){ this.minute=this._defaults.minuteMin;this._defaults.secondMin=minDateTime.getSeconds()}else if(this.second<=this._defaults.secondMin){ this.second=this._defaults.secondMin;this._defaults.millisecMin=minDateTime.getMilliseconds()}else{ if(this.millisec<this._defaults.millisecMin)this.millisec=this._defaults.millisecMin;this._defaults.millisecMin=this.millisecMinOriginal}}else{ this._defaults.minuteMin=this.minuteMinOriginal;this._defaults.secondMin=this.secondMinOriginal;this._defaults.millisecMin=this.millisecMinOriginal}}else{ this._defaults.hourMin=this.hourMinOriginal;this._defaults.minuteMin=this.minuteMinOriginal;this._defaults.secondMin=this.secondMinOriginal;this._defaults.millisecMin=this.millisecMinOriginal}}if($.datepicker._get(dp_inst,'maxDateTime')!==null&&$.datepicker._get(dp_inst,'maxDateTime')!==undefined&&dp_date){ var maxDateTime=$.datepicker._get(dp_inst,'maxDateTime'),maxDateTimeDate=new Date(maxDateTime.getFullYear(),maxDateTime.getMonth(),maxDateTime.getDate(),0,0,0,0);if(this.hourMaxOriginal===null||this.minuteMaxOriginal===null||this.secondMaxOriginal===null){ this.hourMaxOriginal=o.hourMax;this.minuteMaxOriginal=o.minuteMax;this.secondMaxOriginal=o.secondMax;this.millisecMaxOriginal=o.millisecMax}if(dp_inst.settings.timeOnly||maxDateTimeDate.getTime()==dp_date.getTime()){ this._defaults.hourMax=maxDateTime.getHours();if(this.hour>=this._defaults.hourMax){ this.hour=this._defaults.hourMax;this._defaults.minuteMax=maxDateTime.getMinutes();if(this.minute>=this._defaults.minuteMax){ this.minute=this._defaults.minuteMax;this._defaults.secondMax=maxDateTime.getSeconds()}else if(this.second>=this._defaults.secondMax){ this.second=this._defaults.secondMax;this._defaults.millisecMax=maxDateTime.getMilliseconds()}else{ if(this.millisec>this._defaults.millisecMax)this.millisec=this._defaults.millisecMax;this._defaults.millisecMax=this.millisecMaxOriginal}}else{ this._defaults.minuteMax=this.minuteMaxOriginal;this._defaults.secondMax=this.secondMaxOriginal;this._defaults.millisecMax=this.millisecMaxOriginal}}else{ this._defaults.hourMax=this.hourMaxOriginal;this._defaults.minuteMax=this.minuteMaxOriginal;this._defaults.secondMax=this.secondMaxOriginal;this._defaults.millisecMax=this.millisecMaxOriginal}}if(adjustSliders!==undefined&&adjustSliders===true){ var hourMax=parseInt((this._defaults.hourMax-((this._defaults.hourMax-this._defaults.hourMin)%this._defaults.stepHour)),10),minMax=parseInt((this._defaults.minuteMax-((this._defaults.minuteMax-this._defaults.minuteMin)%this._defaults.stepMinute)),10),secMax=parseInt((this._defaults.secondMax-((this._defaults.secondMax-this._defaults.secondMin)%this._defaults.stepSecond)),10),millisecMax=parseInt((this._defaults.millisecMax-((this._defaults.millisecMax-this._defaults.millisecMin)%this._defaults.stepMillisec)),10);if(this.hour_slider)this.hour_slider.slider("option",{ min:this._defaults.hourMin,max:hourMax}).slider('value',this.hour);if(this.minute_slider)this.minute_slider.slider("option",{ min:this._defaults.minuteMin,max:minMax}).slider('value',this.minute);if(this.second_slider)this.second_slider.slider("option",{ min:this._defaults.secondMin,max:secMax}).slider('value',this.second);if(this.millisec_slider)this.millisec_slider.slider("option",{ min:this._defaults.millisecMin,max:millisecMax}).slider('value',this.millisec)}},_onTimeChange:function(){ var hour=(this.hour_slider)?this.hour_slider.slider('value'):false,minute=(this.minute_slider)?this.minute_slider.slider('value'):false,second=(this.second_slider)?this.second_slider.slider('value'):false,millisec=(this.millisec_slider)?this.millisec_slider.slider('value'):false,timezone=(this.timezone_select)?this.timezone_select.val():false,o=this._defaults;if(typeof(hour)=='object')hour=false;if(typeof(minute)=='object')minute=false;if(typeof(second)=='object')second=false;if(typeof(millisec)=='object')millisec=false;if(typeof(timezone)=='object')timezone=false;if(hour!==false)hour=parseInt(hour,10);if(minute!==false)minute=parseInt(minute,10);if(second!==false)second=parseInt(second,10);if(millisec!==false)millisec=parseInt(millisec,10);var ampm=o[hour<12?'amNames':'pmNames'][0];var hasChanged=(hour!=this.hour||minute!=this.minute||second!=this.second||millisec!=this.millisec||(this.ampm.length>0&&(hour<12)!=($.inArray(this.ampm.toUpperCase(),this.amNames)!==-1))||timezone!=this.timezone);if(hasChanged){ if(hour!==false)this.hour=hour;if(minute!==false)this.minute=minute;if(second!==false)this.second=second;if(millisec!==false)this.millisec=millisec;if(timezone!==false)this.timezone=timezone;if(!this.inst)this.inst=$.datepicker._getInst(this.$input[0]);this._limitMinMaxDateTime(this.inst,true)}if(o.ampm)this.ampm=ampm;this.formattedTime=$.datepicker.formatTime(this._defaults.timeFormat,this,this._defaults);if(this.$timeObj)this.$timeObj.text(this.formattedTime+o.timeSuffix);this.timeDefined=true;if(hasChanged)this._updateDateTime()},_onSelectHandler:function(){ var onSelect=this._defaults.onSelect;var inputEl=this.$input?this.$input[0]:null;if(onSelect&&inputEl){ onSelect.apply(inputEl,[this.formattedDateTime,this])}},_formatTime:function(time,format){ time=time||{ hour:this.hour,minute:this.minute,second:this.second,millisec:this.millisec,ampm:this.ampm,timezone:this.timezone};var tmptime=(format||this._defaults.timeFormat).toString();tmptime=$.datepicker.formatTime(tmptime,time,this._defaults);if(arguments.length)return tmptime;else this.formattedTime=tmptime},_updateDateTime:function(dp_inst){ dp_inst=this.inst||dp_inst;var dt=$.datepicker._daylightSavingAdjust(new Date(dp_inst.selectedYear,dp_inst.selectedMonth,dp_inst.selectedDay)),dateFmt=$.datepicker._get(dp_inst,'dateFormat'),formatCfg=$.datepicker._getFormatConfig(dp_inst),timeAvailable=dt!==null&&this.timeDefined;this.formattedDate=$.datepicker.formatDate(dateFmt,(dt===null?new Date():dt),formatCfg);var formattedDateTime=this.formattedDate;if(dp_inst.lastVal!==undefined&&(dp_inst.lastVal.length>0&&this.$input.val().length===0))return;if(this._defaults.timeOnly===true){ formattedDateTime=this.formattedTime}else if(this._defaults.timeOnly!==true&&(this._defaults.alwaysSetTime||timeAvailable)){ formattedDateTime+=this._defaults.separator+this.formattedTime+this._defaults.timeSuffix}this.formattedDateTime=formattedDateTime;if(!this._defaults.showTimepicker){ this.$input.val(this.formattedDate)}else if(this.$altInput&&this._defaults.altFieldTimeOnly===true){ this.$altInput.val(this.formattedTime);this.$input.val(this.formattedDate)}else if(this.$altInput){ this.$altInput.val(formattedDateTime);this.$input.val(formattedDateTime)}else{ this.$input.val(formattedDateTime)}this.$input.trigger("change")}});$.fn.extend({ timepicker:function(o){ o=o||{ };var tmp_args=arguments;if(typeof o=='object')tmp_args[0]=$.extend(o,{ timeOnly:true});return $(this).each(function(){ $.fn.datetimepicker.apply($(this),tmp_args)})},datetimepicker:function(o){ o=o||{ };tmp_args=arguments;if(typeof(o)=='string'){ if(o=='getDate')return $.fn.datepicker.apply($(this[0]),tmp_args);else return this.each(function(){ var $t=$(this);$t.datepicker.apply($t,tmp_args)})}else return this.each(function(){ var $t=$(this);$t.datepicker($.timepicker._newInst($t,o)._defaults)})}});$.datepicker.formatTime=function(format,time,options){ options=options||{ };options=$.extend($.timepicker._defaults,options);time=$.extend({ hour:0,minute:0,second:0,millisec:0,timezone:'+0000'},time);var tmptime=format;var ampmName=options['amNames'][0];var hour=parseInt(time.hour,10);if(options.ampm){ if(hour>11){ ampmName=options['pmNames'][0];if(hour>12)hour=hour%12}if(hour===0)hour=12}tmptime=tmptime.replace(/(?:hh?|mm?|ss?|[tT]{ 1,2}|[lz])/g,function(match){ switch(match.toLowerCase()){ case'hh':return('0'+hour).slice(-2);case'h':return hour;case'mm':return('0'+time.minute).slice(-2);case'm':return time.minute;case'ss':return('0'+time.second).slice(-2);case's':return time.second;case'l':return('00'+time.millisec).slice(-3);case'z':return time.timezone;case't':case'tt':if(options.ampm){ if(match.length==1)ampmName=ampmName.charAt(0);return match.charAt(0)=='T'?ampmName.toUpperCase():ampmName.toLowerCase()}return''}});tmptime=$.trim(tmptime);return tmptime};$.datepicker._base_selectDate=$.datepicker._selectDate;$.datepicker._selectDate=function(id,dateStr){ var inst=this._getInst($(id)[0]),tp_inst=this._get(inst,'timepicker');if(tp_inst){ tp_inst._limitMinMaxDateTime(inst,true);inst.inline=inst.stay_open=true;this._base_selectDate(id,dateStr);inst.inline=inst.stay_open=false;this._notifyChange(inst);this._updateDatepicker(inst)}else this._base_selectDate(id,dateStr)};$.datepicker._base_updateDatepicker=$.datepicker._updateDatepicker;$.datepicker._updateDatepicker=function(inst){ var input=inst.input[0];if($.datepicker._curInst&&$.datepicker._curInst!=inst&&$.datepicker._datepickerShowing&&$.datepicker._lastInput!=input){ return}if(typeof(inst.stay_open)!=='boolean'||inst.stay_open===false){ this._base_updateDatepicker(inst);var tp_inst=this._get(inst,'timepicker');if(tp_inst)tp_inst._addTimePicker(inst)}};$.datepicker._base_doKeyPress=$.datepicker._doKeyPress;$.datepicker._doKeyPress=function(event){ var inst=$.datepicker._getInst(event.target),tp_inst=$.datepicker._get(inst,'timepicker');if(tp_inst){ if($.datepicker._get(inst,'constrainInput')){ var ampm=tp_inst._defaults.ampm,dateChars=$.datepicker._possibleChars($.datepicker._get(inst,'dateFormat')),datetimeChars=tp_inst._defaults.timeFormat.toString().replace(/[hms]/g,'').replace(/TT/g,ampm?'APM':'').replace(/Tt/g,ampm?'AaPpMm':'').replace(/tT/g,ampm?'AaPpMm':'').replace(/T/g,ampm?'AP':'').replace(/tt/g,ampm?'apm':'').replace(/t/g,ampm?'ap':'')+" "+tp_inst._defaults.separator+tp_inst._defaults.timeSuffix+(tp_inst._defaults.showTimezone?tp_inst._defaults.timezoneList.join(''):'')+(tp_inst._defaults.amNames.join(''))+(tp_inst._defaults.pmNames.join(''))+dateChars,chr=String.fromCharCode(event.charCode===undefined?event.keyCode:event.charCode);return event.ctrlKey||(chr<' '||!dateChars||datetimeChars.indexOf(chr)>-1)}}return $.datepicker._base_doKeyPress(event)};$.datepicker._base_doKeyUp=$.datepicker._doKeyUp;$.datepicker._doKeyUp=function(event){ var inst=$.datepicker._getInst(event.target),tp_inst=$.datepicker._get(inst,'timepicker');if(tp_inst){ if(tp_inst._defaults.timeOnly&&(inst.input.val()!=inst.lastVal)){ try{ $.datepicker._updateDatepicker(inst)}catch(err){ $.datepicker.log(err)}}}return $.datepicker._base_doKeyUp(event)};$.datepicker._base_gotoToday=$.datepicker._gotoToday;$.datepicker._gotoToday=function(id){ var inst=this._getInst($(id)[0]),$dp=inst.dpDiv;this._base_gotoToday(id);var now=new Date();var tp_inst=this._get(inst,'timepicker');if(tp_inst&&tp_inst._defaults.showTimezone&&tp_inst.timezone_select){ var tzoffset=now.getTimezoneOffset();var tzsign=tzoffset>0?'-':'+';tzoffset=Math.abs(tzoffset);var tzmin=tzoffset%60;tzoffset=tzsign+('0'+(tzoffset-tzmin)/60).slice(-2)+('0'+tzmin).slice(-2);if(tp_inst._defaults.timezoneIso8609)tzoffset=tzoffset.substring(0,3)+':'+tzoffset.substring(3);tp_inst.timezone_select.val(tzoffset)}this._setTime(inst,now);$('.ui-datepicker-today',$dp).click()};$.datepicker._disableTimepickerDatepicker=function(target,date,withDate){ var inst=this._getInst(target),tp_inst=this._get(inst,'timepicker');$(target).datepicker('getDate');if(tp_inst){ tp_inst._defaults.showTimepicker=false;tp_inst._updateDateTime(inst)}};$.datepicker._enableTimepickerDatepicker=function(target,date,withDate){ var inst=this._getInst(target),tp_inst=this._get(inst,'timepicker');$(target).datepicker('getDate');if(tp_inst){ tp_inst._defaults.showTimepicker=true;tp_inst._addTimePicker(inst);tp_inst._updateDateTime(inst)}};$.datepicker._setTime=function(inst,date){ var tp_inst=this._get(inst,'timepicker');if(tp_inst){ var defaults=tp_inst._defaults,hour=date?date.getHours():defaults.hour,minute=date?date.getMinutes():defaults.minute,second=date?date.getSeconds():defaults.second,millisec=date?date.getMilliseconds():defaults.millisec;if((hour<defaults.hourMin||hour>defaults.hourMax)||(minute<defaults.minuteMin||minute>defaults.minuteMax)||(second<defaults.secondMin||second>defaults.secondMax)||(millisec<defaults.millisecMin||millisec>defaults.millisecMax)){ hour=defaults.hourMin;minute=defaults.minuteMin;second=defaults.secondMin;millisec=defaults.millisecMin}tp_inst.hour=hour;tp_inst.minute=minute;tp_inst.second=second;tp_inst.millisec=millisec;if(tp_inst.hour_slider)tp_inst.hour_slider.slider('value',hour);if(tp_inst.minute_slider)tp_inst.minute_slider.slider('value',minute);if(tp_inst.second_slider)tp_inst.second_slider.slider('value',second);if(tp_inst.millisec_slider)tp_inst.millisec_slider.slider('value',millisec);tp_inst._onTimeChange();tp_inst._updateDateTime(inst)}};$.datepicker._setTimeDatepicker=function(target,date,withDate){ var inst=this._getInst(target),tp_inst=this._get(inst,'timepicker');if(tp_inst){ this._setDateFromField(inst);var tp_date;if(date){ if(typeof date=="string"){ tp_inst._parseTime(date,withDate);tp_date=new Date();tp_date.setHours(tp_inst.hour,tp_inst.minute,tp_inst.second,tp_inst.millisec)}else tp_date=new Date(date.getTime());if(tp_date.toString()=='Invalid Date')tp_date=undefined;this._setTime(inst,tp_date)}}};$.datepicker._base_setDateDatepicker=$.datepicker._setDateDatepicker;$.datepicker._setDateDatepicker=function(target,date){ var inst=this._getInst(target),tp_date=(date instanceof Date)?new Date(date.getTime()):date;this._updateDatepicker(inst);this._base_setDateDatepicker.apply(this,arguments);this._setTimeDatepicker(target,tp_date,true)};$.datepicker._base_getDateDatepicker=$.datepicker._getDateDatepicker;$.datepicker._getDateDatepicker=function(target,noDefault){ var inst=this._getInst(target),tp_inst=this._get(inst,'timepicker');if(tp_inst){ this._setDateFromField(inst,noDefault);var date=this._getDate(inst);if(date&&tp_inst._parseTime($(target).val(),tp_inst.timeOnly))date.setHours(tp_inst.hour,tp_inst.minute,tp_inst.second,tp_inst.millisec);return date}return this._base_getDateDatepicker(target,noDefault)};$.datepicker._base_parseDate=$.datepicker.parseDate;$.datepicker.parseDate=function(format,value,settings){ var date;try{ date=this._base_parseDate(format,value,settings)}catch(err){ if(err.indexOf(":")>=0){ date=this._base_parseDate(format,value.substring(0,value.length-(err.length-err.indexOf(':')-2)),settings)}else{ throw err}}return date};$.datepicker._base_formatDate=$.datepicker._formatDate;$.datepicker._formatDate=function(inst,day,month,year){ var tp_inst=this._get(inst,'timepicker');if(tp_inst){ tp_inst._updateDateTime(inst);return tp_inst.$input.val()}return this._base_formatDate(inst)};$.datepicker._base_optionDatepicker=$.datepicker._optionDatepicker;$.datepicker._optionDatepicker=function(target,name,value){ var inst=this._getInst(target),tp_inst=this._get(inst,'timepicker');if(tp_inst){ var min=null,max=null,onselect=null;if(typeof name=='string'){ if(name==='minDate'||name==='minDateTime')min=value;else if(name==='maxDate'||name==='maxDateTime')max=value;else if(name==='onSelect')onselect=value}else if(typeof name=='object'){ if(name.minDate)min=name.minDate;else if(name.minDateTime)min=name.minDateTime;else if(name.maxDate)max=name.maxDate;else if(name.maxDateTime)max=name.maxDateTime}if(min){ if(min==0)min=new Date();else min=new Date(min);tp_inst._defaults.minDate=min;tp_inst._defaults.minDateTime=min}else if(max){ if(max==0)max=new Date();else max=new Date(max);tp_inst._defaults.maxDate=max;tp_inst._defaults.maxDateTime=max}else if(onselect)tp_inst._defaults.onSelect=onselect}if(value===undefined)return this._base_optionDatepicker(target,name);return this._base_optionDatepicker(target,name,value)};function extendRemove(target,props){ $.extend(target,props);for(var name in props)if(props[name]===null||props[name]===undefined)target[name]=props[name];return target};$.timepicker=new Timepicker();$.timepicker.version="1.0.0"})(jQuery);

/* CKeditor */
var CKEDITOR_BASEPATH = "{Request.applicationUrl}/static-resources/Shared/js/ckeditor/";
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
	$("form .form-actions .btn").click(function(event) {
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
	$("input.datetime").datetimepicker({
		dateFormat: "d MM yy",
		timeFormat: "hh:mm"
		
	});

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
			{ name: "tools", items : [ "Source", "Maximize", "ShowBlocks" ] }
		],
		extraPlugins: "internpage",
		internpage: {
			pageArrayUrl: "{RouteUrlWithArea( 'Controls', 'Async', 'GetPages', '1' )}",
			baseUrl: "{Request.applicationUrl}/"
		}
	});
	CKFinder.setupCKEditor( null, {
		basePath: "{StaticResourcePathUrl( 'Shared/js/ckfinder' )}",
		connectorPath: "{MakeUrl( 'CKFinder.Connector' )}"
	});
	
})(window.jQuery);

/* ==|== Control Dialog Control ==========================================
Author: Premotion Software Solutions
========================================================================== */
; (function ($, undefined) {
	/* CONTROL DIALOG CLASS DEFINITION
	* ============================ */
	var ControlDialog = function ($element, options) {
		this.options = options;
		this.$element = $element;
		this.message = undefined;
		this.result = undefined;
	};
	ControlDialog.prototype = {
		constructor: ControlDialog,
		open: function (href, callback) {
			// create the frame
			var frame = $('<iframe class="seamless" name="modal-frame" />');
			frame.load(function() {
				this.style.height = this.contentWindow.document.body.offsetHeight + 'px';
			});

			// navigate
			frame.attr('src', href);

			// assemble dialog
			this.$element.html(frame);

			// show it
			var that = this;
			this.$element.on('hidden', function() {
				callback(that.message, that.result);
			}).modal('show');
		},
		close: function(message, result) {
			this.message = message;
			this.result = result;
			this.$element.modal('hide');
		}
	};

	/* CONTROL DIALOG PLUGIN DEFINITION
	* ============================== */
	$.fn.controlDialog = function (option) {
		var args = arguments;
		return this.each(function () {
			var $this = $(this)
				, data = $this.data('control-dialog')
				, options = $.extend({}, $.fn.controlDialog.defaults, $this.data(), typeof option == 'object' && option);
			if (!data)
				$this.data('control-dialog', (data = new ControlDialog($this, options)));
			if (typeof option == 'string')
				data[option].apply(data, Array.prototype.slice.call( args, 1 ));
		});
	};
	$.fn.controlDialog.defaults = { };
	$.fn.controlDialog.Constructor = ControlDialog;

	/* CONTROL DIALOG DATA-API
	* ===================== */
	$(function () {
		// add bindings
		var $document = $(document);
		$document.bind('control.dialog.close', function(event, controlId, message, result) {
			var $control = $('#' + controlId + ' > .modal');
			$control.controlDialog('close', message, result);
		});
	});

})(window.jQuery);


/* ==|== Node Selector Control ===========================================
Author: Premotion Software Solutions
========================================================================== */
; (function ($, undefined) {
	/* NODE SELECTOR CLASS DEFINITION
	* ============================ */
	var NodeSelector = function ($element, options) {
		this.options = options;
		this.$element = $element;
		this.$valueElement = this.$element.find('.value');
		this.$labelsElement = this.$element.find('.labels');
		this.$modal = this.$element.find('.modal');
	};
	NodeSelector.prototype = {
		constructor: NodeSelector,
		init: function() {
		},
		selectValues: function() {
			var that = this;
				href = this.$modal.data('href') + '&selected=' + this.$valueElement.val();
			this.$modal.controlDialog('open', href, function(message, selected) {
				if (message === 'selected') {
					var json = JSON.parse(selected);
					that.updateValues(json);
				}
			});
		},
		clearValues: function() {
			this.$valueElement.val('');
			this.$labelsElement.empty();
		},
		updateValues: function (json) {
			var that = this;
			that.$valueElement.val('');
			that.$labelsElement.empty();
			$.each(json, function(index, sel) {
				// append the value
				var val = that.$valueElement.val();
				if (val != null && val != '')
					val += ',';
				that.$valueElement.val(val + sel.value);

				// add a label
				that.$labelsElement.append('<li data-value="' + sel.value + '">' + sel.label + '</li>');
			});
		}
	};
	
	/* NODE SELECTOR PLUGIN DEFINITION
	* ============================== */
	$.fn.nodeSelector = function (option) {
		var args = arguments;
		return this.each(function () {
			var $this = $(this)
				, data = $this.data('node-selector')
				, options = $.extend({}, $.fn.nodeSelector.defaults, $this.data(), typeof option == 'object' && option);
			if (!data)
				$this.data('node-selector', (data = new NodeSelector($this, options)));
			if (typeof option == 'string')
				data[option].apply(data, Array.prototype.slice.call( args, 1 ));
		});
	};
	$.fn.nodeSelector.defaults = { };
	$.fn.nodeSelector.Constructor = NodeSelector;

	/* NODE SELECTOR DATA-API
	* ==================== */
	$(function () {
		$('body').on('click', "[data-behavior='node-selector'] .btn-select-values", function ( e ) {
			e.preventDefault();
			var $this = $(this)
				, $parent = $this.parents("[data-behavior='node-selector']");
			$parent.nodeSelector('selectValues');
		});
		$('body').on('click', "[data-behavior='node-selector'] .btn-clear-selection", function ( e ) {
			e.preventDefault();
			var $this = $(this)
				, $parent = $this.parents("[data-behavior='node-selector']");
			$parent.nodeSelector('clearValues');
		});
		
		// init all controls
		$("[data-behavior='node-selector']").nodeSelector('init');
	});
	
})(window.jQuery);