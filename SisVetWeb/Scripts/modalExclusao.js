
$(function () {

    $(".btn.btn-default.btn-danger").click(function (e) {
        e.preventDefault();
        var id = this.id;
        var nome = this.name;
        var url = location.pathname.replace("Index", "Delete/");
        $("#deleteModal .modal-body input[type=hidden]").val(id);
        $("#deleteModal .modal-body span").text(nome);
        $("#deleteModal").modal('show');

        $("#deleteModal .modal-footer button").click(function (es) {
            es.preventDefault();

            var rowId = "#row-" + id;

            $.ajax({
                url: url,
                type: 'post',
                dataType: 'json',
                data: { id: id },
                beforeSend: function () {
                    //var loading = "<span><em>Excluindo</em>&nbsp;<i>glyphicon glyphicon-refresh icon-refresh-animate</i></span>";
                    //$('#deleteModal .modal-header h4').after(loading);
                },
                success: function () {
                    $("#deleteModal").modal('hide');
                    $(rowId).animate({ opacity: 0.0 }, 400, function () {
                        $(rowId).remove();
                    });
                },
                complete: function (data) {
                    //$("#divExcluir").empty();
                    //$("#divExcluir").addClass("alert alert-danger");
                    //$("#divExcluir").data(data.responseText);
                },
                error: function(data) { 
                    $("#deleteModal").modal('hide');                   
                }
            });
        });
    });
});

