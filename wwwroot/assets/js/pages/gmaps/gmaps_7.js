var map;
$(document).ready(function () {
	(map = new GMaps({
		div: "#gmaps-markers",
		lat: 59.9099365,	
		lng: 30.27597332,
	})).addMarker({
		lat: 59.9099365,
		lng: 30.27597332,
		title: "набережная Обводного канала, дом 211-213, литера А, кв. 3",
		details: {
			database_id: 7,
			author: "Админ"
		},
		click: function (a) {
			console.log && console.log(a), alert("Нажата метка №7")
		}
	}), (map = new GMaps({
		div: "#gmaps-overlay",
		lat: 59.9099365,
		lng: 30.27597332,
	})).drawOverlay({
		lat: map.getCenter().lat(),
		lng: map.getCenter().lng(),
		content: '<div class="gmaps-overlay">набережная Обводного канала, дом 211-213, литера А, кв. 3<div class="gmaps-overlay_arrow above"></div></div>',
		verticalAlign: "top",
		horizontalAlign: "center"
	}), map = GMaps.createPanorama({
		el: "#panorama",
		lat: 59.9099365,
		lng: 30.27597332,
	}), (map = new GMaps({
		div: "#gmaps-types",
		lat: 59.9099365,
		lng: 30.27597332,
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