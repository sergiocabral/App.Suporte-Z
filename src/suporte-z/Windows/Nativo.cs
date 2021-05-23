using System;
using System.Runtime.InteropServices;

namespace suporteZ.Windows
{
    /// <summary>
    /// <para>Classe para agrupar métodos nativos da API do Windows.</para>
    /// </summary>
    public static class Nativo
    {
        #region Message

        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public static IntPtr HWND_MESSAGE = new IntPtr(-3);

        #endregion

        #region Console

        /// <summary>
        /// <para>Aloca um novo console para o processo atual.</para>
        /// </summary>
        /// <returns><para>Retorna <c>true</c> em casp de sucesso.</para></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        /// <summary>
        /// <para>Desassocia o processado atual do seu console.</para>
        /// </summary>
        /// <returns><para>Retorna <c>true</c> em casp de sucesso.</para></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();

        /// <summary>
        /// <para>Associa o processo atual ao console de outro processo.</para>
        /// </summary>
        /// <param name="dwProcessId">Identificado do processo (PID) que tem o console que será usado.</param>
        /// <returns><para>Retorna <c>true</c> em casp de sucesso.</para></returns>
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// <para>Retorna o handle da janela do console.</para>
        /// </summary>
        /// <returns><para>Handle da janela do console. Se não houver console, retorna vazio.</para></returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        #endregion

        #region Janelas e processos

        /// <summary>
        /// <para>Obtem o Handle da janela em uso pelo usuário no momento.</para>
        /// </summary>
        /// <returns><para>Handle da janela.</para></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// <para>Obtem o identificado de um Thread criado por uma janela.</para>
        /// <para>Também retorna o identificador do processo (PID) que criou a janela.</para>
        /// </summary>
        /// <param name="hWnd"><para>Handle da janela.</para></param>
        /// <param name="lpdwProcessId"><para>Identificador do processo (PID) que criou a janela.</para></param>
        /// <returns><para>Identificador do Thread criado por uma janela.</para></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        #endregion

        #region Cleipboard
    
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        #endregion

    }
}
