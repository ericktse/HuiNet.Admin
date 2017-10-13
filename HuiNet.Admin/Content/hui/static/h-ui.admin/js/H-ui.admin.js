/* -----------H-ui前端框架-------------
* H-ui.admin.js v3.0
* http://www.h-ui.net/
* Created & Modified by guojunhui
* Date modified 2017.02.03
* Copyright 2013-2017 北京颖杰联创科技有限公司 All rights reserved.
* Licensed under MIT license.
* http://opensource.org/licenses/MIT
*/
var oUl = $("#min_title_list"), hide_nav = $("#Hui-tabNav");

/*获取顶部选项卡总长度*/
function tabNavallwidth() {
    var taballwidth = 0,
		$tabNav = hide_nav.find(".acrossTab"),
		$tabNavWp = hide_nav.find(".Hui-tabNav-wp"),
		$tabNavitem = hide_nav.find(".acrossTab li"),
		$tabNavmore = hide_nav.find(".Hui-tabNav-more");
    if (!$tabNav[0]) { return }
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60));
    });
    $tabNav.width(taballwidth + 25);
    var w = $tabNavWp.width();
    if (taballwidth + 25 > w) {
        $tabNavmore.show();
    }
    else {
        $tabNavmore.hide();
        $tabNav.css({ left: 0 });
    }
}

/*左侧菜单响应式*/
function Huiasidedisplay() {
    if ($(window).width() >= 768) {
        $(".Hui-aside").show();
    }
}

/*菜单导航*/
function Hui_admin_tab(obj, topwindow) {
    var topWindow = topwindow;
    if (topwindow === undefined) {
        topWindow = $(window.parent.document);
    }

    var bStop = false,
		bStopIndex = 0,
		href = $(obj).attr('data-href'),
		title = $(obj).attr("data-title"),
		show_navLi = topWindow.find("#min_title_list li"),
		iframe_box = topWindow.find("#iframe_box");

    if (!href || href == "") {
        alert("data-href不存在，v2.5版本之前用_href属性，升级后请改为data-href属性");
        return false;
    } if (!title) {
        alert("v2.5版本之后使用data-title属性");
        return false;
    }
    if (title == "") {
        alert("data-title属性不能为空");
        return false;
    }
    show_navLi.each(function () {
        if ($(this).find('span').attr("data-href").indexOf(href) != -1) {
            bStop = true;
            bStopIndex = show_navLi.index($(this));
            return false;
        }
    });
    if (!bStop) {
        creatIframe(href, title, topWindow);
        min_titleList();
    }
    else {
        //show_navLi.removeClass("active").eq(bStopIndex).addClass("active");
        //iframe_box.find(".show_iframe").hide().eq(bStopIndex).show().find("iframe").attr("src", href);

        show_navLi.removeClass("active").eq(bStopIndex).addClass("active");
        var showBox = iframe_box.find(".show_iframe").hide().eq(bStopIndex);
        showBox.find('.loading').show();
        showBox.find('iframe').load(function () {
            showBox.find('.loading').hide();
        });
        showBox.show().find("iframe").attr("src", href);
    }

    locationTab(topWindow);
}

/*定位tab*/
function locationTab(topwindow) {
    var topWindow = topwindow;
    if (topwindow === undefined) {
        topWindow = $(window.parent.document);
    }
    var $tabNavWp = topWindow.find(".Hui-tabNav-wp"),
        showTab = topWindow.find(".acrossTab li.active"),
		i = showTab.index();

    var taballwidth = 0;
    var $tabNavitem = topWindow.find(".acrossTab li");
    var maxIndex = 0;
    var w = $tabNavWp.width();
    var iscover = false;
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60));
        maxIndex = index;
        if (taballwidth + 25 > w) {
            iscover = true;
            return false;
        }
    });

    taballwidth = 0;
    $tabNavitem.each(function (index, element) {
        if (index > i)
            return false;
        taballwidth += Number(parseFloat($(this).width() + 60));
    });

    if (i > maxIndex || (iscover && i === maxIndex)) {
        var width = w - taballwidth - 25;
        oUl.stop().animate({ 'left': width }, 100);
    } else {
        oUl.stop().animate({ 'left': 0 }, 100);
    }
}

/*最新tab标题栏列表*/
function min_titleList() {
    var topWindow = $(window.parent.document),
		show_nav = topWindow.find("#min_title_list"),
		aLi = show_nav.find("li");
}

/*创建iframe*/
function creatIframe(href, titleName, topwindow) {
    var topWindow = topwindow;
    if (topwindow === undefined) {
        topWindow = $(window.parent.document);
    }

    var show_nav = topWindow.find('#min_title_list'),
		iframe_box = topWindow.find('#iframe_box'),
		iframeBox = iframe_box.find('.show_iframe'),
		$tabNav = topWindow.find(".acrossTab"),
		$tabNavWp = topWindow.find(".Hui-tabNav-wp"),
		$tabNavmore = topWindow.find(".Hui-tabNav-more");
    var taballwidth = 0;

    show_nav.find('li').removeClass("active");
    show_nav.append('<li class="active"><span data-href="' + href + '">' + titleName + '</span><i></i><em></em></li>');
    if ('function' == typeof $('#min_title_list li').contextMenu) {
        $("#min_title_list li").contextMenu('Huiadminmenu', {
            bindings: {
                'closethis': function (t) {
                    var $t = $(t);
                    if ($t.find("i")) {
                        $t.find("i").trigger("click");
                    }
                },
                'closeall': function (t) {
                    $("#min_title_list li i").trigger("click");
                },
            }
        });
    } else {

    }
    var $tabNavitem = topWindow.find(".acrossTab li");
    if (!$tabNav[0]) { return }
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60));
    });
    $tabNav.width(taballwidth + 25);
    var w = $tabNavWp.width();
    if (taballwidth + 25 > w) {
        $tabNavmore.show()
    }
    else {
        $tabNavmore.hide();
        $tabNav.css({ left: 0 })
    }
    iframeBox.hide();
    iframe_box.append('<div class="show_iframe"><div class="loading"></div><iframe frameborder="0" src=' + href + '></iframe></div>');
    var showBox = iframe_box.find('.show_iframe:visible');
    showBox.find('iframe').load(function () {
        showBox.find('.loading').hide();
    });
}


/*关闭iframe*/
function removeIframe() {
    var topWindow = $(window.parent.document),
		iframe = topWindow.find('#iframe_box .show_iframe'),
		tab = topWindow.find(".acrossTab li"),
		showTab = topWindow.find(".acrossTab li.active"),
		showBox = topWindow.find('.show_iframe:visible'),
		i = showTab.index();
    tab.eq(i - 1).addClass("active");
    tab.eq(i).remove();
    iframe.eq(i - 1).show();
    iframe.eq(i).remove();
}

/*关闭所有iframe*/
function removeIframeAll() {
    var topWindow = $(window.parent.document),
		iframe = topWindow.find('#iframe_box .show_iframe'),
		tab = topWindow.find(".acrossTab li");
    for (var i = 0; i < tab.length; i++) {
        if (tab.eq(i).find("i").length > 0) {
            tab.eq(i).remove();
            iframe.eq(i).remove();
        }
    }
}

/*重新加载当前iframe*/
function reloadCurrentIframe(topwindow) {
    var topWindow = topwindow;
    if (topwindow === undefined) {
        topWindow = $(window.parent.document);
    }

    var iframe = topWindow.find('#iframe_box .show_iframe:visible');

    var curiframe = iframe.find("iframe");
    var href = curiframe.attr("src");
    curiframe.attr("src", href);
}


/*弹出层*/
/*
	参数解释：
	title	标题
	url		请求的url
	id		需要操作的数据id
	w		弹出层宽度（缺省调默认值）
	h		弹出层高度（缺省调默认值）
*/
function layer_show(title, url, w, h) {
    if (title == null || title == '') {
        title = false;
    };
    if (url == null || url == '') {
        url = "404.html";
    };
    if (w == null || w == '') {
        w = 800;
    };
    if (h == null || h == '') {
        h = ($(window).height() - 50);
    };
    layer.open({
        type: 2,
        area: [w + 'px', h + 'px'],
        fix: false, //不固定
        maxmin: true,
        shade: 0.4,
        title: title,
        content: url
    });
}
/*关闭弹出框口*/
function layer_close() {
    var index = parent.layer.getFrameIndex(window.name);
    parent.layer.close(index);
}

/*时间*/
function getHTMLDate(obj) {
    var d = new Date();
    var weekday = new Array(7);
    var _mm = "";
    var _dd = "";
    var _ww = "";
    weekday[0] = "星期日";
    weekday[1] = "星期一";
    weekday[2] = "星期二";
    weekday[3] = "星期三";
    weekday[4] = "星期四";
    weekday[5] = "星期五";
    weekday[6] = "星期六";
    _yy = d.getFullYear();
    _mm = d.getMonth() + 1;
    _dd = d.getDate();
    _ww = weekday[d.getDay()];
    obj.html(_yy + "年" + _mm + "月" + _dd + "日 " + _ww);
};

$(function () {
    //getHTMLDate($("#top_time"));
    Huiasidedisplay();
    var resizeID;
    $(window).resize(function () {
        clearTimeout(resizeID);
        resizeID = setTimeout(function () {
            Huiasidedisplay();
        }, 500);
    });

    $(".nav-toggle").click(function () {
        $(".Hui-aside").slideToggle();
    });
    $(".Hui-aside").on("click", ".menu_dropdown dd li a", function () {
        if ($(window).width() < 768) {
            $(".Hui-aside").slideToggle();
        }
    });
    /*左侧菜单*/
    //$.Huifold(".menu_dropdown dl dt", ".menu_dropdown dl dd", "fast", 1, "click");
    $(".Hui-aside").on("click", ".menu_dropdown dl dt", function (e) {

        e.stopPropagation();
        e.preventDefault();
        e.stopImmediatePropagation();

        var speed = 'fast';
        if ($(this).next().is(":visible")) {
            $(this).next().slideUp(speed).end().removeClass("selected");
            if ($(this).find("b")) {
                $(this).find("b").html("+");
            }
        } else {
            $(".menu_dropdown dl dd").slideUp(speed);
            $(".menu_dropdown dl dt").removeClass("selected");
            if ($(this).find("b")) {
                $(".menu_dropdown dl dt").find("b").html("+");
            }
            $(this).next().slideDown(speed).end().addClass("selected");
            if ($(this).find("b")) {
                $(this).find("b").html("-");
            }
        }
    });

    /*选项卡导航*/
    $(".Hui-aside").on("click", ".menu_dropdown a", function () {
        Hui_admin_tab(this);
    });

    $(document).on("click", "#min_title_list li", function () {
        var bStopIndex = $(this).index();
        var iframe_box = $("#iframe_box");
        $("#min_title_list li").removeClass("active").eq(bStopIndex).addClass("active");
        iframe_box.find(".show_iframe").hide().eq(bStopIndex).show();

        //刷新滚动条
        iframe_box.find(".show_iframe").eq(bStopIndex).find("iframe").contents().find("html").css("overflow", "");
        iframe_box.find(".show_iframe").eq(bStopIndex).find("iframe").contents().find("html").css("overflow", "auto");
    });
    $(document).on("click", "#min_title_list li i", function () {
        var aCloseIndex = $(this).parents("li").index();
        if (aCloseIndex > 0) {
            $(this).parent().remove();
            $('#iframe_box').find('.show_iframe').eq(aCloseIndex).remove();
            $("#min_title_list li").removeClass("active").eq(aCloseIndex - 1).addClass("active");
            $("#iframe_box").find(".show_iframe").hide().eq(aCloseIndex - 1).show();
            tabNavallwidth();
        } else {
            return false;
        }
    });
    $(document).on("dblclick", "#min_title_list li", function () {
        var aCloseIndex = $(this).index();
        var iframe_box = $("#iframe_box");
        if (aCloseIndex > 0) {
            $(this).remove();
            $('#iframe_box').find('.show_iframe').eq(aCloseIndex).remove();
            $("#min_title_list li").removeClass("active").eq(aCloseIndex - 1).addClass("active");
            iframe_box.find(".show_iframe").hide().eq(aCloseIndex - 1).show();
            tabNavallwidth();
        } else {
            return false;
        }
    });
    tabNavallwidth();
    var tabNavall;
    $(window).resize(function () {
        clearTimeout(tabNavall);
        tabNavall = setTimeout(function () {
            tabNavallwidth();
        }, 300);
    });

    $('#js-tabNav-next').click(function () {
        var $tabNavWp = hide_nav.find(".Hui-tabNav-wp");
        var maxw = parseFloat(oUl.css('width').substring(0, oUl.css('width').length - 2)) - $tabNavWp.width();
        var left = parseFloat(oUl.css('left').substring(0, oUl.css('left').length - 2));
        if ((left + maxw) > 0) {
            var tw = left + maxw;
            var leftw = tw > 125 ? left - 125 : left - tw;
            oUl.stop().animate({ 'left': leftw }, 100);
        }
    });
    $('#js-tabNav-prev').click(function () {
        var left = parseFloat(oUl.css('left').substring(0, oUl.css('left').length - 2));
        if (left != 0) {
            var leftw = (left + 125) > 0 ? 0 : (left + 125);
            oUl.stop().animate({ 'left': leftw }, 100);
        }
    });
});
