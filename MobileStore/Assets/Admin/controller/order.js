var order = {
    init: function () {
        order.registerEvent();
    },
    registerEvent: function () {
        $('.btnUpdateOrder').on('click', function () {
            var productId = $(this).attr("productId");
            var quantity = $("#qt_" + productId).val();

            if (quantity < 1 || quantity === "") {
                alert("Số lượng tối thiểu phải là 1");
            }
            else {
                $.ajax({
                    url: '/Order/ChangeQuantity',
                    method: 'POST',
                    data:
                        {
                            productId: productId,
                            orderId: $(this).attr("orderId"),
                            quantity: quantity
                        },
                    success: function (result) {
                        if (result.Status) {
                            $('#lblTotalPrice').text(order.ConvertPrice(result.TotalPrice));
                            $('#lblAmount').text(result.Amount);
                            $('#lblOrderTotal').text(order.ConvertPrice(result.OrderTotal));
                            alert('Cập nhật số lượng sản phẩm thành công.');
                        }
                        else
                            alert('Có lỗi khi thay đổi số lượng sản phẩm. Vui lòng thử lại.');
                    }
                });
            }
        });

        $('.statusLink').click(function (e) {
            e.preventDefault();
            if (confirm('Bạn có chắc là muốn thanh toán hóa đơn?')) {
                var orderId = $(this).data('order-id');
                if (orderId != '') {
                    $.post('/Order/PaidOrdering', { orderId: orderId }, function (result) {
                        if (!result.Status && result.ListName === '')
                            alert('Có lỗi trong quá trình thanh toán đơn hàng. Vui lòng thử lại');
                        if (!result.Status)
                            $('#AlertMessage').removeClass('hide').find('div').text(result.ListName);
                        else
                        {
                            alert('Thanh toán đơn hàng thành công.');
                            window.location.reload();
                        }
                    });
                }
            }
        });
    },

    ConvertPrice : function(price) {
        return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "đ";
    }
};
order.init();
