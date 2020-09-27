$(document).ready(function(){
    if(sessionStorage.getItem("token") != "" && sessionStorage.getItem("token") != null){
        window.location = "index.html";
    }
});

$("#btnLogin").on("click", function(){
    Login();
})

$('#messageModal').on('hidden.bs.modal', function (e) {
    $("#titleMessageModal").html("");
    $("#responseSuccess").css("display","none");
    $("#responseSuccess").html("");
    $("#responseError").css("display","none");
    $("#responseError").html("");
});

function Login(){
	if($("#email").val() != "" && $("#password").val() != ""){
		var options = {};
        options.url = "https://localhost:44372/api/Login";
        options.type = "POST";
        var obj = {};
        obj.email = $("#email").val();
        obj.password = $("#password").val();
        options.data = JSON.stringify(obj);
        options.contentType = "application/json";
        options.dataType = "json";
        options.success = function (obj) {
            if(obj.authenticated){
                sessionStorage.setItem("token", obj.accessToken);
                $('#messageModal').modal('show');
                $("#titleMessageModal").html("Success");
                $("#responseSuccess").css("display","block");
                $("#responseSuccess").html("User successfully authenticated");
                setTimeout(function()
                { 
                    window.location = "index.html";
                }, 2000);
            }else{
                $('#messageModal').modal('show');
                $("#titleMessageModal").html("Error");
                $("#responseError").css("display","block");
                //$("#responseError").html(obj.message);

                if(obj.message == "" || obj.message == null)
                    obj.message = "Unauthenticated user";
                    
                $("#responseError").html(obj.message);
            }           
        };
        options.error = function (request, status, error) {

            if(request.responseText == "")
                request.responseText = "Status: " + request.status + " Error: Request failed with server";

            $('#messageModal').modal('show');
            $("#titleMessageModal").html("Error");
            $("#responseError").css("display","block");
            $("#responseError").html(request.responseText);
        };
        $.ajax(options);
	}else{
        $('#messageModal').modal('show');
        $("#titleMessageModal").html("Error");
        $("#responseError").css("display","block");
        $("#responseError").html("Email and Password is required");
	}
}