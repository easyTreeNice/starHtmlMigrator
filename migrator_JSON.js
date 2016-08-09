(function() {
    function trimmed(str) {
        return !!str ? str.trim() : str;
    }
    function visualTidy() {
        function hilight(selector, colorCode) {
            colorCode = colorCode || 'pink';
            $(selector).css({ backgroundColor: colorCode });
        }
        function hide(selector) {
            $(selector).css({ display: 'none' });
        }
        // break the height-limited scrollihg div
        $('#assignedstudydata-div').attr('style', '');

        // remove inline style
        $('*[style]').attr('style', '');

        // put table header into appropriate table - for sanity
        var row = $('#assignedstudydata-tableheader > tbody > tr.header');
        var target = $('#assignedstudydata-table');
        target.prepend(['<thead>', '<tr>', row.html(), '</tr>', '</thead>'].join(''));
        row.css({ display: "none" });

        // hide clutter
        hide('#messages');
        hide('#header');
        hide('.breadcrumb');
        hide('.quick-help');
        hide('.primary-actions');
        hide('#quick-links-content');
        hide('#copyright');
        hide('.gridcontrols');
    }

    function extractFieldValue(name, fieldContainer) {
        var value;
        switch (name) {
            case 'Quality score':
                value = trimmed(fieldContainer.find('>div>input').attr('value'));
                break;
            default:
                value = trimmed(fieldContainer.find('>div').html());
                break;
        }

        return value;
    }

    function extract() {
        var studyFieldNames = [];

        function getColumnFields(td) {
            var fields = [];
            td.find('>.assignedstudyfield')
                .each(function (idx, elem) {
                    var f = $(elem);
                    var name = trimmed($(f.find('>label')).text());
                    fields.push({
                        name: name,
                        value: extractFieldValue(name, f),
                        fieldId: trimmed($(f.find('>div>*:first-child')).attr('id'))
                    });
                });
            return fields;
        }

        var colulmnTitles = (function () {
            var titles = [];
            $('#assignedstudydata-table > thead > tr > th')
                .each(function (idx, elem) {
                    titles.push(trimmed($(this).text()));
                });
            return titles;
        })();

        function arrayContains(ar, value) {
            var match = false;

            $.each(ar, function (idx, val) {
                var more = true;
                if (val.toLocaleLowerCase() === value.toLocaleLowerCase()) {
                    match = true;
                    more = false;
                }
                return more;
            });

            return match;
        }

        function getColumnFieldValue(rowColumns, columnNames, fieldName) {
            var col, value;

            if (rowColumns) {
                $.each(rowColumns, function(idx, rc) {
                    var more = true;
                    if (arrayContains(columnNames, rc.column)) {
                        col = rc;
                        more = false;
                    }
                    return more;
                });
                if (!!col) {
                    $.each(col.fields, function(idx, f) {
                        var more = true;
                        if (f.name === fieldName) {
                            var html = f.value;
                            value = $(html).text();
                            more = false;
                        }
                        return more;
                    });
                }
            }
            return value;
        }

        var studies = [];
        $('#assignedstudydata-table > tbody > tr')
            .each(function (idx, elem) {
                var tr = $(this);
                //var id = tr.attr('id');
                var tds = tr.find('>td');

                var rowColumns = (function () {
                    var fields = [];
                    $.each(tds,
                        function (idx, td) {
                            if (idx === 0) return;
                            td = $(td);
                            fields.push({
                                column: colulmnTitles[idx],
                                columnId: td.attr('id'),
                                fields: getColumnFields(td)
                            });
                        });
                    return fields;
                })();

                var id = getColumnFieldValue(rowColumns, ['Study details', 'Bibliographic details'], 'Ref Id');

                studies.push({
                    studyId: id,
                    columns: rowColumns
                });
            });

        var titleRegex = /(.*)(?:\(\))/;
        var reviewTitle = (function() {
            var html = trimmed($('#main > h2').text());
            var titleRegex = /(.*)(?:\(step [0-9]+ and [0-9]+ search(?:es)?\))?/;
            var match = titleRegex.exec(html);
            var title = !!match ? match[1].trim() : "";
            var maxLen = 255;
            if (title && title.length > maxLen) {
                title = title.substr(0, maxLen);
            }
            return title;
        })();

        var data = {
            review: {
                title: reviewTitle
            },
            studies: studies
        };
        return data;
    }

    var ta = $('<textarea></textarea>');

    function prettifyJSON(ob) {
        ta.text(JSON.stringify(ob, null, '   '));
        return ta.text();
    }

    function getCitations(data) {
        function getFullCitation(study) {
            var fc = "";

            $.each(study.columns,
                function(idx, col) {
                    if (col.column === "Study details") {
                        $.each(col.fields,
                            function(idx, field) {
                                if (field.name === "Full citation") {
                                    fc = field.value;
                                }
                            });
                    }
                });

            return fc;
        }

        var projection = [];
        $.each(data.studies, function(idx, study) {
            projection.push([
                study.studyId,
                getFullCitation(study)
            ].join (','));
        });

        return projection.join('\r\n');
    }

    function displayAsJSON(data) {
        var json = prettifyJSON(data);
        var citations = getCitations(data);
        var body = $('body');

        body.prepend([
            "<textarea id='citationOutput' class='collapsed' title='click to expand/collapse'>",
            citations,
            "</textarea>"
        ].join(''));
        body.prepend([
            "<textarea id='output' class='collapsed' title='click to expand/collapse'>",
            json,
            "</textarea>"
        ].join(''));

        //body.prepend($("<button id='copyCitationList'>Copy Citation list to clipboard</button>"));

        function toggle(target) {
            if (target.hasClass('collapsed')) {
                target.removeClass('collapsed');
            } else {
                target.addClass('collapsed');
            }
        }

        body.on('click', '#output,#citationOutput', function () {
            toggle($('#output'));
            toggle($('#citationOutput'));
        })
            //.on('click', '#copyCitationList', function () {
            //var citations = getCitations(data);
            //window.prompt("Copy to clipboard: Ctrl+C, Enter", citations);
            //})
        ;
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
        if (demo >= 2) displayAsJSON(data);
    }

    $(function () {
        $('body').prepend('<div>migrator_JSON.js loaded at !' + Date() + "</div>");
        go();
    });
}) ();
