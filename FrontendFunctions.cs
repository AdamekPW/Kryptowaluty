﻿using ASP_.NET_nauka.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;

namespace ASP_.NET_nauka;
public class FrontendFunctions
{
	public static decimal Make5Digits(decimal number)
	{
		switch (number) {
			case > 10000: return Math.Round(number, 0);
			case > 1000: return Math.Round(number, 1);
			case > 100: return Math.Round(number, 2);
			case > 10: return Math.Round(number, 3);
			default: return Math.Round(number, 4);
		}

	}
	public static string RemoveZeros(decimal number)
	{
		string SNumber = number.ToString();
		int Slength = SNumber.Length;
		int i = Slength - 1;
        while (i >= 0 && SNumber[i] == '0') 
        {
			i--;
        }

		return SNumber.Substring(0, i) + '0';
	}
	public static decimal CalculateTotalEquity(WalletPackage WP)
	{
		decimal totalEquity = WP.CurrenciesValues.Sum(x => x.Value * x.currency.Value);
		totalEquity += WP.User.USDT_balance;
		return totalEquity;
	}
}

