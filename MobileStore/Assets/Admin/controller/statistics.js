$(function () {
    $("#tabs-statistics li").click(function () {
        $("#tabs-statistics li").removeClass("active").children().css({
            backgroundColor: "transparent",
            color: "#337ab7",
            borderColor: "transparent"
        });

        $(this).addClass("active").children().css({
            backgroundColor: "#eee",
            color: "#000",
            borderColor: "#eee"
        });
    });
});

function fnOnBegin() {
    var fromDate = $("#from-date").val();
    var toDate = $("#to-date").val();
    if (fromDate === "" || toDate === "") {
        alert("Vui lòng cả 2 ngày bắt đầu và kết thúc");
        return false;
    }
    return true;
}