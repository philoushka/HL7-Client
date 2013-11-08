function SetMessageToUser(msgToUser) {
    if (msgToUser) {
        $("#msg").html(msgToUser);
    }
}
function ForceNumericOnlyInputs() {
    $("#numMsgsToSend").ForceNumericOnly();
    $("#destinationPort").ForceNumericOnly();
}

jQuery.fn.ForceNumericOnly = function () {
    return this.each(function () {
        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;
            // allow backspace, tab, delete, arrows, numbers and keypad numbers ONLY, home, and end.
            return (
                key == 8 || key == 9 || key == 190 ||
                (key >= 35 && key <= 40) ||
                (key >= 48 && key <= 57) ||
                (key >= 96 && key <= 105));
        });
    });
};