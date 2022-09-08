var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 60.0532688,
		lng: 30.34929862,
	})).addMarker({
		lat: 60.0532688,
		lng: 30.34929862,
		title: "Санкт-Петербург, Сиреневый бульвар, дом 7, корпус 1, литера А, кв. 4",
		details: {
			database_id: 5,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №5")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 60.0532688,
		lng: 30.34929862,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Санкт-Петербург, Сиреневый бульвар, дом 7, корпус 1, литера А, кв. 4<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 60.0532688,
		lng: 30.34929862,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 60.0532688,
		lng: 30.34929862,
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