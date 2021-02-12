var returnResult = new Array();
var index = 0, count = 0, group = 0, previousCol = 0, errorRow = 0, additional = 0;

$(document).ready(function () {
    dialogInit();
    window.webkit.messageHandlers.sendLog.postMessage("get!!!");

    if (typeof identity != "undefined") {
        $("#identity").html("您的編號為 : " + identity);
    }

    if (typeof (voteObj) != "undefined") {
        $("#vote").load("./web/" + voteObj.getRelativePath(), function () {
            if (orderBy) {
                tableInit();
                eventInit();
            }
            else {
                tableInitOrderByFalse();
                eventInitOrderByFalse();
            }
            dialogInit();
        });
    }
});

function tableInit() {
    while (1) {
        index++;
        if (count === 0 && !~$("tr:gt(" + index + "):lt(1)").text().replace(/\s+/g, "").indexOf('單位')) {
            continue;
        } else if (count === 0) {
            index++;
        }
        if (~$("tr:gt(" + index + "):lt(1)").text().replace(/\s+/g, "").indexOf('備註')) {
            errorRow = ++index;
            break;
        }
        if ($("tr:gt(" + index + "):lt(1) td").size() < previousCol) {
            $("tr:gt(" + index + "):lt(1)").find("td:gt(4):lt(1)").html('<input type="checkbox" id="agree' + count + '" /><label for="agree' + count + '"></label>');
            $("tr:gt(" + index + "):lt(1)").find("td:gt(5):lt(1)").html('<input type="checkbox" id="reject' + count + '" /><label for="reject' + count + '"></label>');
            $("tr:gt(" + index + "):lt(1)").find("td:gt(6):lt(1)").html('<input type="number" step="1" class="group' + group + '" id="rank' + count + '" style="font-size:24px;width:120px;" required />');
            count++;
        } else if ($("tr:gt(" + index + "):lt(1)").find("td:gt(2):lt(1)").text().includes("名")) {
            if (previousCol != 0) {
                group++;
            }
            $("tr:gt(" + index + "):lt(1)").find("td:gt(5):lt(1)").html('<input type="checkbox" id="agree' + count + '" /><label for="agree' + count + '"></label>');
            $("tr:gt(" + index + "):lt(1)").find("td:gt(6):lt(1)").html('<input type="checkbox" id="reject' + count + '" /><label for="reject' + count + '"></label>');
            $("tr:gt(" + index + "):lt(1)").find("td:gt(7):lt(1)").html('<input type="number" step="1" class="group' + group + '" id="rank' + count + '" style="font-size:24px;width:120px;" required />');
            previousCol = $("tr:gt(" + index + "):lt(1) td").size();
            count++;
        } else if (additional === 0) {
            additional = index;
        }
    }

    $('input[type=number]').prop("disabled", !orderBy);
    $('input[type=number]').val("");
    for (var i = 0; i <= group; i++) {
        var total = 0;
        $(".group" + i).each(function () {
            total++;
        });
        if (total <= 1) {
            $(".group" + i).prop("disabled", true);
        }
    }

    $('input[type=checkbox]').click(function () {
        if (~$(this).attr("id").indexOf("agree")) {
            var limit = 0;
            var agreeTotal = 0;

            var first = true;
            var groupId = $(this).closest("tr").find("input[class*=group]").attr("class").replace(/[^0-9]/ig, "");

            $(".group" + groupId).each(function () {
                if (first) {
                    limit = Number($(this).closest("tr").find("td:gt(2):lt(1)").text().replace(/[^0-9]/ig, ""));
                    first = false;
                }
                if ($(this).closest("tr").find("input[id*=agree]").prop("checked")) {
                    agreeTotal++;
                }
            });

            if (agreeTotal > limit) {
                //$(this).prop("checked", false);
                $("#" + $(this).attr("id").replace("agree", "reject")).prop("checked", false);
            } else if ($(this).prop("checked") && $("#" + $(this).attr("id").replace("agree", "reject")).prop("checked")) {
                $("#" + $(this).attr("id").replace("agree", "reject")).prop("checked", false);
            }
        } else if (~$(this).attr("id").indexOf("reject")) {
            if ($(this).prop("checked") && $("#" + $(this).attr("id").replace("reject", "agree")).prop("checked")) {
                $("#" + $(this).attr("id").replace("reject", "agree")).prop("checked", false);
            }
        }
    });
}

function eventInit() {
    $('#sendButton').click(function () {
        var checkboxIndex = 0;

        for (var i = 0; i <= group; i++) {
            var total = 0;
            var agreeCount = 0;
            var tempCheckboxIndex = checkboxIndex;

            $(".group" + i).each(function () {
                total++;
                if ($("#agree" + tempCheckboxIndex).prop("checked")) {
                    agreeCount++;
                }
                tempCheckboxIndex++;
            });

            var isValid = true, tempValue = [];
            $(".group" + i).each(function () {
                for (var j = 1; j <= total; j++) { // 檢查是否在範圍內
                    if ($(this).val() == j) {
                        break;
                    }
                    if (j === total) {
                        isValid = false;
                    }
                }
                tempValue.push($(this).val());
            });
            if (!isValid && total > 1) { // 輸出錯誤訊息
                $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(0):lt(1)").html('<font color="red">序位輸入有誤</font>');
                return false;
            }
            isValid = true;
            tempCheckboxIndex = checkboxIndex;
            $(".group" + i).each(function () { // 檢查是否不同意名次小於同意名次
                if ($("#reject" + tempCheckboxIndex).prop("checked") && $(this).val() <= agreeCount) {
                    isValid = false;
                }
                tempCheckboxIndex++;
            });
            if (!isValid && total > 1) { // 輸出錯誤訊息
                $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(0):lt(1)").html('<font color="red">不同意者的名次不可在同意者的名次之前</font>');
                return false;
            }
            isValid = true;
            $(".group" + i).each(function () {
                var tmp = 0;
                for (var j = 0; j < tempValue.length; j++) { // 檢查是否重複
                    if ($(this).val() == tempValue[j]) {
                        tmp++;
                    }
                }
                if (tmp > 1) {
                    isValid = false;
                }
            });
            if (!isValid && total > 1) { // 輸出錯誤訊息
                $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(0):lt(1)").html('<font color="red">序位輸入重複</font>');
                return false;
            }

            checkboxIndex += total;
        }

        for (var i = 0; i < count; i++) {
            if (!$("#agree" + i).prop("checked") && !$("#reject" + i).prop("checked")) {
                $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(0):lt(1)").html('<font color="red">每個選項皆要勾選</font>');
                return false;
            }
        }

        $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(0):lt(1)").html('<font color="red"></font>');
        $('#dialog').dialog('open');

        var resultItem;
        var checkIndex = 0, checkCount = 0, checkPreviousCol = 0;
        while (1) {
            checkIndex++;
            if (checkCount === 0 && !~$("tr:gt(" + checkIndex + "):lt(1)").text().replace(/\s+/g, "").indexOf('單位')) {
                continue;
            } else if (checkCount === 0) {
                checkIndex++;
            }
            if (checkIndex === additional || ~$("tr:gt(" + checkIndex + "):lt(1)").text().replace(/\s+/g, "").indexOf('備註')) {
                break;
            }
            resultItem = new Object();
            resultItem.department = $("tr:gt(" + checkIndex + "):lt(1)").find("td:gt(0):lt(1)").text();
            resultItem.unit = $("tr:gt(" + checkIndex + "):lt(1)").find("td:gt(1):lt(1)").text();
            resultItem.name = "";
            resultItem.orderBy = true;
            resultItem.agreeChoose = false;
            resultItem.rejectChoose = false;
            resultItem.rank = $("#rank" + checkCount).val() == "" ? "X" : $("#rank" + checkCount).val();
            if ($("#agree" + checkCount).prop("checked")) {
                resultItem.agreeChoose = true;
            } else if ($("#reject" + checkCount).prop("checked")) {
                resultItem.rejectChoose = true;
            }
            if ($("tr:gt(" + checkIndex + "):lt(1) td").size() < checkPreviousCol) {
                resultItem.name = $("tr:gt(" + checkIndex + "):lt(1)").find("td:gt(2):lt(1)").text();
            } else {
                resultItem.name = $("tr:gt(" + checkIndex + "):lt(1)").find("td:gt(3):lt(1)").text();
                checkPreviousCol = $("tr:gt(" + checkIndex + "):lt(1) td").size();
            }
            returnResult.push(resultItem);
            checkCount++;
        }
        for (var i = 0; i < returnResult.length; i++) {
            if (returnResult[i] == null) {
                returnResult.splice(i, 1);
            }
        }
        dialogInit();
    });
}

function dialogInit() {
    $('#dialog').dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: '900px',
        position: 'center',
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
        buttons: {
            "確認": function () {
                if (typeof (voteObj) != "undefined") {
                    window.webkit.messageHandlers.sendResultData.postMessage(JSON.stringify(returnResult));
                    voteObj.sendJsonVote(JSON.stringify(returnResult));
                }
            },
            "取消": function () {
                $(this).dialog("close");
                returnResult = new Array();
            }
        }
    }).css('font-size', '32px');
}

function tableInitOrderByFalse() {
    while (1) {
        index++;
        if (count === 0 && !~$("tr:gt(" + index + "):lt(1)").text().replace(/\s+/g, "").indexOf('同意')) {
            continue;
        } else if (count === 0) {
            index++;
        }
        if ($("tr:gt(" + index + "):lt(1)").text().replace(/\s+/g, "") == "") {
            errorRow = index;
            break;
        }
        $("tr:gt(" + index + "):lt(1)").find("td:gt(3):lt(1)").html('<input type="checkbox" id="agree' + count + '" /><label for="agree' + count + '"></label>');
        $("tr:gt(" + index + "):lt(1)").find("td:gt(4):lt(1)").html('<input type="checkbox" id="reject' + count + '" /><label for="reject' + count + '"></label>');
        count++;
    }
}

function eventInitOrderByFalse() {
    $('input[type=checkbox]').click(function () {
        if (~$(this).attr("id").indexOf("agree")) {
            if ($(this).prop("checked") && $("#" + $(this).attr("id").replace("agree", "reject")).prop("checked")) {
                $("#" + $(this).attr("id").replace("agree", "reject")).prop("checked", false);
            }
        } else if (~$(this).attr("id").indexOf("reject")) {
            if ($(this).prop("checked") && $("#" + $(this).attr("id").replace("reject", "agree")).prop("checked")) {
                $("#" + $(this).attr("id").replace("reject", "agree")).prop("checked", false);
            }
        }
    });

    $('#sendButton').click(function () {
        for (var i = 0; i < count; i++) {
            if (!$("#agree" + i).prop("checked") && !$("#reject" + i).prop("checked")) {
                $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(2):lt(1)").html('<font color="red">每個選項皆要勾選</font>');
                return false;
            }
        }
        $("tr:gt(" + errorRow + "):lt(1)").find("td:gt(2):lt(1)").html('<font color="red"></font>');
        $('#dialog').dialog('open');
        var str = "";
        var resultItem;
        var checkIndex = 0, checkCount = 0;
        while (1) {
            checkIndex++;
            if (checkCount === 0 && !~$("tr:gt(" + checkIndex + "):lt(1)").text().replace(/\s+/g, "").indexOf('同意')) {
                continue;
            } else if (checkCount === 0) {
                checkIndex++;
            }
            if ($("tr:gt(" + checkIndex + "):lt(1)").text().replace(/\s+/g, "") == "") {
                break;
            }
            resultItem = new Object();
            resultItem.name = $("tr:gt(" + checkIndex + "):lt(1)").find("td:gt(2):lt(1)").text().replace(/\s+/g, "");
            resultItem.orderBy = false;
            resultItem.agreeChoose = false;
            resultItem.rejectChoose = false;
            str = str + (checkCount + 1).toString() + ". " + resultItem.name + " ";
            if ($("#agree" + checkCount).prop("checked")) {
                str += " <font color=\"blue\">同意</font><br />";
                resultItem.agreeChoose = true;
            }
            else if ($("#reject" + checkCount).prop("checked")) {
                str += " <font color=\"red\">不同意</font><br />";
                resultItem.rejectChoose = true;
            }
            returnResult.push(resultItem);
            checkCount++;
        }
        $("#voteResult").html(str);
        dialogInit();
    });
}

