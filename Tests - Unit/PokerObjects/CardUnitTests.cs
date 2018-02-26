﻿using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using PokerCalculator.Tests.Shared.TestData;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class CardUnitTests : AbstractUnitTestBase
	{
		#region Constructor

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsTestCaseData))]
		public void Constructor(CardValue value, CardSuit suit)
		{
			//act
			var actual = new Card(value, suit);

			//assert
			Assert.That(actual.Value, Is.EqualTo(value));
			Assert.That(actual.Suit, Is.EqualTo(suit));
		}

		[Test, TestCaseSource(typeof(CardTestCaseData), nameof(CardTestCaseData.AllCardsAsString))]
		public void Constructor_string(string cardAsString, CardValue expectedCardValue, CardSuit expectedCardSuit)
		{
			//act
			var actual = new Card(cardAsString);

			//assert
			Assert.That(actual.Value, Is.EqualTo(expectedCardValue));
			Assert.That(actual.Suit, Is.EqualTo(expectedCardValue));
		}

		#endregion
	}
}
