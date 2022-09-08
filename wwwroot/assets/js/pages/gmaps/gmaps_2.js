var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9466185,
		lng: 30.355454417,
	})).addMarker({
		lat: 59.9466185,
		lng: 30.355454417,
		title: "Санкт-Петербург, Чайковского д.36, лит. А, кв. 4",
		details: {
			database_id: 2,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №2")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9466185,
		lng: 30.355454417,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Санкт-Петербург, Чайковского д.36, лит. А, кв. 4<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9466185,
		lng: 30.355454417,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9466185,
		lng: 30.355454417,
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