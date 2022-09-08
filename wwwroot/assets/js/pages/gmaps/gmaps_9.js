var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9368308,
		lng: 30.371511617,
	})).addMarker({
		lat: 59.9368308,
		lng: 30.371511617,
		title: "Старорусская д. 16\8-я Советская, д. 57, литера А, кв. 19",
		details: {
			database_id: 9,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №9")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9368308,
		lng: 30.371511617,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Старорусская д. 16\8-я Советская, д. 57, литера А, кв. 19<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9368308,
		lng: 30.37151162,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9368308,
		lng: 30.371511617,
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