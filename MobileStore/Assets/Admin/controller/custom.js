$(document).ready(function () {
    $('#chkDeleteAll').click(function () {
        if ($(this).is(":checked"))
            $('.chkFieldId').prop('checked', true);
        else
            $('.chkFieldId').prop('checked', false);
    });

    $('#alertBox').delay(2000).slideUp(1000);

    $('#browse').click(function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            //fileUrl = fileUrl.substring(fileUrl.lastIndexOf('/') + 1);
            $('#Image').val(fileUrl);
        };
        finder.popup();
    });

    
});

function selectImage(action, id) {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $.ajax({
            url: action,
            method: "GET",
            data: { image: fileUrl }
        }).done(function (path) {
            if (path != '')
                alert('Lỗi cập nhật hình ảnh' + path);
            else
                $('#' + id).attr('src', fileUrl);
        });
    };
    finder.popup();
}

function convertMetaTitle(action) {
    var name = $('#Name').val();
    $.ajax({
        method: "GET",
        url: action,
        data: { name: name },
        success: function (result) {
            if (result !== '') {
                $('#MetaTitle').val(result);
            }
        }
    });
}

function slug(str) {
    var slug;
    //Đổi chữ hoa thành chữ thường
    slug = str.toLowerCase();

    //Đổi ký tự có dấu thành không dấu
    slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
    slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
    slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
    slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
    slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
    slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
    slug = slug.replace(/đ/gi, 'd');
    //Xóa các ký tự đặt biệt
    slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
    //Đổi khoảng trắng thành ký tự gạch ngang
    slug = slug.replace(/ /gi, "-");
    //Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
    //Phòng trường hợp người nhập vào quá nhiều ký tự trắng
    slug = slug.replace(/\-\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-\-/gi, '-');
    slug = slug.replace(/\-\-\-/gi, '-');
    slug = slug.replace(/\-\-/gi, '-');
    //Xóa các ký tự gạch ngang ở đầu và cuối
    slug = '@' + slug + '@';
    slug = slug.replace(/\@\-|\-\@|\@/gi, '');
    //In slug ra textbox có id “slug”
    return slug;
}

function sortMenu(n) {
    var rows, i, x, y, switching = true, shouldSwitch, dir = 'asc', flag = false;
    while (switching) {
        switching = false;
        rows = $('#list-data tr');

        for (i = 0; i < rows.length - 1; i++) {
            shouldSwitch = false;
            x = slug($(rows[i]).find('td').eq(n).text());
            y = slug($(rows[i + 1]).find('td').eq(n).text());

            if (dir === 'asc') {
                if (x > y) {
                    shouldSwitch = true;
                    break;
                }
            }
            else if (dir === 'desc') {
                if (x < y) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            $(rows[i]).before(rows[i + 1]);
            switching = true;
            flag = true;
        }

        if (dir === 'asc' && !flag) {
            dir = 'desc';
            switching = true;
        }
    }
}

function filterTable(id) {
    var td = $('#list-data tr td.name');
    var filter = $(id).val();
    filter = slug(filter);

    var length = td.length, i = 0;
    
    for (i; i < length; i++) {
        var value = slug($(td[i]).text());

        if (value.indexOf(filter) > -1) {
            $(td[i]).parent().show();
        } else {
            $(td[i]).parent().hide();
        }
    }
}





