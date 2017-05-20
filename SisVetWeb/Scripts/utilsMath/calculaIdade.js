$(function () {
    calculaIdade();//calcula a idade no onload da edição
    $('#DataNascimento').blur(function() { calculaIdade(); }); //blur chamado aqui porque o editfor da edição não possui o evento
});



function calculaIdade() {
    var dataAtual = new Date();
    if (document.getElementById('DataNascimento').value == "")
        return;
    var dataNascimento = new Date(document.getElementById('DataNascimento').value.replace('-', ',').replace('-', ','));    
    var diferencaDiaMesAno = calculaDiferenca(dataNascimento,dataAtual);
    document.getElementById('idade').value = diferencaDiaMesAno[0] +" Ano(s) "+ diferencaDiaMesAno[1] +" Mês(es) "+diferencaDiaMesAno[2]+" Dia(s)";
}

function calculaDiferenca(dataNascimento,dataAtual) {
    var anoNascimento = dataNascimento.getFullYear();
    var anoAtual = dataAtual.getFullYear();
    var mesNascimento = dataNascimento.getMonth();
    var mesAtual = dataAtual.getMonth();
    var diaNascimento = dataNascimento.getDate();
    var diaAtual = dataAtual.getDate();

    if (diaAtual < diaNascimento) {
        mesAtual--;
        diaAtual += diaNoMes(anoNascimento,mesNascimento);
    }

    if (mesAtual < mesNascimento) {
        anoAtual--;
        mesAtual += 12;
    }

    return [anoAtual - anoNascimento,mesAtual - mesNascimento,diaAtual - diaNascimento];
}

function diaNoMes(anoNascimento, mesNascimento) {
    with (new Date(anoNascimento,mesNascimento, 1, 12)) {
        setDate(0);
        return getDate();
    }
}