var product = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $('#btn-images').click(function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                //fileUrl = fileUrl.substring(fileUrl.lastIndexOf('/') + 1);
                var html = '<div class="col-sm-2 pointer">';
                html += '<img class="img-thumbnail" src="' + fileUrl + '" width="100%">';
                html += '<i class="btn-remove-img fa fa-window-close" aria-hidden="true"></i>';
                html += '</div>';
                $('#imageList').prepend(html);
            };
            finder.popup();
        });

        $('#imageList').on('click', '.btn-remove-img', function () {
            $(this).parent('div').remove();
        });

        $('#btn-save').click(function () {
            var images = [];
            var id = $('#hidProductId').val();
            $.each($('#imageList img'), function (i, item) {
                images.push($(item).attr('src'));
            });

            $.ajax({
                url: '/Product/SaveImages',
                type: 'POST',
                dataType: 'json',
                data: { id: id, images: JSON.stringify(images) },
                success: function (response) {
                    if (response.status)
                        alert('Lưu lại thành công.');
                    else
                        alert('Lưu lại thất bại.');
                }
            });
        });
    }
};
product.init();