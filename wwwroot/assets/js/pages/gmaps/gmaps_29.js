var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9960627,
		lng: 30.37702032,
	})).addMarker({
		lat: 59.9960627,
		lng: 30.37702032,
		title: "пр-т Непокорённых, д. 13, к.1, кв. 60",
		details: {
			database_id: 29,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №29")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9960627,
		lng: 30.37702032,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"пр-т Непокорённых, д. 13, к.1, кв. 60"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9960627,
		lng: 30.37702032,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9960627,
		lng: 30.37702032,
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