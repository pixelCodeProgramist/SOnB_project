using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Client
{
   public class CRCAlgorithm
    {
        static string divaider = "11000000000000101";

        static string xor(string message, string divider, ref int pos, string output)
        {
            string tempOutput = "";
            for (int i = 0; i < divider.Length; i++)
            {
                if (divider[i] == output[i]) tempOutput += "0";
                else tempOutput += "1";
            }

            int zerosNumber = countZeroPrefix(tempOutput);
            if (output != divider)
                tempOutput = tempOutput.Substring(zerosNumber, tempOutput.Length - zerosNumber);
            if (pos != message.Length)
            {
                if ((pos + zerosNumber) <= message.Length)
                    tempOutput += message.Substring(pos, zerosNumber);
                else
                {
                    tempOutput += message.Substring(pos, message.Length - pos);
                }
            }
            pos += zerosNumber;
           // Console.WriteLine(tempOutput);
            return tempOutput;
        }

        static int countZeroPrefix(string str)
        {
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '0')
                    count++;
                else break;
            }
            return count;
        }

        public bool computeCRC(string message)
        {
            int pos = divaider.Length;
            string helpMessage = message.Substring(0, divaider.Length);
            while (pos <= message.Length)
                helpMessage = xor(message, divaider, ref pos, helpMessage);
          //  Console.WriteLine(helpMessage);
            if (helpMessage.Contains('1'))
                return false;
            else 
                return true;
        }
    }
}
