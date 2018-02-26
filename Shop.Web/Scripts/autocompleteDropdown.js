
Number.prototype.format = function (n, x) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
    return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,');
};
$(document).ready(function () {
    $("#keySearchdown").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/ajax/HandlerAutocomplete.ashx?keyword=" + request.term, //da sua duong dan de up len server
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 1,//bao nhieu ki tu de search ra
        select: function (event, ui) {
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li class='search_li'>")
            .append("<a class='rs-auto-search' href='" + item.link + ".html'><img class='search_img' max-height='90' max-width=80 src='/Upload/Files/" + item.image + "?width=350&height=391'/><div class='search_ten'><span>" + item.label + "</span><span class='search_gia'>" + parseFloat(item.price).format() + "đ</span></div></a>")
            .appendTo(ul);
    };
});
