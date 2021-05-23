using System;
using System.Collections.Generic;
using System.Dynamic;

namespace suporteZ.Reflection
{
    /// <summary>
    /// <para>Classe usada para declaração <c>dynamic</c> que armazena pares
    /// de chave e valores em tempo de execução.</para>
    /// <para>Similar a ViewBag das páginas MVC.</para>
    /// </summary>
    public class ObjetoExpansivel : DynamicObject, IDynamicMetaObjectProvider
    {
        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        /// <param name="sempreRetornarValor"><para>Quando <c>true</c> sempre será retornado valor. Caso não exista,
        /// retorna <c>null</c>, mas não dispara exceção de erro.</para></param>
        public ObjetoExpansivel(bool sempreRetornarValor)
        {
            SempreRetornarValor = sempreRetornarValor;
            Valores = new Dictionary<string, object>();
        }

        /// <summary>
        /// <para>Quando <c>true</c> sempre será retornado valor. Caso não exista,
        /// retorna <c>null</c>, mas não dispara exceção de erro.</para>
        /// </summary>
        public bool SempreRetornarValor { get; set; }

        /// <summary>
        /// <para>Armazenador das chaves e valores.</para>
        /// </summary>
        public Dictionary<string, object> Valores { get; set; }
        
        /// <summary>
        /// <para>Tenta obter o valor de uma chave.</para>
        /// </summary>
        /// <param name="binder"><para>Semântica e detalhes da operação.</para></param>
        /// <param name="valor"><para>Valor encontrado.</para></param>
        /// <returns><para>Retorna <c>true</c> caso tenha sucesso na consulta.</para></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object valor)
        {
            if (Valores.TryGetValue(binder.Name, out valor))
            {
                return true;
            }
            valor = null;
            return false || SempreRetornarValor;
        }

        /// <summary>
        /// <para>Tenta definir um valor para uma chave.</para>
        /// </summary>
        /// /// <param name="binder"><para>Semântica e detalhes da operação.</para></param>
        /// <param name="valor"><para>Valor a definir.</para></param>
        /// <returns><para>Retorna <c>true</c> caso tenha sucesso na definição.</para></returns>
        public override bool TrySetMember(SetMemberBinder binder, object valor)
        {
            try
            {
                Valores[binder.Name] = valor;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
