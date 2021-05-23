using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suporteZ
{
    /// <summary>
    /// <para>Classe usada para disparar exceções conhecidas na execução dos comandos.</para>
    /// </summary>
    public class ComandoException: Exception
    {
        /// <summary>
        /// <para>Lista de sinalização possível que determinará o comportamento do tratador de exceções.</para>
        /// </summary>
        public enum ListaParaSinal
        {
            /// <summary>
            /// <para>Sinaliza finalização por causa de erros do aplicativo.</para>
            /// </summary>
            FinalizarComErroDoAplicativo = 1,

            /// <summary>
            /// <para>Sinaliza finalização por causa de erros da biblioteca.</para>
            /// </summary>
            FinalizarComErroDaBiblioteca = 1,

            /// <summary>
            /// <para>Sinaliza finalização com sucesso, sem erros.</para>
            /// </summary>
            FinalizarComSucesso = 2
        }

        /// <summary>
        /// <para>Sinal recebido com esta instância de exceção.</para>
        /// </summary>
        public ListaParaSinal Sinal { get; private set; }

        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        /// <param name="sinal"><para>Sinalização do tipo de exceção.</para></param>
        public ComandoException(ListaParaSinal sinal) : this(sinal, string.Empty) { }

        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        /// <param name="sinal"><para>Sinalização do tipo de exceção.</para></param>
        /// <param name="mensagem"><para>Mensagem do erro para o usuário.</para></param>
        public ComandoException(ListaParaSinal sinal, string mensagem)
            : base(mensagem)
        {
            Sinal = sinal;
        }
    }
}
