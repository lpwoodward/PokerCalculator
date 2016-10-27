﻿using NUnit.Framework;
using PokerCalculator.Domain;
using System.Security.Policy;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration
{
	[TestFixture]
	public class CardComparerIntegrationTests
	{
		private CardComparer _instance;

		[SetUp]
		public void Setup()
		{
			_instance = new CardComparer();
		}

		#region Equals

		[Test]
		public void Equals_WHERE_both_cards_are_null_SHOULD_return_true()
		{
			//act
			var actual = _instance.Equals(null, null);

			//asssert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_first_card_is_null_and_second_is_not_SHOULD_return_false()
		{
			//arrange
			var card = Card.Create(CardValue.Ace, CardSuit.Hearts);

			//act
			var actual = _instance.Equals(null, card);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_second_card_is_null_and_first_is_not_SHOULD_return_false()
		{
			//arrange
			var card = Card.Create(CardValue.Ace, CardSuit.Hearts);

			//act
			var actual = _instance.Equals(card, null);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_cards_are_same_value_in_memory_SHOULD_return_true()
		{
			//arrange
			var card = Card.Create(CardValue.Ace, CardSuit.Hearts);

			//act
			var actual = _instance.Equals(card, card);

			//asssert
			Assert.That(actual, Is.True);
		}

		[Test]
		public void Equals_WHERE_value_is_different_SHOULD_return_false()
		{
			//arrange
			const CardSuit suit = CardSuit.Clubs;
			var card1 = Card.Create(CardValue.Ace, suit);
			var card2 = Card.Create(CardValue.Six, suit);

			//act
			var actual = _instance.Equals(card1, card2);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_suit_is_different_SHOULD_return_false()
		{
			//arrange
			const CardValue value = CardValue.Seven;
			var card1 = Card.Create(value, CardSuit.Diamonds);
			var card2 = Card.Create(value, CardSuit.Clubs);

			//act
			var actual = _instance.Equals(card1, card2);

			//asssert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void Equals_WHERE_value_and_suit_are_the_same_SHOULD_return_true()
		{
			//arrange
			const CardValue value = CardValue.Seven;
			const CardSuit suit = CardSuit.Diamonds;
			var card1 = Card.Create(value, suit);
			var card2 = Card.Create(value, suit);

			//act
			var actual = _instance.Equals(card1, card2);

			//asssert
			Assert.That(actual, Is.True);
		}

		#endregion

		#region GetHashCode

		[Test]
		public new void GetHashCode()
		{
			//arrange
			var card = Card.Create(CardValue.King, CardSuit.Spades);

			//act
			var actual = _instance.GetHashCode(card);

			//assert
			Assert.That(actual, Is.Not.Null);
		}

		#endregion
	}
}