using System;
using System.Globalization;
using System.Linq;

namespace suporteZ.Globalization
{
    /// <summary>
    /// <para>Esta classe tem métodos utilitários 
    /// relacionados a <see cref="System.Globalization.CultureInfo"/>.</para>
    /// </summary>
    public static class CultureUtil
    {
        /// <summary>
        /// <para>Consulta um nome de cultura desde que seja válido.</para>
        /// </summary>
        /// <param name="nome"><para>Nome de cultura.</para></param>
        /// <returns><para>Retorna uma instância de <see cref="System.Globalization.CultureInfo"/>
        /// caso o nome de cultura seja válido.</para></returns>
        public static CultureInfo ObterCultureInfo(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return null;
            }
            else
            {
                return
                    CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                    .FirstOrDefault(c => c.Name.Equals(nome, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
