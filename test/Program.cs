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

        private static string Xor(string message, ref int position, string input)
        {
            string output = "";
            for (int i = 0; i < _divider.Length; i++)
            {
                if (IsBitsEqual(_divider[i], input[i])) output += "0";
                else output += "1";
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

        private static bool IsBitsEqual(char firstBit, char secondBit)
        {
            return firstBit == secondBit;
        }

        private static int CountZeroPrefix(string result)
        {
            int count = 0;
            for (int index = 0; index < result.Length; index++)
            {
                if (result[index] == '0')
                    count++;
                else break;
            }
            return count;
        }

        public static bool ComputeCRC(string polynomial)
        {
            int position = _divider.Length;
            string polynomialPart = polynomial.Substring(0, _divider.Length);
            while (!IsDividingEnded(position, polynomial.Length))
                polynomialPart = Xor(polynomial, ref position, polynomialPart);
            //  Console.WriteLine(helpMessage);
            if (IsResultCorrect(polynomialPart))
                return true;
            else
                return false;
        }

        private static bool IsDividingEnded(int position, int polynomialLength)
        {
            return position > polynomialLength;
        }

        private static bool IsResultCorrect(string result)
        {
            return !result.Contains('1');
        }

    }
}
