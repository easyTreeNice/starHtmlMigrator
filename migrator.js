
function visualTidy() {
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
                    value: $(f.find('>div')).text().trim()
                });
            });
        return fields;
    }

    var colulmnTitles = (function() {
        var titles = [];
        $('#assignedstudydata-table > thead > tr > th')
            .each(function(idx, elem) {
                titles.push($(this).text().trim());
            });
        return titles;
    })();

    var data = [];
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
                        fields.push({
                            column: colulmnTitles[idx],
                            fields: getColumnFields($(td))
                        });
                    });
                return fields;
            })();

            data.push({
                studyId: id,
                columns: rowColumns
            });
        });

    return data;
}

function prettifyJSON(ob) {
    var ta = $('<textarea></textarea>');
    ta.text(JSON.stringify(ob, null, '   '));
    return ta.text();
}

function display(data) {
    var json = prettifyJSON(data);
    $('body').prepend([
        "<textarea id='output' class='collapsed' title='click to expand/collapse'>",
        "Extracted data as JSON:",
        json,
        "</textarea>"
    ].join(''));
    $('body').on('click', '#output', function() {
        var output = $(this);
        if (output.hasClass('collapsed')) {
            output.removeClass('collapsed');
        } else {
            output.addClass('collapsed');
        }
    });
}

$(function () {
    visualTidy();
    var data = extract();
    display(data);
});
