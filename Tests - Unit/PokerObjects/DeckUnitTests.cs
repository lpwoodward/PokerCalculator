﻿using Castle.MicroKernel.Registration;
using NUnit.Framework;
using PokerCalculator.Domain.Helpers;
using PokerCalculator.Domain.PokerEnums;
using PokerCalculator.Domain.PokerObjects;
using PokerCalculator.Tests.Shared;
using Rhino.Mocks;
using System;
using System.Collections.Generic;

namespace PokerCalculator.Tests.Unit.PokerObjects
{
	[TestFixture]
	public class DeckUnitTests : AbstractUnitTestBase
	{
		private Deck _instance;
		private IRandomNumberGenerator _randomNumberGenerator;
		private IUtilitiesService _utilitiesService;

		[SetUp]
		public override void Setup()
		{
			base.Setup();

			_randomNumberGenerator = MockRepository.GenerateStrictMock<IRandomNumberGenerator>();
			_utilitiesService = MockRepository.GenerateStrictMock<IUtilitiesService>();

			_utilitiesService.Stub(x => x.GetEnumValues<CardSuit>()).Return(new List<CardSuit>()).Repeat.Once();
			_utilitiesService.Stub(x => x.GetEnumValues<CardValue>()).Return(new List<CardValue>()).Repeat.Once();

			_instance = MockRepository.GeneratePartialMock<Deck>(_randomNumberGenerator, _utilitiesService);
		}

		#region Constructor

		[Test]
		public void Constructor()
		{
			//arrange
			WindsorContainer.Register(Component.For<IUtilitiesService>().Instance(_utilitiesService));
			WindsorContainer.Register(Component.For<IRandomNumberGenerator>().Instance(_randomNumberGenerator));

			var cardSuits = new List<CardSuit> { CardSuit.Clubs, CardSuit.Hearts };
			_utilitiesService.Stub(x => x.GetEnumValues<CardSuit>()).Return(cardSuits);

			var cardValues = new List<CardValue> { CardValue.Eight, CardValue.King };
			_utilitiesService.Stub(x => x.GetEnumValues<CardValue>()).Return(cardValues);

			//act
			var actual = new Deck();

			//assert
			Assert.That(actual.Cards, Has.Count.EqualTo(4));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.Eight &&
																  x.Suit == CardSuit.Clubs));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.Eight &&
																  x.Suit == CardSuit.Hearts));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.King &&
																  x.Suit == CardSuit.Clubs));
			Assert.That(actual.Cards, Has.Some.Matches<Card>(x => x.Value == CardValue.King &&
																  x.Suit == CardSuit.Hearts));
		}

		#endregion

		#region Instance Methods

		#region Shuffle

		[Test]
		public void Shuffle()
		{
			//arrange
			var card1 = new Card(CardValue.Ace, CardSuit.Clubs);
			var card2 = new Card(CardValue.Two, CardSuit.Clubs);
			var card3 = new Card(CardValue.Three, CardSuit.Clubs);
			var card4 = new Card(CardValue.Four, CardSuit.Clubs);

			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				card1, card2, card3, card4
			});

			const int firstRandomNumber = 1781;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(firstRandomNumber).Repeat.Once();

			const int secondRandomNumber = 514;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(secondRandomNumber).Repeat.Once();

			const int thirdRandomNumber = 4981;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(thirdRandomNumber).Repeat.Once();

			const int fourthRandomNumber = 45;
			_randomNumberGenerator.Stub(x => x.Next(5000)).Return(fourthRandomNumber).Repeat.Once();

			_instance.Expect(x => x.Cards = Arg<List<Card>>.Matches(y => y.Count == 4 &&
																		 y[0] == card4 &&
																		 y[1] == card2 &&
																		 y[2] == card1 &&
																		 y[3] == card3));

			//act
			_instance.Shuffle();

			//assert
			_instance.VerifyAllExpectations();
		}

		#endregion

		#region RemoveCard

		[Test]
		public void RemoveCard_WHERE_card_is_not_in_deck_SHOULD_throw_error()
		{
			//arrange
			var cardToRemove = new Card(CardValue.Two, CardSuit.Clubs);

			var card1InDeck = new Card(CardValue.Two, CardSuit.Diamonds);
			var card2InDeck = new Card(CardValue.Seven, CardSuit.Clubs);
			_instance.Stub(x => x.Cards).Return(new List<Card> { card1InDeck, card2InDeck });

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.RemoveCard(cardToRemove));
			Assert.That(actualException.Message, Is.EqualTo("Cannot remove Card, it is not in Deck."));
		}

		[Test]
		public void RemoveCard()
		{
			//arrange
			var cardToRemove = new Card(CardValue.Two, CardSuit.Clubs);

			var card1InDeck = new Card(CardValue.Seven, CardSuit.Hearts);
			var card2InDeck = new Card(CardValue.Two, CardSuit.Clubs);

			var cardsInDeck = new List<Card> { card1InDeck, card2InDeck };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			//act
			_instance.RemoveCard(cardToRemove);

			//assert
			Assert.That(cardsInDeck, Has.Count.EqualTo(1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1InDeck));
			Assert.That(cardsInDeck, Has.None.EqualTo(card2InDeck));
		}

		#endregion

		#region TakeRandomCard

		[Test]
		public void TakeRandomCard_WHERE_no_more_cards_left_in_deck_SHOULD_throw_error()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>());

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.TakeRandomCard());
			Assert.That(actualException.Message, Is.EqualTo("No cards left in Deck to take."));
		}

		[Test]
		public void TakeRandomCard()
		{
			//arrange
			var card1 = new Card(CardValue.Seven, CardSuit.Hearts);
			var card2 = new Card(CardValue.Nine, CardSuit.Hearts);
			var card3 = new Card(CardValue.Four, CardSuit.Clubs);
			var card4 = new Card(CardValue.Two, CardSuit.Diamonds);
			var card5 = new Card(CardValue.Two, CardSuit.Spades);

			var cardsInDeck = new List<Card> { card1, card2, card3, card4, card5 };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			_randomNumberGenerator.Stub(x => x.Next(5)).Return(3);

			//act
			var actual = _instance.TakeRandomCard();

			//assert
			Assert.That(actual, Is.EqualTo(card4));

			Assert.That(cardsInDeck, Has.Count.EqualTo(4));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card2));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card3));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card5));
			Assert.That(cardsInDeck, Has.None.EqualTo(card4));
		}

		#endregion

		#region GetRandomCards

		[Test]
		public void GetRandomCards_WHERE_not_enough_cards_left_in_deck()
		{
			//arrange
			_instance.Stub(x => x.Cards).Return(new List<Card>
			{
				new Card(CardValue.Ace, CardSuit.Hearts),
				new Card(CardValue.Nine, CardSuit.Spades)
			});

			//act + assert
			var actualException = Assert.Throws<Exception>(() => _instance.GetRandomCards(3));
			Assert.That(actualException.Message, Is.EqualTo("Cannot get more cards than there are left in the Deck."));
		}

		[Test]
		public void GetRandomCards()
		{
			//arrange
			var card1 = new Card(CardValue.Eight, CardSuit.Clubs);
			var card2 = new Card(CardValue.Ace, CardSuit.Hearts);
			var card3 = new Card(CardValue.Nine, CardSuit.Spades);
			var card4 = new Card(CardValue.King, CardSuit.Spades);
			var card5 = new Card(CardValue.Two, CardSuit.Clubs);

			var cardsInDeck = new List<Card> { card1, card2, card3, card4, card5 };
			_instance.Stub(x => x.Cards).Return(cardsInDeck);

			_randomNumberGenerator.Stub(x => x.Next(5)).Return(2);
			_randomNumberGenerator.Stub(x => x.Next(4)).Return(2);
			_randomNumberGenerator.Stub(x => x.Next(3)).Return(0);

			//act
			var actual = _instance.GetRandomCards(3);

			//assert
			Assert.That(actual, Has.Count.EqualTo(3));
			Assert.That(actual, Has.Some.EqualTo(card1));
			Assert.That(actual, Has.Some.EqualTo(card3));
			Assert.That(actual, Has.Some.EqualTo(card4));

			Assert.That(cardsInDeck, Has.Count.EqualTo(5));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card1));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card2));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card3));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card4));
			Assert.That(cardsInDeck, Has.Some.EqualTo(card5));
		}

		#endregion

		#endregion
	}
}
