(function() {

    if (typeof DayPilot === 'undefined') {
        window.DayPilot = {};
    }

    if (typeof DayPilot.Splitter !== 'undefined') {
        return;
    }

    var Util = {};

    // returns pageX, pageY (calculated from clientX if pageX is not available)
    Util.page = function(ev) {
        ev = ev || window.event;
        if (typeof ev.pageX !== 'undefined') {
            return { x: ev.pageX, y: ev.pageY };
        }
        if (typeof ev.clientX !== 'undefined') {
            return {
                x: ev.clientX + document.body.scrollLeft + document.documentElement.scrollLeft,
                y: ev.clientY + document.body.scrollTop + document.documentElement.scrollTop
            };
        }
        // shouldn't happen
        return null;
    };

    // register event
    Util.re = function(el, ev, func) {
        if (el.addEventListener) {
            el.addEventListener(ev, func, false);
        } else if (el.attachEvent) {
            el.attachEvent("on" + ev, func);
        }
    };

    var Splitter = function(id) {
        var This = this;

        this.id = id;
        //this.count = 3;
        this.widths = [];
        this.titles = [];
        this.height = 20;
        this.splitterWidth = 3;
        this.color = "#000000";
        this.opacity = 60;
        this.padding = '0px 2px 0px 2px';

        // internal
        this.blocks = [];
        this.drag = {};

        // callback
        this.updated = function() { };

        this.Init = function() {
            var div;

            if (!id) {
                throw "error: id not provided";
            }
            else if (typeof id === 'string') {
                div = document.getElementById(id);
            }
            else if (typeof id.appendChild === 'function') {
                div = id;
            }
            else {
                throw "error: invalid object provided";
            }
            /*
            if (this.widths.length == 0) {  // default widths
            for (var i = 0; i < this.count; i++) {
            this.widths[i] = 10;
            }
            }
            */

            this.div = div;
            this.blocks = [];

            for (var i = 0; i < this.widths.length; i++) {
                var s = document.createElement("div");
                s.style.display = "inline-block";
                s.style.height = this.height + "px";
                s.style.width = (this.widths[i] - this.splitterWidth) + "px";
                s.style.display.overflow = 'hidden';
                s.style.verticalAlign = "top";
                s.setAttribute("unselectable", "on");
                //s.innerHTML = this.titles[i];
                div.appendChild(s);

                var inner = document.createElement("div");
                inner.innerHTML = this.titles[i];
                inner.style.verticalAlign = "baseline";
                inner.style.height = this.height + "px";
                inner.style.padding = this.padding;
                inner.setAttribute("unselectable", "on");
                s.appendChild(inner);

                var handle = document.createElement("div");
                handle.style.display = "inline-block";
                handle.style.height = this.height + "px";
                handle.style.width = this.splitterWidth + "px";
                handle.style.backgroundColor = this.color;
                if (this.opacity >= 0 && this.opacity <= 100) {
                    handle.style.opacity = this.opacity / 100;
                    handle.style.filter = "alpha(opacity=" + this.opacity + ")";
                }
                handle.style.cursor = "col-resize";
                handle.setAttribute("unselectable", "on");

                var data = {};
                data.index = i;
                data.width = this.widths[i];

                handle.data = data;

                handle.onmousedown = function(ev) {
                    This.drag.start = Util.page(ev);
                    This.drag.data = this.data;
                    This.div.style.cursor = "col-resize";
                    document.body.style.cursor = "col-resize";
                    ev = ev || window.event;
                    ev.preventDefault ? ev.preventDefault() : ev.returnValue = false;
                };

                div.appendChild(handle);

                var block = {};
                block.section = s;
                block.handle = handle;
                this.blocks.push(block);
            }

            this.registerGlobalHandlers();
        }; // Init

        this.updateWidths = function() {
            for (var i = 0; i < this.blocks.length; i++) {
                this.updateWidth(i);
            }
        };

        this.updateWidth = function(i) {
            var block = this.blocks[i];
            block.section.style.width = this.widths[i] + "px";
        };

        this.totalWidth = function() {
            var t = 0;
            for (var i = 0; i < this.widths.length; i++) {
                t += this.widths[i];
            }
            return t;
        };

        this.gMouseMove = function(ev) {
            if (!This.drag.start) {
                return;
            }

            var data = This.drag.data;

            var now = Util.page(ev);
            var delta = now.x - This.drag.start.x;
            var i = data.index;

            This.widths[i] = data.width + delta;
            This.updateWidth(i);
        };

        this.gMouseUp = function(ev) {
            if (!This.drag.start) {
                return;
            }
            This.drag.start = null;
            document.body.style.cursor = "";
            This.div.style.cursor = "";

            var data = This.drag.data;

            var params = {};
            params.widths = this.widths;
            params.index = data.index;

            data.width = This.widths[data.index];

            This.updated(params);

        }

        this.registerGlobalHandlers = function() {
            Util.re(document, 'mousemove', this.gMouseMove);
            Util.re(document, 'mouseup', this.gMouseUp);
        };
    };

    DayPilot.Splitter = Splitter;
})();