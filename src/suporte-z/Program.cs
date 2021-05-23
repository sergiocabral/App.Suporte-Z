using NDesk.Options;
using suporteZ.Globalization;
using suporteZ.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace suporteZ
{
    /// <summary>
    /// <para>Classe principal do aplicativo.</para>
    /// <para>Contem o método principal (<see cref="Program.Main"/>)
    /// que é chamado pelo sistema operacional.</para>
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// <para>Método principal chamado pelo sistema operacional para dar
        /// início à execução do aplicativo.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        [STAThread]
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = ObterUICultureInfo(args);

            KeyValuePair<string, string> comandoInfo = ObterComandoAtual(args);
            if (string.IsNullOrWhiteSpace(comandoInfo.Key))
            {
                Windows.Console.WriteLine(ObterTextoDeAjuda());
            }
            else
            {
                try
                {
                    ExecutarComando(comandoInfo, args);
                }
                catch (Exception ex)
                {
                    TratarExceptionDuranteExecucao(comandoInfo, ex);
                }
            }
        }

        /// <summary>
        /// <para>Trata a exception disparada durante a execução do comando.</para>
        /// </summary>
        /// <param name="comandoInfo"><para>Informações sobre o comando.</para></param>
        /// <param name="ex"><para><see cref="System.Exception"/>.</para></param>
        private static void TratarExceptionDuranteExecucao(KeyValuePair<string, string> comandoInfo, Exception ex)
        {
            if (ex is ComandoException && (ex as ComandoException).Sinal == ComandoException.ListaParaSinal.FinalizarComSucesso)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    Windows.Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Windows.Console.Write(ComandoTextoDeAjuda.Instancia.ObterTextoDeAjuda(ComandoTextoDeAjuda.ModoDoTextoDeAjuda.Cabecalho));
                Windows.Console.WriteLine(string.Format(Properties.Comando.msgComandoComBibliotecaInvalida, comandoInfo.Key, comandoInfo.Value));
                Windows.Console.WriteLine();
                if (!(ex is ComandoException) || (ex as ComandoException).Sinal == ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca)
                {
                    Windows.Console.WriteLine(string.Format(Properties.Comando.msgConsulteAAjuda, ComandoTextoDeAjuda.Instancia.NomeDoExecutavel, comandoInfo.Key));
                    Windows.Console.WriteLine();
                }
                if (ex is ComandoException)
                {
                    Windows.Console.Write(Properties.Comando.msgErroDoComando);
                }
                else if (ex is OptionException)
                {
                    Windows.Console.Write(Properties.Comando.msgErroDeSintaxe);
                }
                else
                {
                    Windows.Console.Write(Properties.Comando.msgErroDesconhecido + " " + ex.GetType().FullName + ".");
                }
                Windows.Console.WriteLine(" " + (string.IsNullOrWhiteSpace(ex.Message) ? "?" : ex.Message));
            }
        }

        /// <summary>
        /// <para>Executa o comando de uma biblioteca.</para>
        /// </summary>
        /// <param name="comandoInfo"><para>Informações sobre o comando.</para></param>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        public static void ExecutarComando(KeyValuePair<string, string> comandoInfo, string[] args)
        {
            Assembly comandoAssembly = Assembly.LoadFile(comandoInfo.Value);
            Type comandoType = comandoAssembly.GetType(string.Format("suporteZ.cmd.{0}.Comando", comandoInfo.Key));
            IComando comando = (IComando)Activator.CreateInstance(comandoType);

            string regexVersao = @"[0-9]*\.[0-9]*$";
            string versaoChamador = AssemblyInfo.ExecutingAssembly.Version.ToString();
            string versaoBiblioteca = comandoAssembly.GetName().Version.ToString();
            long versaoChamadorInt = long.Parse(Regex.Match(versaoChamador, regexVersao).Value.Replace(".", string.Empty));
            long versaoBibliotecaInt = long.Parse(Regex.Match(versaoBiblioteca, regexVersao).Value.Replace(".", string.Empty));
            if (versaoChamadorInt > versaoBibliotecaInt)
            {
                throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDoAplicativo, string.Format(Properties.Comando.msgBibliotecaObsoleta, versaoBiblioteca, versaoChamador));
            }

            List<string> argumentos = new List<string>(args);
            argumentos.RemoveRange(0, CultureUtil.ObterCultureInfo(argumentos[0]) == null ? 1 : 2);
            comando.Executar(comandoInfo, comandoAssembly, argumentos.ToArray());
        }

        /// <summary>
        /// <para>Retorna o nome do comando informado pelo usuário e o respectivo
        /// caminho da biblioteca deste comando.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        /// <returns><para>Nome e biblioteca do comando.</para></returns>
        public static KeyValuePair<string, string> ObterComandoAtual(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string comando = args[0];
                CultureInfo culture = CultureUtil.ObterCultureInfo(comando);
                if (culture == null && args.Length >= 1 ||
                    culture != null && args.Length >= 2)
                {
                    if (culture != null)
                    {
                        comando = args[1];
                    }

                    Dictionary<string, string> comandos = ObterListaDeComandos();
                    if (comandos.ContainsKey(comando.ToLower()))
                    {
                        return new KeyValuePair<string, string>(comando, comandos[comando]);
                    }
                }
            }
            return default(KeyValuePair<string, string>);
        }

        /// <summary>
        /// <para>Retorna o <see cref="System.Globalization.CultureInfo"/> carregado atualmente
        /// pelo usuário.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        /// <returns><para>Retorna o <see cref="System.Globalization.CultureInfo"/>.</para></returns>
        public static CultureInfo ObterUICultureInfo(string[] args)
        {
            string cultura = args != null && args.Length > 0 ? args[0] : string.Empty;

            CultureInfo cultureInfo = CultureUtil.ObterCultureInfo(cultura);

            if (cultureInfo == null)
            {
                string executavel = new FileInfo(Assembly.GetEntryAssembly().Location).Name;
                cultureInfo = CultureUtil.ObterCultureInfo(Regex.Match(executavel, @"(?<=\.)[a-z\-]*?(?=\.exe)", RegexOptions.IgnoreCase).Value);

                if (cultureInfo == null)
                {
                    cultureInfo = CultureInfo.CurrentUICulture;
                }
            }

            return cultureInfo;
        }

        /// <summary>
        /// <para>Obtem o texto de ajuda sobre como executar este aplicativo como comando.</para>
        /// </summary>
        /// <returns><para>Texto de ajuda.</para></returns>
        public static string ObterTextoDeAjuda()
        {
            StringBuilder comandos = new StringBuilder();
            foreach (KeyValuePair<string, string> comando in ObterListaDeComandos())
            {
                comandos.Append(Environment.NewLine + "  " + comando.Key);
            }
            if (comandos.Length > 0)
            {
                comandos.Remove(0, Environment.NewLine.Length);
            }
            ComandoTextoDeAjuda.Instancia.ComandosDisponiveis = comandos.ToString();
            return ComandoTextoDeAjuda.Instancia.ObterTextoDeAjuda();
        }


        /// <summary>
        /// <para>Obtem a lista de comandos disponíveis.</para>
        /// </summary>
        /// <returns><para>Dicionários tendo <c>Key</c> como nome do comando,
        /// e <c>Value</c> como o caminho da biblioteca DLL.</para></returns>
        private static Dictionary<string, string> ObterListaDeComandos()
        {
            DirectoryInfo caminhoAtual = new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo caminhoDoExecutavel = new FileInfo(Assembly.GetEntryAssembly().Location).Directory;

            string parteInicialDoNomeDosArquivos = ComandoTextoDeAjuda.Instancia.ParteInicialDoNomeDoExecutavel;

            SortedDictionary<string, string> comandos = new SortedDictionary<string, string>();

            List<FileInfo> arquivo = new List<FileInfo>();
            arquivo.AddRange(caminhoAtual.GetFiles("*.dll"));
            arquivo.AddRange(caminhoDoExecutavel.GetFiles("*.dll"));

            foreach (FileInfo file in arquivo)
            {
                if (Regex.Replace(file.Name, @"(" + parteInicialDoNomeDosArquivos + @")\.[a-z]*?\.dll", string.Empty, RegexOptions.IgnoreCase) == string.Empty)
                {
                    string comando = file.Name.Substring(parteInicialDoNomeDosArquivos.Length + 1).Replace(".dll", string.Empty).ToLower();
                    if (!comandos.ContainsKey(comando))
                    {
                        comandos.Add(comando, file.FullName);
                    }
                }
            }

            return comandos.ToDictionary(k => k.Key, v => v.Value);
        }
    }
}

