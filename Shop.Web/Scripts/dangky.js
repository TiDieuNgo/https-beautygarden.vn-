$(function () {
        $("#NamSinh").datepicker({
            changeMonth: true,
            changeYear: true
        });
    $("#email").focusout(function() {
        kiemTra($(this));
    });
});
function kiemTra(obj) {
    var email = $(obj).val();
    $.post("/DangKy/KiemTraEmail", { email: email }, function (data) {
                    console.log(data);
                    if (data == 1) {//da ton tai
                        $(obj).addClass("input-validation-error");
                        $("#emailTonTai").text("Email này đã có người sử dụng. Vui lòng chọn email khác.");
                    } else {//su dung dc email nay
                        $(obj).removeClass("input-validation-error").addClass("valid");
                    }
                });
}
//function kiemTraEmailTonTai(obj) {
//    console.log("asdasd");
//    var email = $(obj).val();
//    var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
//    if(pattern.test(email))
//    {
//        $.post("/DangKy/KiemTraEmail", { email: email }, function (data) {
//            console.log(data);
//            if (data == 1) {//da ton tai
//                $(obj).addClass("input-validation-error");
//                $("#emailTonTai").text("Email này đã có người sử dụng. Vui lòng chọn email khác.");
//            } else {//su dung dc email nay
//                $(obj).removeClass("input-validation-error").addClass("valid");
//            }
//        });
//    }​
//}
//function isValidEmailAddress(emailAddress) {
//    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
//    return pattern.test(emailAddress);
//};