using NDesk.Options;
using suporteZ.Criptografia;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Reflection;

namespace suporteZ.cmd.crypto
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

            Dados["value"] = string.Empty;
            optionSet.Add("v=|value=", Properties.Comando.optionTextoParaManipular, (string value) =>
            {
                if ((string)Dados["value"] != string.Empty) { ExceptionPorArgumentoEmDuplicidade("value"); }

                Dados["value"] = value;
            });

            Dados["controlv_value"] = false;
            optionSet.Add("controlv_value", Properties.Comando.optionTextoComoClipboard, (string value) =>
            {
                if ((bool)Dados["controlv_value"]) { ExceptionPorArgumentoEmDuplicidade("controlv_value"); }

                Dados["controlv_value"] = true;
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

            Dados["controlc_result"] = false;
            optionSet.Add("controlc_result", Properties.Comando.optionEnviarResultadoParaClipboard, (string value) =>
            {
                if ((bool)Dados["controlc_result"]) { ExceptionPorArgumentoEmDuplicidade("controlc_result"); }

                Dados["controlc_result"] = true;
            });

            Dados["silent"] = false;
            optionSet.Add("s|silent", Properties.Comando.optionModoSilencioso, (string value) =>
            {
                if ((bool)Dados["silent"]) { ExceptionPorArgumentoEmDuplicidade("silent"); }

                Dados["silent"] = true;
            });

            Dados["algorithm"] = string.Empty;
            optionSet.Add("a=|algorithm=", Properties.Comando.optionAlgoritmoDeCriptografia.Replace("\\n", Environment.NewLine), (string algorithm) =>
            {
                if ((string)Dados["algorithm"] != string.Empty) { ExceptionPorArgumentoEmDuplicidade("algorithm"); }

                Dados["algorithm"] = algorithm;
            });
        }

        /// <summary>
        /// <para>Execução específica do comando.</para>
        /// </summary>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        protected override void Executar(string[] args)
        {
            List<string> naoProcessado = OptionSet.Parse(args);

            if ((bool)Dados["controlv_value"])
            {
                if ((string)Dados["value"] != string.Empty)
                {
                    throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgConflitoValorClipboard);
                }
                else
                {
                    Dados["value"] = Clipboard.GetText();
                }
            }

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

            if (naoProcessado.Count > ((string)Dados["value"] != string.Empty ? 0 : 1))
            {
                ExceptionPorArgumentosInvalidos(naoProcessado);
            }
            else if (naoProcessado.Count == 1)
            {
                Dados["value"] = naoProcessado[0];
            }

            if ((bool)Dados["crypt"] == (bool)Dados["decrypt"])
            {
                if (!(bool)Dados["crypt"])
                {
                    Dados["crypt"] = true;
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

            if ((bool)Dados["gui"])
            {
                AbrirInterfaceGrafica(typeof(FormPrincipal));
            }
            else
            {
                Windows.Console.WriteLine(ProcessarCriptografia());
            }
        }

        /// <summary>
        /// <para>Realiza o processo de criptografia baseado nas configurações informadas
        /// pelo usuário.</para>
        /// </summary>
        /// <returns><para>Resultado da criptografia.</para></returns>
        public string ProcessarCriptografia()
        {
            ParametrosParaCriptografia parametros = new ParametrosParaCriptografia(Dados);
            string resultado;
            try
            {
                resultado = ProcessarCriptografia(parametros);
            }
            catch (Exception ex)
            {
                if (!parametros.ParaEntrada)
                {
                    throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, Properties.Comando.msgTextoNaoEstavaCriptografado);
                }
                else
                {
                    throw ex;
                }
            }

            if ((bool)Dados["controlc_result"])
            {
                try
                {
                    Clipboard.SetText(resultado);
                }
                catch (Exception) { }
            }

            return !(bool)Dados["silent"] ? resultado : string.Empty;
        }

        /// <summary>
        /// <para>Realiza o processo de criptografia baseado nas configurações informadas
        /// pelo usuário.</para>
        /// </summary>
        /// <param name="parametros"><para>Configurações informadas pelo usuário.</para></param>
        /// <returns><para>Resultado da criptografia.</para></returns>
        private string ProcessarCriptografia(ParametrosParaCriptografia parametros)
        {
            Type symmetricAlgorithmType = Type.GetType("System.Security.Cryptography." + (string)Dados["algorithm"]);
            if (symmetricAlgorithmType == null)
            {
                throw new ComandoException(ComandoException.ListaParaSinal.FinalizarComErroDaBiblioteca, string.Format(Properties.Comando.msgAlgoritmoInvalido, Dados["algorithm"]));
            }

            Type criptografiaGenericType = typeof(CriptografiaSimetrica<>);
            Type[] criptografiaGenericTypesArguments = new Type[] { symmetricAlgorithmType };
            Type criptografiaType = criptografiaGenericType.MakeGenericType(criptografiaGenericTypesArguments);
            object criptografia = Activator.CreateInstance(criptografiaType);
            MethodInfo criptografiaMetodo = criptografia.GetType().GetMethod("Aplicar", new Type[] {
                    typeof(bool),
                    typeof(string),
                    typeof(string),
                    typeof(byte[]),
                    typeof(Encoding)
                });
            return (string)criptografiaMetodo.Invoke(criptografia, new object[]{
                    parametros.ParaEntrada,
                    parametros.Texto,
                    parametros.Senha,
                    new byte[0],
                    Encoding.Unicode
                });
        }

        /// <summary>
        /// <para>Configura os parâmetros necessários para o metodo de criptografia.</para>
        /// </summary>
        private class ParametrosParaCriptografia
        {
            /// <summary>
            /// <para>Construtor.</para>
            /// </summary>
            /// <param name="dados"><para>Dados recebidos da linha de comando.</para></param>
            public ParametrosParaCriptografia(IDictionary<string, object> dados)
            {
                Dados = dados;
            }

            /// <summary>
            /// <para>Dados recebidos da linha de comando.</para>
            /// </summary>
            protected IDictionary<string, object> Dados { get; set; }

            /// <summary>
            /// <para>Quando igual a <c>true</c>, define o processo como Criptografia.
            /// Mas se for igual a <c>false</c>, define como Descriptografia.</para>
            /// </summary>
            public bool ParaEntrada { get { return (bool)Dados["crypt"] && !(bool)Dados["decrypt"]; } }
            
            /// <summary>
            /// <para>Texto de entrada.</para>
            /// </summary>
            public string Texto { get { return (string)Dados["value"]; } }
            
            /// <summary>
            /// <para>Senha, ou chave de criptografia, usada para des/criptografar.</para>
            /// </summary>
            public string Senha { get { return (string)Dados["password"]; } }
        }
    }
}
