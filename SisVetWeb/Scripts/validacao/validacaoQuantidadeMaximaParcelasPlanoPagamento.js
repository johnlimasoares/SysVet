var eventBlur = document.getElementById('quantidadeParcelas');
eventBlur.addEventListener('blur', OnBlurQuantidadeParcelasDelegate, true);

function OnBlurQuantidadeParcelasDelegate() {
    var formaPagamentoSelecionada = document.getElementById('FinanceiroPlanoDePagamentoId');
    var indice = formaPagamentoSelecionada.selectedIndex;
    var planoPagamentoId = formaPagamentoSelecionada.options[indice].value;
    var quantidadeParcelasPretendida = document.getElementById('quantidadeParcelas').value;
    if (quantidadeParcelasPretendida > 0 && planoPagamentoId > 0) {
        callAjax(planoPagamentoId);
    }
}

function callAjax(planoPagamentoId) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/FinanceiroContasReceber/ValidarQuantidadeMaximaParcelasPlano', false);
    xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    var data = 'planoPagamentoId=' + planoPagamentoId;
    xhr.onreadystatechange = onReadyStateDelegate;
    xhr.send(data);
}

function onReadyStateDelegate() {
    if (this.status == 200) {
        var quantidadeMaximoParcelas = this.responseText;
        var quantidadeParcelasPretendida = document.getElementById('quantidadeParcelas').value;
        validarLimiteParcelas(quantidadeParcelasPretendida, quantidadeMaximoParcelas);
    } else {
        alert(this.responseText);
    }
}

function validarLimiteParcelas(quantidadeParcelasPretendida, quantidadeMaximoParcelas) {
    if (parseInt(quantidadeParcelasPretendida) > parseInt(quantidadeMaximoParcelas)) {
        document.getElementById('quantidadeParcelas').value = String.empty;
        document.getElementById('ValorTotal').focus();
        alert('O Plano de Pagamento escolhido permite até ' + quantidadeMaximoParcelas + ' parcela(s)!');
    }
}