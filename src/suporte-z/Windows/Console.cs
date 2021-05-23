using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suporteZ.Windows
{
    /// <summary>
    /// <para>Gerencia o console de prompt de comando.</para>
    /// </summary>
    public static class Console
    {
        /// <summary>
        /// <para>Verifica se existe console associado a este processo.</para>
        /// </summary>
        public static bool PossuiConsole
        {
            get
            {
                return Nativo.GetConsoleWindow() != IntPtr.Zero;
                
                try
                {
                    return System.Console.WindowHeight> 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// <para>Garante que o processo atual tenha uma janela de console.</para>
        /// </summary>
        /// <para>Retorna <c>true</c> quando há sucesso na operação.</para>
        public static bool GarantirJanelaDeConsole()
        {
            if (!PossuiConsole)
            {
                IntPtr ptr = Nativo.GetForegroundWindow();
                int foregroundWindowProcessId;
                Nativo.GetWindowThreadProcessId(ptr, out foregroundWindowProcessId);
                Process process = Process.GetProcessById(foregroundWindowProcessId);

                if (process.ProcessName == "cmd")
                {
                    return Nativo.AttachConsole(process.Id);
                }
                else
                {
                    return Nativo.AllocConsole();
                }
            }
            return true;
        }

        /// <summary>
        /// <para>Descarta a janela de console para o processo atual.</para>
        /// </summary>
        /// <para>Retorna <c>true</c> quando há sucesso na operação.</para>
        public static bool DescartarConsole()
        {
            return Nativo.FreeConsole();
        }

        /// <summary>
        /// <para>Similar a <see cref="System.Console.Write(string, object[])"/>.</para>
        /// <para>Funciona para aplicação não baseadas em console.
        /// Caso não exista Console associado ao processo cria um.</para>
        /// </summary>
        /// <param name="textoFormat"><para>Texto para escrita. Suporta string formatada.</para></param>
        /// <param name="args"><para>Argumentos usados em caso de string formatada.</para></param>
        public static void Write(string textoFormat, params object[] args)
        {
            GarantirJanelaDeConsole();
            System.Console.Write(textoFormat, args);
        }

        /// <summary>
        /// <para>Similar a <see cref="System.Console.WriteLine(string, object[])"/>.</para>
        /// <para>Funciona para aplicação não baseadas em console.
        /// Caso não exista Console associado ao processo cria um.</para>
        /// </summary>
        /// <param name="textoFormat"><para>Texto para escrita. Suporta string formatada.</para></param>
        /// <param name="args"><para>Argumentos usados em caso de string formatada.</para></param>
        public static void WriteLine(string textoFormat, params object[] args)
        {
            GarantirJanelaDeConsole();
            System.Console.WriteLine(textoFormat, args);
        }

        /// <summary>
        /// <para>Similar a <see cref="System.Console.WriteLine()"/>.</para>
        /// <para>Funciona para aplicação não baseadas em console.
        /// Caso não exista Console associado ao processo cria um.</para>
        /// </summary>
        public static void WriteLine()
        {
            GarantirJanelaDeConsole();
            System.Console.WriteLine();
        }
    }
}
