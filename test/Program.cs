using System;

namespace test
{
    class Program
    {
        static string message = "11011101";
        static string divider = "100101";

        static void Main(string[] args)
        {
            message += "10101";
            computeCRC();
        }

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
                if (pos != message.Length) tempOutput += message.Substring(pos, zerosNumber);
                pos += zerosNumber;
            
           // Console.WriteLine(tempOutput);
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
            Console.WriteLine(helpMessage);

        }
    }
}
