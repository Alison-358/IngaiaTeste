$(document).ready(function(){
    if(sessionStorage.getItem("token") == "" || sessionStorage.getItem("token") == null){
        window.location = "login.html";
    }else{
        GetPlaylist();
    }
});

$('#messageModal').on('hidden.bs.modal', function (e) {
    $("#titleMessageModal").html("");
    $("#responseSuccess").css("display","none");
    $("#responseSuccess").html("");
    $("#responseError").css("display","none");
    $("#responseError").html("");
});

$('#modalStatistics').on('hidden.bs.modal', function (e) {
    $("#chartContainer").html("");
});

function GetPlaylist(){
    if($("#filter").val() != ""){
        var options = {};
        options.url = "https://localhost:44372/api/Playlists?filter=" + encodeURIComponent($("#filter").val());
        options.type = "GET";
        options.beforeSend = function (request) {
            request.setRequestHeader("Authorization", "Bearer " + sessionStorage.getItem("token"));
            busyi = new busy_indicator(document.getElementById("busybox"),
            document.querySelector("#busybox div"));
            busyi.show();
        };
        options.dataType = "json";
        options.success = function (data) {
            var datas = data;

            if(data.statusCode == 200){
                if(data.value.length > 0){
                    $("#tbodyTable").html("");
                    var count = 0;
                    var row = "";
                    var rowConcat = "";
                    data.value.forEach(function(element){
                        count = count + 1;
                        row = "<tr>";
                        row += "<td>";
                        row += count;
                        row += "</td>";
                        row += "<td>";
                        row += element.name;
                        row += "</td>";
                        row += "</tr>";
                        rowConcat += row;
                    });
                    $("#tbodyTable").append(rowConcat);
                    count = 0;
                    row = "";
                    rowConcat = "";
                    busyi.hide();
                }
            }else{
                busyi.hide();
                $('#messageModal').modal('show');
                $("#titleMessageModal").html("Error");
                $("#responseError").css("display","block");
                //$("#responseError").html(obj.message);
                $("#responseError").html("Unexpected error");
            }
        };
        options.error = function (request, message, message) {  
            busyi.hide();    

            if(request.responseText == "")
                request.responseText = "Status: " + request.status + " Error: Request failed with server";//GetErrors(request);
            
            $('#messageModal').modal('show');
            $("#titleMessageModal").html("Error");
            $("#responseError").css("display","block");
            $("#responseError").html(request.responseText);
            if(request.status == 401 || request.status == 0){
                sessionStorage.clear("token");
                setTimeout(function()
                { 
                    window.location = "login.html";
                }, 2000);
                return;
            }
        };
        $.ajax(options);
    }else{
        $('#messageModal').modal('show');
        $("#titleMessageModal").html("Error");
        $("#responseError").css("display","block");
        $("#responseError").html("City name is required");
    }
}

function LogOut(){
    if(sessionStorage.getItem("token") != "" && sessionStorage.getItem("token") != null){
        sessionStorage.clear("token");
        window.location = "index.html";
    }
}

function ShowStatistic(){
    $('#modalStatistics').modal('show');

    var options = {};
    options.url = "https://localhost:44372/api/CityLog";
    options.type = "GET";
    options.beforeSend = function (request) {
        request.setRequestHeader("Authorization", "Bearer " + sessionStorage.getItem("token"));
        // busyi = new busy_indicator(document.getElementById("busybox"),
        // document.querySelector("#busybox div"));
        // busyi.show();
    };
    options.dataType = "json";
    options.success = function (data) {

        if(data != null){
            if(data.length > 0){
                var resultPie = [];
                data.forEach(function(element){
                    resultPie.push({y: element.count, name: element.nameWithoutAccent });
                });
                var options = {
                    exportEnabled: false,
                    animationEnabled: true,
                    title:{
                        text: "Cities"
                    },
                    legend:{
                        horizontalAlign: "right",
                        verticalAlign: "center"
                    },
                    data: [{
                        type: "pie",
                        showInLegend: true,
                        toolTipContent: "<b>{name}</b>: {y} (#percent%)",
                        indexLabel: "{name}",
                        legendText: "{name} (#percent%)",
                        indexLabelPlacement: "inside",
                        dataPoints: resultPie
                            //{ y: element.count, name: element.nameWithoutAccent },
                            //{ y: 2599.2, name: "Food" },
                            // { y: 1231.2, name: "Fun" },
                            // { y: 1368, name: "Clothes" },
                            // { y: 684, name: "Others"},
                            // { y: 1231.2, name: "Utilities" }
                    }]
                };
                $("#chartContainer").CanvasJSChart(options);
            }
        }else{
            //busyi.hide();
            setTimeout(function()
            { 
                $('#modalStatistics').modal('hide'); 
            }, 1000);
             
            $('#messageModal').modal('show');
            $("#titleMessageModal").html("Error");
            $("#responseError").css("display","block");
            //$("#responseError").html(obj.message);
            $("#responseError").html("Unexpected error");
        }
    };
    options.error = function (request, status, error) {  
        //busyi.hide(); 
        setTimeout(function()
        { 
            $('#modalStatistics').modal('hide'); 
        }, 1000);               
        $('#messageModal').modal('show');
        $("#titleMessageModal").html("Error");
        $("#responseError").css("display","block");
        $("#responseError").html("Status: " + request.status + " Error: Request failed with server");
        if(request.status == 401 || request.status == 0){
            sessionStorage.clear("token");
            setTimeout(function()
            { 
                window.location = "login.html";
            }, 2000);
            return;
        }
    };
    $.ajax(options);
}

function GetErrors(request){
    
    var htmlText = "";

    switch (request.status) {
        case 0:
            htmlText = "Status: " + request.status + " Error: Request failed with server";
            break;
        case 500:

            htmlText = this.GetMessageError(request);

            break;
        case 504:

            htmlText = this.GetMessageError(request);

            break;
        case 404:

            htmlText = this.GetMessageError(request);

            break;
        default:
            htmlText = "Status: " + request.status + " Error: Request failed with server";
    }

    return htmlText;
}

function GetMessageError (request, node = 'title') {
    if (request.responseText !== null && request.responseText !== undefined) {
        dom_nodes = $($.parseHTML(request.responseText));
        return dom_nodes.filter(node).text();
    }
}