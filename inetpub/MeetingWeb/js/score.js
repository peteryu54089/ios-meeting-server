var scores = [];
var error = 0, sum = 0, passScore = 0;
var defaultValue;
var averagePosition, totalPosition;

var teachingMin = 0, teachingMax = 0, serviceMin = 0, serviceMax = 0; // 教學評分範圍, 服務評分範圍
var teachingScore = 0, serviceScore = 0; // 教學評分, 服務評分

$(function () {
    dialogInit();

    if (typeof identity != "undefined") {
        $("#identity").html("您的編號為 : " + identity);
    }

    if (typeof scoreObj != "undefined") {
        $("#vote").load("./web/" + scoreObj.getRelativePath(), function () {
            setPassScore();
            inputInit();
            checkboxInit();
            appendReason();
            eventInit();
            refresh();

            defaultValue = $(".cleardefault").val();

            $(".cleardefault").focus(function () {
                if (defaultValue == $(this).val()) {
                    $(this).val("");
                }
            });
        });
    }
});

function setPassScore() {
    $("td:contains('職稱')").next().find('span:first').show();

    if ($("td:contains('送審職級')").next().text() === "副教授") {
        passScore = 78;
    }
    if ($("td:contains('送審職級')").next().text() === "教授") {
        passScore = 80;
    }
    if ($("td:contains('送審職級')").next().text() === "教授級專業技術人員") {
        passScore = 80;
    }
    if ($("td:contains('送審職級')").next().text() === "副教授級專業技術人員") {
        passScore = 78;
    }
    if ($("td:contains('送審職級')").next().text() === "兼任教授") {
        passScore = 80;
    }
    if ($("td:contains('送審職級')").next().text() === "兼任副教授") {
        passScore = 78;
    }
}

function inputInit() {
    index = 0;

    var counter = 0;
    $("#vote").find("table td").each(function () {
        $(this).css("border-width", "1px").css("padding", "7px").css("font-weight", "bold");

        if ($(this).css("border-top-color") == "rgb(255, 0, 0)") {
            $(this).css("border-width", "2px").css("font-weight", "normal");

            input_id = "arg" + index++;
            range = $(this).html();
            min = range.split('~')[0];
            max = range.split('~')[1];
            input = '<input autocomplete="off" type="number" name="' + input_id + '" id="' + input_id + '" title="' + range + '" min="' + min + '" max="' + max + '" step="0.01" value="" style="width: 70%;" required />';

            if (counter === 0) {
                teachingMin = min;
                teachingMax = max;
                counter++;
            } else {
                serviceMin = min;
                serviceMax = max;
            }

            $(this).css("border-color", "red").html(input);
        }
    });

    $('input[type="number"]').keyup(valueChanged);
    $('input[type="number"]').change(valueChanged);
}

function valueChanged() {
    refresh();

    if (!$(this)[0].validity.valid) {
        msg = $('<div class="messagebox" id="' + $(this).attr("id").replace("arg", "msg") + '">' + "<b> 評分範圍為: " + $(this).attr("title") + ' 分 </b></div>').hide();
        $(this).bubbletip(msg, { deltaDirection: "left", bindShow: "blur", bindHide: "focus" });
    } else {
        $(this).removeBubbletip();
    }
}

function refresh() {
    error = 0, sum = 0;
    scores = [];

    if (!isNaN(parseFloat($(".X64").html()))) {
        averagePosition = ".X64";
        totalPosition = ".X55";
    }
    if (!isNaN(parseFloat($(".X67").html()))) {
        averagePosition = ".X67";
        totalPosition = ".X58";
    }
    if (!isNaN(parseFloat($(".X68").html()))) {
        averagePosition = ".X68";
        totalPosition = ".X59";
    }

    sum = parseFloat($(averagePosition).html());

    var counter = 0;
    $('input[type="number"]').each(function () {
        var val = parseFloat($(this).val());

        if (counter === 0) {
            teachingScore = val;
            counter++;
        } else {
            serviceScore = val;
        }

        if (!isNaN(val)) {
            sum += val;
        }

        scores.push(isNaN(val) ? -1 : val);

        if (!$(this)[0].validity.valid) {
            error++;
        }

        var score = Math.round(sum * 100) / 100;
        var color = score < passScore || score > 100 ? "red" : "blue";

        $("#score").text(score);
        $(totalPosition).html('<b><font color="' + color + '">' + score + '</font></b>').css('font-size', '32px');
    });

    scores.push(error);
}

function checkboxInit() {
    index = 0;

    $('td:contains("□")').each(function () {
        var text = $(this).html().replace('□', '');
        $(this).html('<input type="checkbox" id="check' + index + '"><label for="check' + index++ + '"></label>' + text);
    });

    $('td:contains("___________________")').each(function () {
        var html = $(this).html().replace('___________________', '<input type="text" id="arg' + index++ + '" tabindex="-1" value="___________________" class="cleardefault">');
        $(this).html(html);
    });
}

function appendReason() {
    var start = $("tr:contains('教師升等未過理由勾選')").index();
    var end = $("tr:contains('備註：')").index();
    var size = end - start;

    for (i = 0; i < size; i++) {
        $($("tr")[start]).find("td").removeClass();
        $($("tr")[start]).appendTo("#reason_table");
    }
}

function eventInit() {
    $("#sendButton").click(function () {
        $("#dialog").dialog("open");
        if (error != 0) {
            $("#total").hide();
            $("#error").html("您輸入的教學評分為 " + (isNaN(teachingScore) ? "0" : teachingScore) + "，服務評分為 " + (isNaN(serviceScore) ? "0" : serviceScore) + "，總分為 " + (Math.round(sum * 100) / 100) + "，您輸入的教學(服務)評分低於(超過)最低(高)分數範圍" + teachingMin + "~" + teachingMax + "分(" + serviceMin + "~" + serviceMax + "分)，本評分表將以廢票計，且依本校教師評審委員會設置辦法第6條規定，<font color='red'>廢票視為不同意</font>，是否確認送出？");
            $("#error").show();
            $("#confirm").hide();
        } else {
            $("#total").show();
            $("#error").hide();
            $("#confirm").show();
        }
        return false;
    });
}

function dialogInit() {
    $("#comformDialog").dialog({ autoOpen: false, modal: true,
        buttons: {
            "確認": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#reason_ck").dialog({ autoOpen: false, modal: true, width: "300px", position: "center",
        buttons: {
            "確認": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#reason_dialog").dialog({ autoOpen: false, modal: true, width: "1200px", position: "center",
        buttons: {
            "確認": function () {
                if($("#check9").prop("checked")) {
                    var value = $(".cleardefault").val();

                    if(value == "___________________" || value == "") {
                        $("#reason_ck").dialog('open');
                        return;
                    }
                }

                var reasons = [];

                $('input[type=checkbox]:checked').each(function () {
                    var other = $(this).parent().find('input[type=text]');
                    var msg = $(this).parent().text().replace(/\s/g, '');

                    if (other.length != 0) {
                        msg += $(other).val();
                    }
                    
                    reasons.push(msg);
                    console.log(reasons);
                });

                if (typeof scoreObj != "undefined" && reasons.length != 0) {
                    window.webkit.messageHandlers.sendResultData.postMessage("\"Scores\":" + JSON.stringify(scores) + ",\"Reasons\":" + JSON.stringify(reasons) + "}");
                    scoreObj.sendJsonScore(JSON.stringify(scores), JSON.stringify(reasons));
                } else {
                    $("#comformDialog").dialog("open");
                }
            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialog").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: "650px",
        position: "center",
        open: function (event, ui) { $(".ui-dialog-titlebar-close").hide(); },
        buttons: {
            "確認": function () {
                if (error != 0 || sum < passScore) {
                    $("#reason_dialog").dialog("open");
                } else if (typeof scoreObj != "undefined") {
                    window.webkit.messageHandlers.sendResultData.postMessage("\"Scores\":" + JSON.stringify(scores) + ",\"Reasons\":[]}");
                    scoreObj.sendJsonScore(JSON.stringify(scores), JSON.stringify([]));
                }
            },
            "取消": function () {
                $(this).dialog("close");
            }
        }
    }).css("font-size", "32px");
}
