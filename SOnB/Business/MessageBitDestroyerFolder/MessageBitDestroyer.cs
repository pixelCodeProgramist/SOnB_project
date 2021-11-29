using System;
using System.Collections.Generic;
using System.Text;

namespace SOnB.Business.MessageBitDestroyerFolder
{
    class MessageBitDestroyer
    {
        private String message;

        public MessageBitDestroyer(string message)
        {
            this.message = message;
        }

        public String destroy()
        {
            StringBuilder messageStringBuilder = new StringBuilder(message);
            Random random = new Random();
            int numberOfChanges = random.Next(1, message.Length);
            List<int> positions = new List<int>();
            List<int> allPosibilitesPosition = new List<int>();
            fillAllPosibilities(ref allPosibilitesPosition);
            while (positions.Count != numberOfChanges)
            {
                int position = random.Next(0, allPosibilitesPosition.Count);
                positions.Add(allPosibilitesPosition[position]);
                allPosibilitesPosition.Remove(allPosibilitesPosition[position]);
            }
            for(int i = 0; i < positions.Count; i++)
            {
                if (messageStringBuilder[positions[i]] == '0') messageStringBuilder[positions[i]] = '1';
                else messageStringBuilder[positions[i]] = '0';
            }

            return messageStringBuilder.ToString();
        }

        public void fillAllPosibilities(ref List<int> allPosibilitesPosition)
        {
            for (int i = 0; i < message.Length; i++) allPosibilitesPosition.Add(i);
        }
    }
}
