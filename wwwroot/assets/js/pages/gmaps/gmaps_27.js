var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9271694,
		lng: 30.31140642,
	})).addMarker({
		lat: 59.9271694,
		lng: 30.31140642,
		title: "Казначейская ул. д.5, кв.9",
		details: {
			database_id: 27,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №27")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9271694,
		lng: 30.31140642,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"Казначейская ул. д.5, кв.9"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9271694,
		lng: 30.31140642,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9271694,
		lng: 30.31140642,
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