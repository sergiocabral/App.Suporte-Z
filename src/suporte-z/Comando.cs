using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace suporteZ
{
    /// <summary>
    /// <para>Classe básica para um comando executado pelo prompt.</para>
    /// <para>Projetos a parte implementam esta classe para disponibilizar o comando.</para>
    /// </summary>
    public abstract class Comando : IComando
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public Comando()
        {
            Dados = new Dictionary<string, object>();
        }

        /// <summary>
        /// <para>Conjunto de opções para execução de comandos.</para>
        /// </summary>
        protected OptionSet OptionSet { get; private set; }

        /// <summary>
        /// <para>Assembly carregado para execução deste comando.</para>
        /// </summary>
        internal protected Assembly Assembly { get; private set; }

        /// <summary>
        /// <para>Informações sobre o comando, tendo <c>Key</c> como
        /// o nome do comando e <c>Value</c> como o caminho da biblioteca do assembly.</para>
        /// </summary>
        internal protected KeyValuePair<string, string> ComandoInfo { get; private set; }

        /// <summary>
        /// <para>Agrupa informações diversas usadas pelo comando.</para>
        /// </summary>
        public IDictionary<string, object> Dados { get; private set; }

        ///// <summary>
        ///// <para>Quando <c>true</c> cancela a execução do comando e finaliza.</para>
        ///// </summary>
        //protected bool CancelarExecucao { get; set; }

        /// <summary>
        /// <para>Disparar uma exceção de erro por causa de argumentos inválidos.</para>
        /// </summary>
        /// <param name="args"><para>Lista de argumentos inválidos.</para></param>
        protected void ExceptionPorArgumentosInvalidos(IEnumerable<string> args)
        {
            StringBuilder argsInvalidos = new StringBuilder();
            args.Select(s => s).ToList().ForEach(arg => { argsInvalidos.Append((argsInvalidos.Length == 0 ? string.Empty : ", ") + (arg.Contains(" ") ? "\"" + arg + "\"" : arg)); });
            throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgArgumentosInvalidos, argsInvalidos.ToString()));
        }

        /// <summary>
        /// <para>Dispara uma exceção de erro por causa de argumento que foi acionado mais de uma vez.</para>
        /// </summary>
        /// <param name="arg"><para>Argumento.</para></param>
        protected void ExceptionPorArgumentoEmDuplicidade(string arg)
        {
            throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgDuplicidadeDeArgumento, arg));
        }

        /// <summary>
        /// <para>Dispara uma exceção de erro por causa de argumento que deveria ser usado sozinho.</para>
        /// </summary>
        /// <param name="arg"><para>Argumento.</para></param>
        protected void ExceptionPorArgumentoQueDeviaEstarSozinho(string arg)
        {
            throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgArgumentoDeveSerSozinho, arg));
        }

        /// <summary>
        /// <para>Descrição deste comando.</para>
        /// </summary>
        internal protected abstract string Descricao { get; }

        /// <summary>
        /// <para>Texto de ajuda deste comando.</para>
        /// </summary>
        /// <returns><para>Texto de ajuda.</para></returns>
        internal protected string ObterTextoDeAjudaDesteComando()
        {
            StringBuilder ajuda = new StringBuilder();

            ajuda.AppendLine(Descricao.Replace("\\n", Environment.NewLine));
            ajuda.AppendLine();
            ajuda.AppendLine(string.Format(Properties.Comando.msgAjudaParaComando, ComandoInfo.Key, Assembly.GetName().Version.ToString()));
            ajuda.AppendLine();

            StringWriter stream = new StringWriter();
            OptionSet.WriteOptionDescriptions(stream);
            ajuda.AppendLine(stream.ToString());
            stream.Dispose();

            return ajuda.ToString();
        }
        
        /// <summary>
        /// <para>Método principal para a execução do comando.</para>
        /// </summary>
        /// <param name="comandoInfo"><para>Informações sobre o comando, tendo <c>Key</c> como
        /// o nome do comando e <c>Value</c> como o caminho da biblioteca do assembly.</para></param>
        /// <param name="assembly"><para>Assembly carregado para execução deste comando.</para></param>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        public void Executar(KeyValuePair<string, string> comandoInfo, Assembly assembly, string[] args)
        {
            ComandoInfo = comandoInfo;
            Assembly = assembly;

            OptionSet = new OptionSet()
                .Add("?|h|help", Properties.Comando.optionAjuda,
                    v =>
                    {
                        Windows.Console.Write(ComandoTextoDeAjuda.Instancia.ObterTextoDeAjuda(suporteZ.ComandoTextoDeAjuda.ModoDoTextoDeAjuda.Cabecalho));
                        if (v == "help")
                        {
                            Windows.Console.WriteLine(ComandoFormSobre.DesenhoAleatorio());
                            Windows.Console.WriteLine();
                        }
                        Windows.Console.Write(ObterTextoDeAjudaDesteComando());

                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComSucesso);
                    });
            ConfigurarOptionSet(OptionSet);
            Executar(args);
        }
        
        /// <summary>
        /// <para>Configura as informações do <see cref="NDesk.Options.OptionSet"/>.</para>
        /// </summary>
        /// <param name="optionSet"><para><see cref="NDesk.Options.OptionSet"/> pre configurado.</para></param>
        protected abstract void ConfigurarOptionSet(OptionSet optionSet);

        /// <summary>
        /// <para>Configura os parâmetros do <se</para>
        /// </summary>
        /// <summary>
        /// <para>Execução específica do comando.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        protected abstract void Executar(string[] args);

        /// <summary>
        /// <para>Abre a interface gráfica e desassocia o processo atual do seu console.</para>
        /// </summary>
        /// <param name="formType"><para>Tipo do <see cref="System.Windows.Forms.Form"/> principal da
        /// interface gráfica (GUI) para execução como aplicativo Windows.</para></param>
        public void AbrirInterfaceGrafica(Type formType)
        {
            if (formType != null && typeof(ComandoFormBase).IsAssignableFrom(formType))
            {
                Windows.Console.DescartarConsole();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run((ComandoFormBase)formType.GetConstructor(new Type[] { typeof(Comando) }).Invoke(new object[] { this }));
            }
            else
            {
                throw new ArgumentException("O tipo não pode ser nule é deve herdade de System.Windows.Forms");
            }
        }
    }
}
