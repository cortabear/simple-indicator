# Simple Indicator
Simple indicator to call short and long signals.

## Getting Started
1. First we will load NinjaTrader 8. Once loaded we will navigate to "New" / "NinjaScript Editor".  
1. Once the **NinjaScript Editor** has loaded navigate to the "Indicators" in the **NinjaScript Explorer**.
1. Right click on "Indicators" and click "New Indicator"
1. In the **New Indicator** dialog window under "General" give the indicator the name of "Demo".
1. Click "Input Parameters" > "Plots and Lines". Then click "**Add**".
1. **Name**: "Long", **Type**: Plot , **Plot Style**: Bar, **Color**: "SpringGreen"  
1. **Name**: "Short", **Type**: Plot , **Plot Style**: Bar, **Color**: "HotPink"   
1. Click **Generate**  

## Pseudo Code  
Now we will update the "OnBarUpdate()" function with [pseudo code](https://en.wikipedia.org/wiki/Pseudocode) to quickly map out what we will be trying to accomplish.  
1. Compute the [Stochastic](https://www.investopedia.com/terms/s/stochasticoscillator.asp) cross bias.
1. Compute the Oscillator Bias.  
1. With that bias, we will check to see if both bias agree. If "Long" print a long signal. If "Short" print a short signal.

```C
protected override void OnBarUpdate() {
  // Compute the Stochastic Cross Bias ("Long", "Short" or "Flat").

  // Compute the Oscillator Bias

  // With that bias, we will check to see if both bias agree.
  // If "Long" print a long signal.

  // If "Short" print a short signal.
}
```

## Scaffold  
Now that we have mapped out what we want to accomplish we can now begin to [scaffold](https://en.wikipedia.org/wiki/Scaffold_(programming)) the code for our indicator. For best practices we want to always use what is available to us inside of NinjaTrader. In this case, instead of declaring our own enumerated enumeration we will use what is already out there. Thus the Stochastic Cross. Re-use as much as possible.

```C
protected override void OnBarUpdate() {
  // Compute the Stochastic Cross Bias ("Long", "Short" or "No Bias").

  // Compute the Oscillator Bias

  // With that bias, we will check to see if both bias agree.
  // If "Long" print a long signal.

  // If "Short" print a short signal.
}
```
