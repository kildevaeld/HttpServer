
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


	$('#submit').click(function (e) {
		e.preventDefault();
		console.log($('[name]','form'));
		var out = {}
		$('[name]', 'form').each(function (i) {
			
			out[$(this).attr('name')] = $(this).val();
		});


		var data = out["type"] == "application/json" ? JSON.stringify(out) : out
		$.ajax({
			type: "POST",
			url: '/post-test',
			data: data,
			dataType: 'json',
			contentType: out['type']
		}).done(function (data) {
			$('#json-result .result').text(JSON.stringify(data, null, 4));
		})
		
	})

});
