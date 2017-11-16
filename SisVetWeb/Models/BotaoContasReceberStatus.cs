using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Enum;

namespace SisVetWeb.Models
{
    public static class BotaoContasReceberStatus
    {
        public static string PathIcon { get; private set; }
        public static string Status { get; private set; }
        public static string ColorStatus { get; private set; }

        public static void SetBotaoContasReceberStatus(SituacaoParcelaFinanceira situacaoParcelaFinanceira)
        {
            switch (situacaoParcelaFinanceira)
            {
                case SituacaoParcelaFinanceira.Aberto:
                    PathIcon = "\\Images\\arrow-right.png";
                    Status = "Aberta";
                    ColorStatus = "";
                    break;
                case SituacaoParcelaFinanceira.Liquidado:
                    PathIcon = "\\Images\\arrow-up.png";
                    Status = "Recebida";
                    ColorStatus = "Color:green";
                    break;
                case SituacaoParcelaFinanceira.Cancelado:
                    PathIcon = "\\Images\\arrow-down.png";
                    Status = "Cancelada";
                    ColorStatus = "Color:red";
                    break;
            }
            
        }
    }
}