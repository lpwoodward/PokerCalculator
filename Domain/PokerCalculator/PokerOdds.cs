﻿using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using System.Collections.Generic;
using System.Linq;

namespace PokerCalculator.Domain.PokerCalculator
{
	public class PokerOdds
	{
		#region Properties and Fields

		public virtual int NumWins { get; set; }
		public virtual int NumDraws { get; set; }
		public virtual int NumLosses { get; set; }
		protected internal virtual int TotalNumHands => NumWins + NumDraws + NumLosses;

		public virtual double WinPercentage => TotalNumHands == 0 ? 0 : (double)NumWins / TotalNumHands;
		public virtual double DrawPercentage => TotalNumHands == 0 ? 0 : (double)NumDraws / TotalNumHands;
		public virtual double LossPercentage => TotalNumHands == 0 ? 0 : (double)NumLosses / TotalNumHands;

		public virtual Dictionary<PokerHand, int> PokerHandFrequencies { get; }
		public virtual Dictionary<PokerHand, double> PokerHandPercentages => PokerHandFrequencies.ToDictionary(x => x.Key, x => TotalNumHands == 0 ? 0 : (double)x.Value / TotalNumHands);

		#endregion

		#region Constructor

		public PokerOdds(IUtilitiesService utilitiesService)
		{
			PokerHandFrequencies = utilitiesService.GetEnumValues<PokerHand>().ToDictionary(x => x, x => 0);
		}

		#endregion
	}
}