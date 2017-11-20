var checkout = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        this.loadProvince();
        $('#ddlProvince').change(function () {
            var provinceId = $(this).val();
            if (provinceId != '')
                checkout.loadDistrict(provinceId);
            else
                $('#ddlDistrict').html('');
        });
    },
    loadProvince: function () {
        $.ajax({
            url: '/Checkout/LoadProvince',
            dataType: 'json',
            success: function (response) {
                var html = '<option value="">--Chọn tỉnh / thành--</option>';
                var template = $('#province-district-template').html();
                $.each(response, function (key, value) {
                    html += Mustache.render(template, {
                        DataValue: value.Id,
                        DataText: value.Name
                    });
                });
                $('#ddlProvince').html(html);
            }
        });
    },
    loadDistrict: function (provinceId) {
        $.ajax({
            url: '/Checkout/LoadDistrict',
            dataType: 'json',
            data: { provinceId: provinceId },
            success: function (response) {
                var html = '<option value="">--Chọn quận / huyện--</option>';
                var template = $('#province-district-template').html();
                $.each(response, function (key, value) {
                    html += Mustache.render(template, {
                        DataValue: value.Id,
                        DataText: value.Name
                    });
                });
                $('#ddlDistrict').html(html);
            }
        });
    }
};
checkout.init();


