var name = "#floatRightBanner";
	var menuYloc = null;
	$(document).ready(function () {
	    var offset = $("#templatemo_header").offset();
	    var bannerRighttwidth = $("#floatRightBanner").width();
	    $('#floatRightBanner').css("right", offset.left - parseInt(bannerRighttwidth) + 5 + "px");
		});  