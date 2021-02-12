var returnResult = new Array();
var titleRow = 6;

$(document).ready(function () {
    if (typeof identity != "undefined") {
        $("#identity").html("您的編號為 : " + identity);
    }

	if (typeof voteObj != "undefined") {
		$("#vote").load("./web/" + voteObj.getRelativePath(), function() {
            evnetInit();
            dialogInit();
			tableInit();
		});
	}
});

function filterSpace() {
    return $(this).find("td:eq(1)").html() != "&nbsp;" && $(this).find("td:eq(1)").html() != "備註：" && $(this).find("td:eq(1)").html() != "請於圈選欄內擇一打ˇ，不得複選。";
}

function tableInit() {
    var index = 0;
    var length = $("tr:gt(" + titleRow + ")").filter(filterSpace).length;
    $("table").css("margin", "auto");
    $("tr:gt(" + titleRow + "):lt(" + length + "):even td").css("background-color","#ccc");
	
    $("tr:gt(" + titleRow + "):lt(" + length + ")").find("td:gt(1):lt(" + titleRow + ")").each(function() {
        var row_index = $(this).parent().index("tr");
        var col_index = $(this).index("tr:eq(" + row_index + ") td");
        $(this).html("<input type='checkbox' id='check" + index + "' /><label for='check" + index + "'></label>");		 
        index++;
    }); 
    
    $("input[type=checkbox]").click(function(){
        var check = $(this).prop("checked");          
        $("input[type=checkbox]").prop("checked",false);
        $(this).prop("checked", check);
    });
}

function evnetInit() {
	$("#sendButton").click(function() {
		$("#dialog").dialog("open");
		var length = $("tr:gt(" + titleRow + ")").filter(filterSpace).length;
		var str = "";
        var index = 0;
		var resultItem;
		$("tr:gt(" + titleRow + "):lt(" + length  + ")").each(function() {
            resultItem = new Object();
            resultItem.name = $(this).find("td:eq(1)").text();
            resultItem.agreeChoose = false;
            resultItem.rejectChoose = false;
            if ($(this).find("input[type=checkbox]:checked").length != 1) { 
                resultItem.rejectChoose = true;
            } else {
               str += resultItem.name;
               str += "<font color='green'>同意</font><br/>";
               resultItem.agreeChoose = true;
			} 
			returnResult[index++] = resultItem;
		});
        $("#voteResult").html(str);
        dialogInit();
	});
}

function dialogInit() {
	$('#dialog').dialog({
		autoOpen: false,
		modal: true,
		resizable: false,
		width: "900px",
		position: "center",
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
		buttons: {
			"確認": function () {
				if (typeof voteObj != "undefined") {
                    console.log(JSON.stringify(returnResult));
                    //swift
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
