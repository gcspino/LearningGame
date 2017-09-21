using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LearningGame.Core
{
    public static class PlayerStateHelper
    {
        public static PlayerState ReadState(string fileName = "PlayerState.osl")
        {
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();

            PlayerState playerState = (PlayerState)bformatter.Deserialize(stream);
            stream.Close();
            return playerState;
        }

        public static void WriteState(PlayerState playerState, string fileName = "PlayerState.osl")
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            bformatter.Serialize(stream, playerState);
            stream.Close();
        }

        public static int GetReward(List<int> factors, string operatorType, int challenge, Combatant player, Combatant opponent, bool BossMode)
        {
            string officialOperatorType = operatorType == "x" ? "*" : operatorType;
            if (factors.Min() <= 1)
            {
                return 0;
            }

            double multiFactorBonus = officialOperatorType == "*" ? (factors.Count() - 1) * (int)(challenge / 2) : 0;

            double challengeBonus = challenge + (int)(0.4 * Math.Pow(challenge, 2));

            double manaBossFactor = BossMode ? 1 + (0.4 * challenge / 10) : player.CurrentMana / player.MaxMana;

            double hpRewardWeight = challenge;
            double hpFactor = challenge > 8 ? (player.CurrentHP + challenge / 10 * player.MaxHP) / player.MaxHP : 0;

            double operatorFactor = operatorType == "x" || operatorType == "*" ? 1 : operatorType == "-" ? 0.5 : 0.2;
            
            return (int)(operatorFactor * manaBossFactor * (multiFactorBonus + challengeBonus + hpRewardWeight * hpFactor));
        }
    }
}
