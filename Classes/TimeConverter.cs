using System;
using System.Text.RegularExpressions;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        //I've change method name to start with capital letter (to be consistend, as we are also using methods starting with capital letters in specflow tests).
        public string ConvertTime(string aTime)
        {
            if (!Regex.IsMatch(aTime, "^((0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9])|(24:00:00)$"))
                throw new FormatException("Input string is not in correct format (hh:mm:ss)");

            var timeParts = aTime.Split(':');
            var hours = short.Parse(timeParts[0]);
            var minutes = short.Parse(timeParts[1]);
            var seconds = short.Parse(timeParts[2]);

            return String.Format("{0}\n{1}\n{2}",
                                    ConvertSeconds(seconds),
                                    ConvertHours(hours),
                                    ConvertMinutes(minutes));
        }

        private string ConvertSeconds(short seconds)
        {
            var numberOfSetChars = seconds % 2;
            return LineWriter('O', numberOfSetChars, 1, 'Y');
        }

        private string ConvertHours(short hours)
        {
            var numberOfSetCharsInFirstRow = hours / 5;
            var numberOfSetCharsInSecondRow = hours % 5;

            return String.Format("{0}\n{1}", 
                                        LineWriter('R', numberOfSetCharsInFirstRow, 4), 
                                        LineWriter('R', numberOfSetCharsInSecondRow, 4));
        }

        private string ConvertMinutes(short minutes)
        {
            var numberOfSetCharsInFirstRow = minutes / 5;
            var numberOfSetCharsInSecondRow = minutes % 5;

            return String.Format("{0}\n{1}",
                                        LineWriter('Y', numberOfSetCharsInFirstRow, 11, 'O', 'R', 3),
                                        LineWriter('Y', numberOfSetCharsInSecondRow, 4));
        }

        private string LineWriter(char charToUse, int numberOfOccurrance, int lineLength, char charToComplement = 'O', char? alternateChar = null, int? alternateCharPosition = null)
        {
            var result = new char[lineLength];
            for (int i = 0; i < lineLength; i++)
            {
                if (i < numberOfOccurrance)
                {
                    if (alternateChar.HasValue && alternateCharPosition.HasValue && (i + 1) % alternateCharPosition.Value == 0)
                        result[i] = alternateChar.Value;
                    else 
                        result[i] = charToUse;
                }
                else
                    result[i] = charToComplement;
            }
            return new String(result);
        }
    }
}
