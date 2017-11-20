$(document).ready(function () {
    $('.editingRole').click(function () {
        var btn = $(this);
        var id = btn.data('id');
        var name = btn.parent().prev().children().val().trim();
        if (name === '')
            alert('Tên nhóm người dùng không được để trống');
        else if (name.length > 256)
            alert('Tên nhóm người dùng tối đa là 256 ký tự.');
        else {
            $.post('/Role/Edit/' + id, { name: name }, function (status) {
                var message;
                switch (status) {
                    case 1: message = 'Cập nhật thành công.';
                        location.reload();
                        break;
                    case 2: message = 'Tên nhóm người dùng đã tồn tại';
                        break;
                    default: message = 'Có lỗi xảy ra khi cập nhật! Vui lòng thử lại.';
                }
                alert(message);
            });
        }
    });

    $('#btnCreateNewRole').click(function () {
        var html = '<tr>';
        html += '<td>';
        html += '<button type="button" class="btn btn-outline btn-warning" id="btnCancel"><i class="fa fa-times"></i></button>';
        html += '</td>';
        html += '<td><input type="text" class="form-control" /></td>';
        html += '<td>';
        html += '<button id="btnCreate" type="button" class="btn btn-primary"><i class="fa fa-plus"></i> Thêm</button>';
        html += '</td>';
        html += '</tr>';
        $('.table tbody').prepend(html);
        $(this).attr('disabled', true);
    });

    $('.table tbody').on('click', '#btnCreate', function () {
        var name = $(this).parent().prev().children().val();
        if (name.length > 50)
            alert('Tên nhóm người dùng tối đa là 50 ký tự.');
        else if (name != '') {
            $.post('/Role/Create/', { name: name }, function (status) {
                var message;
                switch (status) {
                    case 1: message = 'Tạo mới thành công.';
                        location.reload();
                        break;
                    case 2: message = 'Tên nhóm người dùng đã tồn tại';
                        break;
                    default:
                        message = 'Có lỗi xảy ra khi tạo mới! Vui lòng thử lại.';
                }
                alert(message);
            });
        }
        else
            alert('Vui lòng nhập tên nhóm người dùng!');
    });

    $('tbody').on('click', '#btnCancel', function () {
        $(this).parents('tr').remove();
        $('#btnCreateNewRole').removeAttr('disabled')
    });
    //===========================================
});