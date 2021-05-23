using NDesk.Options;
using suporteZ.Criptografia;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Reflection;

namespace suporteZ.cmd.cryptof
{
    /// <summary>
    /// <para>Implementação de comando de prompt.</para>
    /// <para>Comandos dentro do contexto de criptografia.</para>
    /// </summary>
    public class Comando : suporteZ.Comando
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public Comando()
        {
            AlgoritmosDeCriptografia = new List<string>(new string[] { 
                typeof(DES).Name,
                typeof(RC2).Name,
                typeof(TripleDES).Name,
                typeof(Rijndael).Name,
            });
        }

        /// <summary>
        /// <para>Lista de algoritmos de criptografia disponíveis para uso.</para>
        /// </summary>
        public IList<string> AlgoritmosDeCriptografia { get; private set; }

        /// <summary>
        /// <para>Descrição deste comando.</para>
        /// </summary>
        protected override string Descricao { get { return Properties.Comando.msgDescricao; } }

        /// <summary>
        /// <para>Configura as informações do <see cref="NDesk.Options.OptionSet"/>.</para>
        /// </summary>
        /// <param name="optionSet"><para><see cref="NDesk.Options.OptionSet"/> pre configurado.</para></param>
        protected override void ConfigurarOptionSet(OptionSet optionSet)
        {
            Dados["gui"] = false;
            optionSet.Add("g|gui", Properties.Comando.optionInterfaceGrafica, v =>
            {
                if ((bool)Dados["gui"]) { ExceptionPorArgumentoEmDuplicidade("gui"); }

                Dados["gui"] = true;
            });

            Dados["file_in"] = new List<string>();
            optionSet.Add("i=|file_in=", Properties.Comando.optionArquivoEntrada, (string arquivo) =>
            {
                ((List<string>)Dados["file_in"]).Add(arquivo);
            });

            Dados["file_out"] = new List<string>();
            optionSet.Add("o=|file_out=", Properties.Comando.optionArquivoSaida, (string arquivo) =>
            {
                ((List<string>)Dados["file_out"]).Add(arquivo);
            });

            Dados["equals_out"] = false;
            optionSet.Add("e|equals_out", Properties.Comando.optionSaidaNoMesmoArquivoDeEntrada, (string arquivo) =>
            {
                if ((bool)Dados["equals_out"]) { ExceptionPorArgumentoEmDuplicidade("equals_out"); }

                Dados["equals_out"] = true;
            });

            Dados["password"] = string.Empty;
            optionSet.Add("p=|password=", Properties.Comando.optionSenha, (string password) =>
            {
                if ((string)Dados["password"] != string.Empty) { ExceptionPorArgumentoEmDuplicidade("password"); }

                Dados["password"] = password;
            });

            Dados["controlv_password"] = false;
            optionSet.Add("controlv_password", Properties.Comando.optionSenhaComoClipboard, (string value) =>
            {
                if ((bool)Dados["controlv_password"]) { ExceptionPorArgumentoEmDuplicidade("controlv_password"); }

                Dados["controlv_password"] = true;
            });

            Dados["crypt"] = false;
            optionSet.Add("c|crypt", Properties.Comando.optionCriptografar, v =>
            {
                if ((bool)Dados["crypt"]) { ExceptionPorArgumentoEmDuplicidade("crypt"); }

                Dados["crypt"] = true;
            });

            Dados["decrypt"] = false;
            optionSet.Add("d|decrypt", Properties.Comando.optionDescriptografar, v =>
            {
                if ((bool)Dados["decrypt"]) { ExceptionPorArgumentoEmDuplicidade("decrypt"); }

                Dados["decrypt"] = true;
            });

            Dados["algorithm"] = string.Empty;
            optionSet.Add("a=|algorithm=", Properties.Comando.optionAlgoritmoDeCriptografia.Replace("\\n", Environment.NewLine), (string algorithm) =>
            {
                if ((string)Dados["algorithm"] != string.Empty) { ExceptionPorArgumentoEmDuplicidade("algorithm"); }

                Dados["algorithm"] = algorithm;
            });

            Dados["windows_on"] = false;
            optionSet.Add("windows_on", Properties.Comando.optionIntegrarComWindowsAtivado, (string algorithm) =>
            {
                Dados["windows_on"] = true;
            });

            Dados["windows_off"] = false;
            optionSet.Add("windows_off", Properties.Comando.optionIntegrarComWindowsDesativado, (string algorithm) =>
            {
                Dados["windows_off"] = true;
            });
        }

        /// <summary>
        /// <para>Execução específica do comando.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        protected override void Executar(string[] args)
        {
            List<string> naoProcessado = OptionSet.Parse(args);

            if (naoProcessado.Count > 0)
            {
                ExceptionPorArgumentosInvalidos(naoProcessado);
            }
            
            if ((bool)Dados["windows_on"] && args.Length != 1)
            {
                ExceptionPorArgumentoQueDeviaEstarSozinho("windows_on");
            }
            else if ((bool)Dados["windows_off"] && args.Length != 1)
            {
                ExceptionPorArgumentoQueDeviaEstarSozinho("windows_off");
            }

            if (!(bool)Dados["windows_on"] && !(bool)Dados["windows_off"])
            {
                if ((bool)Dados["controlv_password"])
                {
                    if ((string)Dados["password"] != string.Empty)
                    {
                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgConflitoSenhaClipboard);
                    }
                    else
                    {
                        Dados["password"] = Clipboard.GetText();
                    }
                }

                if ((bool)Dados["crypt"] == (bool)Dados["decrypt"])
                {
                    if (!(bool)Dados["crypt"])
                    {
                        if (!(bool)Dados["gui"])
                        {
                            throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgNecessarioIndicarAcao, "crypt", "decrypt"));
                        }
                    }
                    else
                    {
                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgConflitoCryptDecrypt);
                    }
                }

                if (string.IsNullOrWhiteSpace((string)Dados["algorithm"]))
                {
                    Dados["algorithm"] = typeof(Rijndael).Name;
                }

                if (!(bool)Dados["gui"])
                {
                    if (((List<string>)Dados["file_in"]).Count == 0)
                    {
                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgNenhumArquivoDeEntrada);
                    }

                    if (((List<string>)Dados["file_out"]).Count > 0 && ((List<string>)Dados["file_out"]).Count != ((List<string>)Dados["file_in"]).Count)
                    {
                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgQuantidadeDeArquivosDeEntradaESaidaDevemSerIguais);
                    }
                    if (((List<string>)Dados["file_out"]).Count == 0 && !(bool)Dados["equals_out"])
                    {
                        throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgNecessarioIndicarSaida, "equals_out", "file_out"));
                    }
                }
            }

            if ((bool)Dados["gui"])
            {
                AbrirInterfaceGrafica(typeof(FormPrincipal));
            }
            else
            {
                if ((bool)Dados["windows_on"] || (bool)Dados["windows_off"])
                {
                    Windows.Console.WriteLine(IntegracaoComWindows((bool)Dados["windows_on"]));
                }
                else
                {
                    Windows.Console.WriteLine(ProcessarCriptografia());
                }
            }
        }

        /// <summary>
        /// <para>Ativa (ou desativa) a integração da ferramenta com o sistema operacional.</para>
        /// </summary>
        /// <param name="ativar"><para>Ativa ou desativa a integração.</para></param>
        /// <returns><para>Resultado da operação.</para></returns>
        private string IntegracaoComWindows(bool ativar)
        {
            return "Windows on/off... (em implementação)";
        }

        /// <summary>
        /// <para>Realiza o processo de criptografia baseado nas configurações informadas
        /// pelo usuário.</para>
        /// </summary>
        /// <returns><para>Resultado da criptografia.</para></returns>
        public string ProcessarCriptografia()
        {
            return "criptografar/descriptografar... (em implementação)";
        }

    }
}
