function sliceSize(dataNum, dataTotal) {
    return (dataNum / dataTotal) * 360;
}
function addSlice(sliceSize, pieElement, offset, sliceID, color) {
    $(pieElement).append("<div class='slice " + sliceID + "'><span></span></div>");
    var offset = offset - 1;
    var sizeRotation = -179 + sliceSize;
    $(pieElement+ " ." + sliceID).css({
        "transform": "rotate(" + offset + "deg) translate3d(0,0,0)"
    });
    $(pieElement + " ." + sliceID + " span").css({
        "transform": "rotate(" + sizeRotation + "deg) translate3d(0,0,0)",
        "background-color": color
    });
}
function iterateSlices(sliceSize, pieElement, offset, dataCount, sliceCount, color) {
    var sliceID = "s" + dataCount + "-" + sliceCount;
    var maxSize = 179;
    if (sliceSize <= maxSize) {
        addSlice(sliceSize, pieElement, offset, sliceID, color);
    } else {
        addSlice(maxSize, pieElement, offset, sliceID, color);
        iterateSlices(sliceSize - maxSize, pieElement, offset + maxSize, dataCount, sliceCount + 1, color);
    }
}
function createPie(dataElement, pieElement, showTotal = true) {
    var listData = [];
    $(dataElement + " span").each(function () {
        listData.push(Number($(this).html()));
    });
    var listTotal = 0;
    for (var i = 0; i < listData.length; i++) {
        listTotal += listData[i];
    }
    var offset = 0;
    var color = [
        "#F5A623",
        "#6DC30F",
        "#F7464A",
        "#46BFBD",
        "#FDB45C",
        "#949FB1",
        "#4D5360"
    ];
    var size = sliceSize(listData[i], listTotal);
    listData.reverse();

    for (var i = 0; i < listData.length; i++) {
        var size = sliceSize(listData[i], listTotal);
        iterateSlices(size, pieElement, offset, i, 0, color[i]);
        $(dataElement + " li:nth-child(" + (i + 1) + ")").css("border-color", color[i]);
        offset += size;
    }

    if (showTotal) $('head').append("<style>" + pieElement + "::before{content:'" + listTotal + "';}</style>");
    else $('head').append("<style>" + pieElement + "::before{content:' ';}</style>");
}
$(document).ready(function () {
    createPie(".govt.rfp-pieID.legend", ".govt.rfp-pieID.pie", true);
    createPie(".vendor.rfp-pieID.legend", ".vendor.rfp-pieID.pie", false);
    createPie(".rfq-pieID.legend", ".rfq-pieID.pie-mini", false);
    createPie(".prop-pieID.legend", ".prop-pieID.pie", false);
});
