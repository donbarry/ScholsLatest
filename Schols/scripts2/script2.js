// JavaScript source code
var uri = 'api/scholarships';

//$(document).ready(function () {
    //$("#progressbar").progressbar({ value: false });
    //$("#central").attr("href","https://centrallogin.illinoisstate.edu:443/login?service=http://dev21.iwss.ilstu.edu/Index.html");

    //on page load, check session storage for token. if exist, "login" with token so as to get user details for UI...

//});
function getApplication() {
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax(
            {
                url: "api/getapplication",//?&username=" + user.username + "&userPassword=" + user.userPassword + "&accessToken=lfkds&refreshToken=krwe",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    fund_acct: $("#fund_acct").val()
                }),         //without stringify i get 415 media unsupported...
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
                },
                success: function (result) {
                    if (result.
                    $("#applicationform").hide();
                    $("#applied").show();
                    console.log(spanref);
                    console.log(result);
                }
            });
    } else {
        $("#loginprompt").modal("show");
    }

}
function apply() {
    
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax(
            {
                url: "api/apply",//?&username=" + user.username + "&userPassword=" + user.userPassword + "&accessToken=lfkds&refreshToken=krwe",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    firstname: $('#firstname').val(),
                    middlename: $('#middlename').val(),
                    lastname: $('#lastname').val(),
                    email: $('#email').val(),
                    universityid: $('#universityid').val(),
                    address: $('#address').val(),
                    phonenumber: $('#phonenumber').val(),
                    essayfilename: $("#essayfilename").val(),
                    reffilename: $("#reffilename").val(),
                    fund_acct: $("#fund_acct").val(),
                    scholarshipyear: $("#scholarshipyear").val()
                }),         //without stringify i get 415 media unsupported...
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
                },
                success: function (result) {
                    $("#applicationform").hide();
                    $("#applied").show();
                    console.log(spanref);
                    console.log(result);
                }
            });
    } else {
        $("#loginprompt").modal("show");
    }
}
/*
$('#myfile').fileupload({
    url: 'api/fileupload',
    add: function (e, data) {
        console.log('add', data);
        $('#progressbar').show();
        data.submit();
    },
    progress: function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progressbar div').css('width', progress + '%');
    },
    success: function (response, status) {
        $('#progressbar').hide();
        $('#progressbar div').css('width', '0%');
        console.log('success', response);
    },
    error: function (error) {
        $('#progressbar').hide();
        $('#progressbar div').css('width', '0%');
        console.log('error', error);
    }
});
*/