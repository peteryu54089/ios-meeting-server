var returnResult = new Array();
var startRow = 0, endRow = 0, limitScore = 0, isError = false;

$(document).ready(function () {
    if (typeof identity != "undefined") {
        $("#identity").html("您的編號為 : " + identity);
    }

    if (typeof assessmentObj != "undefined") {
        $("#assessment").load("./web/" + assessmentObj.getRelativePath(), function () {
            tableInit();
            eventInit();
            submitInit();
            dialogInit();
        });
    }
});

function tableInit() {
    while (!~$("tr:gt(" + startRow + "):lt(1)").text().replace(/\s+/g, "").indexOf("編號")) {
        startRow++;
    }
    
    limitScore = Number($("tr:gt(" + startRow + "):lt(1)").find("td:gt(2):lt(1)").text().replace(/[^0-9]/gi, ""));
    startRow++;
    endRow = startRow;
    
    while ($("tr:gt(" + endRow + "):lt(1)").find("td:gt(1):lt(1)").text().replace(/\s+/g, "") != "") {
        $("tr:gt(" + endRow + "):lt(1)").find("td:gt(2):lt(1)").html('<input type="number" step="1" class="score" id="' + endRow + '" style="font-size:24px;width:120px;" />');
        endRow++;
    }
}

function eventInit() {
    $(".score").keyup(function () {
        $(this).val($(this).val().replace(/[^\d]/g, ""));
    });
}

function submitInit() {
    $("#sendButton").click(function () {
        isError = false;
        returnResult = new Array();

        var row = startRow;
        
        $("input[type=number]").each(function () {
            var resultItem = new Object();

            resultItem.department = $("tr:gt(" + row + "):lt(1)").find("td:gt(0):lt(1)").text();
            resultItem.name = $("tr:gt(" + row + "):lt(1)").find("td:gt(1):lt(1)").text();
            resultItem.scores = $(this).val();

            returnResult.push(resultItem);
            row++;

            if ($(this).val() === "" || $(this).val() > limitScore) {
                isError = true;
            }
        });

        $("#assessmentResult").html(isError ? "輸入不合法，請再次檢查" : "是否確認送出?");
        $("#dialog").dialog("open");
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
                if (!isError && typeof assessmentObj != "undefined") {
                    window.webkit.messageHandlers.sendResultData.postMessage(JSON.stringify(returnResult));
                }
                $(this).dialog("close");
            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    }).css("font-size", "32px");
}
