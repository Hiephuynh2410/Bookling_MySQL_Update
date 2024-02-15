var timeoutInMinutes = 1;
var timeout = timeoutInMinutes * 60 * 1000;
var timeoutTimer;

function startTimeoutTimer() {
    timeoutTimer = setTimeout(function() {
        window.location.href = '/Admin/Login/Login'; 
    }, timeout);
}

function resetTimeoutTimer() {
    clearTimeout(timeoutTimer);
    startTimeoutTimer();
}

document.addEventListener('click', resetTimeoutTimer);
document.addEventListener('mousemove', resetTimeoutTimer);
document.addEventListener('keypress', resetTimeoutTimer);

startTimeoutTimer();
