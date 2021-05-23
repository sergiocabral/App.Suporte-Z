using suporteZ.Inutil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suporteZ
{
    /// <summary>
    /// <para>Janela padrão para exibir informações tipo "Sobre".</para>
    /// </summary>
    public partial class ComandoFormSobre : Form
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public ComandoFormSobre()
        {
            InitializeComponent();
            Text = Properties.Comando.formBase_menuItem_Aplicativo_Sobre.Replace("&", string.Empty);

            buttonFechar.Text = Properties.Comando.formSobre_ButtonFechar;

            Load += (sender, e) =>
            {
                textBoxTexto.SelectionStart = 0;
                textBoxTexto.SelectionLength = 0;
            };

            buttonMusica.Click += (sender, e) =>
            {
                MusicaAleatorio();
            };

            buttonReuzirFonte.Click += (sender, e) =>
            {
                textBoxTexto.Font = new Font(textBoxTexto.Font.FontFamily, textBoxTexto.Font.Size * (float)0.9);
            };

            buttonAmentarFonte.Click += (sender, e) =>
            {
                textBoxTexto.Font = new Font(textBoxTexto.Font.FontFamily, textBoxTexto.Font.Size * (float)1.1);
            };

            FormClosed += (sender, e) =>
            {
                BeepMusical.PararTodas();
            };
        }

        private string textoInicial;
        /// <summary>
        /// <para>Texto exibido na parte inicial da janela de informações.</para>
        /// </summary>
        internal string TextoInicial
        {
            get
            {
                return textoInicial;
            }
            set
            {
                textoInicial = value;
                textBoxTexto.Text = TextoInicial + TextoExtra;
            }
        }

        private string textoExtra;
        /// <summary>
        /// <para>Texto exibido na parte final da janela de informações.</para>
        /// </summary>
        internal string TextoExtra
        {
            get
            {
                return textoExtra;
            }
            set
            {
                textoExtra = value;
                textBoxTexto.Text = TextoInicial + TextoExtra;
            }
        }

        /// <summary>
        /// <para>Toca uma música aleatóriamente.</para>
        /// </summary>
        public static void MusicaAleatorio()
        {
            List<Action> actions = new List<Action>();
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.BeethovenFurElise, true); });
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.DarthVader, true); });
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.DoReMiFa, true); });
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.FinalFantasyVictoryTheme, true); });
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.MusicalQuardust, true); });
            actions.Add(() => { BeepMusical.Tocar(BeepMusical.Musicas.SuperMarioBrosTheme, true); });
            int random = (int)Math.Round((float)DateTime.Now.Millisecond / ((float)999 / (float)(actions.Count - 1)));

            BeepMusical.PararTodas();
            actions[random]();
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII aleatório.</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoAleatorio()
        {
            List<Func<string>> actions = new List<Func<string>>();
            actions.Add(() => { return DesenhoSergioCabral(); });
            actions.Add(() => { return DesenhoRosanaCabral(); });
            actions.Add(() => { return DesenhoFamilia(); });
            actions.Add(() => { return DesenhoServicoDePioneiro(); });
            actions.Add(() => { return DesenhoSerginho(); });
            actions.Add(() => { return DesenhoLogotipo(); });
            int random = (int)Math.Round((float)DateTime.Now.Millisecond / ((float)999 / (float)(actions.Count - 1)));

            return actions[random]();
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoSergioCabral()
        {
           return
"▓▓▓▒▒▒▒▒▒▓▒▒▓▓▓▓▒█▓██▓▒██▓███▒▒▒█▒░▒█▒▒░▒▒▒▒▓▓█▓████▓▒▒███▒░▒▒░░ ▒█░░░░░░░░░░▒░" + Environment.NewLine +
"▓█▓▒▒▒░▒▓▓▒▓▓██░░░▒▒▓█▒██████▓▒▓█▒░▒█░▒░▒█▓░████████▓█████▓▒█▒░░ ▒█   ░░░░░░  ▒" + Environment.NewLine +
"▒▓▒░░░░░▒░ ▒▒▒██▓▒▓  ▓▒▒▒▒▒▒▒▓▒███░▒█░░░░▒░░▒███████▓████████    ▒▒   ░░░░   ░▒" + Environment.NewLine +
"░░░ ░▒▒░░░▒▒▒▒▒█░▒▒░░▓▒▒▓░░░░▓░█▓▓░            ░▓▒▒▓▓██▓███▒▓    ░▓   ░░▒▒░░░░░" + Environment.NewLine +
"▒░░█▓▓▒░░███▒██░ ▒▓░▒▓▓█▓▒░▒▒▒░▓░                 ░▒▓██▓ ░ ░▒░▒░ ▒▒░░░░░░▒░░░ ░" + Environment.NewLine +
"░▒▒▓█░░██░▒░████ ░░   ▒▒▒░░▓▒░                      ░▓██▒▒ ▓░░▒▒░░░▒▒░░░▒▓░▒░░░" + Environment.NewLine +
"░░░ ░░▓███▓█░███░   ▒░▒▒▒░                            ░▓██▓▓░▒▒▒░░░▒▒░░░░░▒▓░ ░" + Environment.NewLine +
"░    ▓▒░▓▓████░░░▒░▒░▒▓█▒▒       ░░░░                   ░█▓▒▒░  ░▒░▒░▒░░░▒░░░▒▒" + Environment.NewLine +
"░ ░░░██░ ░███▓░█▓▒▒▒█░▓█▒     ░▒██████▓▒▒░░░░░░░░░░       ░▓█▒▒░░░░░▒▒░░░░░▒█▓▒" + Environment.NewLine +
"░░░ ░░▒██▓█▒   ███▓ ██▓▒░    ░▓██████████████████████▒     ░░▓▒░░ ░░░▒░░░ ░░░▒▒" + Environment.NewLine +
"▒▒▒░░  ▓███▒  ▓████▒▓▒░░   ░░▓████████████████████████▒    ░▓▒░  ░░░░░░░░░░░░▒▓" + Environment.NewLine +
"  ▒ ░░░▒░▓░ ░▒ ▓▒███  ░░  ░░▒██████████████████████████░    ▒██░▒▒░░ ░░ ░░░ ▒░░" + Environment.NewLine +
"▓░░      ░▒█ ▒▓  ▒█▓░ ░░   ░▒██████████████████████████▓░   ░▓█▒░░░▒▒░  ░░░░░██" + Environment.NewLine +
"▒█▒ ░   ▓████▓███░▓█▓░▒░   ░░▓██████████████████████████░   ░░░░░░░░▒░  ░░░▒░▒▒" + Environment.NewLine +
" ██░░░░ ▒██▒░░ ███ ░░░░     ▒██████████████████████████▓░  ░░▒▒▒▒░░░░░░░░░░░  ▒" + Environment.NewLine +
"  ░▒░▒█░   ▓██▓▓██▒   ░    ▒██████▓▓▓▓█████████████████▓  ░░░░░▒▒▒█▒░▒░░▒░░░ ░▒" + Environment.NewLine +
"   ░░ ░░  ░▓████░███▒▓█░  ░███▓▒░░░   ░▒▓████▓▒░░ ░▒▓███░ ░░░ ░░░ ▒▓▒▒░ ▒░░░░░░" + Environment.NewLine +
"░ ░  ░░ ░░   ▓▒▓█▓▓████░  ░███▓▓▒▒░░░░▒▓▓█▓▓▒░░░░░░  ░██▒ ░░░ ░▒▒▓▒▒░░░░ ░░░░░░" + Environment.NewLine +
"▒░░▒░░░░░░   ░░▒▒▓▒ ░░▓█░  ▓███▓▓▓▓▓▒░░▒████▒░░░░░░░░▒▓█▒ ░░░░░░░█░░░░░░░░  ░░░" + Environment.NewLine +
"░▓▒░░░░░░░░   ░  ░▒░░░███▒ ▓████████████████▓▒▒▒▓▓▓▒▒▒██  ▒▒░░░ ░░░   ░░░░░░ ░░" + Environment.NewLine +
"░▒░ ░░░░ ░▒▒█░  ░░░░░ ▓███▓██████████████████▓▓█████████ ░▒▓░▒░░░░░▒▒░  ░░░░░░░" + Environment.NewLine +
"░   ░░░    ░▓█ ░▒░    ░██████████████████████▓▓████████▓░▓▓▒░▒░▒▒░░░▒▒▒▒▒▒░░░░░" + Environment.NewLine +
"    ░░░   ░░▒█▒▒▒ ░░░▒▒████████████▓▓▓▓▓▓▓█▓▓▓▓▒▓██████▓▒█▒ ░▒▒▒▒▒▒▒░░░░░░░░░█░" + Environment.NewLine +
"░░░░░░    ░░░▒░ ░░  ░░▓███████████▓▓▓▓▒░░▒▒░░░▒▒▓▓▓█▓▓▓▓▓▓  ░░▒▒░░▒▒▓░    ░░░▒░" + Environment.NewLine +
"▓▒██░░░ ░▒░ ░░ ░░░    ░▓▓▓▓▓█████▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▓▓▓▓▓▓██░    ░  ░░░░░░░░░░░░░░" + Environment.NewLine +
"░░░░░▒▒░▒▒▓▒░░░▒░░▒░▒░     ▓█████▓▓▒▒▓▓▓▓▓▒▒▒▒▓▒▒▒▓▓▓▓▓▓▒░▒░    ░  ░░░▓▓▒▒░░░▒▒" + Environment.NewLine +
"░ ░░░▒▒▒▒▒▒░░░░░▒░░░▓█▓    █▓▓███▓▓▒░▓██████▓▓░ ▒▒▓▓▓▓░░░▒▒▒░░░░     ▓██▒█▒░░░▓" + Environment.NewLine +
"   ░░░▓▓░▒░░▒▒▒░░░ ░ ░██░▒▒██▓▓██████▓▓▓████▓▓▒▒▓█▓▒▓░  ░░▒▒░░▒▒▒░░░░░█▒░█▒░░ ░" + Environment.NewLine +
"░░ ░ ░▒▓░░░░░░░░      ░▓█░░██▓▓▓▓█████▓▓▒▒▒▒▒▒▓██▓▒▒░     ░░░░ ░░░░░░ ░░░░ ░░░░" + Environment.NewLine +
"░░░░░░░░▒░░░░░▒▓█▒░        ████▓▓▓█████▓▒▒▒▒▓▓▓▓▒▒▒▒░░░░░░░░░░░░░░░▒░ ░▒░▓░ ░░▒" + Environment.NewLine +
"▒░▒▒░░░▒▒▒░▒░▒▒██▒░      ░░▓████▓▓▓███████████▓▓▒▒█▒▒▒░░▓█░░▒░░░░▒▒▒▒░ ░░█▓░░░▒" + Environment.NewLine +
"▒░░░░░░▒▒▒▒▒░░░░▒▓█▓▒▒░▒█▓  █████▓▓▓▓████████▓▓▒▒▓█▒ ░░░░░░▒░  ░░░░░░░▒░ ░▒░░░░" + Environment.NewLine +
"▒░░░░░░░░░░░░░░░▒▒▒▓▒████▓   ██████▓▓▒▒▒▓▓▒▒▒▒▒▒▒▓▒▓ ░▒░░░░░ ░░░  ░░░░▓█▒ ░▒░░░" + Environment.NewLine +
"▒░░░░░░░░░░░░░░▒▒▓▓▓███▓▓▓░   ▓███████▓▓▒▒▒▒▒▒▒▓▓▒ ▓▓▒░░░░░░░░░░░░▒▒░░  ░░  ░░░" + Environment.NewLine +
"▒░░░░░░░░░▒▒▒▓▓█▓▒▓█▓▓▓▓▓▓▒     ▓████████▓▓▓▒▓▓▓░  ▓██░▒▒▒▒▒▒▒▒▒▒▒▒▒▓▒▒░░░░░░░░" + Environment.NewLine +
"░░▒▒▒▒▓▓▓█████▓▒░▓▓▓▓▓▒▒▓▓▓░      ▒██████▓▓▓▓▓▒░   ░▓▓▓█████████████████▒░░█▒░░" + Environment.NewLine +
"██████████▓▓▓▓▒░▓▓▓▓▒▒▒▒▒▓▓▒        ▒▓███▓▓▓▒░      ▒▓▓▓▓▓▓██████████████░░░░░░" + Environment.NewLine +
"████▓▓▓▓▓▓▓▓▒▒▒▓▓▒▒▒▒▒▒▒▒▒▒▓░         ▒▒▓▒░         ░▓▓▒▒▒▒░░▒▒▒▓████████▒ ░░░░" + Environment.NewLine +
"█▓▓▓▓▓▓▓▓▓▓▓▒▒▓▓▓▒▒▒▒▓▒▒▒▒▒▓▒          ░░            ▒▓▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▒░░   ░░" + Environment.NewLine +
"▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▓▓▓▓▓▓▒▒▒▒▒▒▒▒░          ░░▒░         ▒▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓███▓▒░░" + Environment.NewLine +
"▓▒▓▓▓▓▓▓▓▓▓▓▓▒░░░▒▒▓▒▒▒▒▒▒▒▓▒▒      ░░░░░░▒▒▒        ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓█████▓" + Environment.NewLine +
"▓▒▓▓▓▓▓▓▓▓▓▓▒▓▒░▒▒▒▒▒▒▒▒▒▒▒▓▒▒      ▒▒░░░▒░░░        ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓█" + Environment.NewLine +
"▓▒▒▒▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▓▒▒░     ░▓▓▒▒░░░         ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓" + Environment.NewLine +
"▓▒▒▒▓▓▓▒▒▒▒▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒      ░▒▒░ ░          ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░▒▒▒▒▒▒" + Environment.NewLine +
"▓▒▒▒▒▒▒▒▒▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒      ▒▒░ ▒▓░          ▒▒▒▒▒▒▒▒▒▒▒▒░░▒▒▒▒░▒▒▒░▒▒" + Environment.NewLine +
"▓▒▒▒▒▒▒▒▒▒▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░    ░░░░▒▓▓░          ▒▒▒▒▒▒▒▒▒▒░  ▒▒▒░▒▒▒▒▒░░▒" + Environment.NewLine +
"▒▒▒▓▓▓▓▓▓▓▓▓▓▓▒▒▒▓▒▒▒▒▒▒▒▒▒▓▓▒▒░    ░▒▓▒▒▒░░          ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░▒▒▒▒▒▒▒";
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoRosanaCabral()
        {
            return
"████▓████▓█▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒░░ ░░░░░░░░░░░░░░░▒▓███▓▓▓█▓▒░░░░░░░░░▒▒▒▒▒▒▒▒▓█████" + Environment.NewLine +
"█████████████████████▓▒▒▒▒░░ ░░░░░░░░░░░░░░░░ ░▒███████████████████████████████" + Environment.NewLine +
"██▓▓▓▓▓▓▓▓██▓▓▓▓▓▓█▓▒░░░░░  ░░  ░░░░░░░░░░░░     ▒▓█▓▓█████████████████████████" + Environment.NewLine +
"▓█▓█▓▓▓▓▓▓█▓▓▓▓▓██▒  ░░░░  ▒██▓▓░░░░░░░░░░░░░     ░▓███▓▓▓▓██████████████████▓█" + Environment.NewLine +
"▓█▓█▓▓▓█████▓▓██▓░ ░░    ░▓██████▒░░ ░░░░░░░ ░░░░░ ░▓██▒░░▒▓▓██████████████████" + Environment.NewLine +
"▓█▓█████▓▓█████▒  ▒▒    ░▒████████▓░  ░░░░░░░  ░░   ░▒▓█▓░▓██████████████████▓ " + Environment.NewLine +
"▓█▓███▒   ░▓▓▒░ ░░░░   ░▒██████████▓░   ░░░░░░       ░▒▓▓▓▓▓██▓█████████████▒  " + Environment.NewLine +
"██████         ░░░░    ░▓█▓▓▓▓▓▓████▓░   ░  ░░░░░     ▒█████████████▓█▓████▓  ░" + Environment.NewLine +
"▓█▓▓▓▓        ░░░░░░  ░▒▓▓▓▓▓▓▓▓▓▓▓██▓▒░     ░░ ░░     ██████████▓▓▓██▓▓██▓▒░░ " + Environment.NewLine +
"░      ░░░  ░░ ░░░░░  ░▓▓▓▓▓▓▓▓▓▓▓███▓▓▒░              ▒███████▓▓▓██████▓▓▓▓▓░ " + Environment.NewLine +
"      ░░░░░░░▒░░░░░░  ░▓▓▓███████▓▓▓█▓▓▓▓▒░             ███████▓█▓▓▓▓█▓▒▓▓▒░▒▒▒" + Environment.NewLine +
"░    ░ ░░    ░░ ░░░░  ░▓▓▓▓▓▓▒▒▓▓▓▓▓▓▓▒▓▓▒░             ▓█████▓▓▓▓▓▓▓▓▒▒▓░░▓██▒" + Environment.NewLine +
"░░░░ ░░   ░      ░▒▓  ▓▓▓▓▓▓▒░ ░▒▒▒▓▓▒░▒▒░▒▒▒░          ░█████████▓▓▓▒▒▓▒▒▓████" + Environment.NewLine +
"░░░░░ ░ ░██▓░ ░░ ░░▒░▒█▓▒░░    ░░░▒▓▓▒▒▒░░░              ██████▒▒███████████▒▒▒" + Environment.NewLine +
"░░░░ ░░ ░███▒ ░█▒ ░▒░▓█▒         ░▒▓█▓▒░  ░░             ▒█████▒░▓████████▓░   " + Environment.NewLine +
"░░░     ▒███▒░▓▓▒  ░░▒▓▓▓▒░░░░░░░▒▓███▓▒▒▒▒▒▒▒▒▒▒▒░░      ██▓████████████████▓█" + Environment.NewLine +
"░░░     ▒██▓▒▒▓░   ░▒▓▓████▓▓▓▓▓████████▓▓▓▓▓▓▓▓▓▓▓░      ░▓▒▒▒▓▒▒▒▒▒▓▓▓▓▓▓████" + Environment.NewLine +
"░░░      ▒▓▒░░░    ░▒▓▓██████████████████▓█████████▒       ▒▓▒▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒" + Environment.NewLine +
"░░░  ░░             ▒▓▓▓██████▓░▒█████▓██▓▒▓▓▓▓▓▓▓█▒       ▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▓" + Environment.NewLine +
" ░░  ░░  ░   ░░       ░▓▓▓▓▓▓▒░░▒▒░▒▒▒▒░▒▒░░▒▒▓▓▓▓▓▒       ░▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░    ░ ░▓   ░░       ░▓▒▒▒▒▒░░▒▒░░░░░░░░▒▒░░░▒▒▒▒▓▒        ▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░      ░▒   ░░▒░     ░▓▒▒▒░░▒▒▓▓▒▒▒▒▒▒▒▒▒▓▒▒░░▒▒▒▓░        ▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░    ░  ░ ░▒░░▓▒      ▓▓▒▒░░░  ░▒░░░░░░░▒░  ░▒▒▒▒▒         ▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░  ░░░    ▒█████      ░█▓▓▒▒▒░ ░█▓▓▓▓▓▓▓▒  ░▒▒▒▒▒░         ▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░  ░░░░░░░░▒▒▒█▓       ░▓▓▓▓▓█▓▒▓█▓▓▓▓▒▒▒▒▓▓▓▒▒▒          ░▓██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
" ░  ░▒▒░░░░               ▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▓▓▓▓▒▒░          ░▒▓▓▓▓▓▓▓▓▓▓▓█▓▓▓▓▓▓▓" + Environment.NewLine +
" ░  ░░                     ▒▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒           ░░▒▓▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"    ░          ░░░          ▒▓▓█▓▓▓▓▓▓▒▓▓▓▓▓▒           ░░▒█▒░█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"░░░ ░ ░░░░░░░░░░░░          ░▒▒▓████▓▓▓▓▓▓▓▒            ░░░  ░█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▓██▒  ░░░░░░░░ ░░░           ▒▒▒▒▓▓▓▓▓▓▓▒▒         ░░   ░░   ▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▒▓▓▒  ░░░░░░░   ░░        ░  ░▒▒▒░▒▒▒▒▒░         ░░    ░░░░ ░██▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▒▓▓░        ░░▓▓░░        ░ ░░▒▒▒▒▒░░░                         ▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▒▓▓░      ▒████ ░░  ░    ░░░░░░▒▒▒▒▒░░░        ░         ░    ▒███▓▓▓▓▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▒▓▓░    ▒██▒▓▓  ░  ░░░   ░░░▒▒▒▒▒▒▒▒▒░░   ░       ░░░    ░░  ▒████████▓▓▓▓▓▓▓▓▓" + Environment.NewLine +
"▒▓▓  ░░▓█░░▓░  ░  ░░     ░░▒▒▓▓▓▓▒▒▒▒▒░            ░░░      ░██▓▓▓▓▓████▓▓████▓" + Environment.NewLine +
"▒▓▓░  ▒▓░░▓▓░           ░░░▒▒▓▓▓▓▓▓▓▓▓▓░                    ▒█▓▓▓▓▓▓▓▓▓██▒░░▒▓▓" + Environment.NewLine +
"▒▓▓▒░▒▒░░░░    ░        ░░░▒▒▒▓▓▓▓▓▓▓▓▓▓░                   ▓▓▓▓▓▓██▓▓▓▓▓█░    " + Environment.NewLine +
"▓▓▒▒▓█▓░      ░  ░      ░░░▒▒▒▒▓▓▓▓▓▓▓▓░ ░░░░░             ▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓█░   " + Environment.NewLine +
"▒░ ░▓██▒░ ░  ░  ░░     ░░░▒▒▒▒▒▒▓▓▓▓▓▓▓░    ░░░░           ▒█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░░" + Environment.NewLine +
"▓███████▓░             ░░░▒▒▓▓▓▓▓▓▓▓▒▒▓▒    ░░░░░░░        ▓▓▒▓▓▓▓▓▓▓▓▓▓▒▒▓▓▒░░" + Environment.NewLine +
"████▓▓▓█▓▓░              ░▒▒▒░▒▓▓▓▓▓▓▓▓▓▓▒░                  ░▓▓▓▓▓▓▓▓▓▓▒▒▒▓▓░ " + Environment.NewLine +
"████▓▓▓▓▓░              ░░░▒▒  ▒▓▓▓▓▓▓▓▓▓▓▒▒▒               ░░▒▒▓▓▓▓▓▓▓▓▒▒▒▒▓▒ " + Environment.NewLine +
"███▓▓▓▓▓▓▓░          ░░░░  ▒░   ▒▓▓▓▓▓▓▓▓▓▓█▒▒░           ░▒░░ ░▒▓▓▓▓▓▓▓▒▒▒▒▓▓ " + Environment.NewLine +
"███▓▓▓▓▓▓▓▒   ░░               ░░▒▒▒▓▓▓▓▓▓██▓█▒            ░   ░▒▓▓▓▓▓▓▓▒▒▒▒▒▓▒" + Environment.NewLine +
"██▓▓▓▓▓▓▓▒░                   ░░░░▒▒▓▓▓▓█▓█▒▒█░               ░▒▓▓▓▓▓▓▓▓▒▒▒▒▒▓█" + Environment.NewLine +
"██▓▒▓▓▓▓▒▒░                    ░░░▒▒▓▓▓▒░▓█▓▓▓     ░░░░░   ░▒▒▒▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▓" + Environment.NewLine +
"██▓▓▓▓▓▓▓▒▒░        ░░         ░░░░░░░   ▒█▒▓▓░            ░▒▒▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒";
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoFamilia()
        {
            return
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"█████████████████████████████████████████████▓░      ░▒████████████████████████" + Environment.NewLine +
"██████████████████████████████████████████▓              ▒█████████████████████" + Environment.NewLine +
"████████████████████████████████████████▓                  ▓███████████████████" + Environment.NewLine +
"███████████████████████████████████████                      ██████████████████" + Environment.NewLine +
"██████████████████████████████████████     ▒███▓▒░            ▓████████████████" + Environment.NewLine +
"█████████████████████████████████████▓    ████████████████▓    ▓███████████████" + Environment.NewLine +
"█████████████████████████████████████░   ▓██████████████████    ███████████████" + Environment.NewLine +
"█████████████████████▓▒▒░▒███████████    ███████████████████    ███████████████" + Environment.NewLine +
"██████████████████░         ▒████████    ███████████████████░  ░███████████████" + Environment.NewLine +
"████████████████              ███████   ▒██▓▒▒▒▒████████████▒  ▓███████████████" + Environment.NewLine +
"██████████████▒                ██████   ███      ▓██░     ▒█▓  ████████████████" + Environment.NewLine +
"█████████████▒           ▒██▒   ████▓   ███▓░░   ███       █▓ ░████████████████" + Environment.NewLine +
"████████████▒           █████▓  ░█████▒ ████████████░     ░█▒  ▓███████████████" + Environment.NewLine +
"████████████           ███████▒  ▓███████████████████ ▓█████░  ▓███████████████" + Environment.NewLine +
"███████████           ▓████████   █████████████▓▒▓▓▓░  █████  ░████████████████" + Environment.NewLine +
"███████████       ▒████████████▒   ████████████         █████▒█████████████████" + Environment.NewLine +
"██████████░       ▓█████▓▒▓▓████   ██████████▓░            ████████████████████" + Environment.NewLine +
"██████████     ▒░░ ░███░ ░   ███   ▓█████████▒  ████░      ▒███████████████████" + Environment.NewLine +
"██████████  ░██████▓██████████████ ▒████████████░      ▒▒ ░████████████████████" + Environment.NewLine +
"█████████▓  ▒████████████████████░ ░███░█████████        ░█████████████████████" + Environment.NewLine +
"█████████▒  ░██████████████████▓▒  ░██▒ ▒████████████▓   ▒█████████████████████" + Environment.NewLine +
"█████████░   ██▓░██▓███████████    ░█░   ▓████▓▒▒▓▓▓▒    ▒█████████████████████" + Environment.NewLine +
"█████████    ▓██▒  ▒░▒▓▓░ ▓███▓           ░█████▓        ░█████████████████████" + Environment.NewLine +
"█████████     ▓██▓▒██████▓████              ░█████▒       ▒████████████████████" + Environment.NewLine +
"█████████       ███▓▓███████▒       ░          ▓██▒         ▒▓▓████████████████" + Environment.NewLine +
"████████▒        █████████▒         ░                             ▒████████████" + Environment.NewLine +
"███░░███          ░██████           ░░                               ░▓████████" + Environment.NewLine +
"███                 ░░░              ░                                  ░██████" + Environment.NewLine +
"███                                  ░                                    ░████" + Environment.NewLine +
"███▒                                 ░  ▒████▓▒▒                           ░███" + Environment.NewLine +
"████                ▒██████     ░    ▒██████████████▒░                      ███" + Environment.NewLine +
"████             ▒█████████  ░░░▓██████▓   ▒███████████▒                ░▒▓████" + Environment.NewLine +
"████▓         ░▓████████▓░  ░░  ░███████▓▒▒▓████████████▓ ░░░░▒▒▒▓▓████████████" + Environment.NewLine +
"█████▓      ▒███████████     ░░░░▓█████████████▓███████████████████████████████" + Environment.NewLine +
"███████▓▓██████████████████▒░░▓░░▓▓████████████▓███████████████████████████████" + Environment.NewLine +
"████████████████████████████████▓░       ░█████▒███████████████████████████████" + Environment.NewLine +
"███████████████████████████████           ██████▓██▓▓██████████████████████████" + Environment.NewLine +
"███████████████████████████████░  ░▓██▓░ ██████████▓███████████████████████████" + Environment.NewLine +
"████████████████████████████████████████▒██████████████████████████████████████" + Environment.NewLine +
"█████████████████████████████████████████▓█████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████" + Environment.NewLine +
"███████████████████████████████████████████████████████████████████████████████";
        }


        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoServicoDePioneiro()
        {
            return
"                          ░░▒▒▓▒ ░▒▓░                                          " + Environment.NewLine +
"                        ███████████████████▓                                   " + Environment.NewLine +
"                     ▓███████▓████████████████▓▒                               " + Environment.NewLine +
"                    ▓███████████░▒███████████████▓░                            " + Environment.NewLine +
"                    ░▓█▓▓███████▒█▒░      ▒▒▓███████                           " + Environment.NewLine +
"                     ██▒▒ ░██▓█▓             ▒░███▓██                          " + Environment.NewLine +
"                    ▓████░                ▒████▓█████▒                         " + Environment.NewLine +
"                     ▓███░               ▒█████▓██████                         " + Environment.NewLine +
"                      ███              ▒████▓▓████████                         " + Environment.NewLine +
"                       ██             ░████▒  ████████                         " + Environment.NewLine +
"                       ▒█               ▓████▓████████                         " + Environment.NewLine +
"                        ░░██   ░██░       ███████████░                         " + Environment.NewLine +
"                       ▒███▒    ███▓      ██▓▓█▒████░                          " + Environment.NewLine +
"                       ░                 ▒██▒ ▓   ▒█░                          " + Environment.NewLine +
"                        ░█▒    ▒███▓░    ░███       ██                         " + Environment.NewLine +
"                         ▓▓      ░▓▓     ░██▓  ▒▓    █▒                        " + Environment.NewLine +
"                       ░█▒                   ▓█▒     ▓█                        " + Environment.NewLine +
"                      ░█░                  ▓█        █▒                        " + Environment.NewLine +
"                      █▒                   ░░ ▓  ░▒██░                         " + Environment.NewLine +
"                     ██                        ██▓▒                            " + Environment.NewLine +
"                    ░█   ░░░                   █                               " + Environment.NewLine +
"                    ░████▓▓▒                   █▓                              " + Environment.NewLine +
"                     ░█▓                   ██▒▒█░                              " + Environment.NewLine +
"                    ░█▒                   ▒█ ░░                                " + Environment.NewLine +
"                    █▓            ░▒░     █▒                                   " + Environment.NewLine +
"                    ░██▓▓ ░▓▓▓▓█████      █                                    " + Environment.NewLine +
"                     █▓ ██ ██░ ██░ █░     █                                    " + Environment.NewLine +
"                    ▒█░ ██  █   █▒░█      █                                    " + Environment.NewLine +
"                    ▒██▓█▓ ░██▒▒▓▒▒       █                                    " + Environment.NewLine +
"                      ▒░ ▓▓▓░██           █▓       ░░                          " + Environment.NewLine +
"               ██▓            ░▓▓          ▓     ░█▓███▓    ▒█▒▓               " + Environment.NewLine +
"              ██ ██▒           ▒█▓ ░▒▒▓█████▓    █▓    ▒█▒ ██ ░█               " + Environment.NewLine +
"             ░█   ██           ████████████▓▓▒   ░█▒░   ░██░   ███             " + Environment.NewLine +
"            ▒██▓▒██▒▒░░░░░░    ▒▓▓ ████████░▒▓▓▓▓▓████▓▓██░ ░▒▒██▓             " + Environment.NewLine +
"            ░█░ ░░▒▓▓▒▒▓▓▓▓▓▓▓▓▒░░▒░░░                   █████▓                " + Environment.NewLine +
"           ▒█░  ▓▒▒▒▓█    ▓                                ██     ▒█           " + Environment.NewLine +
"            █  ██    ░█  ░█  █░  █▓▒██      ░               ▓█▒▒▓██▒           " + Environment.NewLine +
"            █  █▓     █ ▒█   █▒ ██    █▒    █▒ █▓ ░█▓  ▒█▓    ██▒              " + Environment.NewLine +
"            █   ██████ ▒█    █░ █     ▓█    ▒█░█ ░█░█▒██░▓    ██░░  ▒▓         " + Environment.NewLine +
"            █  ▓█░   █ ▒▓▓▓▓▒█░ █      █     ▓█▒ █▓ █░ ▒█     ░████▓█▓         " + Environment.NewLine +
"            █ ██     ░█    ░░█▒ █      █▒     █ ░█░░  ▒░▓█     ░██▓            " + Environment.NewLine +
"            ██▓       ▓█     ▒█ ██     █▒    █   █░    ██▓                     " + Environment.NewLine +
"            ██▓       ▒█         ▒██▓▒██    █▒    ▒▒            ▒░             " + Environment.NewLine +
"            █▒▓▓▓▓▒░░▓█             ▒▒░                         █▓             " + Environment.NewLine +
"            ▒█░░▒▒▒▒▒░                                     ░█▓▒▓██             " + Environment.NewLine +
"              ░▒▓▓▓▒░   ░░░░░░░       ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░   █▓▓██              " + Environment.NewLine +
"                   ░░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░▒▒▒▒▒▓█▒▒░               ";
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoSerginho()
        {
            return
"                                 ▓██████████████▒                              " + Environment.NewLine +
"                              █████████████████████                            " + Environment.NewLine +
"                          █████████████████████████████                        " + Environment.NewLine +
"                        ▓████████████████████████████████                      " + Environment.NewLine +
"                       ████████████████████████████████████                    " + Environment.NewLine +
"                      ██████████████████████████████████████▒                  " + Environment.NewLine +
"                     ████████████         ▒███████████████████                 " + Environment.NewLine +
"                     ██████████               ▒█████    ▓█████                 " + Environment.NewLine +
"                    ██████████                           ▓█████                " + Environment.NewLine +
"                   ▓██████████                            █████                " + Environment.NewLine +
"                   ████████████                           ████                 " + Environment.NewLine +
"                   ████████████                           ███                  " + Environment.NewLine +
"                   ███████████      ▒█████████            ██                   " + Environment.NewLine +
"                   ██████████    █████████████    ▒████▓  █                    " + Environment.NewLine +
"                    ████████    ▒█████████▓▒      ▓██████▒█                    " + Environment.NewLine +
"                   ███░█████      ██████▓█░      ▓█████▓▒ █░                   " + Environment.NewLine +
"                  █████▓ ████     ▓░░██▓         ████████▓█                    " + Environment.NewLine +
"                  ███████  ██                     █░    ░██                    " + Environment.NewLine +
"                  ▓█ ░████                        ▓▓      █                    " + Environment.NewLine +
"                   ██▓░███                ██░▒░  █ █▓▒    █▓                   " + Environment.NewLine +
"                    ░██▒█▓                ██████ ▓████    █                    " + Environment.NewLine +
"                      ███           █▓                ██ ██                    " + Environment.NewLine +
"                      ░█████       ██░    ▒█████████▓ ██ █▓                    " + Environment.NewLine +
"                       ▓████       ██ ████████████████   █                     " + Environment.NewLine +
"                        ▓████      ░  ▓███████████████░ ▓█                     " + Environment.NewLine +
"                          ███▒              ▓██████    ██▒                     " + Environment.NewLine +
"                           ████              ▓████▓   ██▓                      " + Environment.NewLine +
"                             ████                    ██▒                       " + Environment.NewLine +
"                              ██████▓               ██                         " + Environment.NewLine +
"                              ██  █████▓          ▓██                          " + Environment.NewLine +
"                              ██     ▓██████████████                           " + Environment.NewLine +
"                            ▓█▒███▒     ░████████▒                             " + Environment.NewLine +
"                         ▒███   ▒████▓░  ███▒▒█▓                               " + Environment.NewLine +
"                    ░████████      ▒███▓▒█░   ▒████▓▓░                         " + Environment.NewLine +
"                    ███▓░   ███      ▓█████▒  ██▒████████                      " + Environment.NewLine +
"                   ▓█        ▒██  █████████████        ▓█▓                     " + Environment.NewLine +
"                   ██          ████▓████▓ ██▒██         ██                     " + Environment.NewLine +
"                   ██    ████   ▓▓    █████             ██                     " + Environment.NewLine +
"                   ██    ▓███         █████▒      ███   ██                     " + Environment.NewLine +
"                  ▒█      ██▓        ███████      ███   ██                     " + Environment.NewLine +
"                   █▓      ██        ███▒  █▓     ▓█    ▓█                     " + Environment.NewLine +
"                  ▓█       ██       ██▒ ░████     ██    ▓█                     " + Environment.NewLine +
"                   █        █      ██ ▓██████▓    ██    ▓█                     " + Environment.NewLine +
"                  ██       ██      ████████ ██    ██    ░█                     " + Environment.NewLine +
"                  ██       ██▒      █████  ██     ██     █▓                    " + Environment.NewLine +
"                   █       ██░       ▓█▓ ▓█▓      ██    ██                     " + Environment.NewLine +
"                   █       █           ███        █▒   ▓█                      ";
        }

        /// <summary>
        /// <para>Retorna um desenho ASCII: Serginho como Caricatura</para>
        /// </summary>
        /// <returns><para>Desenho como texto ASCII</para></returns>
        public static string DesenhoLogotipo()
        {
            return
"████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████████" + Environment.NewLine +
"██████▒                                                                 ▒██████" + Environment.NewLine +
"████▓░                                                                   ░▓████" + Environment.NewLine +
"██▓░ ▒█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒░ ░▓██" + Environment.NewLine +
"█▓   ▒█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒░   ██" + Environment.NewLine +
"█▓   ▓█▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒░                                                       ▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒░                                                       ▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒░                                                       ▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒░                                                       ▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓█▓▓▓▒░        ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░     ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░     ▒▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓▓▓▓▓▒░        ▒███████████████░     ░██████████████████▒     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   █▓▓▓▓▒░     ▒▒▓████▒▒▒▒▒▒▒▒▒▒▒▒░     ░▒▒▒▒▒▒▒▒▒▒▒▒██████▒     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   █▓▓▓▓▒░     ▓██████                               ██████▒     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   █▓▓▓▓▒░     ▓██████                            ▒▓▓███▓░░░     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▓▒░     ▒▓▓████░░░░░░░░░                 ░░████▓▓▒        ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▓▒░        ▒███████████▓                ▒█████▒           ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▓▒░        ░▓▓▓▓▓▓▓▓████▒░░          ░░░████▓▓░           ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▓▒░                 ▒██████░        ░██████░              ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░                 ▒██████░     ░░░▒███▓▓▒               ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░                 ▒██████░     ░██████▒                 ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░     ░▒▒▒▒▒▒▒▒▒▒▒▓███▓▒▒░     ░██████▓▒▒▒▒▒▒▒▒▒▒▒░     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░     ▓██████████████▓         ░██████████████████▓     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░     ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒░         ░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░     ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░█▓▓▓▒▒░                                                       ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░▓▓▓▓▒▒░                                                       ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░▓▓▓▓▒▒░                                                       ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░▓▓▓▓▒▒░                                                       ▒█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓  ░▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓█▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒░   ▓█" + Environment.NewLine +
"█▓   ▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒░   ██" + Environment.NewLine +
"██▓░  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░ ▒███" + Environment.NewLine +
"████▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░                             ░▓████" + Environment.NewLine +
"██████▒                                                                 ▒██████" + Environment.NewLine +
"████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓████████";
        }
    }
}