var returnResult = new Array();
var startRow = 0, endRow = 0, limitScore = 0;

$(document).ready(function () {
    if (typeof identity != "undefined") {
        $("#identity").html("您的編號為 : " + identity);
    }

    if (typeof(promotionObj) != "undefined") {
        $("#promotion").load("./web/" + promotionObj.getRelativePath(), function () {
            tableInit();
            evnetInit();
            submitInit();
            dialogInit();
        });
    }
});

function tableInit() {
    while (!~$("tr:gt(" + startRow + "):lt(1)").text().replace(/\s+/g, "").indexOf("編號")) {
        startRow++;
    }
    limitScore = Number($("tr:gt(" + startRow + "):lt(1)").find("td:gt(12):lt(1)").text().replace(/[^0-9]/gi, ""));
    startRow += 2;
    endRow = startRow;
    while ($("tr:gt(" + endRow + "):lt(1)").find("td:gt(1):lt(1)").text().replace(/\s+/g, "") != "") {
        $("tr:gt(" + endRow + "):lt(1)").find("td:gt(15):lt(1)").html('<input type="number" step="1" class="score" id="' + endRow + '" style="font-size:24px;width:120px;" />');
        $("tr:gt(" + endRow + "):lt(1)").find("td:gt(16):lt(1)").html('<font color="red">請輸入</font>');
        $("tr:gt(" + endRow + "):lt(1)").find("td:gt(17):lt(1)").html('<font color="red">請輸入</font>');
        endRow++;
    }
    for (var i = startRow; i < endRow; i++) {
        var total = 0;
        for (var j = 4; j <= 8; j++) {
            total += Number($("tr:gt(" + i + "):lt(1)").find("td:gt(" + j + "):lt(1)").text().replace(/\s+/g, ""));
        }
        $("tr:gt(" + i + "):lt(1)").find("td:gt(9):lt(1)").html('<font color="blue">' + parseFloat(total.toFixed(1)) + '</font>');
    }
    // 去除「甄審委員簽名」字樣
    var tempRow = 0;
    while (!~$("tr:gt(" + tempRow + "):lt(1)").text().replace(/\s+/g, "").indexOf("甄審委員簽名")) {
        tempRow++;
    }
    $("tr:gt(" + tempRow + "):lt(1)").empty();
}

function evnetInit() {
    $(".score").keyup(function () {
        $(this).val($(this).val().replace(/[^\d]/g, ""));
        var row = $(this).attr("id");
        if ($(this).val().replace(/\s+/g, "") == "") {
            $("tr:gt(" + row + "):lt(1)").find("td:gt(16):lt(1)").html('<font color="red">請輸入</font>');
            $("tr:gt(" + row + "):lt(1)").find("td:gt(17):lt(1)").html('<font color="red">請輸入</font>');
        } else if ($(this).val() > limitScore) {
            $("tr:gt(" + row + "):lt(1)").find("td:gt(16):lt(1)").html('<font color="red">超出範圍</font>');
            $("tr:gt(" + row + "):lt(1)").find("td:gt(17):lt(1)").html('<font color="red">超出範圍</font>');
        } else {
            var totalA = 0, totalB = 0, totalC = 0;
            totalA = Number($("tr:gt(" + row + "):lt(1)").find("td:gt(9):lt(1)").text().replace(/\s+/g, ""));
            totalB = Number($("tr:gt(" + row + "):lt(1)").find("td:gt(10):lt(1)").text().replace(/\s+/g, "")) + Number($("tr:gt(" + row + "):lt(1)").find("td:gt(11):lt(1)").text().replace(/\s+/g, "")) + Number($(this).val());
            totalC = totalA + totalB;
            $("tr:gt(" + row + "):lt(1)").find("td:gt(16):lt(1)").html('<font color="blue">' + totalB + '</font>');
            $("tr:gt(" + row + "):lt(1)").find("td:gt(17):lt(1)").html('<font color="blue">' + totalC + '</font>');
        }
    });
}

function submitInit() {
    $("#sendButton").click(function () {
        for (var r = startRow; r < endRow; r++) {
            if (~$("tr:gt(" + r + "):lt(1)").text().replace(/\s+/g, "").indexOf("請輸入") || ~$("tr:gt(" + r + "):lt(1)").text().replace(/\s+/g, "").indexOf("超出範圍")) {
                $("tr:gt(" + endRow + "):lt(1)").find("td:gt(2):lt(1)").html('<font color="red">請再次檢查輸入</font>');
                return false;
            }
        }

        var str = "";
        for (var i = startRow; i < endRow; i++) {
            var tempScore = "";
            var resultItem = new Object();
            resultItem.department = $("tr:gt(" + i + "):lt(1)").find("td:gt(2):lt(1)").text().replace(/\s+/g, "");
            resultItem.name = $("tr:gt(" + i + "):lt(1)").find("td:gt(3):lt(1)").text().replace(/\s+/g, "");
            for (var j = 4; j <= 17; j++) {
                if (j === 4) {
                    resultItem.scores = "";
                }
                if (j === 15) {
                    resultItem.scores += $("#" + i).val();
                } else {
                    tempScore = $("tr:gt(" + i + "):lt(1)").find("td:gt(" + j + "):lt(1)").text().replace(/\s+/g, "");
                    resultItem.scores += tempScore;
                }
                if (j !== 17) {
                    resultItem.scores += ",";
                }
            }
            returnResult.push(resultItem);
            str = str + resultItem.department + ":" + resultItem.name + " 您的評分為" + $("#" + i).val() + " 總計<font color=\"blue\">" + tempScore + "</font><br />";
        }

        $("#promotionResult").html(str);
        $("tr:gt(" + endRow + "):lt(1)").find("td:gt(2):lt(1)").html('<font color="red"></font>');
        $("#dialog").dialog("open");
        dialogInit();
    });
}

function dialogInit() {
    $("#dialog").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: "auto",
        height: "auto",
        position: "center",
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
        buttons: {
            "確認": function () {
                if (typeof(promotionObj) != "undefined") {
                    window.webkit.messageHandlers.sendResultData.postMessage(JSON.stringify(returnResult));
                }
            },
            "取消": function () {
                $(this).dialog("close");
                returnResult = new Array();
            }
        }
    }).css("font-size", "32px");
}
