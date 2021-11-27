using System;

namespace test
{
    class Program
    {
        private static string message = "00000111101010101011111111111111111001";
        private static string _divider = "11000000000000101";

        static void Main(string[] args)
        {
            Console.WriteLine(ComputeCRC(message));
            Console.ReadKey();
        }

        private static string Xor(string message, ref int position, string input)
        {
            string output = "";
            for (int i = 0; i < _divider.Length; i++)
            {
                if (IsBitsEqual(_divider[i], input[i])) output += "0";
                else output += "1";
            }

            int zerosNumber = CountZeroPrefix(output);
            output = output.Substring(zerosNumber);
            if (!IsDividingEnded(position + zerosNumber, message.Length))
                output += message.Substring(position, zerosNumber);
            else
                output += message.Substring(position, message.Length - position);
            position += zerosNumber;
            // Console.WriteLine(tempOutput);
            return output;
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
