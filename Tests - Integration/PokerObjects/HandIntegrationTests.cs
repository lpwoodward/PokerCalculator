﻿using System.Collections.Generic;
using NUnit.Framework;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;

namespace PokerCalculator.Tests.Integration.PokerObjects
{
	[TestFixture]
	public class HandIntegrationTests
	{
		Hand _instance;
		
		[SetUp]
		public void Setup()
		{
			_instance = Hand.Create();
		}
		
		#region Create

		[Test]
		public void Create_WHERE_no_cards_supplied_SHOULD_create_empty_hand()
		{
			//act
			var actual = Hand.Create();

			//assert
			Assert.That(actual.Cards, Is.Empty);
		}

		[Test]
		public void Create_WHERE_empty_card_list_supplied_SHOULD_create_empty_hand()
		{
			//act
			var actual = Hand.Create(new List<Card>());

			//assert
			Assert.That(actual.Cards, Is.Empty);
		}

		[Test]
		public void Create_WHERE_cards_supplied_SHOULD_create_hand_with_those_cards()
		{
			//arrange
			var card1 = Card.Create(CardValue.Five, CardSuit.Clubs);
			var card2 = Card.Create(CardValue.Queen, CardSuit.Diamonds);
			var card3 = Card.Create(CardValue.Eight, CardSuit.Spades);

			//act
			var actual = Hand.Create(new List<Card> { card1, card2, card3 });

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
		}

		[Test]
		public void Create_SHOULD_assign_new_list_rather_than_doing_memory_copy_to_stop_changes_to_list_later_affecting_hand()
		{
			//arrange
			var card1 = Card.Create(CardValue.Five, CardSuit.Clubs);
			var card2 = Card.Create(CardValue.Queen, CardSuit.Diamonds);
			var card3 = Card.Create(CardValue.Eight, CardSuit.Spades);

			var cards = new List<Card> { card1, card2, card3 };
			var actual = Hand.Create(cards);

			//act
			var cardAddedAfterHandCreated = Card.Create(CardValue.Seven, CardSuit.Clubs);
			cards.Add(cardAddedAfterHandCreated);

			//assert
			Assert.That(actual.Cards, Is.Not.SameAs(cards));
			Assert.That(actual.Cards, Has.Count.EqualTo(3));
			Assert.That(actual.Cards, Has.Some.EqualTo(card1));
			Assert.That(actual.Cards, Has.Some.EqualTo(card2));
			Assert.That(actual.Cards, Has.Some.EqualTo(card3));
			Assert.That(actual.Cards, Has.None.EqualTo(cardAddedAfterHandCreated));
		}

		#endregion

		#region AddCard

		[Test]
		public void AddCard_WHERE_hand_is_initially_empty_SHOULD_add_card()
		{
			//arrange
			var cardToAdd = Card.Create(CardValue.Four, CardSuit.Hearts);
			_instance.Rank = HandRank.Create(PokerHand.Flush);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(1));
			Assert.That(_instance.Cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(_instance._rank, Is.Null);
		}

		[Test]
		public void AddCard_WHERE_hand_already_has_cards_SHOULD_add_card_and_maintain_existing_cards()
		{
			//arrange
			var initialCard = Card.Create(CardValue.Ten, CardSuit.Diamonds);
			_instance.AddCard(initialCard);
			_instance.Rank = HandRank.Create(PokerHand.ThreeOfAKind);

			var cardToAdd = Card.Create(CardValue.Seven, CardSuit.Spades);

			//act
			_instance.AddCard(cardToAdd);

			//assert
			Assert.That(_instance.Cards, Has.Count.EqualTo(2));
			Assert.That(_instance.Cards, Has.Some.EqualTo(initialCard));
			Assert.That(_instance.Cards, Has.Some.EqualTo(cardToAdd));
			Assert.That(_instance._rank, Is.Null);
		}

		#endregion

	}
}