let ibk = 0;
let arc = 0;
let zil = 0;

function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
    document.getElementById("mainContent").style.marginRight = "250px";
    document.getElementById("openNews").style.display = "none";
	$('#showWeather').html("<a class='link' href ='#' style ='font-size: 20px;'> Innsbruck " + ibk + " °C</a><br><a class='link' href ='#' style ='font-size: 20px;'> Arco " + arc + " °C</a><br><a class='link' href ='#' style ='font-size: 20px;'> Zillertal " + zil + " °C</a>")
}

function getData() {
	$.ajax({
		url: "https://api.openweathermap.org/data/2.5/weather?lat=47.259659&lon=11.400375&appid=793416b22b7fc77cbf763bbdd73b02fb&units=metric",
		type: "GET",
		dataType: "JSON",
		data: JSON.stringify({}),
		success: function (data) {
			ibk = ((data.main.temp).toFixed(0));
		}
	});

	$.ajax({
		url: "https://api.openweathermap.org/data/2.5/weather?lat=45.91772&lon=10.88672&appid=793416b22b7fc77cbf763bbdd73b02fb&units=metric",
		type: "GET",
		dataType: "JSON",
		data: JSON.stringify({}),
		success: function (data) {
			arc = ((data.main.temp).toFixed(0));
		}
	});
	$.ajax({
		url: "https://api.openweathermap.org/data/2.5/weather?lat=47.333332&lon=11.8666632&appid=793416b22b7fc77cbf763bbdd73b02fb&units=metric",
		type: "GET",
		dataType: "JSON",
		data: JSON.stringify({}),
		success: function (data) {
			zil = ((data.main.temp).toFixed(0));
		}
	});
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("mainContent").style.marginRight = "0px";
    document.getElementById("openNews").style.display = "inline";
}