var columnArray = [];
var TABLE_INSTANCE;

$(document).ready(function () {
    //set defaults for datatables
    setDataTableDefaults();
});



function setDataTableDefaults() {
    $.extend(true, $.fn.dataTable.defaults, {
        order: [],
        paging: false,
        scrollY: '60vh',
        scrollX: true,
        scrollCollapse: false,
        orderCellsTop: true,
        colReorder: true,
        autoWidth: false,
        pageLength: 25,
        dom: '<"html5buttons"B>Tfgri<"crTableBody"t>lp', 
        buttons: [
            {
                extend: 'collection',
                text: 'Export',
                buttons: [
                    {
                        extend: 'copy',
                        exportOptions: {
                            columns: [':visible'],
                            format: {
                                header: function (data, column, row) {
                                    if (data.indexOf("<span") > 0) {
                                        return data.substring(0, data.indexOf("<span"));
                                    }
                                    else {
                                        return data.toString();
                                    }
                                }
                            }
                        }
                    },
                    {
                        extend: 'csv',
                        exportOptions: {
                            columns: [':visible'],
                            format: {
                                header: function (data, column, row) {
                                    if (data.indexOf("<span") > 0) {
                                        return data.substring(0, data.indexOf("<span")).trim();
                                    }
                                    else {
                                        return data.toString().trim();
                                    }
                                }
                            }
                        }
                    },
                    {
                        extend: 'excel',
                        exportOptions: {
                            columns: [':visible'],
                            format: {
                                header: function (data, column, row) {
                                    if (data.indexOf("<span") > 0) {
                                        return data.substring(0, data.indexOf("<span")).trim();
                                    }
                                    else {

                                        return data.toString().trim();
                                    }
                                }
                            }
                        }
                    },
                    {
                        extend: 'pdf',
                        exportOptions: {
                            columns: [':visible'],
                            format: {
                                header: function (data, column, row) {
                                    if (data.indexOf("<span") > 0) {
                                        return data.substring(0, data.indexOf("<span"));
                                    }
                                    else {
                                        return data.toString();
                                    }
                                }
                            }
                        }
                    },
                    {
                        extend: 'print',
                        exportOptions: {
                            columns: [':visible'],
                            format: {
                                header: function (data, column, row) {
                                    if (data.indexOf("<span") > 0) {
                                        return data.substring(0, data.indexOf("<span"));
                                    }
                                    else {
                                        return data.toString();
                                    }
                                }
                            }
                        },
                        customize: function (win) {
                            $(win.document.body).addClass('white-bg');
                            $(win.document.body).css('font-size', '10px');

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .css('font-size', 'inherit');
                        }
                    },
                ]
            },
            {
                extend: 'colvis',
                text: 'Visibility'
            },
            {
                text: 'Reset',
                action: function (e, dt, node, config) {
                    dt.state.clear();
                    document.location.reload();
                }
            }
        ],
        "processing": true,
        "stateSave": true,
        "stateDuration": 0,
        "footerCallback": function (row, data, start, end, display) {
            var api = $(this).DataTable();
            $('.columnTotal').each(function () {
                let column = $(this).closest('td');
                let visIdx = column.index();
                let columnIndex = api.column.index('fromVisible', visIdx);
                totalColumn(api, columnIndex);
            });
        },
        initComplete: function () {
            var api = this.api();
            var state = api.state.loaded();
        
            //iterate through each column
            api.columns().indexes().flatten().each(function (i) {
                var column = api.column(i);
                var h = column.header();
                var s = $(h).attr("class");
                
                if (s.includes('hidden-col')) {
                    if (state === null) {
                        column.visible(false);
                    }
                }
            });

            $(this).data('selectedArray','');
        }
    });
    //$.fn.dataTable.moment('DD/MM/YYYY');
}

function FormatDataTables(tableClass) {
    if (tableClass == null) {
        tableClass = '.xlantDataTable';
    }
    //add additional rows
    addFunctionalRows(tableClass);

    //init table

    var table;
    if ($.fn.dataTable.isDataTable(tableClass)) {
        table = $(tableClass).DataTable();
    }
    else {
        table = $(tableClass).DataTable({

        });
    }
    
    if (columnArray.length > 0) {
        yadcf.init(table, columnArray, { filters_tr_index: 1 });
    }
    table.search('').columns().search('').draw();
}

function addFunctionalRows(tableClass) {
    let newHeader = '<tr role="row">';
    let newFooter = '<tr>';
    //foreach td in the row above
    $(tableClass).find('thead tr:last').find('th').each(function () {
        newHeader = newHeader + '<th rowspan="1" colspan="1" align="right"><i class="fa fa-filter" onClick="addTableFilterLauncher(this)"></i></th>';
        newFooter = newFooter + '<td rowspan="1" colspan="1" align="right"><i class="fa fa-calculator" onClick="totalColumnLauncher(this)"></i></td>';
    });
    //lastly
    newHeader = newHeader + '</tr>';
    newFooter = newFooter + '</tr>';

    $('thead').append(newHeader);
    $('tfoot').append(newFooter);
}

function totalColumn(table, columnIndex) {
    // Total over visible lines
    var column;
    let pageTotal = 0;
    try {
        column = table.column(columnIndex, { search: 'applied' });
        pageTotal = column.data().reduce(function (a, b) {
            //ignore HTML after text
            if (b.indexOf('<') !== -1) {
                b = b.toString().substring(0, b.indexOf('<'));
            }
            return intVal(a) + intVal(b);
        }, 0);

        //find format of numbers
        //try to work it out from the data in the cells.
        let firstValue = table.cell(0, columnIndex).data();
        var digits = 0;
        if (firstValue != undefined) {
            if (firstValue.toString().split(".")[1]) {
                digits = 2;
            }
        }
        //format the numbers to our style
        let pageTotalString = formatNumber(pageTotal, digits);
        $(column.footer()).html('<div class="columnTotal">' + pageTotalString + '</div>');
    }
    catch (err) {
        //Scroll Y creates 3 tables 1 for the header, footer and body respectively, as a consquence this errors twice
        //for each operation.  This handles it gracefully(ish) so that the code keeps running.
    }
}

function totalColumnLauncher(obj) {
    let table = $('.xlantDataTable').DataTable();
    let column = $(obj).closest('td');
    let visIdx = column.index();
    let columnIndex = table.column.index('fromVisible', visIdx);
    totalColumn(table, columnIndex);
}

function addTableFilter(table, columnIndex) {
    //get datatype
    var foundData = false;
    let i = 0;
    var isHTML = false;
    while (!foundData) {
        let cellData = table.cell(i, columnIndex).data();
        if (cellData.indexOf('>') !== -1 && cellData.indexOf('<') !== -1) {
            //most likely html so strip
            cellData = cellData.toString().substring(cellData.indexOf('>') + 1, cellData.lastIndexOf('<'));
            isHTML = true;
        }
        if (cellData === "") {
            i = i + 1;
            continue;
        }

        if (!isNaN(cellData.replace(',', ''))) {
            console.log('is a number');
            columnArray.push({ column_number: columnIndex, filter_type: "range_number", ignore_char: "," });
            foundData = true;
            continue;
        }
        if (moment(cellData, 'DD/MM/YYYY', true).isValid()) {
            console.log('is a date');
            columnArray.push({ column_number: columnIndex, filter_type: "range_date", date_format: "dd/mm/yyyy", moment_date_format: "DD/MM/YYYY", datepicker_type: "bootstrap-datepicker" });
            foundData = true;
            continue;
        }
        if (isHTML) {
            columnArray.push({ column_number: columnIndex, filter_type: "multi_select", select_type: "select2", column_data_type: "html", html_data_type: "text" });
        }
        else {
            columnArray.push({ column_number: columnIndex, filter_type: "multi_select", select_type: "select2" });
        }
        foundData = true;
    }
    if (columnArray.length > 0) {
        yadcf.init(table, columnArray, { filters_tr_index: 1 });
    }
}

function addTableFilterLauncher(obj) {
    let table = $('.xlantDataTable').DataTable();
    let column = $(obj).closest('th');
    let visIdx = column.index();
    let columnIndex = table.column.index('fromVisible', visIdx);
    addTableFilter(table, columnIndex);
}

//showing and hiding columns
function ShowColumns() {
    $('.xlantDataTable').DataTable().columns().indexes().flatten().each(function (i) {
        var column = this.column(i);
        var h = column.header();
        var s = $(h).attr("class");
        if (s.includes('hidden-col')) {
            column.visible(true);
        }
    })
}

function HideColumns() {
    $('.xlantDataTable').DataTable().columns().indexes().flatten().each(function (i) {
        var column = this.column(i);
        var h = column.header();
        var s = $(h).attr("class");
        if (s.includes('hidden-col')) {
            column.visible(false);
        }
    })
}

function intVal(i) {
        let multiplier = 1;
        if (typeof i === "string" && i.indexOf('(') === 0) {
            //is negative so alter the multiplier
            multiplier = -1;
        }
        return typeof i === 'string' ?
            i.replace(/[\£,()]/g, '') * multiplier :
            typeof i === 'number' ?
                i : 0;
}

////deal with select tables
//$('.xlantDataTable tbody').on('click', 'input[type="checkbox"]', function (e) {
//    selectDeselect($(this).val());
//    $('#selectedArray').val(selectedCheckBoxArray);
//});

//$(".xlantDataTable").on('draw.dt', function () {
//    checkThoseInArray();
//});

//function selectAll() {
//    let rows = $('.xlantDataTable').DataTable().rows({ filter: 'applied' }).every(function (rowIdx, tableLoop, rowLoop) {
//        selectDeselect(this.id());
//    });
//    checkThoseInArray();
//    $('#selectedArray').val(selectedCheckBoxArray);
//}

//function checkThoseInArray() {
//    for (var i = 0; i < selectedCheckBoxArray.length; i++) {
//        checkboxId = selectedCheckBoxArray[i];
//        $('#check-' + checkboxId).attr('checked', true);
//    }
//}

//function selectDeselect(id) {
//    var rowIndex = $.inArray(id, selectedCheckBoxArray);
//    if (rowIndex === -1) {
//        selectedCheckBoxArray.push(id);
//        $('#check-' + id).attr('checked', true)
//    }
//    else if (rowIndex !== -1) {
//        selectedCheckBoxArray.splice(rowIndex, 1);
//        $('#check-' + id).attr('checked', false)
//    }
//}

//$('input[type="checkbox"]').click(function (e) {
//    if (!lastChecked) {
//        lastChecked = this;
//        return;
//    }

//    if (e.shiftKey) {
//        var from = $('input[type="checkbox"]').index(this);
//        var to = $('input[type="checkbox"]').index(lastChecked);

//        var start = Math.min(from, to);
//        var end = Math.max(from, to) + 1;

//        $('input[type="checkbox"]').slice(start, end)
//            .filter(':not(:disabled)')
//            .prop('checked', lastChecked.checked);
//    }
//    lastChecked = this;
//});


//handle ddls
function AddDropDown(column, visibleIndex) {
    var options = [];
    var select = $('<select class="tableMultipleDropDown" multiple="multiple"><option value=""></option></select>')
        .appendTo($(".table thead tr:eq(1) th").eq(visibleIndex).empty())
        .on('change', function () {
            var selected = [];
            $(this).each(function (index, value) {
                let stringWithoutBrackets = $(this).val();
                stringWithoutBrackets = stringWithoutBrackets.toString().split("(");
                selected.push(stringWithoutBrackets[0]);
            })
            var searchString = selected.toString().split(",").join("|");
            column
                .search(searchString, true, false)
                .draw();
        });
    column.data().unique().sort().each(function (d, j) {
        //strip hyperlinks
        var t = d.toString().substring(d.indexOf('>') + 1, d.lastIndexOf('<'));
        if (t == '') {
            options.push(d);
        }
        else {
            if (!options.includes(t)) {
                options.push(t);
            }
        }
    });
    //iterate through results in order
    options = options.sort();
    let len = options.length;
    for (j = 0; j < len; j++) {
        select.append('<option value="' + options[j] + '">' + options[j] + '</option>')
    };
    $('.tableMultipleDropDown').multiselect({
        numberDisplayed: 1,
        nonSelectedText: "Select"
    });
}

function formatNumber(numberForConversion, digits) {
    var chosenFormatter = new Intl.NumberFormat('en-UK', {
        style: 'decimal',
        currency: 'GBP',
        minimumFractionDigits: digits,
        maximumFractionDigits: digits,
    });
    //Add parentheses to negative numbers
    let returnString = chosenFormatter.format(numberForConversion);
    if (numberForConversion < 0) {
        returnString = returnString.replace('-', '');
        returnString = '(' + returnString + ')';
    }
    return returnString;
}