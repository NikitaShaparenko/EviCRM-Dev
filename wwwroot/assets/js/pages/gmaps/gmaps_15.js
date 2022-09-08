var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 60.0094267,
		lng: 30.25191632,
	})).addMarker({
		lat: 59.8999617,
		lng: 30.47531032,
		title: "ул. Шотмана д.5, к.1, кв. 180",
		details: {
			database_id: 15,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №15")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.8999617,
		lng: 30.47531032,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"ул. Шотмана д.5, к.1, кв. 180"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.8999617,
		lng: 30.47531032,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.8999617,
		lng: 30.47531032,
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