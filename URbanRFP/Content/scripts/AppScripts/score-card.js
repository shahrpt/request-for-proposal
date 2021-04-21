var total_score = 100;

$(document).ready(function () {
    create_score_chart();

    $(".btn-exp-coll").click(function () {
        $(this).find("i").toggleClass("fa-plus");
        $(this).find("i").toggleClass("fa-minus");
        $("#tbl-score-card").toggleClass("collapsed-panel");
    });
});

function create_score_chart() {
    //total_score = 100;
    var chart_data = [];
    
    $(score_rules).each(function (i, e) {
        chart_data.push({ x: parseFloat(e.eva_point_max), y: parseFloat(e.eva_weight), label: e.eva_criteria_name });
        //total_score -= parseFloat(e.eva_weight);
    });

    //chart_data.push({x: 0, y: total_score, label: "", color: "white" });

    var chart = new CanvasJS.Chart("chartContainer", {
        animationEnabled: true,
        title: {
            text: "",
            horizontalAlign: "left"
        },
        legend: {
            fontFamily: "ibm_plex_sansregular",
            fontWeight: "normal",
            fontSize: 14,
            fontColor: "#403f2d",
            fontStyle: "normal"
        },
        data: [{
            type: "doughnut",
            startAngle: -90,
            innerRadius: 50,
            indexLabelFontSize: 14,
            indexLabelFontFamily: "ibm_plex_sansregular",
            indexLabel: "{label} ({y})",
            toolTipContent: "{label} ({y})",
            xValueType: "number",
            dataPoints: chart_data
        }]
    });

    chart.render();
}