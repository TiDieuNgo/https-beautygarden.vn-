var name = "#floatLeftBanner";
	var menuYloc = null;
	
	$(document).ready(function () {
	    var offset = $("#templatemo_header").offset();
	    var bannerLeftwidth = $("#floatLeftBanner").width();
	    $('#floatLeftBanner').css("left", offset.left - parseInt(bannerLeftwidth) +5+ "px");
		});  
