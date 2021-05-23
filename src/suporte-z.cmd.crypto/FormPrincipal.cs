using suporteZ.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suporteZ.cmd.crypto
{
    /// <summary>
    /// <para>Janela principal da interface gráfica deste comando.</para>
    /// </summary>
    public partial class FormPrincipal : ComandoFormBase
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public FormPrincipal() : base()
        {
            InitializeComponent();
            InitializeComponent2();
        }

        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        /// <param name="comando"><para>Comando.</para></param>
        public FormPrincipal(suporteZ.Comando comando) : base(comando)
        {
            InitializeComponent();
            InitializeComponent2();
            
            Comando = comando;

            menuItem_Biblioteca_AlgoritmoControle.Items.AddRange(ComandoEx.AlgoritmosDeCriptografia.ToArray<object>());

            menuItem_Biblioteca_AlgoritmoControle.Text = (string)Comando.Dados["algorithm"];
            textBoxPassword.Text = (string)Comando.Dados["password"];
            textBoxValorEntrada.Text = (string)Comando.Dados["value"];
            menuItem_Biblioteca_ClipboardControlVPassword.Checked = (bool)Comando.Dados["controlv_password"];
            menuItem_Biblioteca_ClipboardControlVValue.Checked = (bool)Comando.Dados["controlv_value"];
            menuItem_Biblioteca_ClipboardControlCResult.Checked = (bool)Comando.Dados["controlc_result"];
            menuItem_Biblioteca_Silent.Checked = (bool)Comando.Dados["silent"];
            radioButtonCrypt.Checked = (bool)Comando.Dados["crypt"];
            radioButtonDecrypt.Checked = (bool)Comando.Dados["decrypt"];

            RegistrarEventos();
            RegistrarTeclasDeAtalho();
            AjustarUsabilidade();
            DefinirToolTips();
        }

        /// <summary>
        /// <para>Implementa comportamentos que visam facilitar o uso.</para>
        /// </summary>
        private void AjustarUsabilidade()
        {
            textBoxPassword.KeyDown += EventoSelecionarTodoTexto;
            textBoxValorEntrada.KeyDown += EventoSelecionarTodoTexto;
            textBoxValorSaida.KeyDown += EventoSelecionarTodoTexto;
            panelIconePassword.Click += (sender, e) => { textBoxPassword.Focus(); };
            panelIconeValorEntrada.Click += (sender, e) => { textBoxValorEntrada.Focus(); };
            panelIconeValorSaida.Click += (sender, e) => { textBoxValorSaida.Focus(); };
        }

        /// <summary>
        /// <para>Método para evento: Quando é pressionado as teclas
        /// para seleção do texto todo.</para>
        /// </summary>
        /// <param name="sender"><para>Originador do evento.</para></param>
        /// <param name="e"><para>Informação sobre o evento.</para></param>
        private void EventoSelecionarTodoTexto(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A || e.KeyCode == Keys.T))
            {
                (sender as TextBox).SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// <para>Define nos controles informações de tooltip (dicas, hitns) para o usuário.</para>
        /// </summary>
        private void DefinirToolTips()
        {
            toolTip.SetToolTip(panelIconePassword, Properties.Comando.formPrincipal_panelIconePassword_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(panelIconeValorEntrada, Properties.Comando.formPrincipal_panelIconeValorEntrada_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(panelIconeValorSaida, Properties.Comando.formPrincipal_panelIconeValorSaida_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(radioButtonCrypt, Properties.Comando.formPrincipal_radioButtonCrypt_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(radioButtonDecrypt, Properties.Comando.formPrincipal_radioButtonDecrypt_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(checkBoxAcaoAutomatico, Properties.Comando.formPrincipal_checkBoxAcaoAutomatico_Hint.Replace("\\n", Environment.NewLine));
            toolTip.SetToolTip(buttonAcao, Properties.Comando.formPrincipal_buttonAcao_Hint.Replace("\\n", Environment.NewLine));
        }

        /// <summary>
        /// <para>Registrar teclas de atalho</para>
        /// </summary>
        private void RegistrarTeclasDeAtalho()
        {
            KeyDown += (sender, e) =>
            {
            };
        }

        /// <summary>
        /// <para>Registrar eventos de comportamento dos controles.</para>
        /// </summary>
        private void RegistrarEventos()
        {
            Load += (sender, e) =>
            {
                checkBoxAcaoAutomatico.Checked = !checkBoxAcaoAutomatico.Checked;
                checkBoxAcaoAutomatico.Checked = !checkBoxAcaoAutomatico.Checked;
                EventoSelecaoCryptDecrypt(null, null);

                if (checkBoxAcaoAutomatico.Checked)
                {
                    AplicarCriptografia(false);
                }
            };
            Activated += (sender, e) =>
            {
                NotificacaoDoClipboard.Atualizacao -= NotificacaoDoClipboard_Atualizacao;
            };
            Deactivate += (sender, e) =>
            {
                NotificacaoDoClipboard.Atualizacao += NotificacaoDoClipboard_Atualizacao;
            };
            Shown += (sender, e) =>
            {
                TextBox textBox = textBoxValorEntrada.Text.Length > 0 ? textBoxValorSaida : textBoxValorEntrada;
                textBox.Focus();
                textBox.SelectAll();
            };
            buttonAcao.Image = Properties.Resources.iconeCadeadoFechado32;
            radioButtonCrypt.CheckedChanged += EventoSelecaoCryptDecrypt;
            radioButtonCrypt.CheckedChanged += EventoAlteracaoDeParametros;
            radioButtonDecrypt.CheckedChanged += EventoSelecaoCryptDecrypt;
            radioButtonDecrypt.CheckedChanged += EventoAlteracaoDeParametros;
            checkBoxAcaoAutomatico.CheckedChanged += EventoAlteracaoDeParametros;
            checkBoxAcaoAutomatico.CheckedChanged += (sender, e) => { buttonAcao.Enabled = !(sender as CheckBox).Checked; };
            menuItem_Biblioteca_AlgoritmoControle.TextChanged += EventoAlteracaoDeParametros;
            textBoxPassword.TextChanged += EventoAlteracaoDeParametros;
            textBoxValorEntrada.TextChanged += EventoAlteracaoDeParametros;
            menuItem_Biblioteca_ClipboardControlVPassword.Click += EventoMenuItemComoCheckbox;
            menuItem_Biblioteca_ClipboardControlVValue.Click += EventoMenuItemComoCheckbox;
            menuItem_Biblioteca_Silent.Click += EventoMenuItemComoCheckbox;
            menuItem_Biblioteca_Silent.CheckedChanged += EventoAlteracaoDeParametros;
            menuItem_Biblioteca_ClipboardControlCResult.Click += EventoMenuItemComoCheckbox;
            menuItem_Biblioteca_ClipboardControlCResult.CheckedChanged += EventoAlteracaoDeParametros;
            buttonAcao.Click += (sender, e) => { AplicarCriptografia(); };
        }

        /// <summary>
        /// <para>Chave para a última captura de texto do clipboard.</para>
        /// </summary>
        private string ultimoClipboard;

        /// <summary>
        /// <para>Método para evento: Quando o valor do clipboard é atualizado.</para>
        /// </summary>
        private void NotificacaoDoClipboard_Atualizacao()
        {
            try
            {
                string clipboard = Clipboard.GetText();
                if (ultimoClipboard != clipboard)
                {
                    ultimoClipboard = clipboard;

                    if (menuItem_Biblioteca_ClipboardControlVPassword.Checked)
                    {
                        textBoxPassword.Text = clipboard;
                    }
                    if (menuItem_Biblioteca_ClipboardControlVValue.Checked)
                    {
                        textBoxValorEntrada.Text = clipboard;
                    }
                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// <para>Método para evento: Quando é alterado o modo de criptografia
        /// para descriptografia (vice-versa).</para>
        /// </summary>
        /// <param name="sender"><para>Originador do evento.</para></param>
        /// <param name="e"><para>Informação sobre o evento.</para></param>
        private void EventoSelecaoCryptDecrypt(object sender, EventArgs e)
        {
            if (radioButtonCrypt.Checked)
            {
                buttonAcao.Image = Properties.Resources.iconeCadeadoFechado32;
            }
            else if (radioButtonDecrypt.Checked)
            {
                buttonAcao.Image = Properties.Resources.iconeCadeadoAberto32;
            }
        }

        /// <summary>
        /// <para>Método para evento: Quando algum item de menu é acionado e
        /// precisa se comportar como checkbox.</para>
        /// </summary>
        /// <param name="sender"><para>Originador do evento.</para></param>
        /// <param name="e"><para>Informação sobre o evento.</para></param>
        private void EventoMenuItemComoCheckbox(object sender, EventArgs e)
        {
            (sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;
        }

        /// <summary>
        /// <para>Método para evento: Quando algum valor de parâmetro é alterado.</para>
        /// </summary>
        /// <param name="sender"><para>Originador do evento.</para></param>
        /// <param name="e"><para>Informação sobre o evento.</para></param>
        private void EventoAlteracaoDeParametros(object sender, EventArgs e)
        {
            if (checkBoxAcaoAutomatico.Checked)
            {
                AplicarCriptografia(false);
            }
        }

        /// <summary>
        /// <para>Aplica a criptografia conforme o comando.</para>
        /// </summary>
        /// <param name="interativo"><para>Quando <c>true</c> emite alertas em
        /// caso de erro e faz outras interações com o usuário</para></param>
        protected void AplicarCriptografia(bool interativo = true)
        {
            Comando.Dados["algorithm"] = menuItem_Biblioteca_AlgoritmoControle.Text;
            Comando.Dados["password"] = textBoxPassword.Text;
            Comando.Dados["value"] = textBoxValorEntrada.Text;
            Comando.Dados["crypt"] = radioButtonCrypt.Checked;
            Comando.Dados["decrypt"] = radioButtonDecrypt.Checked;
            Comando.Dados["controlc_result"] = menuItem_Biblioteca_ClipboardControlCResult.Checked;
            Comando.Dados["silent"] = menuItem_Biblioteca_Silent.Checked;

            try
            {
                textBoxValorSaida.Text = ultimoClipboard = ComandoEx.ProcessarCriptografia();
            }
            catch (Exception ex)
            {
                textBoxValorSaida.Clear();
                if (interativo)
                {
                    MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        /// <summary>
        /// <para>Referência ao objeto principal do comando em uso com a classe especialaizada.</para>
        /// </summary>
        internal protected Comando ComandoEx
        {
            get
            {
                return (Comando)Comando;
            }
        }

        /// <summary>
        /// <para>Tipo da do objeto que contém as definições de texto em vários idiomas.</para>
        /// </summary>
        protected override Type TypeStringResource { get { return typeof(Properties.Comando); } }

        #region InitializeComponent

        private ToolStripMenuItem menuItem_Biblioteca_Algoritmo;
        private ToolStripComboBox menuItem_Biblioteca_AlgoritmoControle;
        private ToolStripSeparator menuItem_Biblioteca_Separador1;
        private ToolStripMenuItem menuItem_Biblioteca_ClipboardControlVPassword;
        private ToolStripMenuItem menuItem_Biblioteca_ClipboardControlVValue;
        private ToolStripSeparator menuItem_Biblioteca_Separador2;
        private ToolStripMenuItem menuItem_Biblioteca_ClipboardControlCResult;
        private ToolStripMenuItem menuItem_Biblioteca_Silent;
        
        /// <summary>
        /// <para>Inicialização dos componentes visuais.</para>
        /// </summary>
        private void InitializeComponent2()
        {
            this.menuItem_Biblioteca_Algoritmo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Biblioteca_AlgoritmoControle = new System.Windows.Forms.ToolStripComboBox();
            this.menuItem_Biblioteca_Separador1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_Biblioteca_ClipboardControlVPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Biblioteca_ClipboardControlVValue = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Biblioteca_Separador2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_Biblioteca_ClipboardControlCResult = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Biblioteca_Silent = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem_Biblioteca
            // 
            this.menuItem_Biblioteca.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuItem_Biblioteca_Algoritmo,
                this.menuItem_Biblioteca_AlgoritmoControle,
                this.menuItem_Biblioteca_Separador1,
                this.menuItem_Biblioteca_ClipboardControlVPassword,
                this.menuItem_Biblioteca_ClipboardControlVValue,
                this.menuItem_Biblioteca_Separador2,
                this.menuItem_Biblioteca_ClipboardControlCResult,
                this.menuItem_Biblioteca_Silent,
            });
            // 
            // menuItem_Biblioteca_Algoritmo
            //
            this.menuItem_Biblioteca_Algoritmo.Enabled = false;
            this.menuItem_Biblioteca_Algoritmo.Image = Properties.Resources.iconeSenha;
            this.menuItem_Biblioteca_Algoritmo.Name = "menuItem_Biblioteca_Algoritmo";
            this.menuItem_Biblioteca_Algoritmo.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_Algoritmo.Text = "formPrincipal_menuItem_Biblioteca_Algoritmo";
            // 
            // menuItem_Biblioteca_AlgoritmoControle
            // 
            this.menuItem_Biblioteca_AlgoritmoControle.Name = "menuItem_Biblioteca_AlgoritmoControle";
            this.menuItem_Biblioteca_AlgoritmoControle.Size = new System.Drawing.Size(150, 23);
            // 
            // menuItem_Biblioteca_Separador1
            // 
            this.menuItem_Biblioteca_Separador1.Name = "menuItem_Biblioteca_Separador1";
            this.menuItem_Biblioteca_Separador1.Size = new System.Drawing.Size(121, 23);
            // 
            // menuItem_Biblioteca_ClipboardControlVPassword
            // 
            this.menuItem_Biblioteca_ClipboardControlVPassword.Name = "menuItem_Biblioteca_ClipboardControlVPassword";
            this.menuItem_Biblioteca_ClipboardControlVPassword.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_ClipboardControlVPassword.Text = "formPrincipal_menuItem_Biblioteca_ClipboardControlVPassword";
            // 
            // menuItem_Biblioteca_ClipboardControlVValue
            // 
            this.menuItem_Biblioteca_ClipboardControlVValue.Name = "menuItem_Biblioteca_ClipboardControlVValue";
            this.menuItem_Biblioteca_ClipboardControlVValue.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_ClipboardControlVValue.Text = "formPrincipal_menuItem_Biblioteca_ClipboardControlVValue";
            // 
            // menuItem_Biblioteca_Separador2
            // 
            this.menuItem_Biblioteca_Separador1.Name = "menuItem_Biblioteca_Separador2";
            this.menuItem_Biblioteca_Separador1.Size = new System.Drawing.Size(121, 23);
            // 
            // menuItem_Biblioteca_Clipboard
            // 
            this.menuItem_Biblioteca_ClipboardControlCResult.Name = "menuItem_Biblioteca_Clipboard";
            this.menuItem_Biblioteca_ClipboardControlCResult.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_ClipboardControlCResult.Text = "formPrincipal_menuItem_Biblioteca_Clipboard";
            // 
            // menuItem_Biblioteca_Silent
            // 
            this.menuItem_Biblioteca_Silent.Name = "menuItem_Biblioteca_Silent";
            this.menuItem_Biblioteca_Silent.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_Silent.Text = "formPrincipal_menuItem_Biblioteca_Silent";
            // 
            // ComandoFormBase
            // 
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
