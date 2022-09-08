var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 60.0262847,
		lng: 30.42088932,
	})).addMarker({
		lat: 60.0262847,
		lng: 30.42088932,
		title: "Черкасова д.11 кор.1",
		details: {
			database_id: 18,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №18")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 60.0262847,
		lng: 30.42088932,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Черкасова д.11 кор.1<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 60.0262847,
		lng: 30.42088932,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 60.0262847,
		lng: 30.42088932,
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