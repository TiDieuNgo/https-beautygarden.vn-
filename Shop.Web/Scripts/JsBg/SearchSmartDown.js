
var clicksearch_down = 0;
$("#keySearchdown").on('click', function () {
    if ($("#keySearchdown").val().length == 0) {
        if (clicksearch_down == 0) {
            $.post("/Home/GetTopSearch", {
            }, function (data) {
                if (data.status) {
                    $('#search_down li').remove();
                    var html = '<span class="popular-search-title search_smart" id="xuhuong">Xu hướng tìm kiếm</span>';
                    for (var i = 0; i < data.data.length; i++) {
                        html += '<li class="search_li ui-menu-item search_top10">' +
                            '<a class="search_li ui-menu-item search_li" href="https://beautygarden.vn/tim-kiem.html?keyword=' + data.data[i].TuKhoa + '" tabindex="-1"><div class="search_ten search_top10_ten"><span>' + data.data[i].TuKhoa + '</span></div></a>' +
                            '</li>';
                    }
                    $("#search_down").append(html);
                    $("#search_down").show();
                    clicksearch_down = 1;
                } else {
                    alert(data.message);
                }
            });
        } else {
            $('#search_down li,.search_smart').remove();
            clicksearch_down = 0;
        }
    }
});

//$("#keySearchdown").blur(function () {
//    $('#search_down li,.search_smart').remove();
//    clicksearch_down = 0;
//});
$("#keySearchdown").change(function () {
    $('#search_down li,.search_smart').remove();
    clicksearch_down = 0;
});
$('body').click(function (evt) {
    if (evt.target.id == "keySearchClick_down")
        return;
    //For descendants of menu_content being clicked, remove this check if you do not want to put constraint on descendants.
    if ($(evt.target).closest('#keySearchClick_down').length)
        return;

    //Do processing of click event here for every element except with id menu_content
    $('#search_down li,.search_smart').remove();
    clicksearch_down = 0;
});

