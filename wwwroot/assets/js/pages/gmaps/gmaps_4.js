var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 60.0088431,
		lng: 29.72956032,
	})).addMarker({
		lat: 60.0088431,
		lng: 29.72956032,
		title: "Кронштадское шоссе, дом 28, лит Б.",
		details: {
			database_id: 4,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №4")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 60.0088431,
		lng: 29.72956032,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay"Кронштадское шоссе, дом 28, лит Б.<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 60.0088431,
		lng: 29.72956032,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 60.0088431,
		lng: 29.72956032,
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