using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace suporteZ.Criptografia
{
    /// <summary>
    /// <para>Disponibiliza funcionalidades relacionadas a criptografia de dados 
    /// com algorítimos de criptografia simétrica.</para>
    /// </summary>
    /// <typeparam name="TSymmetricAlgorithmProvider">Tipo do algoritimo de criptografia simétrica.</typeparam>
    public class CriptografiaSimetrica<TSymmetricAlgorithmProvider> where TSymmetricAlgorithmProvider : SymmetricAlgorithm
    {
        /// <summary>
        /// <para>Senha, ou chave de criptografia, usada para des/criptografar.</para>
        /// </summary>
        public string Senha { get; set; }

        /// <summary>
        /// <para>Bytes usados na derivação da chave de criptografia.</para>
        /// </summary>
        public byte[] BytesSalt { get; set; }

        /// <summary>
        /// <para>Codificador do texto.</para>
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// <para>Construtor.</para>
        /// </summary>
        public CriptografiaSimetrica() : this(string.Empty, new byte[] { }) { }

        /// <summary>
        /// <para>Construtor.</para>
        /// <para>Informa o comprimento de bytes usado na criptografia.</para>
        /// </summary>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <param name="bytesSalt"><para>Bytes usados na derivação da chave de criptografia que foi informada pelo usuário.</para></param>
        public CriptografiaSimetrica(string senha, byte[] bytesSalt) : this(string.Empty, new byte[] { }, Encoding.Default) { }

        /// <summary>
        /// <para>Construtor.</para>
        /// <para>Informa o comprimento de bytes usado na criptografia.</para>
        /// </summary>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <param name="bytesSalt"><para>Bytes usados na derivação da chave de criptografia que foi informada pelo usuário.</para></param>
        /// <param name="encoding"><para>Codificador do texto</para></param>
        public CriptografiaSimetrica(string senha, byte[] bytesSalt, Encoding encoding)
        {
            Senha = senha;
            BytesSalt = bytesSalt;
            Encoding = encoding;
        }

        /// <summary>
        /// <para>Criptografa ou descriptografa uma sequencia de texto.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <param name="texto"><para>Texto de entrada.</para></param>
        /// <returns><para>Resulta no mesmo texto de entrada, porém, criptografado.</para></returns>
        public string Aplicar(bool paraEntrada, string texto)
        {
            return Aplicar(paraEntrada, texto, Senha, BytesSalt);
        }

        /// <summary>
        /// <para>Criptografa ou descriptografa uma sequencia de texto.</para>
        /// </summary>
        /// <param name="paraEntrada">
        /// <para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para>
        /// </param>
        /// <param name="texto"><para>Texto de entrada.</para></param>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <returns><para>Resulta no mesmo texto de entrada, porém, criptografado.</para></returns>
        public string Aplicar(bool paraEntrada, string texto, string senha)
        {
            return Aplicar(paraEntrada, texto, senha, BytesSalt, Encoding);
        }

        /// <summary>
        /// <para>Criptografa ou descriptografa uma sequencia de texto.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <param name="texto"><para>Texto de entrada.</para></param>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <param name="bytesSalt"><para>Bytes usados na derivação da chave de criptografia.</para></param>
        /// <returns><para>Resulta no mesmo texto de entrada, porém, criptografado.</para></returns>
        public string Aplicar(bool paraEntrada, string texto, string senha, byte[] bytesSalt)
        {
            return Aplicar(paraEntrada, texto, senha, bytesSalt, Encoding);
        }

        /// <summary>
        /// <para>Criptografa ou descriptografa uma sequencia de texto.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <param name="texto"><para>Texto de entrada.</para></param>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <param name="bytesSalt"><para>Bytes usados na derivação da chave de criptografia.</para></param>
        /// <param name="encoding"><para>Codificador do texto</para></param>
        /// <returns><para>Resulta no mesmo texto de entrada, porém, criptografado.</para></returns>
        public string Aplicar(bool paraEntrada, string texto, string senha, byte[] bytesSalt, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ICryptoTransform cryptoTransform = ObterCryptoTransform(paraEntrada, senha, bytesSalt);
                CryptoStream cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

                if (paraEntrada)
                {
                    byte[] bytes = encoding.GetBytes(texto);
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }
                else
                {
                    byte[] arrayTexto = Convert.FromBase64String(texto);
                    cryptoStream.Write(arrayTexto, 0, arrayTexto.Length);
                }

                cryptoStream.FlushFinalBlock();

                if (paraEntrada)
                {
                    return Convert.ToBase64String(ms.ToArray());
                }
                else
                {
                    return encoding.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// <para>Obtem uma instância de uma classe devidamente configurada
        /// para realizar a des/criptografia.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <returns><para>Retorna uma classe que implementa a interface <see cref="ICryptoTransform"/>.</para></returns>
        public ICryptoTransform ObterCryptoTransform(bool paraEntrada)
        {
            return ObterCryptoTransform(paraEntrada, Senha, BytesSalt);
        }

        /// <summary>
        /// <para>Obtem uma instância de uma classe devidamente configurada
        /// para realizar a des/criptografia.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <returns><para>Retorna uma classe que implementa a interface <see cref="ICryptoTransform"/>.</para></returns>
        public ICryptoTransform ObterCryptoTransform(bool paraEntrada, string senha)
        {
            return ObterCryptoTransform(paraEntrada, senha, BytesSalt);
        }

        /// <summary>
        /// <para>Obtem uma instância de uma classe devidamente configurada
        /// para realizar a des/criptografia.</para>
        /// </summary>
        /// <param name="paraEntrada"><para>Quando igual a <c>true</c>, define o processo como Criptografia.
        /// Mas se for igual a <c>false</c>, define como Descriptografia.</para></param>
        /// <param name="senha"><para>Senha, ou chave de criptografia, usada para des/criptografar.</para></param>
        /// <param name="bytesSalt"><para>Bytes usados na derivação da chave de criptografia.</para></param>
        /// <returns><para>Retorna uma classe que implementa a interface <see cref="ICryptoTransform"/>.</para></returns>
        public ICryptoTransform ObterCryptoTransform(bool paraEntrada, string senha, byte[] bytesSalt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(senha, bytesSalt);

            System.Reflection.MethodInfo method = typeof(TSymmetricAlgorithmProvider).GetMethod("Create", new Type[] { });
            TSymmetricAlgorithmProvider algoritmo = (TSymmetricAlgorithmProvider)method.Invoke(typeof(TSymmetricAlgorithmProvider), new object[] { });
            algoritmo.Key = pdb.GetBytes(algoritmo.Key.Length);
            algoritmo.IV = pdb.GetBytes(algoritmo.IV.Length);
            if (paraEntrada)
            {
                return algoritmo.CreateEncryptor();
            }
            else
            {
                return algoritmo.CreateDecryptor();
            }
        }
    }
}
