var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9875627,
		lng: 30.31395122,
	})).addMarker({
		lat: 59.9875627,
		lng: 30.31395122,
		title: "Белоостровская улица, дом 25, литера А, кв. 3",
		details: {
			database_id: 6,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №6")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9875627,
		lng: 30.31395122,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Белоостровская улица, дом 25, литера А, кв. 3<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9875627,
		lng: 30.31395122,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9875627,
		lng: 30.31395122,
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