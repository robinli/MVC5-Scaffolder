$(document).ready(function () {

    var filer_default_opts = {
        changeInput2: '<div class="jFiler-input-dragDrop">\
                         <div class="jFiler-input-inner">\
                             <div class="jFiler-input-icon">\
                               <i class="icon-jfi-cloud-up-o"></i>\
                             </div>\
                            <div class="jFiler-input-text">\
                               <h3>Drag&Drop files here</h3>\
                               <span style="display:inline-block; margin: 10px 0">or</span>\
                            </div>\
                            <div class="jFiler-input-choose-btn btn-custom blue-light">\
                                 <span>Browse Files</span>\
                            </div></div></div>',
        templates: {
            box: '<ul class="jFiler-items-list jFiler-items-grid"></ul>',
            item: '<li class="jFiler-item">\
                     <div class="jFiler-item-container">\
                        <div class="jFiler-item-inner">\
                             <div class="jFiler-item-icon pull-left">\
                                 {{fi-icon}}\
                             </div>\
                             <div class="jFiler-item-info pull-left">\
                                <div class="jFiler-item-title" title="{{fi-name}}">\
                                    {{fi-name | limitTo:30}}\
                                </div>\
                                <div class="jFiler-item-others">\
                                     <span>size: {{fi-size2}}</span>\
                                     <span>type: {{fi-extension}}</span>\
                                     <div class="jFiler-item-status">{{fi-progressBar}}</div>\
                                </div>\
                           <div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',
            itemAppend: '<li class="jFiler-item"><div class="jFiler-item-container"><div class="jFiler-item-inner"><div class="jFiler-item-icon pull-left">{{fi-icon}}</div><div class="jFiler-item-info pull-left"><div class="jFiler-item-title">{{fi-name | limitTo:35}}</div><div class="jFiler-item-others"><span>size: {{fi-size2}}</span><span>type: {{fi-extension}}</span><div class="jFiler-item-status"></div></div><div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',
            progressBar: '<div class="bar"></div>',
            itemAppendToEnd: false,
            removeConfirmation: true,
            _selectors: {
                list: '.jFiler-items-list',
                item: '.jFiler-item',
                progressBar: '.bar',
                remove: '.jFiler-item-trash-action'
            }
        },
        dragDrop: {
            dragEnter: function () { },
            dragLeave: function () { },
            drop: function () { }
        },
        uploadFile: {
            url: "/FileUpload/Upload",
            data: { FileType: entityname },
            type: 'POST',
            enctype: 'multipart/form-data',
            synchron: true,
            beforeSend: function (el, l, p, o, s, id, jqXHR, settings) {
                settings.data.append('ver', '1.3');
            },
            success: function (data, el, listEl, boxEl, newInputEl, inputEl, id) {

                var parent = el.find(".jFiler-jProgressBar").parent();
                filerKit = inputEl.prop("jFiler");
                filerKit.files_list[id].name = data.filename;
                if (data.success) {
                    el.find(".jFiler-jProgressBar").fadeOut("slow", function () {
                        $("<div class=\"jFiler-item-others text-success\"><i class=\"fa fa-check-circle\" aria-hidden=\"true\"></i> Success</div>").hide().appendTo(parent).fadeIn("slow");
                    });
                    $.messager.alert('导入完成', '导入完成！<br> 耗时：' + data.elapsedTime, 'info');
                    $dg.datagrid('reload');
                    $('#importwindow').window('close');

                } else {
                    el.find(".jFiler-jProgressBar").fadeOut("slow", function () {
                        $("<div class=\"jFiler-item-others text-danger\"><i class=\"fa fa-times-circle\" aria-hidden=\"true\"></i> Error</div>").hide().appendTo(parent).fadeIn("slow");
                    });
                    $.messager.alert('导入错误', data.message, 'error');

                }
            },
            error: function (el) {
                var parent = el.find(".jFiler-jProgressBar").parent();
                el.find(".jFiler-jProgressBar").fadeOut("slow", function () {
                    $("<div class=\"jFiler-item-others text-danger\"><i class=\"fa fa-times-circle\" aria-hidden=\"true\"></i> Error</div>").hide().appendTo(parent).fadeIn("slow");
                });
            },
            statusCode: null,
            onProgress: null,
            onComplete: null
        },

        editor: {
            // editor cropper
            cropper: {
                // cropper ratio
                // example: null
                // example: '1:1'
                // example: '16:9'
                // you can also write your own
                ratio: null,

                // cropper minWidth in pixels
                // size is adjusted with the image natural width
                minWidth: null,

                // cropper minHeight in pixels
                // size is adjusted with the image natural height
                minHeight: null,

                // show cropper grid
                showGrid: true
            },

            // editor on save quality (0 - 100)
            // only for client-side resizing
            quality: null,

            // editor on save maxWidth in pixels
            // only for client-side resizing
            maxWidth: null,

            // editor on save maxHeight in pixels
            // only for client-size resizing
            maxHeight: null,

            // Callback fired after saving the image in editor
            onSave: function (blobOrDataUrl, item, listEl, parentEl, newInputEl, inputEl) {
                // callback will go here
            }
        }
    };



    $('#filer_input').filer({
        limit: 1,
        maxSize: 30,
        extensions: ["xlsx"],
        changeInput: filer_default_opts.changeInput2,
        showThumbs: true,
        appendTo: '.fileInput-thumbs',
        theme: "dragdropbox",
        addMore: false,
        clipBoardPaste: false,
        captions: {
            button: "Choose Files",
            feedback: "Choose files To Upload",
            feedback2: "files were chosen",
            drop: "Drop file here to Upload",
            removeConfirmation: "Are you sure you want to remove this file?",

            errors: {
                filesLimit: "当前仅支持 {{fi-limit}} 文件同时上传.",
                filesType: "当前只能上传 xlsx 文件上传.",
                filesSize: "上传的文件 {{fi-name}} 太大! 不要超过 {{fi-maxSize}} MB.",
                filesSizeAll: "上传的文件太大! 不要超过 {{fi-maxSize}} MB."
            }
        },
        templates: {
            item: '<li class="jFiler-item"><div class="jFiler-item-container"><div class="jFiler-item-inner"><div class="jFiler-item-icon pull-left">{{fi-icon}}</div><div class="jFiler-item-info pull-left"><div class="jFiler-item-title" title="{{fi-name}}">{{fi-name | limitTo:30}}</div><div class="jFiler-item-others"><span>size: {{fi-size2}}</span><span>type: {{fi-extension}}</span><div class="jFiler-item-status">{{fi-progressBar}}</div></div><div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',
            itemAppend: '<li class="jFiler-item"><div class="jFiler-item-container"><div class="jFiler-item-inner"><div class="jFiler-item-icon pull-left">{{fi-icon}}</div><div class="jFiler-item-info pull-left"><div class="jFiler-item-title">{{fi-name | limitTo:35}}</div><div class="jFiler-item-others"><span>size: {{fi-size2}}</span><span>type: {{fi-extension}}</span><div class="jFiler-item-status"></div></div><div class="jFiler-item-assets"><ul class="list-inline"><li><a class="icon-jfi-trash jFiler-item-trash-action"></a></li></ul></div></div></div></div></li>',
            progressBar: '<div class="bar"></div>',
            itemAppendToEnd: false,
            removeConfirmation: false,
            _selectors: {
                list: '.jFiler-items-list',
                item: '.jFiler-item',
                progressBar: '.bar',
                remove: '.jFiler-item-trash-action'
            }
        },
        dragDrop: filer_default_opts.dragDrop,
        uploadFile: filer_default_opts.uploadFile,
        onRemove: function (itemEl, file, id, listEl, boxEl, newInputEl, inputEl) {

            var filerKit = inputEl.prop("jFiler"),
                file_name = filerKit.files_list[id].name;
            $.post('/FileUpload/Remove', { filename: file_name });
        }

    });

});
