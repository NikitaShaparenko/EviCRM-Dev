var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.8258677,
		lng: 30.15687932,
	})).addMarker({
		lat: 59.8258677,
		lng: 30.15687932,
		title: "Проспект Народного Ополчения, д. 231, кв. 23",
		details: {
			database_id: 16,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №16")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.8258677,
		lng: 30.15687932,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Проспект Народного Ополчения, д. 231, кв. 23<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.8258677,
		lng: 30.15687932,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 60.0094267,
		lng: 30.25191632,
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