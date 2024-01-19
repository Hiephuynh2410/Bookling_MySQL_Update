$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Admin/Product/Create',
            type: 'POST',
            dataType: 'json',
            data: $('form').serialize(),
            success: function (data) {
                if (data.success) {
                    $('#message-container').removeClass('alert-danger').addClass('alert-success').text(data.message).show();
                    $('form')[0].reset();
                } else {
                    $('#message-container').removeClass('alert-success').addClass('alert-danger').text(data.message).show();
                }

                setTimeout(function () {
                    if (data.success) {
                        window.location.href = '/Admin/Product/Index';
                    }
                }, 3000);
            },
            error: function () {
                $('#message-container').removeClass('alert-success').addClass('alert-danger').text('Please Enter Fill Again').show();
                $('form')[0].reset();
                setTimeout(function () {
                }, 3000);
            }
        });
    });
});
