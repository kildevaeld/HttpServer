
$(function () {
	var menu = $('<div><a href="/subdir/nested">Click here</a> or <a href="/router">Click here</a></div>')
	menu.hide();

	var i = 1;
	var int = setInterval(function (){
		$('.timer','#main').text(i--);
		if (i == 0) {
			clearInterval(int);
			$('#main').html(menu);
			menu.fadeIn();
		}
	},1000);

});
