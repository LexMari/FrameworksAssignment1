const settings = {
    validClass: "is-valid",
    errorClass: "is-invalid"
};
$.validator.setDefaults(settings);
$.validator.unobtrusive.options = settings;

$(document).ready(function () {
    configureBoostrapValidation();
});

function configureBoostrapValidation() {
    $("span.field-validation-valid, span.field-validation-error").each(function () {
        $(this).addClass("text-danger");
    });

    $("form").submit(function () {
        if ($(this).valid()) {
            $(this).find("div.form-group").each(function () {
                if ($(this).find("span.field-validation-error").length == 0) {
                    $(this).removeClass("is-invalid");
                }
            });
        }
        else {
            $(this).find("div.form-group").each(function () {
                if ($(this).find("span.field-validation-error").length > 0) {
                    $(this).addClass("is-invalid");
                }
            });
        }
    });
    $("form").each(function () {
        $(this).find("div.form-group").each(function() {
            $(this).find(".input-validation-error").each(function() {
                $(this).addClass("is-invalid")
                    .removeClass("is-valid");
            });
        });
    });
}