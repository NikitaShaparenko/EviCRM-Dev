setTimeout(function() {
    $("#subscribeModal").modal("show")
}, 2e3);
var options = {
        chart: {
            height: 360,
            type: "bar",
            stacked: !0,
            toolbar: {
                show: !1
            },
            zoom: {
                enabled: !0
            }
        },
        plotOptions: {
            bar: {
                horizontal: !1,
                columnWidth: "15%",
                endingShape: "rounded"
            }
        },
        dataLabels: {
            enabled: !1
        },
        series: [{
            name: "Заявки. Источник №1",
            data: [0]
        }, {
            name: "Заявки. Источник №2",
            data: [0]
        }, {
            name: "Заявки. Источник №3",
            data: [0]
        }],
        xaxis: {
            categories: ["Янв", "Фев", "Март", "Апр", "Май", "Июнь", "Июль", "Авг", "Сен", "Окт", "Ноя", "Дек"]
        },
        colors: ["#556ee6", "#f1b44c", "#34c38f"],
        legend: {
            position: "bottom"
        },
        fill: {
            opacity: 1
        }
    },
    chart = new ApexCharts(document.querySelector("#stacked-column-chart"), options);
chart.render();
options = {
    chart: {
        height: 200,
        type: "radialBar",
        offsetY: -10
    },
    plotOptions: {
        radialBar: {
            startAngle: 0,
            endAngle: 0,
            dataLabels: {
                name: {
                    fontSize: "13px",
                    color: void 0,
                    offsetY: 60
                },
                value: {
                    offsetY: 22,
                    fontSize: "16px",
                    color: void 0,
                    formatter: function(e) {
                        return e + "%"
                    }
                }
            }
        }
    },
    colors: ["#556ee6"],
    fill: {
        type: "gradient",
        gradient: {
            shade: "dark",
            shadeIntensity: .15,
            inverseColors: !1,
            opacityFrom: 1,
            opacityTo: 1,
            stops: [0, 50, 65, 91]
        }
    },
    stroke: {
        dashArray: 4
    },
    series: [0],
    labels: ["Показатель отработки"]
};
(chart = new ApexCharts(document.querySelector("#radialBar-chart"), options)).render();