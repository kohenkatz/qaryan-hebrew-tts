//    This file is part of Qaryan.
//
//    Qaryan is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Qaryan is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Qaryan.  If not, see <http://www.gnu.org/licenses/>.
//
//    Based on <http://www.isca-speech.org/archive/sp2004/sp04_001.pdf>.

using System;
using System.Collections.Generic;

namespace Qaryan.Core
{
    struct AccentCommand
    {
        public double T1, T2, A;
        public AccentCommand(double T1, double T2, double A)
        {
            this.T1 = T1;
            this.T2 = T2;
            this.A = A;
        }
    }

    struct PhraseCommand
    {
        public double T0, A;
        public PhraseCommand(double T0, double A)
        {
            this.T0 = T0;
            this.A = A;
        }
    }

    /// <summary>
    /// A straightforward implementation of H. Fujisaki's command-response model of F<sub>0</sub> generation.
    /// </summary>
    public sealed class FujisakiModel
    {
        /// <summary>
        /// &alpha;; The natural angular frequency of the phrase control mechanism.
        /// </summary>
        public double alpha = 2;

        /// <summary>
        /// &beta;;  The natural angular frequency of the accent control mechanism.
        /// </summary>
        public double beta = 15;

        /// <summary>
        /// &gamma;; The relative ceiling level of accent components.
        /// </summary>
        public double gamma = 0.9;

        /// <summary>
        /// F<sub>b</sub>; The baseline value of fundamental frequency.
        /// </summary>
        public double Fb = 96;

        /// <summary>
        /// Not currently used.
        /// </summary>
        public double Threshold = 0.0001;

        /// <summary>
        /// G<sub>p</sub>(t) represents the impulse response function of the phrase control mechanism at time <paramref name="t"/>.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>A value used by <see cref="LogF0">LogF0</see>.</returns>
        double Gp(double t)
        {
            if (t >= 0)
                return (t * alpha * alpha * Math.Exp(-alpha * t));
            else
                return 0;
        }

        /// <summary>
        /// G<sub>a</sub>(t) represents the step response function of the accent control mechanism.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>A value used by <see cref="LogF0">LogF0</see>.</returns>		
        double Ga(double t)
        {
            if (t >= 0)
                return Math.Min(1 - (1 + beta * t) * Math.Exp(-beta * t), gamma);
            else
                return 0;
        }

        /// <summary>
        /// Calculates log<sub>e</sub>F<sub>0</sub>(t).
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The natural logarithm of the F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        double LogF0(double t)
        {
            double Result = Math.Log(Fb);
            double x;
            for (int i = 0; i < I; i++)
            {
                x = Ap(i) * Gp(t - T0(i));
                Result += x;
                /*				if (Math.Abs(x)<Threshold) {
                                    p.RemoveAt(i);
                                    i--;
                                }*/
            }
            for (int j = 0; j < J; j++)
            {
                x = Aa(j) * (Ga(t - T1(j)) - Ga(t - T2(j)));
                Result += x;
                /*				if (Math.Abs(x)<Threshold) {
                                    a.RemoveAt(j);
                                    j--;
                                }*/
            }
            return Result;
        }

        /// <summary>
        /// Calculates F<sub>0</sub>(t).
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        public double F0(double t)
        {
            return Math.Exp(LogF0(t));
        }

        /// <summary>
        /// Calculates log<sub>e</sub>F<sub>0</sub>(t) using only its accent components.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The natural logarithm of the partial F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        double LogF0AComponent(double t)
        {
            double Result = 0;
            double x;
            for (int j = 0; j < J; j++)
            {
                x = Aa(j) * (Ga(t - T1(j)) - Ga(t - T2(j)));
                Result += x;
            }
            return Result;
        }

        /// <summary>
        /// Calculates F<sub>0</sub>(t) using only its accent components.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The partial F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        public double F0AComponent(double t)
        {
            return Math.Exp(LogF0AComponent(t));
        }

        /// <summary>
        /// Calculates log<sub>e</sub>F<sub>0</sub>(t) using only its phrase components.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The natural logarithm of the partial F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        double LogF0PComponent(double t)
        {
            double Result = Math.Log(Fb);
            double x;
            for (int i = 0; i < I; i++)
            {
                x = Ap(i) * Gp(t - T0(i));
                Result += x;
                /*				if (Math.Abs(x)<Threshold) {
                                    p.RemoveAt(i);
                                    i--;
                                }*/
            }
            return Result;
        }

        /// <summary>
        /// Calculates F<sub>0</sub>(t) using only its phrase components.
        /// </summary>
        /// <param name="t">The current time, in seconds since an arbitrary t<sub>0</sub>.</param>
        /// <returns>The partial F<sub>0</sub> value at time <paramref name="t"/>.</returns>
        public double F0PComponent(double t)
        {
            return Math.Exp(LogF0PComponent(t));
        }

        public FujisakiModel()
        {
        }

        #region public utilities

        /// <summary>
        /// Adds an accent command with onset <paramref name="T1"/>, offset <paramref name="T2"/> and amplitude <paramref name="A"/>.
        /// </summary>
        /// <param name="T1">Onset of the command.</param>
        /// <param name="T2">Offset of the command.</param>
        /// <param name="A">Amplitude of the command.</param>
        public void AddAccentCommand(double T1, double T2, double A)
        {
            a.Add(new AccentCommand(T1, T2, A));
        }

        internal AccentCommand BeginAccentCommand(double T1, double A)
        {
            a.Add(new AccentCommand(T1, double.MaxValue, A));
            return a[a.Count - 1];
        }

        internal AccentCommand EndAccentCommand(double T2)
        {
            if (a.Count > 0)
            {
                a[a.Count - 1]=new AccentCommand(a[a.Count - 1].T1,T2,a[a.Count - 1].A);
                return a[a.Count - 1];
            }
            return new AccentCommand(0, 0, 0);
        }

        public bool IsAccentCommandUnclosed
        {
            get
            {
                return (a.Count > 0) && (a[a.Count - 1].T2 == double.MaxValue);
            }
        }

        internal AccentCommand ToggleAccentCommand(double T, double A)
        {
            if (IsAccentCommandUnclosed)
                return EndAccentCommand(T);
            else
                return BeginAccentCommand(T, A);
        }

        /// <summary>
        /// Adds a phrase command with timing <paramref name="T0"/> and magnitude <paramref name="A"/>.
        /// </summary>
        /// <param name="T0">Timing of the command.</param>
        /// <param name="A">Magnitude of the command.</param>
        public void AddPhraseCommand(double T0, double A)
        {
            p.Add(new PhraseCommand(T0, A));
        }

        /// <summary>
        /// Resets the model by clearing all accent- and phrase-commands.
        /// </summary>
        public void Reset()
        {
            a.Clear();
            p.Clear();
        }
        #endregion

        #region private utilities
        List<AccentCommand> a = new List<AccentCommand>();
        List<PhraseCommand> p = new List<PhraseCommand>();

        /// <summary>
        /// Gets the number of phrase commands.
        /// </summary>
        int I
        {
            get
            {
                return p.Count;
            }
        }

        /// <summary>
        /// Gets the number of accent commands.
        /// </summary>
        int J
        {
            get
            {
                return a.Count;
            }
        }

        /// <summary>
        /// Retrieves <i>Ap<sub>i</sub></i>.
        /// </summary>
        /// <param name="i">Index of command</param>
        /// <returns>The magnitude of the <paramref name="i"/>th phrase command.</returns>
        double Ap(int i)
        {
            return p[i].A;
        }

        /// <summary>
        /// Retrieves <i>Aa<sub>i</sub></i>.
        /// </summary>
        /// <param name="i">Index of command</param>
        /// <returns>The amplitude of the <paramref name="i"/>th accent command.</returns>
        double Aa(int j)
        {
            return a[j].A;
        }

        /// <summary>
        /// Retrieves <i>T<sub>0<sub>i</sub></sub></i>.
        /// </summary>
        /// <param name="i">Index of command</param>
        /// <returns>The timing of the <paramref name="i"/>th phrase command.</returns>
        double T0(int i)
        {
            return p[i].T0;
        }

        /// <summary>
        /// Retrieves <i>T<sub>1<sub>j</sub></sub></i>.
        /// </summary>
        /// <param name="j">Index of command</param>
        /// <returns>The onset of the <paramref name="j"/>th accent command.</returns>
        double T1(int j)
        {
            return a[j].T1;
        }

        /// <summary>
        /// Retrieves <i>T<sub>2<sub>j</sub></sub></i>.
        /// </summary>
        /// <param name="j">Index of command</param>
        /// <returns>The offset of the <paramref name="j"/>th accent command.</returns>
        double T2(int j)
        {
            return a[j].T2;
        }
        #endregion

    }
}
