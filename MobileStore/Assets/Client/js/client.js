var cartInfo = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        var cart = $('#header .right .cart');
        $(cart).click(function () {
            $info = $(this).children('.info');
            if ($info.height() >= 290) {
                $info.css('overflow-y', 'auto');
            }
            $info.slideToggle(100);

        });
    }
};
cartInfo.init();

var backToTop = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $('#back-to-top').click(function () {
            $('html, body').animate({
                scrollTop: 0
            }, 700);
        });
    }
};
backToTop.init();

$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('#main-nav').addClass('navbar-fixed-top');
        $('#back-to-top').addClass('active');
    } else {
        $('#main-nav').removeClass('navbar-fixed-top');
        $('#back-to-top').removeClass('active');
    }
});

var filter = {
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $('#content .filter ul li').click(function () {
            if (!$(this).children('.dropdown').hasClass('active')) {
                $('#content .filter .dropdown').removeClass('active');
            }

            $(this).children('.dropdown').toggleClass('active');
        });
    }
};
filter.init();

var search = {
    config : {
        timeout : null
    },
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {

        if ($(window).outerWidth() <= 768)
            return false;

        this.config.timeout = null;

        $("#search-box").keyup(function () {

            clearTimeout(search.config.timeout);

            var key = $(this).val();

            if (key.length < 2)
            {
                $("#wrap-suggestion").html("").addClass("hide");
                return false;
            }

            search.config.timeout = setTimeout(function () {

                $("#wrap-suggestion").removeClass("hide");
                var template = $("#suggestion-template").html();

                $.ajax({
                    url: "/Mobile/LoadProductName",
                    type: "GET",
                    data: { key: key },
                    success: function (result) {
                        var html = "";
                        $.each(result, function (i, item) {
                            html += Mustache.render(template, {
                                Id: item.Id,
                                MetaTitle: item.MetaTitle,
                                Image: item.Image,
                                Name: item.Name,
                                Price: item.PromotionPrice > 0 ? "Giá thường: <span class='price'>" + ConvertPrice(item.Price) + "</span>" : "<span class='price'>" + ConvertPrice(item.Price) + "</span>",
                                PromotionPrice: item.PromotionPrice > 0 ? "Giá khuyến mại: <span class='price'>" + ConvertPrice(item.PromotionPrice) + "</span>" : ""
                            });
                        });

                        $("#wrap-suggestion").html(html);
                    }
                });
            }, 500);
        });
    }
};
search.init();

function ConvertPrice(price) {
    return price.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + "đ";
}

