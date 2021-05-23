using System;
using System.Collections.Generic;
using System.Reflection;

namespace suporteZ
{
    /// <summary>
    /// <para>Interface para um comando executado pelo prompt.</para>
    /// <para>Projetos a parte implementam esta interface para disponibilizar o comando.</para>
    /// </summary>
    public interface IComando
    {
        /// <summary>
        /// <para>Método principal para a execução do comando.</para>
        /// </summary>
        /// <param name="comandoInfo"><para>Informações sobre o comando, tendo <c>Key</c> como
        /// o nome do comando e <c>Value</c> como o caminho da biblioteca do assembly.</para></param>
        /// <param name="assembly"><para>Assembly carregado para execução deste comando.</para></param>
        /// <param name="args"><para>Argumentos passados por linha de comando.</para></param>
        void Executar(KeyValuePair<string, string> comandoInfo, Assembly assembly, string[] args);
    }
}
