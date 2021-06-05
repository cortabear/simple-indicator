#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Indicators
{
	public class Demo : Indicator
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "Demo";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event.
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				AddPlot(new Stroke(Brushes.SpringGreen, 2), PlotStyle.Bar, "Long");
				AddPlot(new Stroke(Brushes.HotPink, 2), PlotStyle.Bar, "Short");

				// Make plots larger
				Plots[0].AutoWidth = true;
				Plots[1].AutoWidth = true; 

			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		{

			// Compute the Stochastic Cross Bias ("Long", "Short" or "No Bias").
			MarketPosition stoch = StochCross();

			// Compute the Oscillator Bias
			MarketPosition osc = OscBias();

			// With that bias, we will check to see if both bias agree.
			// If "Long" print a long signal.
			if (osc == MarketPosition.Long && stoch == MarketPosition.Long) {
				LongSignal();

			// If "Short" print a short signal.
			}else if (osc == MarketPosition.Short && stoch == MarketPosition.Short) {
				ShortSignal();
			}

		}

		// SIGNALS
		private void LongSignal(){
			Values[0][0] = 1;
		}

		private void ShortSignal(){
			Values[1][0] = -1;
		}

		#region Properties

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Long
		{
			get { return Values[0]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Short
		{
			get { return Values[1]; }
		}
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private Demo[] cacheDemo;
		public Demo Demo()
		{
			return Demo(Input);
		}

		public Demo Demo(ISeries<double> input)
		{
			if (cacheDemo != null)
				for (int idx = 0; idx < cacheDemo.Length; idx++)
					if (cacheDemo[idx] != null &&  cacheDemo[idx].EqualsInput(input))
						return cacheDemo[idx];
			return CacheIndicator<Demo>(new Demo(), input, ref cacheDemo);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.Demo Demo()
		{
			return indicator.Demo(Input);
		}

		public Indicators.Demo Demo(ISeries<double> input )
		{
			return indicator.Demo(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.Demo Demo()
		{
			return indicator.Demo(Input);
		}

		public Indicators.Demo Demo(ISeries<double> input )
		{
			return indicator.Demo(input);
		}
	}
}

#endregion
