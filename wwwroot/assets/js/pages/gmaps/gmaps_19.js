var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.8352942,
		lng: 30.17831112,
	})).addMarker({
		lat: 59.8352942,
		lng: 30.17831112,
		title: "Авангардная 23, кв. 19",
		details: {
			database_id: 19,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №19")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.8352942,
		lng: 30.17831112,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Авангардная 23, кв. 19<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.8352942,
		lng: 30.17831112,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.8352942,
		lng: 30.17831112,
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