using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace suporteZ.Inutil
{
    /// <summary>
    /// <para>Classe que agrupa beeps musicais.</para>
    /// </summary>
    public static class BeepMusical
    {
        /// <summary>
        /// <para>Lista de músicas.</para>
        /// </summary>
        public enum Musicas
        {
            /// <summary>
            /// <para>Tema do Super Mario Bros.</para>
            /// </summary>
            SuperMarioBrosTheme,

            /// <summary>
            /// <para>Pretty Woman</para>
            /// </summary>
            PrettyWoman,

            /// <summary>
            /// <para>Nome aleatório para a canção</para>
            /// </summary>
            MusicalQuardust,

            /// <summary>
            /// <para>Nome aleatório para a canção</para>
            /// </summary>
            MusicalVinIPSmaker1,

            /// <summary>
            /// <para>Nome aleatório para a canção</para>
            /// </summary>
            MusicalVinIPSmaker2,

            /// <summary>
            /// <para>MerryChristmas</para>
            /// </summary>
            MerryChristmas,

            /// <summary>
            /// <para>Tema Final Fantasy Victory</para>
            /// </summary>
            FinalFantasyVictoryTheme,

            /// <summary>
            /// <para>Tema do Tetris</para>
            /// </summary>
            MusicalTetris,

            /// <summary>
            /// <para>Nome aleatório para a canção</para>
            /// </summary>
            DarthVader,

            /// <summary>
            /// <para>Beethoven's Für Elise
            /// </summary>
            BeethovenFurElise,

            /// <summary>
            /// <para>Nome aleatório para a canção</para>
            /// </summary>
            HarvesterOfSorrow,

            /// <summary>
            /// <para>Dó Ré Mí Fá</para>
            /// </summary>
            DoReMiFa,

            /// <summary>
            /// <para>Som crescente.</para>
            /// </summary>
            Crescente,

            /// <summary>
            /// <para>Som descrescente.</para>
            /// </summary>
            Descrescente
        }

        /// <summary>
        /// <para>Notas musicais.</para>
        /// <para>Onde Key é a frequência, e Value a duração.</para>
        /// </summary>
        private static Dictionary<Musicas, KeyValuePair<int, int>[]> Notas { get; set; }

        /// <summary>
        /// <para>Construtor tipo <c>static</c>.</para>
        /// </summary>
        static BeepMusical()
        {
            ListaDeBackgroundWorker = new List<BackgroundWorker>();
            Notas = new Dictionary<Musicas, KeyValuePair<int, int>[]>();
            Notas.Add(Musicas.SuperMarioBrosTheme, ObterNotasPara_SuperMarioBrosTheme());
            Notas.Add(Musicas.DoReMiFa, ObterNotasPara_DoReMiFa());
            Notas.Add(Musicas.PrettyWoman, ObterNotasPara_PrettyWoman());
            Notas.Add(Musicas.BeethovenFurElise, ObterNotasPara_BeethovenFurElise());
            Notas.Add(Musicas.DarthVader, ObterNotasPara_DarthVader());
            Notas.Add(Musicas.MusicalTetris, ObterNotasPara_MusicalTetris());
            Notas.Add(Musicas.MerryChristmas, ObterNotasPara_MerryChristmas());
            Notas.Add(Musicas.FinalFantasyVictoryTheme, ObterNotasPara_FinalFantasyVictoryTheme());
            Notas.Add(Musicas.MusicalVinIPSmaker1, ObterNotasPara_MusicalVinIPSmaker1());
            Notas.Add(Musicas.MusicalVinIPSmaker2, ObterNotasPara_MusicalVinIPSmaker2());
            Notas.Add(Musicas.MusicalQuardust, ObterNotasPara_MusicalQuardust());
            Notas.Add(Musicas.HarvesterOfSorrow, ObterNotasPara_HarvesterOfSorrow());
        }

        /// <summary>
        /// <para>Toca uma música.</para>
        /// </summary>
        /// <param name="musica"><para>Música.</para></param>
        /// <param name="modoBackground">
        /// <para>Quando <c>==true</c>, a música é executada em outro
        /// Thread, sem travar a aplicação.</para>
        /// </param>
        public static void Tocar(Musicas musica, bool modoBackground)
        {
            if (modoBackground)
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += (sender, e) =>
                {
                    Tocar(musica, sender as BackgroundWorker);
                };
                backgroundWorker.RunWorkerCompleted += (sender, e) =>
                {
                    (sender as BackgroundWorker).Dispose();
                };
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.RunWorkerAsync();
                ListaDeBackgroundWorker.Add(backgroundWorker);
            }
            else
            {
                Tocar(musica, null);
            }
        }

        /// <summary>
        /// <para>Para todas as músicas em execução.</para>
        /// </summary>
        public static void PararTodas()
        {
            foreach (BackgroundWorker backgroundWorker in ListaDeBackgroundWorker)
            {
                if (backgroundWorker.IsBusy)
                {
                    backgroundWorker.CancelAsync();
                }
            }
        }

        /// <summary>
        /// <para>Lista de <see cref="System.ComponentModel.BackgroundWorker"/> já utilizados.</para>
        /// <para>Lista necessária para interromper todas as músicas em execução.</para>
        /// </summary>
        private static List<BackgroundWorker> ListaDeBackgroundWorker { get; set; }

        /// <summary>
        /// <para>Frequência MÍNIMA para o padrão <see cref="Musicas.Crescente"/> e <see cref="Musicas.Descrescente"/>.</para>
        /// <para>Frequência MÍNIMA permitida pelo sistema operacional: 37</para>
        /// </summary>
        private const int FREQUENCIA_MINIMA = 100;

        /// <summary>
        /// <para>Frequência MÁXIMA para o padrão <see cref="Musicas.Crescente"/> e <see cref="Musicas.Descrescente"/>.</para>
        /// <para>Frequência MÁXIMA permitida pelo sistema operacional: 32767</para>
        /// </summary>
        private const int FREQUENCIA_MAXIMA = 4000;


        /// <summary>
        /// <para>Toca uma música.</para>
        /// </summary>
        /// <param name="musica"><para>Música.</para></param>
        /// <param name="backgroundWorker">
        /// <para>Refere-se ao <see cref="System.ComponentModel.BackgroundWorker"/> que
        /// está tocando a música.</para>
        /// <para>Parâmetro necessário para cancelar a execução da música.</para>
        /// </param>
        private static void Tocar(Musicas musica, BackgroundWorker backgroundWorker)
        {
            if (musica != Musicas.Crescente &&
                musica != Musicas.Descrescente &&
                !Notas.ContainsKey(musica))
            {
                throw new NotImplementedException("Música não implementada.");
            }

            switch (musica)
            {
                case Musicas.Crescente:
                    TocarPadraoCrescente(FREQUENCIA_MINIMA, FREQUENCIA_MAXIMA, backgroundWorker);
                    break;
                case Musicas.Descrescente:
                    TocarPadraoCrescente(FREQUENCIA_MAXIMA, FREQUENCIA_MINIMA, backgroundWorker);
                    break;
                default:
                    TocarNotas(musica, backgroundWorker);
                    break;
            }
        }

        /// <summary>
        /// <para>Toca uma música baseada em padrões.</para>
        /// </summary>
        /// <param name="frequenciaInicial"><para>Frequência inicial.</para></param>
        /// <param name="frequenciaFinal"><para>Frequência final.</para></param>
        /// <param name="backgroundWorker">
        /// <para>Refere-se ao <see cref="System.ComponentModel.BackgroundWorker"/> que
        /// está tocando a música.</para>
        /// <para>Parâmetro necessário para cancelar a execução da música.</para>
        /// </param>
        private static void TocarPadraoCrescente(int frequenciaInicial, int frequenciaFinal, BackgroundWorker backgroundWorker)
        {
            int delaySom = 50;
            int delaySilencio = 1;

            bool crescente = frequenciaInicial <= frequenciaFinal;
            int inicio = crescente ? frequenciaInicial : frequenciaFinal;
            int fim = crescente ? frequenciaFinal : frequenciaInicial;

            for (int i = inicio; i <= fim; i++)
            {
                i += 100; if (i > fim) { i = fim; }
                int frequencia = crescente ? i : frequenciaInicial - (i - frequenciaFinal);
                Console.Beep(frequencia, delaySom);
                Thread.Sleep(delaySilencio);
                if (backgroundWorker != null && backgroundWorker.CancellationPending) { break; }
            }
        }

        /// <summary>
        /// <para>Toca uma música baseada em notas.</para>
        /// </summary>
        /// <param name="musica"><para>Música.</para></param>
        /// <param name="backgroundWorker">
        /// <para>Refere-se ao <see cref="System.ComponentModel.BackgroundWorker"/> que
        /// está tocando a música.</para>
        /// <para>Parâmetro necessário para cancelar a execução da música.</para>
        /// </param>
        private static void TocarNotas(Musicas musica, BackgroundWorker backgroundWorker)
        {
            KeyValuePair<int, int>[] notas = Notas[musica];
            foreach (KeyValuePair<int, int> nota in notas)
            {
                if (nota.Key == 0) { Thread.Sleep(nota.Value); }
                else { Console.Beep(nota.Key, nota.Value); }

                if (backgroundWorker != null && backgroundWorker.CancellationPending) { break; }
            }
        }

        /// <summary>
        /// <para>Obtem as notas para a música: SuperMarioBrosTheme</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_SuperMarioBrosTheme()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(0, 375));
            notas.Add(new KeyValuePair<int, int>(392, 125));
            notas.Add(new KeyValuePair<int, int>(0, 375));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(392, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(330, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(494, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(466, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(392, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(880, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(494, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(392, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(330, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(494, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(466, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(392, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(880, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(494, 125));
            notas.Add(new KeyValuePair<int, int>(0, 375));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(415, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 625));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(415, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 1125));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(415, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 625));
            notas.Add(new KeyValuePair<int, int>(784, 125));
            notas.Add(new KeyValuePair<int, int>(740, 125));
            notas.Add(new KeyValuePair<int, int>(698, 125));
            notas.Add(new KeyValuePair<int, int>(0, 42));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(659, 125));
            notas.Add(new KeyValuePair<int, int>(0, 167));
            notas.Add(new KeyValuePair<int, int>(415, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 125));
            notas.Add(new KeyValuePair<int, int>(440, 125));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(622, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(587, 125));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(523, 125));
            notas.Add(new KeyValuePair<int, int>(0, 625));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: PrettyWoman</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_PrettyWoman()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 1000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 1000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(415, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 2000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(370, 2000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 2000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 2000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 2000));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 200));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: BeethovenFurElise</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_BeethovenFurElise()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(622, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(622, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(587, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(523, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(440, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(262, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(330, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(440, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(330, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(415, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(523, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(330, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(622, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(622, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(659, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(587, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(523, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(440, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(262, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(330, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(440, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(330, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(523, 120));
            notas.Add(new KeyValuePair<int, int>(0, 120));
            notas.Add(new KeyValuePair<int, int>(494, 120));
            notas.Add(new KeyValuePair<int, int>(0, 140));
            notas.Add(new KeyValuePair<int, int>(440, 120));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: DarthVader</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_DarthVader()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(392, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 700));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(622, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(369, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 700));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(739, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(659, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(622, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(659, 50));
            notas.Add(new KeyValuePair<int, int>(0, 400));
            notas.Add(new KeyValuePair<int, int>(415, 25));
            notas.Add(new KeyValuePair<int, int>(0, 200));
            notas.Add(new KeyValuePair<int, int>(554, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(493, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 50));
            notas.Add(new KeyValuePair<int, int>(0, 400));
            notas.Add(new KeyValuePair<int, int>(311, 25));
            notas.Add(new KeyValuePair<int, int>(0, 200));
            notas.Add(new KeyValuePair<int, int>(369, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(587, 700));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(784, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(739, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(698, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(659, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(622, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(659, 50));
            notas.Add(new KeyValuePair<int, int>(0, 400));
            notas.Add(new KeyValuePair<int, int>(415, 25));
            notas.Add(new KeyValuePair<int, int>(0, 200));
            notas.Add(new KeyValuePair<int, int>(554, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(523, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(493, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(440, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 50));
            notas.Add(new KeyValuePair<int, int>(0, 400));
            notas.Add(new KeyValuePair<int, int>(311, 25));
            notas.Add(new KeyValuePair<int, int>(0, 200));
            notas.Add(new KeyValuePair<int, int>(392, 350));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 300));
            notas.Add(new KeyValuePair<int, int>(0, 150));
            notas.Add(new KeyValuePair<int, int>(311, 250));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(466, 25));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 700));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: MusicalTetris</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_MusicalTetris()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(494, 159));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(660, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(590, 150));
            notas.Add(new KeyValuePair<int, int>(660, 150));
            notas.Add(new KeyValuePair<int, int>(494, 100));
            notas.Add(new KeyValuePair<int, int>(494, 100));
            notas.Add(new KeyValuePair<int, int>(523, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(494, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(392, 100));
            notas.Add(new KeyValuePair<int, int>(392, 100));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(392, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 100));
            notas.Add(new KeyValuePair<int, int>(330, 100));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(294, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(261, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(311, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(262, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(494, 159));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(660, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(590, 150));
            notas.Add(new KeyValuePair<int, int>(660, 150));
            notas.Add(new KeyValuePair<int, int>(494, 100));
            notas.Add(new KeyValuePair<int, int>(494, 100));
            notas.Add(new KeyValuePair<int, int>(523, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(494, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(392, 100));
            notas.Add(new KeyValuePair<int, int>(392, 100));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(392, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 100));
            notas.Add(new KeyValuePair<int, int>(330, 100));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(294, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(261, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(311, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(247, 100));
            notas.Add(new KeyValuePair<int, int>(262, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(370, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));
            notas.Add(new KeyValuePair<int, int>(330, 150));
            notas.Add(new KeyValuePair<int, int>(37, 40));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: FinalFantasyVictoryTheme</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_MerryChristmas()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(200, 444));
            notas.Add(new KeyValuePair<int, int>(265, 444));
            notas.Add(new KeyValuePair<int, int>(265, 222));
            notas.Add(new KeyValuePair<int, int>(295, 222));
            notas.Add(new KeyValuePair<int, int>(265, 222));
            notas.Add(new KeyValuePair<int, int>(245, 222));
            notas.Add(new KeyValuePair<int, int>(220, 444));
            notas.Add(new KeyValuePair<int, int>(220, 444));
            notas.Add(new KeyValuePair<int, int>(220, 444));
            notas.Add(new KeyValuePair<int, int>(295, 444));
            notas.Add(new KeyValuePair<int, int>(295, 222));
            notas.Add(new KeyValuePair<int, int>(330, 222));
            notas.Add(new KeyValuePair<int, int>(295, 222));
            notas.Add(new KeyValuePair<int, int>(265, 222));
            notas.Add(new KeyValuePair<int, int>(245, 444));
            notas.Add(new KeyValuePair<int, int>(200, 444));
            notas.Add(new KeyValuePair<int, int>(200, 444));
            notas.Add(new KeyValuePair<int, int>(330, 444));
            notas.Add(new KeyValuePair<int, int>(330, 222));
            notas.Add(new KeyValuePair<int, int>(345, 222));
            notas.Add(new KeyValuePair<int, int>(330, 222));
            notas.Add(new KeyValuePair<int, int>(300, 222));
            notas.Add(new KeyValuePair<int, int>(265, 444));
            notas.Add(new KeyValuePair<int, int>(220, 444));
            notas.Add(new KeyValuePair<int, int>(200, 444));
            notas.Add(new KeyValuePair<int, int>(220, 444));
            notas.Add(new KeyValuePair<int, int>(300, 444));
            notas.Add(new KeyValuePair<int, int>(245, 444));
            notas.Add(new KeyValuePair<int, int>(265, 888));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: FinalFantasyVictoryTheme</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_FinalFantasyVictoryTheme()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(987, 53));
            notas.Add(new KeyValuePair<int, int>(0, 53));
            notas.Add(new KeyValuePair<int, int>(987, 53));
            notas.Add(new KeyValuePair<int, int>(0, 53));
            notas.Add(new KeyValuePair<int, int>(987, 53));
            notas.Add(new KeyValuePair<int, int>(0, 53));
            notas.Add(new KeyValuePair<int, int>(987, 428));
            notas.Add(new KeyValuePair<int, int>(784, 428));
            notas.Add(new KeyValuePair<int, int>(880, 428));
            notas.Add(new KeyValuePair<int, int>(987, 107));
            notas.Add(new KeyValuePair<int, int>(0, 214));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(987, 857));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 428));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(880, 428));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(830, 428));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(830, 428));
            notas.Add(new KeyValuePair<int, int>(830, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 428));
            notas.Add(new KeyValuePair<int, int>(622, 428));
            notas.Add(new KeyValuePair<int, int>(659, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(554, 1714));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 428));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(880, 428));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(830, 428));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(830, 428));
            notas.Add(new KeyValuePair<int, int>(830, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(659, 428));
            notas.Add(new KeyValuePair<int, int>(740, 428));
            notas.Add(new KeyValuePair<int, int>(880, 107));
            notas.Add(new KeyValuePair<int, int>(0, 107));
            notas.Add(new KeyValuePair<int, int>(987, 1714));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: MusicalQuardust</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_MusicalVinIPSmaker1()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(300, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(200, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(300, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(100, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(300, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(200, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(300, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(400, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(500, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));
            notas.Add(new KeyValuePair<int, int>(600, 300));
            notas.Add(new KeyValuePair<int, int>(700, 300));


            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: MusicalQuardust</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_MusicalVinIPSmaker2()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));
            notas.Add(new KeyValuePair<int, int>(2500, 300));
            notas.Add(new KeyValuePair<int, int>(3000, 300));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: MusicalQuardust</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_MusicalQuardust()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(658, 125));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 125));
            notas.Add(new KeyValuePair<int, int>(1188, 125));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 750));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1056, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1408, 250));
            notas.Add(new KeyValuePair<int, int>(1760, 500));
            notas.Add(new KeyValuePair<int, int>(1584, 250));
            notas.Add(new KeyValuePair<int, int>(1408, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 750));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 500));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1056, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(0, 500));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 125));
            notas.Add(new KeyValuePair<int, int>(1188, 125));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 750));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1056, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(0, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1408, 250));
            notas.Add(new KeyValuePair<int, int>(1760, 500));
            notas.Add(new KeyValuePair<int, int>(1584, 250));
            notas.Add(new KeyValuePair<int, int>(1408, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 750));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1188, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(990, 500));
            notas.Add(new KeyValuePair<int, int>(990, 250));
            notas.Add(new KeyValuePair<int, int>(1056, 250));
            notas.Add(new KeyValuePair<int, int>(1188, 500));
            notas.Add(new KeyValuePair<int, int>(1320, 500));
            notas.Add(new KeyValuePair<int, int>(1056, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(880, 500));
            notas.Add(new KeyValuePair<int, int>(0, 500));
            notas.Add(new KeyValuePair<int, int>(660, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(594, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(440, 1000));
            notas.Add(new KeyValuePair<int, int>(419, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(660, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(594, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 500));
            notas.Add(new KeyValuePair<int, int>(660, 500));
            notas.Add(new KeyValuePair<int, int>(880, 1000));
            notas.Add(new KeyValuePair<int, int>(838, 2000));
            notas.Add(new KeyValuePair<int, int>(660, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(594, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(440, 1000));
            notas.Add(new KeyValuePair<int, int>(419, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(660, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 1000));
            notas.Add(new KeyValuePair<int, int>(594, 1000));
            notas.Add(new KeyValuePair<int, int>(495, 1000));
            notas.Add(new KeyValuePair<int, int>(528, 500));
            notas.Add(new KeyValuePair<int, int>(660, 500));
            notas.Add(new KeyValuePair<int, int>(880, 1000));
            notas.Add(new KeyValuePair<int, int>(838, 2000));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: HarvesterOfSorrow</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_HarvesterOfSorrow()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(329, 300));
            notas.Add(new KeyValuePair<int, int>(493, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(659, 300));
            notas.Add(new KeyValuePair<int, int>(329, 150));
            notas.Add(new KeyValuePair<int, int>(493, 150));
            notas.Add(new KeyValuePair<int, int>(783, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(659, 300));
            notas.Add(new KeyValuePair<int, int>(329, 300));
            notas.Add(new KeyValuePair<int, int>(493, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(590, 300));
            notas.Add(new KeyValuePair<int, int>(392, 150));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(587, 300));
            notas.Add(new KeyValuePair<int, int>(349, 300));
            notas.Add(new KeyValuePair<int, int>(587, 300));
            notas.Add(new KeyValuePair<int, int>(329, 300));
            notas.Add(new KeyValuePair<int, int>(493, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(659, 300));
            notas.Add(new KeyValuePair<int, int>(329, 150));
            notas.Add(new KeyValuePair<int, int>(493, 150));
            notas.Add(new KeyValuePair<int, int>(783, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(659, 300));
            notas.Add(new KeyValuePair<int, int>(329, 300));
            notas.Add(new KeyValuePair<int, int>(493, 300));
            notas.Add(new KeyValuePair<int, int>(698, 300));
            notas.Add(new KeyValuePair<int, int>(590, 300));
            notas.Add(new KeyValuePair<int, int>(392, 150));
            notas.Add(new KeyValuePair<int, int>(440, 150));
            notas.Add(new KeyValuePair<int, int>(587, 300));
            notas.Add(new KeyValuePair<int, int>(349, 300));
            notas.Add(new KeyValuePair<int, int>(587, 300));

            return notas.ToArray();
        }

        /// <summary>
        /// <para>Obtem as notas para a música: DoReMiFa</para>
        /// </summary>
        /// <returns><para>Conjunto de notas musicais.</para></returns>
        private static KeyValuePair<int, int>[] ObterNotasPara_DoReMiFa()
        {
            List<KeyValuePair<int, int>> notas = new List<KeyValuePair<int, int>>();

            notas.Add(new KeyValuePair<int, int>(261, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 800));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(261, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(261, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 800));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(261, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(392, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 800));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(261, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(293, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(329, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 200));
            notas.Add(new KeyValuePair<int, int>(0, 100));
            notas.Add(new KeyValuePair<int, int>(349, 800));

            return notas.ToArray();
        }
    }
}
