$(() => {
    $("#btn-submit").on('click', function () {

        const passwordTry = $("#password-try").val();
        const passwordReal = $("#password-real").val();
        if (passwordTry !== passwordReal) {
            (".modal").model();
        }
        else {
            
        }

    });



});