using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOnB.Business
{
    public class CRCMessageLogic
    {
        private String data;   
        private const String divider = "11000000000000101";

        public CRCMessageLogic(string data)
        {
            this.data = data;
            fillZero();
        }

        private void fillZero()
        {
            for (int i = 0; i < divider.Length-1; i++)
                this.data += "0";
        }

        private string xor(string message, string divider, ref int pos, string output)
        {
            string tempOutput = "";
            for (int i = 0; i < divider.Length; i++)
            {
                if (divider[i] == output[i]) tempOutput += "0";
                else tempOutput += "1";
            }

            int zerosNumber = countZeroPrefix(tempOutput);
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

        private int countZeroPrefix(string str)
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

        public String computeCRC()
        {
            int pos = divider.Length;
            string helpMessage = this.data.Substring(0, divider.Length);
            while (pos <= this.data.Length)
                helpMessage = xor(this.data, divider, ref pos, helpMessage);
            return helpMessage;
        }

        public String getMessage()
        {
            String crc = computeCRC();
            String dataToModify = new String(data.ToCharArray());
            dataToModify = dataToModify.Substring(0,dataToModify.Length-crc.Length);
            dataToModify += crc;
            return dataToModify;
        }

        public int getDividerLength()
        {
            return divider.Length;
        }

       


    }
}
