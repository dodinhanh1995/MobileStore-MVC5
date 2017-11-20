$(function () {
    $("tbody").on("click", "#btn-create", function () {
        var content = $(".form-reply-insert .form-control").val().trim();

        if (content === "")
            alert("Vui lòng nhập nội dụng phản hồi.");
        else {
            $.ajax({
                url: "/Comment/CreateReply",
                type: "POST",
                dataType: "json",
                data: { 
                    id : $(this).data("id"),
                    productId: $(this).data("product-id"),
                    content : content
                },
                success: function (message) {
                    alert(message);
                    location.href = location.href;
                }
            });
        }
    });

    var isExist = false;

    $(".btn-reply").click(function () {
        if (isExist) {
            if (confirm("Bạn có chắc muốn làm điều này?"))
                InsertFormReply($(this));
        }
        else {
            InsertFormReply($(this));
        }

        isExist = true;
    });

    $("tbody").on("click", "#btn-cancel", function () {
        $(".form-reply-insert").remove();
        isExist = false;
    });

    function InsertFormReply(x) {
        var template = Mustache.render($("#reply-template").html(), {
            Id: $(x).attr("commentid"),
            ProductId : $(x).attr("productid")
        });
        $(".form-reply-insert").remove();

        $(x).parents("tr").after(template);
    }
});

