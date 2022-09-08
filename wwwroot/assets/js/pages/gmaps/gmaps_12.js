var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.8072308,
		lng: 30.09066691,
	})).addMarker({
		lat: 59.8072308,
		lng: 30.09066691,
		title: "Первомайская улица, дом 26, литера А, кв. 3",
		details: {
			database_id: 12,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №12")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.8072308,
		lng: 30.09066691,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">Первомайская улица, дом 26, литера А, кв. 3<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.8072308,
		lng: 30.09066691,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.8072308,
		lng: 30.09066691,
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