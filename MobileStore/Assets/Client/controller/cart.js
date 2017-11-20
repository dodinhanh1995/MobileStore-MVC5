var shoppingCart = {
    init: function () {
        shoppingCart.registerEvent();
    },
    registerEvent: function () {
        $('.btnRemoveItem').off('click').on('click', function () {
            var productId = $(this).data('id');
            if (productId != '') {
                $.post('/Cart/RemoveFromCart', { productId: productId }, function (result) {
                    if (result.Status) {
                        $('#row_' + productId).slideUp(300);
                        if (result.ItemCount == 0) {
                            window.location.href = '/';
                        }
                        $('#CartTotal strong').text(ConvertPrice(result.CartTotal));
                        $('#CartStatus .total').text(result.ItemCount);

                        shoppingCart.loadCartStatus(result);
                    }
                });
            }
        });
    },
    changeQuantity: function (productId, flag) {
        if (productId > 0) {
            var item = $('#item_count_' + productId);
            var quantity = $(item).text();
            if (!flag &&  quantity <= 1) {
                $(item).prev().addClass('disabled');
            }
            else {
                $.post('/Cart/ChangeQuantityFromCart', { productId: productId, flag: flag, quantity: quantity }, function (result) {
                    if (result.Status)
                    {
                        $(item).next().addClass('disabled');
                        return;
                    }
                    $(item).next().removeClass('disabled');
                    $(item).prev().removeClass('disabled');
                    $(item).text(result.ItemCount);

                    $('#CartTotal strong').text(ConvertPrice(result.CartTotal));

                    shoppingCart.loadCartStatus(result);
                });
            }
        }
    },
    loadCartStatus: function (result) {
        var html = '';
        var template = $("#cart-st-info-template").html();
        $.each(result.CartItems, function (i, item) {
            html += Mustache.render(template, {
                ProductImage: item.Image,
                ProductName: item.Name,
                Count: item.Count,
                ProductPrice: ConvertPrice(item.Price)
            });
        });
        html += Mustache.render($("#cart-st-total-template").html(), {
            CartTotal: ConvertPrice(result.CartTotal)
        });

        $("#CartStatus .info").html(html);
    }
};
shoppingCart.init();


