var mobileCtl = {
    config: {
        productContent: $('#product-content'),
        btnLoadMore: $('#load-more button'),
        template: $('#template').html(),
        page: 1,
        pageSize: 10,
        brand: $('#brand').text(),
        sort: $('#sort').text()
    },
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $(this.config.btnLoadMore).click(function () {

            $(this).text('Loadding...');

            mobileCtl.config.page++;

            window.location.hash = mobileCtl.config.page;

            mobileCtl.loadData();
        });

        var hashProduct = window.location.hash.replace('#', '');
        if (hashProduct != '')
            for (var p = 1; p < hashProduct; p++) {
                $(this.config.btnLoadMore).click();
            }
    },
    loadData: function () {
        $.ajax({
            url: '/Mobile/Index',
            type: 'GET',
            dataType: 'json',
            data: { page: this.config.page, metaTitle: this.config.brand, orderBy: this.config.sort },
            success: function (result) {
                var rendered = '';
                if (result.length <= mobileCtl.config.pageSize) {
                    $.each(result, function (i, item) {
                        var del = item.PromotionPrice > 0 ? 'line-through' : '';
                        rendered += Mustache.render(mobileCtl.config.template, {
                            Id: item.Id,
                            Name: item.Name,
                            Price: ConvertPrice(item.Price),
                            PromotionPrice: item.PromotionPrice == 0 ? '' : ConvertPrice(item.PromotionPrice) + 'đ',
                            Image: item.Image,
                            MetaTitle: item.MetaTitle,
                            delPrice: del
                        });
                    });

                    $(mobileCtl.config.productContent).append(rendered);

                    $(mobileCtl.config.btnLoadMore).remove();
                } else {
                    $.each(result, function (i, item) {
                        if (i < result.length - 1) {
                            var del = item.PromotionPrice > 0 ? 'line-through' : '';
                            rendered += Mustache.render(mobileCtl.config.template, {
                                Id: item.Id,
                                Name: item.Name,
                                Price: ConvertPrice(item.Price),
                                PromotionPrice: item.PromotionPrice == 0 ? '' : ConvertPrice(item.PromotionPrice),
                                Image: item.Image,
                                MetaTitle: item.MetaTitle,
                                delPrice: del
                            });
                        }
                    });

                    $(mobileCtl.config.productContent).append(rendered);
                }
            }
        }).always(function () {
            $(mobileCtl.config.btnLoadMore).html('Xem thêm sản phẩm <i class="fa fa-caret-down"></i>');
        });
        return false;
    }
    
};
mobileCtl.init();
