var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9365938,
		lng: 30.37557722,
	})).addMarker({
		lat: 59.9365938,
		lng: 30.37557722,
		title: "9-я Советская улица, дом 22, кв. 32",
		details: {
			database_id: 26,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №26")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9365938,
		lng: 30.37557722,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"9-я Советская улица, дом 22, кв. 32"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9365938,
		lng: 30.37557722,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9577177,
		lng: 30.35073032,
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