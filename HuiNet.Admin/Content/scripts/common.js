/// <reference path="../hui/lib/jquery/1.9.1/jquery.js" />
function Ajax(option) {
    var wait;
    var opt = {
        url: option.url || '',
        data: option.data || {},
        dataType: option.dataType || 'json',
        async: option.async || true,
        type: option.type || 'post',
        beforeSend: function (xhr) {
            //如果存在csrf token字段，则添加到请求头
            if ($('#__CsrfVerificationToken').length > 0) {
                xhr.setRequestHeader('__CsrfVerificationToken', $('#__CsrfVerificationToken').val());
            }

            if (option.beforeSend) {
                option.beforeSend();
            }
            wait = layer.load();
        },
        success: function (response) {
            layer.close(wait);
            if (response.logoff == true) {
                window.location.href = response.url;
            } else {
                option.success(response);
            }
        },
        error: function (errorMsg) {
            layer.close(wait);
            layer.alert('系统错误' + errorMsg, { icon: 5 });
        }
    };
    $.ajax(opt);
}


function AjaxSubmit(target, option) {
    var wait;
    var opt = {
        url: option.url || '',
        dataType: option.dataType || 'json',
        type: option.type || 'post',
        beforeSubmit: function () {
            if (option.beforeSubmit) {
                var res = option.beforeSubmit();
                if (res == false) {
                    return res;
                }
            }
            wait = layer.load();
        },
        xhr: option.xhr || false,
        success: function (response) {
            layer.close(wait);
            if (response.logoff == true) {
                window.location.href = response.url;
            } else {
                option.success(response);
            }
        },
        error: function (errorMsg) {
            layer.close(wait);
            layer.alert('系统错误', { icon: 5 });
        }
    }
    $('#' + target).ajaxSubmit(opt);
}

function getServerGroup(target, groupId, selectValue) {
    var $select = $('#' + target);
    $select.empty();
    $select.append($('<option value="-1">请选择</option>'));
    Ajax({
        url: '/Packages/GetServerGroup',
        data: { groupId: groupId },
        success: function (data) {
            $(data).each(function (index, obj) {
                var value = obj.GroupId;
                var text = obj.GroupName;
                if (selectValue && selectValue == value) {
                    $select.append($('<option value="' + value + '" selected=selected>' + text + '</option>'));
                } else {
                    $select.append($('<option value="' + value + '">' + text + '</option>'));
                }
            })
        }
    })
}

function openWindow(title, url, fnCancel) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url,
        area: ['100%', '100%'],
        end: function () {

            if (fnCancel != undefined) {
                fnCancel();
            }

        }
    });
    layer.full(index);
}

function alertSuccess(message, callback) {
    var index;
    if (callback == undefined) {
        index = layer.alert(message, { icon: 1 });
    } else {
        index = layer.alert(message, { icon: 1 }, callback);
    }
    return index;
}

function alertFail(message, callback) {
    var index;
    if (callback == undefined) {
        index = layer.alert(message, { icon: 2 });
    } else {
        index = layer.alert(message, { icon: 2 }, callback);
    }
    return index;
}
