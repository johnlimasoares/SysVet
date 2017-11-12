$(function () {
    $(".consulta-color-orange").click(function (e) {
        var observacao = this.children[0].defaultValue;
        $("#modalObservacao .modal-body span").text(observacao);
        $("#modalObservacao").modal('show');

        $("#modalObservacao .modal-footer button").click(function (es) {
            $("#modalObservacao").modal('hide');
        });
        e.preventDefault();
    });
});