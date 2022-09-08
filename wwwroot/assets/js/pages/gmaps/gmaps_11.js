var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9033348,
		lng: 30.27531532,
	})).addMarker({
		lat: 59.9033348,
		lng: 30.27531532,
		title: "Старо-Петергофский проспект, дом 42, литера А, кв. 23",
		details: {
			database_id: 11,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №11")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9033348,
		lng: 30.27531532,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Старо-Петергофский проспект, дом 42, литера А, кв. 23<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9033348,
		lng: 30.27531532,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9033348,
		lng: 30.27531532,
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