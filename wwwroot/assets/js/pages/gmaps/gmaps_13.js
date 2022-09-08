var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.8673695,
		lng: 30.37417417,
	})).addMarker({
		lat: 59.8673695,
		lng: 30.37417417,
		title: "ул. Турку д. 9, к.4, кв. 73",
		details: {
			database_id: 13,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №13")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.8673695,
		lng: 30.37417417,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"ул. Турку д. 9, к.4, кв. 73"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.8673695,
		lng: 30.37417417,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.8673695,
		lng: 30.37417417,
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