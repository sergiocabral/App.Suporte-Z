using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suporteZ.Windows
{
    /// <summary>
    /// <para>Disponibiliza a notificação quando ocorre alterações 
    /// no clipboard (área de transferência)</para>
    /// </summary>
    public sealed class NotificacaoDoClipboard
    {
        /// <summary>
        /// <para>Evento para quando houver atualização do clipboard.</para>
        /// </summary>
        public static event Action Atualizacao;

        private static FormParaNotificacao form = new FormParaNotificacao();

        /// <summary>
        /// <para><see cref="System.Windows.Forms.Form"/> oculto para receber a mensagem 
        /// da API WM_CLIPBOARDUPDATE.</para>
        /// </summary>
        private class FormParaNotificacao : Form
        {
            /// <summary>
            /// <para>Construtor.</para>
            /// </summary>
            public FormParaNotificacao()
            {
                Nativo.SetParent(Handle, Nativo.HWND_MESSAGE);
                Nativo.AddClipboardFormatListener(Handle);
            }

            /// <summary>
            /// <para>Sobreescrito o processado de mensagens da API.</para>
            /// </summary>
            /// <param name="mensagemAPI"><para>Mensagem da API.</para></param>
            protected override void WndProc(ref Message mensagemAPI)
            {
                if (mensagemAPI.Msg == Nativo.WM_CLIPBOARDUPDATE)
                {
                    if (Atualizacao != null)
                    {
                        Atualizacao();
                    }
                }
                base.WndProc(ref mensagemAPI);
            }
        }
    }
}