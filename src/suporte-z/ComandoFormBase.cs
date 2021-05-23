using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suporteZ
{
    /// <summary>
    /// <para>Classe básica usada pelas janelas de interface gráfica dos comandos.</para>
    /// </summary>
    public class ComandoFormBase : Form
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public ComandoFormBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        /// <param name="comando"><para>Comando relacionado com a janela.</para></param>
        public ComandoFormBase(Comando comando)
        {
            InitializeComponent();
            Comando = comando;
            RegistrarEventos();
            RegistrarTeclasDeAtalho();
        }
        
        /// <summary>
        /// <para>Registrar teclas de atalho</para>
        /// </summary>
        private void RegistrarTeclasDeAtalho()
        {
            KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter && AcceptButton != null)
                {
                    AcceptButton.PerformClick();
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    if (e.Shift)
                    {
                        menuItem_Aplicativo_Tray.PerformClick(); 
                    }
                    else
                    {
                        if (CancelButton != null)
                        {
                            CancelButton.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.F1)
                {
                    menuItem_Aplicativo_Sobre.PerformClick();
                    e.SuppressKeyPress = true;
                }
            };
        }

        /// <summary>
        /// <para>Registrar eventos de comportamento dos controles.</para>
        /// </summary>
        private void RegistrarEventos()
        {
            Load += (sender, e) =>
            {
                Text = ComandoTextoDeAjuda.Instancia.NomeDoAplicativo + " " + Comando.ComandoInfo.Key;
                TraduzirControles();
                menuItem_Aplicativo.Text = ComandoTextoDeAjuda.Instancia.NomeDoAplicativo;
                TraduzirMenu(typeof(Properties.Comando), menuItem_Aplicativo);
                menuItem_Biblioteca.Text = Comando.ComandoInfo.Key;
                TraduzirMenu(TypeStringResource, menuItem_Biblioteca);
                if (menuItem_Biblioteca.DropDownItems.Count == 0)
                {
                    menuItem_Biblioteca.Dispose();
                }
            };
            menuItem_Aplicativo_Tray.Click += (sender, e) =>
            {
                Hide();
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
            };
            menuItem_Aplicativo_Sobre.Click += (sender, e) =>
            {
                ExibirJanelaSobre(this);
            };
            menuItem_Aplicativo_Sair.Click += (sender, e) =>
            {
                Close();
            };

            notifyIcon.Icon = Icon;
            notifyIcon.Text = Text;
            notifyIcon.BalloonTipTitle = ComandoTextoDeAjuda.Instancia.NomeDoAplicativo;
            notifyIcon.BalloonTipText = ComandoTextoDeAjuda.Instancia.NomeDoExecutavel + " " + Comando.ComandoInfo.Key;
            notifyIcon.Click += (sender, e) =>
            {   
                Show();
                notifyIcon.Visible = false;
            };
        }

        /// <summary>
        /// <para>Referência ao objeto principal do comando em uso.</para>
        /// </summary>
        internal protected Comando Comando { get; set; }

        /// <summary>
        /// <para>ipo da do objeto que contém as definições de texto em vários idiomas.</para>
        /// </summary>
        protected virtual Type TypeStringResource { get { throw new NotImplementedException("É necessário sobreescrever esta propriedade para retornar o tipo da classe de Resource Strings."); } }

        /// <summary>
        /// <para>Realiza a tradução dos textos dos controles,
        /// substituindo o seu valor pelo texto na classe de recursos.</para>
        /// </summary>
        protected virtual void TraduzirControles() 
        {
            foreach (Control control in Controls)
            {
                PropertyInfo propertyInfo = control.GetType().GetProperty("Text");
                if (propertyInfo != null)
                {
                    string valor = (string)propertyInfo.GetValue(control);
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        valor = ObterTextoTraduzido(TypeStringResource, valor);
                        propertyInfo.SetValue(control, valor);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Realiza a tradução dos textos dos itens de menu,
        /// substituindo o seu valor pelo texto na classe de recursos.</para>
        /// </summary>
        /// <param name="resourceType"><para>Tipo da classe de recursos.</para></param>
        /// <param name="menuItem"><para>Item de menu.</para></param>
        private void TraduzirMenu(Type resourceType, ToolStripMenuItem menuItem)
        {
            foreach (ToolStripItem item in menuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    TraduzirMenu(resourceType, item as ToolStripMenuItem);
                }
            }
            menuItem.Text = ObterTextoTraduzido(resourceType, menuItem.Text);
        }

        /// <summary>
        /// <para>Obtem o texto traduzido para um nome de propriedade de classe de recursos.</para>
        /// </summary>
        /// <param name="resourceType"><para>Tipo da classe de recursos.</para></param>
        /// <param name="nome"><para>Nome da propriedade estática na classe de recursos.</para></param>
        /// <returns><para>Caso seja encontrado, retorna o valor da propriedade.
        /// Do contrário, retorna o nome da propriedade.</para></returns>
        protected string ObterTextoTraduzido(Type resourceType, string nome)
        {
            ResourceManager resourceManager = (ResourceManager)resourceType.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            string valor = resourceManager.GetString(nome);
            return valor != null ? valor : nome;
        }

        /// <summary>
        /// <para>Exibe a janela com informações tipo "Sobre".</para>
        /// </summary>
        /// <param name="owner"><para>Janela ancestral.</para></param>
        protected void ExibirJanelaSobre(IWin32Window owner)
        {
            ComandoFormSobre form = new ComandoFormSobre();
            form.TextoInicial =
                ComandoTextoDeAjuda.Instancia.ObterTextoDeAjuda(ComandoTextoDeAjuda.ModoDoTextoDeAjuda.Cabecalho) +
                string.Format(Properties.Comando.formBase_textBoxTexto_Comando.Replace("\\n", Environment.NewLine), Comando.ComandoInfo.Key, Comando.Assembly.GetName().Version.ToString()) + Environment.NewLine + Environment.NewLine +
                Comando.Descricao.Replace("\\n", Environment.NewLine) + Environment.NewLine + Environment.NewLine +
                string.Format(Properties.Comando.formBase_textBoxTexto_ArquivosCarregados.Replace("\\n", Environment.NewLine), Assembly.GetEntryAssembly().Location, Comando.ComandoInfo.Value) + Environment.NewLine + Environment.NewLine +
                string.Format(Properties.Comando.formBase_textBoxTexto_LinhaDeComando.Replace("\\n", Environment.NewLine), Environment.CommandLine) + Environment.NewLine + Environment.NewLine +
                ComandoFormSobre.DesenhoAleatorio() + Environment.NewLine + Environment.NewLine +
                Comando.ObterTextoDeAjudaDesteComando()
                ;
            form.ShowDialog(owner);
        }

        protected MenuStrip menuStrip;
        protected ToolStripMenuItem menuItem_Aplicativo;

        #region InitializeComponent

        private ToolStripMenuItem menuItem_Aplicativo_Tray;
        private ToolStripMenuItem menuItem_Aplicativo_Sobre;
        private ToolStripSeparator menuItem_Aplicativo_Separator1;
        private ToolStripMenuItem menuItem_Aplicativo_Sair;
        protected ToolStripMenuItem menuItem_Biblioteca;
        private NotifyIcon notifyIcon;
        private System.ComponentModel.IContainer components;
        private Panel panelDivisoria2;
        protected ToolTip toolTip;
        private System.Windows.Forms.Panel panelDivisoria1;

        /// <summary>
        /// <para>Inicialização dos componentes visuais.</para>
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComandoFormBase));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuItem_Aplicativo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Aplicativo_Tray = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Aplicativo_Sobre = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Aplicativo_Separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem_Aplicativo_Sair = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Biblioteca = new System.Windows.Forms.ToolStripMenuItem();
            this.panelDivisoria1 = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.panelDivisoria2 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Aplicativo,
            this.menuItem_Biblioteca});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(10, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(400, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuItem_Aplicativo
            // 
            this.menuItem_Aplicativo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_Aplicativo_Tray,
            this.menuItem_Aplicativo_Sobre,
            this.menuItem_Aplicativo_Separator1,
            this.menuItem_Aplicativo_Sair});
            this.menuItem_Aplicativo.Name = "menuItem_Aplicativo";
            this.menuItem_Aplicativo.Size = new System.Drawing.Size(192, 20);
            this.menuItem_Aplicativo.Text = "formBase_menuItem_Aplicativo";
            // 
            // menuItem_Aplicativo_Tray
            // 
            this.menuItem_Aplicativo_Tray.Image = ((System.Drawing.Image)(resources.GetObject("menuItem_Aplicativo_Tray.Image")));
            this.menuItem_Aplicativo_Tray.Name = "menuItem_Aplicativo_Tray";
            this.menuItem_Aplicativo_Tray.Size = new System.Drawing.Size(284, 22);
            this.menuItem_Aplicativo_Tray.Text = "formBase_menuItem_Aplicativo_Tray";
            // 
            // menuItem_Aplicativo_Sobre
            // 
            this.menuItem_Aplicativo_Sobre.Image = ((System.Drawing.Image)(resources.GetObject("menuItem_Aplicativo_Sobre.Image")));
            this.menuItem_Aplicativo_Sobre.Name = "menuItem_Aplicativo_Sobre";
            this.menuItem_Aplicativo_Sobre.Size = new System.Drawing.Size(284, 22);
            this.menuItem_Aplicativo_Sobre.Text = "formBase_menuItem_Aplicativo_Sobre";
            // 
            // menuItem_Aplicativo_Separator1
            // 
            this.menuItem_Aplicativo_Separator1.Name = "menuItem_Aplicativo_Separator1";
            this.menuItem_Aplicativo_Separator1.Size = new System.Drawing.Size(281, 6);
            // 
            // menuItem_Aplicativo_Sair
            // 
            this.menuItem_Aplicativo_Sair.Image = ((System.Drawing.Image)(resources.GetObject("menuItem_Aplicativo_Sair.Image")));
            this.menuItem_Aplicativo_Sair.Name = "menuItem_Aplicativo_Sair";
            this.menuItem_Aplicativo_Sair.Size = new System.Drawing.Size(284, 22);
            this.menuItem_Aplicativo_Sair.Text = "formBase_menuItem_Aplicativo_Sair";
            // 
            // menuItem_Biblioteca
            // 
            this.menuItem_Biblioteca.Name = "menuItem_Biblioteca";
            this.menuItem_Biblioteca.Size = new System.Drawing.Size(193, 20);
            this.menuItem_Biblioteca.Text = "formBase_menuItem_Biblioteca";
            // 
            // panelDivisoria1
            // 
            this.panelDivisoria1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelDivisoria1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDivisoria1.Location = new System.Drawing.Point(0, 24);
            this.panelDivisoria1.Name = "panelDivisoria1";
            this.panelDivisoria1.Size = new System.Drawing.Size(400, 1);
            this.panelDivisoria1.TabIndex = 12;
            // 
            // panelDivisoria2
            // 
            this.panelDivisoria2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelDivisoria2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDivisoria2.Location = new System.Drawing.Point(0, 25);
            this.panelDivisoria2.Name = "panelDivisoria2";
            this.panelDivisoria2.Size = new System.Drawing.Size(400, 1);
            this.panelDivisoria2.TabIndex = 13;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 10000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 100;
            // 
            // ComandoFormBase
            // 
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.panelDivisoria2);
            this.Controls.Add(this.panelDivisoria1);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ComandoFormBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
