$(function () {
    //$('#applybutton2').click({
    $('#myfile1').fileupload({
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
            //$("#filename1").val("File Uploaded...");
            $("#essayfilename").val(response);
            $("#filenamelabel1").html("&nbsp;&nbsp;&nbsp;Essay Uploaded <i class='fa fa-check'></i>");
            //$("#filenamespan").html("File Uploaded...");
        },
        error: function (error) {
            $('#progressbar').hide();
            $('#progressbar div').css('width', '0%');
            console.log('error', error);
        }
    });
    $('#myfile2').fileupload({
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
            //$("#filename2").val("File Uploaded...");
            $("#reffilename").val(response);
            $("#filenamelabel2").html("&nbsp;&nbsp;&nbsp;Reference Uploaded <i class='fa fa-check'></i>");
            //$("#filenamespan").html("File Uploaded...");
        },
        error: function (error) {
            $('#progressbar').hide();
            $('#progressbar div').css('width', '0%');
            console.log('error', error);
        }
    });

    /*
    $("#file1").fileUpload({
        'uploader': 'scripts/uploader.swf',
        'cancelImg': 'images/cancel.png',
        'buttonText': 'Browse Files',
        'script': 'Upload.ashx',
        'folder': 'uploads',
        'fileDesc': 'Image Files',
      'fileExt': '*.jpg;*.jpeg;*.gif;*.png',

        'multi': false,
        'auto': true
    });
    $("#file2").fileUpload({
        'uploader': 'scripts/uploader.swf',
        'cancelImg': 'images/cancel.png',
        'buttonText': 'Browse Files',
        'script': 'Upload.ashx',
        'folder': 'uploads',
        'fileDesc': 'Image Files',
      'fileExt': '*.jpg;*.jpeg;*.gif;*.png',

        'multi': false,
        'auto': true
    });
    */
});