var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9860281,
		lng: 29.78174317,
	})).addMarker({
		lat: 59.9860281,
		lng: 29.78174317,
		title: "Петровская улица, дом 5, лит И",
		details: {
			database_id: 3,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №3")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9860281,
		lng: 29.78174317,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Петровская улица, дом 5, лит И<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9860281,
		lng: 29.78174317,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9860281,
		lng: 29.78174317,
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