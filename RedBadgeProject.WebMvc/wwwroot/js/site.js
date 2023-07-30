// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('#passwordInput').on('click', function () {
        showPasswordRequirements();
    });

    $('#registerForm').on('submit', function (e) {
        var password = $('#passwordInput').val();
        var specialCharacter = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

        if (password.length < 8 || !specialCharacter.test(password)) {
            e.preventDefault();
            showPasswordRequirements();
        }
    });
});

function showPasswordRequirements() {
    $('#passwordRequirementsModal').modal({
        backdrop: 'static',
        keyboard: false
    });
}