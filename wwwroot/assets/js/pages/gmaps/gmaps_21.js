var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9139178,
		lng: 30.33432117,
	})).addMarker({
		lat: 59.9139178,
		lng: 30.33432117,
		title: "Набережная Обводного канала д. 66, лит. А, кв.4",
		details: {
			database_id: 21,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №21")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9139178,
		lng: 30.33432117,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">"Набережная Обводного канала д. 66, лит. А, кв.4"<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9139178,
		lng: 30.33432117,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9139178,
		lng: 30.33432117,
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