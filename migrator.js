(function() {
    function visualTidy() {
        $('head').append('<link href="../../migrator.css" rel="stylesheet" type="text/css">');

        function hilight(selector, colorCode) {
            colorCode = colorCode || 'pink';
            $(selector).css({ backgroundColor: colorCode });
        }
        function hide(selector) {
            $(selector).css({ display: 'none' });
        }
        // break the height-limited scrollihg div
        $('#assignedstudydata-div-').attr('style', '');

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

    function extract() {
        var studyFieldNames = [];

        function getColumnFields(td) {
            var fields = [];
            td.find('>.assignedstudyfield')
                .each(function (idx, elem) {
                    var f = $(elem);
                    fields.push({
                        name: $(f.find('>label')).text().trim(),
                        value: $(f.find('>div')).text().trim(),
                        fieldId: $(f.find('>div>*:first-child')).attr('id').trim()
                    });
                });
            return fields;
        }

        var colulmnTitles = (function () {
            var titles = [];
            $('#assignedstudydata-table > thead > tr > th')
                .each(function (idx, elem) {
                    titles.push($(this).text().trim());
                });
            return titles;
        })();

        var studies = [];
        $('#assignedstudydata-table > tbody > tr')
            .each(function (idx, elem) {
                var tr = $(this);
                var id = tr.attr('id');
                var tds = tr.find('>td');

                var tdCb = tds[0];
                var tdStudyDetails = tds[1];
                var tdPopulation = tds[2];
                var tdIntervientionComparator = tds[3];
                var tdResults = tds[4];
                var tdNotes = tds[5];

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

                studies.push({
                    studyId: id,
                    columns: rowColumns
                });
            });

        var data = { studies: studies };
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
        var body = $('body');

        body.prepend([
            "<textarea id='output' class='collapsed' title='click to expand/collapse'>",
            json,
            "</textarea>"
        ].join(''));

        body.prepend($("<button id='copyCitationList'>Copy Citation list to clipboard</button>"));

        body.on('click', '#output', function () {
            var output = $(this);
            if (output.hasClass('collapsed')) {
                output.removeClass('collapsed');
            } else {
                output.addClass('collapsed');
            }
        }).on('click', '#copyCitationList', function () {
            var citations = getCitations(data);
            window.prompt("Copy to clipboard: Ctrl+C, Enter", citations);
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
        if (demo >= 2) displayAsJSON(data);
    }

    $(function() {
        go();
    });
}) ();
