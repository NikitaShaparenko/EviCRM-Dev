var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9958319,	
		lng: 30.37450762,
	})).addMarker({
		at: 59.9958319,
		lng: 30.37450762,
		title: "Санкт-Петербург, Проспект Непокорённых, дом 9, корпус 1, литера А, кв. 21",
		details: {
			database_id: 10,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №10")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		at: 59.9958319,
		lng: 30.37450762,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Санкт-Петербург, Проспект Непокорённых, дом 9, корпус 1, литера А, кв. 21<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		at: 59.9958319,
		lng: 30.37450762,
	}), (map = new GMaps({
		div: "#gmaps-types",
		at: 59.9958319,
		lng: 30.37450762,
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