using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class Cards : MonoBehaviour
    {
        public abstract class Card
        {
            #region Properties

            public abstract string Name { get; }

            public abstract int ManaCost { get; }

            public abstract string IconName { get; }

            #endregion

            public abstract List<(Vector2Int, Color32)> GetPreviewEffect(Vector2Int vMouseCoord);

            public abstract void Perform(Vector2Int vMouseCoord);
        }

        private static Cards    sm_instance;
        private Stack<Card>     drawCards = new Stack<Card>();
        private Stack<Card>     discardCards = new Stack<Card>();

        #region Properties

        // TODO: Create and store a Stack and return here
        public Stack<Card> DrawCards => drawCards;

        // TODO: Create and store a Stack and return here
        public Stack<Card> DiscardCards => discardCards;

        public static Cards Instance => sm_instance;

        #endregion

        private void Awake()
        {
            sm_instance = this;
        }

        public void InitializeCards()
        {
            // NOTHING TO DO IN THIS FUNCTION... Leave as is :)

            try
            {
                // find all card types
                List<System.Type> cardTypes = new List<System.Type>(System.Array.FindAll(typeof(Card).Assembly.GetTypes(), t => !t.IsAbstract && typeof(Card).IsAssignableFrom(t)));

                // create cards
                List<Card> cards = new List<Card>();
                for (int i = 0; i < 10; ++i)
                {
                    System.Type t = cardTypes[i % cardTypes.Count];
                    Card card = System.Activator.CreateInstance(t) as Card;
                    if (card != null)
                    {
                        cards.Add(card);
                    }
                }

                // shuffle cards
                for (int i = 0; i < cards.Count; ++i)
                {
                    int i1 = Random.Range(0, DrawCards.Count);
                    int i2 = Random.Range(0, DrawCards.Count);
                    if (i1 != i2)
                    {
                        Card tmp = cards[i1];
                        cards[i1] = cards[i2];
                        cards[i2] = tmp;
                    }
                }

                // push onto draw stack
                foreach (Card card in cards)
                {
                    DrawCards.Push(card);
                }
            }
            catch (System.NotImplementedException)
            {
                Debug.Log("Looks like you have some work to do?");
            }
        }

        public Card DrawCard()
        {
            // TODO: get the next card on the DrawCards stack and return it
            // (removing it from the stack in the process)
            return DrawCards.Pop();
        }

        public void DiscardCard(Card card)
        {
            // TODO: push the card being passed in to this function
            // on the DiscardStack
            DiscardCards.Push(card);
        }

        public void ShuffleDeck()
        {
            // TODO: 
            // 1. Add ALL the cards from the DiscardStack into a new temporary List
            // 2. Shuffle the new list by randomly swapping elements in the list 100 times
            // 3. Clear the DiscardCards stack
            // 4. Add all the Cards from your temporary (now shuffled) list into the DrawStack
            List<Card> tempList = new List<Card>();
            foreach (Card card in DiscardCards)
            {
                tempList.Add(card);
            }
            DiscardCards.Clear();
            for (int i = 0; i < 100; i++)
            {
                int cardIndex1 = Random.Range(0, tempList.Count);
                int cardIndex2 = Random.Range(0, tempList.Count);

                while (cardIndex1 == cardIndex2)
                {
                    cardIndex2 = Random.Range(0, tempList.Count);
                }

                Card temp = tempList[cardIndex1];
                tempList[cardIndex1] = tempList[cardIndex2];
                tempList[cardIndex2] = temp;
            }
            foreach (Card card in tempList)
            {
                DrawCards.Push(card);
            }
        }
    }
}