using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suporteZ.cmd.cryptof
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
        }

        /// <summary>
        /// <para>Tipo da do objeto que contém as definições de texto em vários idiomas.</para>
        /// </summary>
        protected override Type TypeStringResource { get { return typeof(Properties.Comando); } }


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

        #region InitializeComponent

        private ToolStripMenuItem menuItem_Biblioteca_EmDesenvolvimento;

        /// <summary>
        /// <para>Inicialização dos componentes visuais.</para>
        /// </summary>
        private void InitializeComponent2()
        {
            this.menuItem_Biblioteca_EmDesenvolvimento = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem_Biblioteca
            // 
            this.menuItem_Biblioteca.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuItem_Biblioteca_EmDesenvolvimento,
            });
            // 
            // menuItem_Biblioteca_Algoritmo
            //
            this.menuItem_Biblioteca_EmDesenvolvimento.Enabled = false;
            this.menuItem_Biblioteca_EmDesenvolvimento.Name = "menuItem_Biblioteca_EmDesenvolvimento";
            this.menuItem_Biblioteca_EmDesenvolvimento.Size = new System.Drawing.Size(239, 22);
            this.menuItem_Biblioteca_EmDesenvolvimento.Text = "Em desenvolvimento";
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
