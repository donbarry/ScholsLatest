function getSearchString(num) {
    var search = "";
    var title = checkNull($("#title").val());
    var department = checkNull($("#department").val());
    var college = checkNull($("#college").val());
    var schoolyear = checkNull($("#schoolyear").val());
    var major = checkNull($("#major").val());
    var undergradGPA = checkNull($("#undergradGPA").val());
    var gradGPA = checkNull($("#gradGPA").val());
    var highschoolGPA = checkNull($("#highschoolGPA").val());
    var keyword = checkNull($("#keyword").val());
    if (title != "") search += (title + ",");
    if (department != "") search += (department + ",");
    if (college != "") search += (college + ",");
    if (schoolyear != "") search += (schoolyear + ",");
    if (major != "") search += (major + ",");
    if (undergradGPA != "") search += (undergradGPA + ",");
    if (gradGPA != "") search += (gradGPA + ",");
    if (highschoolGPA != "") search += (highschoolGPA + ",");
    if (keyword != "") search += (keyword + ",");
    search = search.substring(0, search.length - 1);

    search = "Your search results for \"" + search + "\" below...";
    if (num == null || num === undefined || num == 0) search = "No search results for \"" + search + "\"";
    return search;
}

function getLogDetails() {
    var accesstoken = sessionStorage.getItem("accesstoken");
    $("#logout").hide();
    $("#myprofile").hide();
    $("#myfavorites").hide();
    console.log("here");
    if (accesstoken != undefined && accesstoken != null) {
        console.log("access token dey");
        $.ajax({
            type: "GET",
            url: 'api/loginwithtoken',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", 'Bearer ' + accesstoken);
            },
            success: function (data) {
                if (data.username != undefined && data.username != null) {
                    loggedinUI(data.username);
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
function loggedinUI(fullname) {
    $("#login").hide();
    $("#usernamespan").text(fullname);
    $("#register").hide();
    $("#logout").show();
    $("#myprofile").show();
    $("#myfavorites").show();
    $("#myfavorites").on("click", function () {
        loadfavorites();
    });
    loadprofile();
}
