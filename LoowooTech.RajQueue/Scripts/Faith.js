$(function () {
    $("a[name='Delete']").click(function () {
        if (confirm("您确定要删除吗?")) {
            var node = $(this).parent().parent();
            ShowMessage("正在删除,请稍等......");
            var href = $(this).attr("href");
            $.request(href, null, function (json) {
                if (json.result == 0) {
                    ShowErrorMessage(json.content);
                } else {
                    ShowSuccessMessage("成功删除");
                    node.remove();
                }
            });
        }

        return false;
    });

    $("a[name='Relieve']").click(function () {
        var btn = $(this);
        if (confirm("您确定要撤销解除吗？")) {
            btn.attr("disabled", "disabled");
            var href = btn.attr("href");
            $.request(href, null, function (json) {
                if (json.result == 0) {
                    alert(json.content);
                    btn.removeAttr("disabled");
                } else {
                    alert("完成撤销解除");
                    location.href = "/conduct/index";
                }
            });
            return false;
        }
        return false;
    });

    $("a[name='Recycle']").click(function () {
        var btn = $(this);
        if (confirm("您确定要还原吗？")) {
            btn.attr("disabled", "disabled");
            var href = btn.attr("href");
            $.request(href, null, function (json) {
                if (json.result == 0) {
                    alert(json.content);
                    btn.removeAttr("disabled");
                } else {
                    alert("成功还原");
                    location.href = "/Recycle/index";
                }

            });
            return false;
        }

        return false;
    });

});