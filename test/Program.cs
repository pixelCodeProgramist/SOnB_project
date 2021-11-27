using System;

namespace test
{
    class Program
    {
        static string message = "110100101010010101";
        static string divider = "11000000000000101";

        static void Main(string[] args)
        {
            message += "0000000000000000";
            computeCRC();
        }

        static string xor(string message, string divider,ref int pos, string output)
        {
            string tempOutput = "";
            for(int i = 0; i < divider.Length; i++)
            {
                if (divider[i] == output[i]) tempOutput += "0";
                else tempOutput += "1";
            }
            
            int zerosNumber = countZeroPrefix(tempOutput);
            tempOutput = tempOutput.Substring(zerosNumber, tempOutput.Length-zerosNumber);
            if (pos != message.Length)
            {
                if((pos+zerosNumber) <= message.Length)
                    tempOutput += message.Substring(pos, zerosNumber);
                else
                {
                    tempOutput += message.Substring(pos, message.Length-pos);
                }
            }
            pos += zerosNumber;
            Console.WriteLine(tempOutput);
            return tempOutput;
        }

        static int countZeroPrefix(string str)
        {
            int count = 0;
            for(int i = 0; i < str.Length; i++)
            {
                if (str[i]=='0') 
                    count++;
                else break;
            }
            return count;
        }

        static void computeCRC()
        {
            int pos = divider.Length;
            string helpMessage = message.Substring(0, divider.Length);
            while (pos <= message.Length)
               helpMessage = xor(message, divider, ref pos, helpMessage);

        }
    }
}
