var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9587466,
		lng: 30.28531952,
	})).addMarker({
		lat: 59.9587466,
		lng: 30.28531952,
		title: "ул. Пионерская, д. 37, литера А, кв. 2",
		details: {
			database_id: 24,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №24")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9587466,
		lng: 30.28531952,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"ул. Пионерская, д. 37, литера А, кв. 2"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9587466,
		lng: 30.28531952,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9587466,
		lng: 30.28531952,
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