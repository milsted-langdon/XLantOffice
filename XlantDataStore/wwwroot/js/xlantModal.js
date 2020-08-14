function LaunchModal(id, label, call) {
    ResetModal();
    $('#modalLabel').text(label);
    $('#modal').modal('show');
    if (!call.includes("?") && !call.endsWith("/")){
        call = call + "/";
    }
    var url = call + id;
    if (url.includes("?")){
        url = url + "&partial=true";
    }
    else {
        url = url + "?partial=true";
    }
    $.get(url, function (response) {
        $('#modalLoader').hide();
        if (response.includes("<html>"))
        {
            response = "<span>Error Occured</span>";
        }
        $('#modalPlaceholder').html(response);
        $('#modalPlaceholder').fadeIn('fast');
    })
    .done(function () {
        //once the data is rendered run some scripts to make it lovely

        //set the jquery validation to use GB date format
        $(function () {
            $.validator.methods.date = function (value, element) {
                Globalize.culture("en-GB");
                return this.optional(element) || Globalize.parseDate(value) !== null;
            }
        });

        //add loading icon to submit and save button
        $('[type=submit]').click(function () {
            $(this).button('loading');
        });
        
        //handle editable cells
        $('.edit').editable(function (value, settings) {
            var url = $(this).data('url');
            var id = this.id;
            let detail = id.split('-');
            $.post(url + "?id=" + id + "&value=" + value, function (data) {
                data = JSON.parse(data);
                if (data.Result == "Success") {
                
                }
                else{
                    alert("Data not updated");
                    console.log(data.Result);
                    console.log(data.Error);
                }
            });
            return value;
        },
        {
            tooltip: 'Click to edit...'
        });
        $('.edit_area').editable(function (value, settings) {
            var url = $(this).data('url');
            var id = this.id;
            $.post(url + "?id=" + id + "&value=" + value);
            return value;
        }, {
            type: 'textarea',
            height: '80px',
            cancel: 'Cancel',
            submit: 'OK',
            indicator: '<img src="img/indicator.gif">',
            tooltip: 'Click to edit...'
        });

    });
};

function LaunchModalString(label, s) {
    $('#modalLabel').text(label);
    $('#modal').modal('show');
    $('#modalLoader').hide();
    $('#modalPlaceholder').html(s);
    $('#modalPlaceholder').fadeIn('fast');
}

function LaunchModalDate(label, s, onClickAction) {
    $('#modalLabel').text(label);
    $('#modal').modal('show');
    $('#modalLoader').hide();
    s = s + "<br/><br/><input type='date' id='dateField' class='form-control'/><br/><br/><input class='btn btn-primary' value='Confirm' type='button' data-dismiss='modal' onclick='" + onClickAction + "'/>";
    $('#modalPlaceholder').html(s);
    $('#modalPlaceholder').fadeIn('fast');
}

function LaunchModalNumber(label, s, onClickAction) {
    $('#modalLabel').text(label);
    $('#modal').modal('show');
    $('#modalLoader').hide();
    s = s + "<br/><br/><input type='text' id='numberField' class='form-control'/><br/><br/><input class='btn btn-primary' value='Confirm' type='button' onclick='" + onClickAction + "'/>";
    $('#modalPlaceholder').html(s);
    $('#modalPlaceholder').fadeIn('fast');
}

function ResetModal() {
    $('#modalLabel').text("");
    $('#modalLoader').show();
    $('#modalPlaceholder').empty();
}


function CalculateInterest(id) {
    var date = $('#CalcDate').val();
    ResetModal();
    date = date.replace(/\//g, '-');
    $.get("../../Interest/Calculate/" + date + "/" + id, function (response) {
        $('#modalLoader').hide();
        $('#modalPlaceholder').html(response);
    });
}

