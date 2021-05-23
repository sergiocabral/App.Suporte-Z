using suporteZ.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace suporteZ
{
    /// <summary>
    /// <para>Agrupa informações usadas na exibição do texto de ajuda.</para>
    /// </summary>
    internal class ComandoTextoDeAjuda
    {
        /// <summary>
        /// <para>Construtor privado.</para>
        /// <para>Padrão de projeto Singleton.</para>
        /// </summary>
        private ComandoTextoDeAjuda() { }

        private static ComandoTextoDeAjuda instancia;
        /// <summary>
        /// <para>Retorna uma instância desta classe.</para>
        /// </summary>
        public static ComandoTextoDeAjuda Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ComandoTextoDeAjuda();
                }
                return instancia;
            }
        }

        /// <summary>
        /// <para>Modos de obter o texto de ajuda.</para>
        /// </summary>
        public enum ModoDoTextoDeAjuda
        {
            /// <summary>
            /// <para>Retorna todo o texto de ajuda.</para>
            /// </summary>
            Tudo = 1,

            /// <summary>
            /// <para>Retorna apeas o cabeçalho.</para>
            /// </summary>
            Cabecalho = 2,

            /// <summary>
            /// <para>Retorna apenas as linhas do texto explicativo.</para>
            /// </summary>
            Texto = 4
        }

        /// <summary>
        /// <para>Obtem o texto de ajuda sobre como executar este aplicativo como comando.</para>
        /// <para>Os dados são obtidos do arquivo de recursos assim como estão;
        /// sem substituição de variáveis por valores.</para>
        /// </summary>
        /// <param name="modo"><para>Modo de recuperação do texto da ajuda.</para></param>
        /// <returns><para>Texto de ajuda.</para></returns>
        protected string ObterTextoDeAjudaModelo(ModoDoTextoDeAjuda modo = ModoDoTextoDeAjuda.Tudo)
        {
            SortedDictionary<string, string> linhas = new SortedDictionary<string, string>();
            foreach (PropertyInfo propertyInfo in typeof(Properties.Comando).GetProperties(BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (propertyInfo.Name.StartsWith("ajuda") && propertyInfo.GetMethod.ReturnType.IsAssignableFrom(typeof(string)))
                {
                    if (((modo & ModoDoTextoDeAjuda.Tudo) == ModoDoTextoDeAjuda.Tudo) ||
                        ((modo & ModoDoTextoDeAjuda.Cabecalho) == ModoDoTextoDeAjuda.Cabecalho && propertyInfo.Name.Contains("Cabecalho")) ||
                        ((modo & ModoDoTextoDeAjuda.Texto) == ModoDoTextoDeAjuda.Texto && propertyInfo.Name.Contains("Linha")))
                    {
                        linhas.Add(propertyInfo.Name, propertyInfo.GetValue(null) as string);
                    }
                }
            }
            StringBuilder texto = new StringBuilder();
            linhas.Select(a => a.Value).ToList().ForEach(linha => { texto.AppendLine(linha); });
            return texto.ToString();
        }

        /// <summary>
        /// <para>Obtem o texto de ajuda sobre como executar este aplicativo como comando.</para>
        /// </summary>
        /// <param name="modo"><para>Modo de recuperação do texto da ajuda.</para></param>
        /// <returns><para>Texto de ajuda.</para></returns>
        public string ObterTextoDeAjuda(ModoDoTextoDeAjuda modo = ModoDoTextoDeAjuda.Tudo)
        {
            return SubstituirVariaveis(ObterTextoDeAjudaModelo(modo));
        }

        /// <summary>
        /// <para>Substitui as variáveis do texto pelos seus respectivos valores.</para>
        /// </summary>
        /// <param name="texto"><para>Texto.</para></param>
        /// <returns><para>Texto com as variáveis substituidas por valores.</para></returns>
        public string SubstituirVariaveis(string texto)
        {
            StringBuilder textoSubstituido = new StringBuilder(texto);
            foreach (Match match in Regex.Matches(textoSubstituido.ToString(), @"{.*?}"))
            {
                if (match.Value.Length > 2)
                {
                    string variavel = match.Value.Substring(1, match.Value.Length - 2);
                    string valor;
                    try
                    {
                        valor = this.GetType().GetProperty(variavel).GetValue(this) as string;
                    }
                    catch (Exception)
                    {
                        valor = match.Value;
                    }
                    textoSubstituido.Replace(match.Value, valor);
                }
            }
            return textoSubstituido.ToString();
        }

        #region Parâmetros usados no texto de ajuda.

        /// <summary>
        /// <para>Nome do aplicativo.</para>
        /// </summary>
        public string NomeDoAplicativo { get { return AssemblyInfo.EntryAssembly.Product; } }

        /// <summary>
        /// <para>Versão do aplicativo.</para>
        /// </summary>
        public string VersaoDoAplicativo { get { return AssemblyInfo.EntryAssembly.Version.ToString(); } }

        /// <summary>
        /// <para>Nome do executável.</para>
        /// </summary>
        public string NomeDoExecutavel { get { return new FileInfo(Assembly.GetEntryAssembly().Location).Name; } }

        /// <summary>
        /// <para>Parte inicial do nome do executável antes do idioma (se houver) e da extensão do arquivo.</para>
        /// </summary>
        public string ParteInicialDoNomeDoExecutavel { get { return Regex.Replace(new FileInfo(Assembly.GetEntryAssembly().Location).Name, @"(\.(?<=\.)[a-z\-]*?|)\.exe", string.Empty, RegexOptions.IgnoreCase); } }

        private string comandosDisponiveis;
        /// <summary>
        /// <para>Lista de comandos disponíveis para uso.</para>
        /// <para>Esta propriedade precisa ser definida antes de ser consultada.</para>
        /// </summary>
        public string ComandosDisponiveis
        {
            get
            {
                if (comandosDisponiveis == null)
                {
                    throw new NullReferenceException("O valor desta propriedade não definida.");
                }
                if (string.IsNullOrWhiteSpace(comandosDisponiveis))
                {
                    return Properties.Comando.msgNenhumComandoDisponivel;
                }
                else
                {
                    return comandosDisponiveis;
                }
            }
            set
            {
                comandosDisponiveis = value;
            }
        }

        #endregion
    }
}
