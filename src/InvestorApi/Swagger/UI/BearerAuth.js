// https://github.com/swagger-api/swagger-ui/issues/2915
(function () {
    $(function () {
        var titleElementCount = 0;
        var descriptionElementCount = 0;
        var labelElementCount = 0;
        var textElementCount = 0;

        $(document).bind('DOMSubtreeModified', function () {
            var el = $('.auth__title');
            if (titleElementCount !== el.length) {
                titleElementCount = el.length;
                if (titleElementCount > 0) {
                    el.text('Bearer Authorization');
                }
            }

            el = $('.auth__description');
            if (descriptionElementCount !== el.length) {
                descriptionElementCount = el.length;
                if (descriptionElementCount > 0) {
                    el.hide();
                }
            }

            el = $('.key_auth__label');
            if (labelElementCount !== el.length) {
                labelElementCount = el.length;
                if (labelElementCount > 0) {
                    el.last().text('Bearer');
                }
            }

            el = $('.input_apiKey_entry');
            if (textElementCount !== el.length) {
                textElementCount = el.length;
                if (textElementCount > 0) {
                    el.attr('placeholder', '');
                    el.width('80%');
                    el.change(addApiKeyAuthorization);
                }
            }
        });
    });

    function addApiKeyAuthorization() {
        var key = $('.input_apiKey_entry')[0].value;
        if (key && key.trim() !== "") {
            if (key.toLowerCase().startsWith('bearer')) key = key.subString(7);
            key = encodeURIComponent(key);
            var apiKeyAuth = new SwaggerClient.ApiKeyAuthorization('Authorization', 'Bearer ' + key, 'header');
            window.swaggerUi.api.clientAuthorizations.add('bearer', apiKeyAuth);
        }
    }
})();
