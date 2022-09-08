var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9664191,
		lng: 30.31041217,
	})).addMarker({
		lat: 59.9664191,
		lng: 30.31041217,
		title: "Санкт-Петербург, Большой проспект Петроградской стороны 98 лит. А, кв 25",
		details: {
			database_id: 42,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №1")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9664191,
		lng: 30.31041217,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Санкт-Петербург, Большой проспект Петроградской стороны 98 лит. А, кв 25<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9664191,
		lng: 30.31041217,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9664191,
		lng: 30.31041217,
		mapTypeControlOptions: {
			mapTypeIds: ["hybrid", "roadmap", "satellite", "terrain", "osm"]
		}
	})).addMapType("osm", {
		getTileUrl: function (a, e) {
			return "https://a.tile.openstreetmap.org/" + e + "/" + a.x + "/" + a.y + ".png"
		},
		tileSize: new google.maps.Size(256, 256),
		name: "OpenStreetMap",
		maxZoom: 18
	}), map.setMapTypeId("osm")
});