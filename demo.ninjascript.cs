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

		// PUBLIC PROPERTIES
		public enum EOscChoice {
			McClellan,
			PPO
		}

		public EOscChoice OscChoice { get; set; }
		public double LowerThreshold {get; set;}
		public double UpperThreshold {get; set;}
		public double OscLowerThreshold {get; set;}
		public double OscUpperThreshold {get; set;}

		public bool DebugMode { get; set; } // DEGUB SWITCH

		// PRIVATE
		private ISeries<double> _Oscillator;

		// STOCHASTICS
		protected Stochastics Stoch {
			get {
				return Stochastics(7,14,3); // 'period D', 'period K' and 'smooth'
			}
		}

		// I SERIES
		protected ISeries<double> Oscillator {
			get {

				if( _Oscillator == null ){

					// SWITCH
					switch(OscChoice){
						case EOscChoice.McClellan:
							_Oscillator = McClellanOscillator(19, 39);
							break;

						case EOscChoice.PPO:
							_Oscillator = PPO(12,26,9);
							break;
					}

				}
				return _Oscillator;
			}
		}

		// STATES
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
				// INITIALIZE
				InitializeIndicators();
			}
		}

		// INITALIZE INDICATORS
		protected void InitializeIndicators(){
			if( OscChoice == EOscChoice.McClellan ){
				AddDataSeries( "^ADV" );
				AddDataSeries( "^DECL" );
			}
		}

		// STOCHASTIC CROSS
		protected MarketPosition StochCross(){

			// LONG BIAS: STOCHASTIC CROSS
			if(CrossAbove(Stoch.D, LowerThreshold, 1)){
				return MarketPosition.Long;

			// SHORT BIAS: STOCHASTIC CROSS
			}else if(CrossBelow(Stoch.D, UpperThreshold, 1)){
				return MarketPosition.Short;
			}

			// FLAT BIAS: STOCHASTIC CROSS
			return MarketPosition.Flat;
		}

		// OSCILlATOR
		protected MarketPosition OscBias(){
			// LONG BIAS
			if( Oscillator[0] < OscLowerThreshold ){

				BackBrushes[0] = Brushes.LightGreen; // Paint the background
				return MarketPosition.Long;

			// SHORT BIAS
			}else if( Oscillator[0] > OscUpperThreshold ){

				BackBrushes[0] = Brushes.LightCyan; // Paint the background
				return MarketPosition.Short;
			}

			// FLAT BIAS
			return MarketPosition.Flat;
		}

		protected override void OnBarUpdate()
		{

			// Compute the Stochastic Cross Bias ("Long", "Short" or "No Bias").
			MarketPosition stoch = StochCross();
			DebugOutput(1);

			// Compute the Oscillator Bias
			MarketPosition osc = OscBias();
			DebugOutput(2);

			// With that bias, we will check to see if both bias agree.
			// If "Long" print a long signal.
			if (osc == MarketPosition.Long && stoch == MarketPosition.Long) {
				LongSignal();
				DebugOutput(3);

			// If "Short" print a short signal.
			}else if (osc == MarketPosition.Short && stoch == MarketPosition.Short) {
				ShortSignal();
				DebugOutput(4);
			}

		}

		// DEBUGGING
		private void DebugOutput(object obj){

			if( !DebugMode ){
				return;
			}

			Print(obj);
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
