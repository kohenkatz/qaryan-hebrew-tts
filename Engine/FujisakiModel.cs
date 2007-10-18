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

/*
 * Created by SharpDevelop.
 * User: Moti Z
 * Date: 8/25/2007
 * Time: 8:22 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;

namespace Qaryan.Core
{
	struct AccentCommand {
		public double T1,T2,A;
		public AccentCommand(double T1, double T2, double A) {
			this.T1=T1;
			this.T2=T2;
			this.A=A;
		}
	}
	
	struct PhraseCommand {
		public double T0,A;
		public PhraseCommand(double T0, double A) {
			this.T0=T0;
			this.A=A;
		}
	}
	
	/// <summary>
	/// Description of FujisakiModel.
	/// </summary>
	public class FujisakiModel
	{		
		public double alpha=2,beta=15,gamma=0.9,Fb=96,Threshold=0.0001;

		double Gp(double t) {
			if (t>=0)
				return (t*alpha*alpha*Math.Exp(-alpha*t));
			else
				return 0;
		}
		
		double Ga(double t) {
			if (t>=0)
				return Math.Min( 1 - (1+beta*t)*Math.Exp(-beta*t),gamma);
			else
				return 0;
		}
		
		public double LogF0(double t) {
			double Result=Math.Log(Fb);
			double x;
			for(int i=0;i<I;i++) {
				x=Ap(i)*Gp(t-T0(i));
				Result+=x;
/*				if (Math.Abs(x)<Threshold) {
					p.RemoveAt(i);
					i--;
				}*/
			}
			for(int j=0;j<J;j++) {
				x=Aa(j)*(Ga(t-T1(j))-Ga(t-T2(j)));
				Result+=x;
/*				if (Math.Abs(x)<Threshold) {
					a.RemoveAt(j);
					j--;
				}*/
			}
			return Result;
		}
		
		public double F0(double t) {
			return Math.Exp(LogF0(t));
		}

        public double LogF0AComponent(double t)
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

        public double F0AComponent(double t)
        {
            return Math.Exp(LogF0AComponent(t));
        }

        public double LogF0PComponent(double t)
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

        public double F0PComponent(double t)
        {
            return Math.Exp(LogF0PComponent(t));
        }

		public FujisakiModel()
		{
		}
		
		#region public utilities
		public void AddAccentCommand(double T1,double T2,double A) {
			a.Add(new AccentCommand(T1,T2,A));
		}
		
		public void AddPhraseCommand(double T0,double A) {
			p.Add(new PhraseCommand(T0,A));
		}

        public void Reset()
        {
            a.Clear();
            p.Clear();
        }
		#endregion
		
		#region private utilities
		List<AccentCommand> a=new List<AccentCommand>();
		List<PhraseCommand> p=new List<PhraseCommand>();		
		
		int I {
			get {
				return p.Count;
			}
		}
		
		int J {
			get {
				return a.Count;
			}
		}
		
		double Ap(int i) {
			return p[i].A;
		}
		
		double Aa(int j) {
			return a[j].A;
		}
		
		double T0(int i) {
			return p[i].T0;
		}

		double T1(int j) {
			return a[j].T1;
		}

		double T2(int j) {
			return a[j].T2;
		}		
		#endregion
				
	}
}
