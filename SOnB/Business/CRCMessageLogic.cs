using System;

namespace SOnB.Business
{
    public class CRCMessageLogic
    {
        private String data;   
        private const String divider = "11000000000000101";

        public CRCMessageLogic(string data)
        {
            this.data = data;
            FillZero();
        }

        private void FillZero()
        {
            for (int i = 0; i < divider.Length-1; i++)
                this.data += "0";
        }

        private string Xor(string message, string divider, ref int pos, string output)
        {
            string tempOutput = "";
            for (int i = 0; i < divider.Length; i++)
            {
                if (divider[i] == output[i]) tempOutput += "0";
                else tempOutput += "1";
            }

            int zerosNumber = CountZeroPrefix(tempOutput);
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
            return tempOutput;
        }

        private int CountZeroPrefix(string str)
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

        public String ComputeCRC()
        {
            int pos = divider.Length;
            string helpMessage = this.data.Substring(0, divider.Length);
            while (pos <= this.data.Length)
                helpMessage = Xor(this.data, divider, ref pos, helpMessage);
            return helpMessage;
        }

        public String GetMessage()
        {
            String crc = ComputeCRC();
            String dataToModify = new String(data.ToCharArray());
            dataToModify = dataToModify.Substring(0,dataToModify.Length-crc.Length);
            dataToModify += crc;
            return dataToModify;
        }

        public int GetDividerLength()
        {
            return divider.Length;
        }
    }
}
