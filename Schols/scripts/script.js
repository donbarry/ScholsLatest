// JavaScript source code
var uri = 'api/scholarships';

$(document).ready(function () {
    //$("#progressbar").progressbar({ value: false });
    //$("#central").attr("href","https://centrallogin.illinoisstate.edu:443/login?service=http://dev21.iwss.ilstu.edu/Index.html");
    $("#loadingdeptspinner").show();
    console.log(Date());
    $("#mycontent").load("/Views/Search.html"); 
    $("#loginprompt").load("/Views/loginpromptmodal.html");
    $("#loadingdeptspinner").show();
    $.getJSON("api/dropdowndata")
        .done(function (data) {
            $('#department').append($('<option>').text("(Any Department)").attr('value', -1));
            $('#college').append($('<option>').text("(Any College)").attr('value', -1));
            $('#schoolyear').append($('<option>').text("(Any School Year)").attr('value', -1));
            console.log(data);
            $.each(data.departments, function (key, item) {
                $('#department').append($('<option>').text(item.FUND_DEPT_DESCR).attr('value', item.FUND_DEPT_ATTRB));
            });
            $.each(data.colleges, function (key, item) {
                $('#college').append($('<option>').text(item.FUND_COLL_DESCR).attr('value', item.FUND_COLL_ATTRB));
            });
            $.each(data.schoolyears, function (key, item) {
                $('#schoolyear').append($('<option>').text(item.USER_CD_DESCR).attr('value', item.USER_CD));
            });
            $("#loadingdeptspinner").hide();
            console.log(Date());
        })
        .fail(function (jqXHR, textStatus, err) {
            $('#department').text('Error: ' + err);
        });
    
    //on page load, check session storage for token. if exist, "login" with token so as to get user details for UI...
    getLogDetails();
});
function checkNull(strg) {
    return ((strg == null || strg == "-1" || strg == "") ? "" : strg);
}
function find() {
    var title = $('#title').val();
    //$("#progressbar").progressbar({ value: false });
    $("#loadingspinner").show();
    var accesstoken = sessionStorage.getItem("accesstoken"); // so that endpoint can know "who's searching"
    $.ajax(
        {
            url: "api/Search",
            type: "POST",
            //contentType: "application/json",
            data: {
                title: $('#title').val(),
                department: $('#department').val(),
                college: $('#college').val(),
                schoolYear: $('#schoolyear').val(),
                major: $('#major').val(),
                undergradGPA: $('#undergradGPA').val(),
                gradGPA: $('#gradGPA').val(),
                highschoolGPA: $('#highschoolGPA').val(),
                keyword: $('#keyword').val()
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
            },
            success: function (data) {
                $("#scholarship").empty();
                $.each(data, function (key, item) {
                    // Add a list item for the product.
                    linkurl = $('<a>', { text: item.FRML_SCHLRSHP_NAME, href: "ScholarshipPage.html?f=" + item.FUND_ACCT.trim() + "&s=" + item.SCHLRSHP_NUM.trim(), target: "_blank" }) //consider accordion
                    linkTD = $('<td>').append(linkurl);
                    if (item.fav == "yes")
                        favTD = $('<td>').append("<span class='glyphicon glyphicon-star pointer' id='favbutton' onclick='unfav($(this)," + item.FUND_ACCT.trim() + ",\"" + item.FRML_SCHLRSHP_NAME + "\")' ></span>");
                    else
                        favTD = $('<td>').append("<span class='glyphicon glyphicon-star-empty pointer' id='favbutton' onclick='fav($(this)," + item.FUND_ACCT.trim() + ",\"" + item.FRML_SCHLRSHP_NAME + "\")' ></span>");
                                             
                    //$('<li>').append(linkurl).appendTo("#scholarship");
                    $('<tr>').append(linkTD).append(favTD).appendTo("#scholarship");
                    //$('#scholarship').append("<li><a href='" + "ScholarshipPage.html" + "'>" + item.SCHLRSHP_TITLE + "</a></li>");
                });
                console.log(data.length);
                var num = data.length;
                //$("#progressbar").progressbar({ value: true });
                $("#loadingspinner").hide();
                $("#searchinfo").text(getSearchString(num));
                $("#msg").text(num + " Found.");
                $("html,body").animate({ scrollTop: $("#scholarship").offset().top - 100 });
                //console.log("Data: " + data + "\nStatus: " + status);
            }
        });
/* previous post. seems no room for beforesend
    $.post("api/Search", //uri + "/post",
    {
        title: $('#title').val(),
        department: $('#department').val(),
        college: $('#college').val(),
        schoolYear: $('#schoolyear').val(),
        major: $('#major').val(),
        undergradGPA: $('#undergradGPA').val(),
        gradGPA: $('#gradGPA').val(),
        highschoolGPA: $('#highschoolGPA').val()
    },
    function (data, status) {
        $("#scholarship").empty();
        $.each(data, function (key, item) {
            // Add a list item for the product.
            linkurl = $('<a>', { text: item.FRML_SCHLRSHP_NAME, href: "ScholarshipPage.html?f=" + item.FUND_ACCT.trim() + "&s=" + item.SCHLRSHP_NUM.trim(), target: "_blank" }) //consider accordion
            linkTD = $('<td>').append(linkurl);
            favTD = $('<td>').append("<span class='glyphicon glyphicon-star-empty pointer' id='favbutton' onclick='fav($(this)," + item.FUND_ACCT.trim() + ")' ></span>");
            //$('<li>').append(linkurl).appendTo("#scholarship");
            $('<tr>').append(linkTD).append(favTD).appendTo("#scholarship");
            //$('#scholarship').append("<li><a href='" + "ScholarshipPage.html" + "'>" + item.SCHLRSHP_TITLE + "</a></li>");
        });
        console.log(data.length);
        var num = data.length;
        //$("#progressbar").progressbar({ value: true });
        $("#searchinfo").text(getSearchString(num));
        $("#msg").text(num + " Found.");
        $("html,body").animate({ scrollTop: $("#scholarship").offset().top - 100 });
        //console.log("Data: " + data + "\nStatus: " + status);
    });
*/
}
function fav(spanref,fundacct,schlrshpname) {
    //console.log("added fav");

    console.log("fav " + fundacct + ":" + schlrshpname);
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax(
            {
                url: "api/addfavorite",//?&username=" + user.username + "&userPassword=" + user.userPassword + "&accessToken=lfkds&refreshToken=krwe",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ "fundacct": fundacct,"schlrshpname":schlrshpname }),         //without stringify i get 415 media unsupported...
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
                },
                success: function (result) {
                    console.log(spanref);
                    console.log(result);
                    spanref.removeClass("glyphicon-star-empty");
                    spanref.addClass("glyphicon-star");
                    spanref.on("click", function () {
                        unfav(spanref, fundacct, schlrshpname);
                    });
                }
            });
    } else {
        $("#loginprompt").modal("show");
    }
}
function unfav(spanref, fundacct, schlrshpname) {
    console.log("unfav " + fundacct + ":" + schlrshpname);
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax(
            {
                url: "api/removefavorite",//?&username=" + user.username + "&userPassword=" + user.userPassword + "&accessToken=lfkds&refreshToken=krwe",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ "fundacct": fundacct, "schlrshpname": schlrshpname }),         //without stringify i get 415 media unsupported...
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
                },
                success: function (result) {
                    console.log(spanref);
                    console.log(result);
                    spanref.removeClass("glyphicon-star");
                    spanref.addClass("glyphicon-star-empty");
                    spanref.on("click",function(){
                        fav(spanref,fundacct,schlrshpname);
                    });
                }
            });
    } else {
        $("#loginprompt").modal("show");
    }
}

function register() {
    var user = { username: $("#username").val(), userpassword: $("#password").val(), fullname: $("#fullname").val(), usermajor: $("usermajor").val() };
    //var user = { username: $("#username").val(), password: $("#password").val(), grant_type: "password" };
    console.log(user);
    $.ajax(
        {
            url: "api/register",//?&username=" + user.username + "&userPassword=" + user.userPassword + "&accessToken=lfkds&refreshToken=krwe",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(user),         //without stringify i get 415 media unsupported...
            success: function (result) {
                $("#registerModal").modal("hide");
                console.log(result);
                $("#login_username").val(result.username);
                $("#login_password").val(result.userpassword);
                login();
            }
        });
    //$.post("api/fakelogin", user
    //        , function (data, status) {
    //            console.log(data);
    //        });
}
function loadprofile() {
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax({
            type: "GET",
            url: 'api/loginwithtoken',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
            },
            success: function (data) {
                if (data.username != undefined && data.username != null) {
                    $("#profile_username").text(data.username);
                    $("#profile_fullname").text(data.fullname);
                    $("#profile_usermajor").text(data.usermajor);

                } else {
                    console.log("bad data return");
                    console.log(data);
                }
            }
        }).fail(function (data) {
            console.log("ajax error");
            console.log(data);
        });
    } else {
        console.log("no access token in session");
        console.log(sessionStorage);
    }
}
function loadfavorites() {
    var accesstoken = sessionStorage.getItem("accesstoken");
    if (accesstoken != undefined && accesstoken != null) {
        $.ajax({
            type: "GET",
            url: 'api/favorites',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
            },
            success: function (data) {
                    $("#scholarship").empty();
                    $.each(data, function (key, item) {
                        // Add a list item for the product.
                        linkurl = $('<a>', { text: item.FRML_SCHLRSHP_NAME, href: "ScholarshipPage.html?f=" + item.FUND_ACCT.trim(), target: "_blank" }) //TODO: + "&s=" + item.SCHLRSHP_NUM.trim()
                        linkTD = $('<td>').append(linkurl);
                        //favTD = $('<td>').append("<span class='glyphicon glyphicon-star-empty pointer' id='favbutton' onclick='fav($(this)," + item.FUND_ACCT.trim() + ",\"" + item.FRML_SCHLRSHP_NAME + "\")' ></span>");
                        //$('<li>').append(linkurl).appendTo("#scholarship");
                        $('<tr>').append(linkTD).appendTo("#scholarship"); //no need fav link for already fav
                        //$('#scholarship').append("<li><a href='" + "ScholarshipPage.html" + "'>" + item.SCHLRSHP_TITLE + "</a></li>");
                    });
                    console.log(data.length);
                    var num = data.length;
                    //$("#progressbar").progressbar({ value: true });
                    //$("#searchinfo").text(getSearchString(num));
                    $("#searchinfo").text("Your Favorite Scholarships : ");
                    $("#msg").text(num + " Found.");
                    $("html,body").animate({ scrollTop: $("#scholarship").offset().top - 100 });
            }
        }).fail(function (data) {
            console.log("ajax error");
            console.log(data);
        });
    } else {
        console.log("no access token in session");
        console.log(sessionStorage);
    }

}

function login() {
    var user = { username: $("#login_username").val(), userpassword: $("#login_password").val() };
    console.log(user);
    $.ajax(
        {
            url: "api/login",//?&username=" + user.username + "&userpassword=" + user.userpassword + "&accesstoken=lfkds&refreshToken=krwe",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(user),
            success: function (result) {
                console.log(result);
                $("#loginModal").modal("hide");
                //$("#usernamespan").text(result.fullname);
                sessionStorage.setItem("accesstoken", result.accesstoken);
                //login_dialog.dialog("close");
                loggedinUI(result.fullname);
            }
        });
}
