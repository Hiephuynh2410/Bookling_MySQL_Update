$(document).ready(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        var form = $(this);
        var url = form.attr('action');
        var redirectUrl = form.data('redirect-url');

        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            data: form.serialize(),
            success: function (data) {
                handleFormSubmissionResult(data, redirectUrl);
            },
            error: function () {
                handleFormSubmissionError();
            }
        });
    });

    function handleFormSubmissionResult(data, redirectUrl) {
        var messageContainer = $('#message-container');
    
        if (data.success) {
            messageContainer.removeClass('alert-danger').addClass('alert-success').text(data.message).show();
            $('form')[0].reset();
    
            setTimeout(function () {
                if (redirectUrl) {
                    window.location.href = redirectUrl;
                } else {
                    messageContainer.hide();
                }
            }, 1000);
        } else {
            messageContainer.removeClass('alert-success').addClass('alert-danger').text(data.message).show();
    
            setTimeout(function () {
                messageContainer.hide();
            }, 1000);
        }
    }
    
    function handleFormSubmissionError() {
        var messageContainer = $('#message-container');
        messageContainer.removeClass('alert-success').addClass('alert-danger').text('An error occurred. Please try again.').show();

        $('form')[0].reset();
        
        setTimeout(function () {
            messageContainer.hide();
        }, 3000);
    }
});
