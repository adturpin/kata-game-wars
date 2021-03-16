using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace game_war.tests
{
    public class Tests
    {

        [Test]
        public void Should_king_card_win_versus_queen()
        {
            BattleResult<CARD_VALUE> winner = GameWar.Battle(CARD_VALUE.KING, CARD_VALUE.QUEEN);
            Assert.AreEqual(CARD_VALUE.KING, winner.CardValue);
        }

        [Test]
        public void Should_ten_lose_versus_king()
        {
            BattleResult<CARD_VALUE> winner = GameWar.Battle(CARD_VALUE.TEN, CARD_VALUE.KING);
            Assert.AreEqual(CARD_VALUE.KING, winner.CardValue);
        }

        [Test]
        public void Should_ten_equal_versus_ten()
        {
            var result = GameWar.Battle(CARD_VALUE.TEN, CARD_VALUE.TEN);
            Assert.IsTrue(result.IsEquality);
        }

        // player 
        [Test]
        public void Should_player_one_win_versus_player_two()
        {
            GameWar game = new GameWar();

            Player p1 = new Player();
            CARD_VALUE v1 = CARD_VALUE.ACE;
            p1.PlaceCard(game, v1);

            CARD_VALUE v2 = CARD_VALUE.TWO;
            Player p2 = new Player();
            p2.PlaceCard(game, v2);

            Assert.AreEqual(p1, game.ResolveRound());
        }
    }

    internal class Player
    {
        internal void PlaceCard(GameWar game, CARD_VALUE v1)
        {
            game.PlaceCard(v1,this);
        }
    }

    internal class GameWar
    {
        internal static BattleResult<CARD_VALUE> Battle(CARD_VALUE v1, CARD_VALUE v2)
        {
            if (v1.Equals(v2))
                return BattleResult<CARD_VALUE>.CreateEquality();
            return BattleResult<CARD_VALUE>.CreateWinner((CARD_VALUE)Math.Max((int)v1, (int)v2));
        }

        Round currentRound ;
        public GameWar()
        {
            currentRound = new Round();
        }
        internal void PlaceCard(CARD_VALUE v1, Player player)
        {            
            currentRound.Play(player, v1);
        }

        internal Player ResolveRound()
        {
            return currentRound.ResolveRound();
        }

        private class Round
        {
            Dictionary<Player,CARD_VALUE>  currentRound ;
            public Round()
            {
                currentRound = new Dictionary<Player, CARD_VALUE>();
            }

            internal void Play(Player player, CARD_VALUE v1)
            {
                currentRound[player] = v1;
            }

            internal Player ResolveRound()
            {
                 BattleResult<CARD_VALUE> battleResult = 
                    GameWar.Battle(currentRound.Values.First(),currentRound.Values.Last());
                if (!battleResult.IsEquality)
                {
                    if( currentRound.Values.First() == battleResult.CardValue) return currentRound.Keys.First();
                    return currentRound.Keys.Last();
                }
                return null;
            }
        }
    }

    public class BattleResult<CARD_VALUE>
    {
        public static BattleResult<CARD_VALUE> CreateWinner(CARD_VALUE winnerCard)
        {
            return new BattleResult<CARD_VALUE>(winnerCard);
        }
        public static BattleResult<CARD_VALUE> CreateEquality()
        {
            return new BattleResult<CARD_VALUE>();
        }

        public readonly CARD_VALUE CardValue;
        public bool IsEquality;
        private BattleResult(CARD_VALUE cardValue)
        {
            CardValue = cardValue;
            IsEquality = false;
        }
        private BattleResult()
        {
            IsEquality = true;
        }
    }

    public enum CARD_VALUE
    {
        ACE = 14,
        KING = 13,
        QUEEN = 12,
        JACK = 11,
        TEN = 10,
        NINE = 9,
        EIGHT = 8,
        SEVEN = 7,
        SIX = 6,
        FOUR = 4,
        THREE = 3,
        TWO = 2
    }
}