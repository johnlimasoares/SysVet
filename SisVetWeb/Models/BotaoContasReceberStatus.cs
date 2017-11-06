using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Enum;

namespace SisVetWeb.Models
{
    public class BotaoContasReceberStatus
    {
        public string PathIcon { get; set; }
        public string Status { get; set; }        
        public string ColorStatus { get; set; }

        public static BotaoContasReceberStatus GetBotaoContasReceberStatus(SituacaoParcelaFinanceira situacaoParcelaFinanceira){
            var botao = new BotaoContasReceberStatus();
            switch (situacaoParcelaFinanceira)
            {
                case SituacaoParcelaFinanceira.Aberto:
                    botao.PathIcon = "\\Images\\arrow-right.png";
                    botao.Status = "Aberta";
                    break;
                case SituacaoParcelaFinanceira.Liquidado:
                    botao.PathIcon = "\\Images\\arrow-up.png";
                    botao.Status = "Recebida";
                    botao.ColorStatus = "Color:green";
                    break;
                case SituacaoParcelaFinanceira.Cancelado:
                    botao.PathIcon = "\\Images\\arrow-down.png";
                    botao.Status = "Cancelada";
                    botao.ColorStatus = "Color:red";
                    break;
            }

            return botao;
        }
    }
}