/*Função para gravar data e hora atual nos campos com id = observacao*/
document.onkeyup = KeyCheck;
function KeyCheck(e) {
    var observacao = '###DATA REGISTRO: ';
    var dataHora = '\n ' + observacao.toUpperCase() + (new Date().getDate() + '/' + (new Date().getMonth() + 1) + '/' + new Date().getFullYear()) + ' ÁS '+
                                                            (new Date().getHours() + ':' +new Date().getMinutes() ) + '\n';
    var KeyID = (window.event) ? event.keyCode : e.keyCode;
    if (KeyID == 113) {
        $('#observacao').val($('#observacao').val() + dataHora);
        $('#observacao').focus();
    }
}

/*força o scroll do textarea ir ao fim*/
document.getElementById("observacao").scrollTop = document.getElementById("observacao").scrollHeight;
