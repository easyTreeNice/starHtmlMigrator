(function() {
    function visualTidy() {
        function hilight(selector, colorCode) {
            colorCode = colorCode || 'pink';
            $(selector).css({ backgroundColor: colorCode });
        }
        function hide(selector) {
            $(selector).css({ display: 'none' });
        }
        // break the height-limited scrollihg div
        $('#assignedstudy-div').attr('style', '');

        // remove inline style
        $('*[style]').attr('style', '');

        // put table header into appropriate table - for sanity
        var row = $('#assignedstudy-tableheader > tbody > tr.header');
        var target = $('#assignedstudy-table');
        target.prepend(['<thead>', '<tr>', row.html(), '</tr>', '</thead>'].join(''));
        row.css({ display: "none" });

        $('div.page').addClass('page-scroller');

        // hide clutter
        hide('#messages');
        hide('#header');
        hide('.breadcrumb');
        hide('.quick-help');
        hide('.primary-actions');
        hide('#quick-links-content');
        hide('#copyright');
        hide('.gridcontrols');
        hide('#fieldPicker');
        hide('#unreviewedstudies');
        hide('.status');
        hide('.bulkactions');
    }

    function extract() {
        var columnTitles = (function () {
            var titles = [];
            $('#assignedstudy-table > thead > tr > th')
                .each(function (idx, elem) {
                    titles.push($(this).text().trim());
                });
            return titles;
        })();

        function getColumnFieldValue(fields, columnName, fieldName) {
            var col, value;

            if (fields) {
                $.each(fields, function (idx, rc) {
                    var more = true;
                    if (rc.column === columnName) {
                        col = rc;
                        more = false;
                    }
                    return more;
                });
                if (!!col) {
                    value = col.value;
                }
            }
            return value;
        }

        function filterFields(fields) {
            var filtered = [];

            $.each(fields, function (idx, field) {
                switch (field.column) {
                    case "Status":
                        filtered.push({
                            column: field.value,
                            value: ""
                        });
                        break;
                }
            });
            return filtered;
        }

        var citations = [];
        var rows = $('#assignedstudy-table > tbody > tr');
        var len = rows.length;
        rows.each(function (idx, elem) {
            showProgress('extract', idx + 1, len);

            var tr = $(this);
            var tds = tr.find('>td');

            var fields = (function () {
                var values = [];
                $.each(tds, function (idx, td) {
                    if (idx === 0) return;
                    td = $(td);

                    values.push({
                        column: columnTitles[idx],
                        columnId: td.attr('id'),
                        value: td.text().trim()
                    });
                });
                return values;
            })();

            var id = getColumnFieldValue(fields, 'Star Study Id');
            citations.push({
                id: id,
                fields: filterFields(fields)
            });
        });

        var data = { citations: citations };
        return data;
    }

    var ta = $('<textarea></textarea>');

    function prettifyJSON(ob) {
        ta.text(JSON.stringify(ob, null, '   '));
        return ta.text();
    }

    var pagesRegex = /([0-9]+)(-)?([0-9]+)?/;
    var auRegex = /([^,]+),([^,]+)/g;
    function getTrimmedAuthors(str) {
        var aus = [];

        function push(m) {
            aus.push(m[1].trim() + "," + m[2].trim());
        }

        var match = auRegex.exec(str);
        while (match != null) {
            push(match);
            match = auRegex.exec(str);
        }

        return aus;
    }

    function sleepFor(sleepDuration) {
        var now = new Date().getTime();
        while (new Date().getTime() < now + sleepDuration) {}
    }

    function showProgress(msg, itemNumber, total) {
        $('title').text(msg + " - " + itemNumber + ' / ' + total);
    }

    function getRisText(data) {
        var lines = [];

        function appendLine(code, value) {
            lines.push((code || "") + (!!code ? "  - " : "") + (value || ''));
        }

        function appendRisRecord(citation) {
            var fc = "";

            appendLine('TY', 'JOUR');
            
            $.each(citation.fields, function (idx, field) {
                switch (field.column) {
                    case "Author":
                        var aus = getTrimmedAuthors(field.value);
                        $.each(aus, function (idx, au) {
                            appendLine('AU', au.trim());
                        });
                        break;
                    case "Date":
                        appendLine('PY', field.value);
                        break;
                    case "Title":
                        appendLine('TI', field.value);
                        break;
                    case "Notes":
                        appendLine('N1', field.value);
                        break;
                    case 'Star Study Id':
                        appendLine('ID', field.value);
                        break;
                    case "Status":
                        appendLine('C1', field.value);

                        // Use this opportunity to write out the id
                        //appendLine('ID', citation.id);
                        appendLine('U1', citation.id);
                        break;
                    case "Volume":  // THIS IS NOT PART OF THE HTML!!!
                        appendLine('VL', field.value);
                        break;
                    case "Number":  // THIS IS NOT PART OF THE HTML!!!
                        appendLine('IS', field.value);
                        break;
                    case "Pages":
                        var match = pagesRegex.exec(field.value);
                        if (match != null) {
                            var fromPage = match[1];

                            appendLine('SP', fromPage);

                            if (match[2] === '-') {
                                appendLine('EP', match[3]);
                            }
                        }
                        break;
                    case "Study Type":
                        appendLine('C3', field.value);
                        break;
                    case "Study Sub Type":
                        appendLine('C4', field.value);
                        break;
                    case "Status Date":
                        appendLine('C5', field.value);
                        break;
                    case "Periodical":
                        appendLine('T2', field.value);
                        break;
                    case "Abstract":
                        appendLine('AB', field.value);
                        break;
                    case "Keywords":
                        var items = field.value.split(',');
                        $.each(items, function (idx, item) {
                            appendLine('KW', item.trim());
                        });
                        break;
                    case "Reviewer comments":
                        appendLine('C6', field.value);
                        break;


                    case "Reason for exclusion":
                        // do nothing
                        break;
                }
            });

            appendLine('ER');
            appendLine();

            return fc;
        }

        var len = data.citations.length;
        $.each(data.citations, function (idx, citation) {
            showProgress('get RIS', idx + 1, len);
            appendRisRecord(citation);
        });

        var risText = lines.join('\r\n');

        return risText;
    }

    function displayOutput(data) {
        var json = prettifyJSON(data.citations);
        var body = $('body');
        var risText = getRisText(data);
        showProgress('displaying output...', 0, 1);
        body.prepend([
            "<textarea id='risOutput' class='collapsed' title='click to expand/collapse'>",
            risText,
            "</textarea>"
        ].join(''));
        body.prepend([
            "<textarea id='output' class='collapsed' title='click to expand/collapse'>",
            json,
            "</textarea>"
        ].join(''));

        function toggle(target) {
            if (target.hasClass('collapsed')) {
                target.removeClass('collapsed');
            } else {
                target.addClass('collapsed');
            }
        }

        body.on('click', '#output,#risOutput', function () {
            toggle($('#output'));
            toggle($('#risOutput'));
            toggle($('.page-scroller'));
        });
    }

    var qs = (function (a) {
        if (a == "") return {};
        var b = {};
        for (var i = 0; i < a.length; ++i) {
            var p = a[i].split('=', 2);
            if (p.length == 1)
                b[p[0]] = "";
            else
                b[p[0]] = decodeURIComponent(p[1].replace(/\+/g, " "));
        }
        return b;
    })(window.location.search.substr(1).split('&'));

    function go () {
        var demo = parseInt(qs['demo'] || "2", 10);
        if (demo >= 1) visualTidy();
        var data = extract();
        if (demo >= 2) displayOutput(data);
    }

    $(function() {
        $('body').prepend('<div>migrator_RIS.js loaded at !' + Date() + "</div>");
        go();
    });
}) ();
