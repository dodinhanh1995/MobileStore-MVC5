var productDetailCtl = {
    config: {
        mainImage: $("#main-img"),
        btnImgThub: $(".btn-img-thub"),
        btnComment: $("#btn-comment"),
        productId: $("#btn-comment").data("product-id"),
        txtCommentContent: $('#txt-comment-content'),
        listComment: $('.list-comment'),
        page: null
    },
    init: function () {
        this.registerEvent();
    },
    registerEvent: function () {
        this.changeImage();
        this.createComment();
        this.loadComment();

        var currentPage = window.location.hash.replace("#page", "");

        if (currentPage != '') {
            var url = "/dien-thoai?page=" + currentPage;
            $('#paging a[href="' + url + '"]').click();
        }

    },
    changeImage: function () {
        $(this.config.btnImgThub).click(function () {
            $currentImage = $(this).children();
            $(productDetailCtl.config.mainImage).attr({
                src: $currentImage.attr("src"),
                alt: $currentImage.attr("alt"),
                title: $currentImage.attr("title"),
            });
                       

            $(productDetailCtl.config.btnImgThub).children().removeClass("border-blue");
            $currentImage.addClass("border-blue");
        });

        $(".btn-img-thub:first-child").click();
    },
    createComment: function () {
        $(this.config.btnComment).click(function () {
            var content = productDetailCtl.config.txtCommentContent.val();
            if (content == '') {
                alert('Vui lòng nhập nội dung');
            }
            else {
                var data = {
                    productId: productDetailCtl.config.productId,
                    content: content
                };
                productDetailCtl.sendAjax("/DetailProduct/CreateComment", data, window.location.href = window.location.hash.replace(/#page./, '#page1'));
            }
        });
    },
    loadComment: function () {
        $('#comment').on('click', '#paging a', function () {
            productDetailCtl.config.page = $(this).data('page');

            $.ajax({
                url: '/DetailProduct/LoadComment',
                type: 'GET',
                data: {
                    productId: productDetailCtl.config.productId,
                    page: productDetailCtl.config.page
                },
                dataType: 'json',
                success: function (result) {
                    if (result.hasOwnProperty('comment') && result.hasOwnProperty('paging')) {
                        var html = '';
                        var commentTemplate = $("#comment-template").html();
                        var replyTemplate = $("#commentReply-template").html();
                        $.each(result['comment'], function (key, cm) {
                            html += "<li class='comment-ask'>";
                            html += Mustache.render(commentTemplate, {
                                RepresentCommenter: cm.RepresentCommenter,
                                Commenter: cm.Commenter,
                                Content: cm.Content,
                                Id: cm.Id,
                                CommentDate: cm.CommentDate
                            });

                            if (cm.ReplyList.length > 0) {

                                html += "<div class='list-reply form-reply-" + cm.Id + "'>";
                                $.each(cm.ReplyList, function (i, reply) {
                                    html += Mustache.render(replyTemplate, {
                                        RepresentRespondent: reply.RepresentRespondent,
                                        Respondent: reply.Respondent,
                                        Administrator: reply.RoleName === "Administrators" ? "<span class='label label-warning'>Quản trị viên</span>" : "",
                                        Content: reply.Content,
                                        Id: cm.Id,
                                        ReplyDate: reply.ReplyDate,
                                    });
                                });
                                html += "</div>";
                            }
                            html += "</li>";

                            $(productDetailCtl.config.listComment).html(html);

                            $('#paging').html(result['paging']);
                        });
                    }
                }
            }).done(function () {
                var top = $(productDetailCtl.config.txtCommentContent).offset().top;
                $("html, body").animate({
                    scrollTop: top
                }, 700);
                window.location.hash = "page" + productDetailCtl.config.page;
            });
            return false;
        });
    },
    replyComment: function (commentId, hasReplyList, current) {
        var template = $("#reply-template").html();
        var html = Mustache.render(template, {
            Recipient: $(current).attr('recipient'),
            CommentId: commentId
        });
        $("#form-to-reply").remove();
        $(current).parents(!hasReplyList ? "li" : ".reply").append(html);
    },
    createReply: function (x, commentId) {
        var content = $(x).prev().children().val();
        if (content === '') {
            alert('Vui lòng nhập nội dung');
        }
        else {
            var data = {
                productId : productDetailCtl.config.productId,
                commentId: commentId,
                content: content
            };
            productDetailCtl.sendAjax("/DetailProduct/CreateCommentReply", data);
        }
    },
    sendAjax: function (url, data, reload) {
        $.ajax({
            url: url,
            method: 'POST',
            dataType: 'json',
            data: data,
            success: function (response) {
                if (!response.isAuth)
                    alert('Vui lòng đăng nhập để bình luận.');
                else if (response.commentId < 1)
                    alert('Không thể gửi bình luận. Vui lòng thử lại.');
                else {
                    reload;
                    window.location.reload();
                }
            }
        });
    }
};
productDetailCtl.init();
