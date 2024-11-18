using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiweessOS.Commands
{
    public class CalCommand : ICommand
    {
        public string Execute(string[] args)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            if (args.Length == 1)
            {
                if (int.TryParse(args[0], out month) && month >= 1 && month <= 12)
                {
                    year = DateTime.Now.Year;
                }
                else
                {
                    return "Invalid month.";
                }
            }
            else if (args.Length == 2)
            {
                if (int.TryParse(args[0], out month) && int.TryParse(args[1], out year)
                    && month >= 1 && month <= 12 && year >= 1)
                {
                }
                else
                {
                    return "Invalid arguments. Usage: cal [month] [year]";
                }
            }
            else if (args.Length > 2)
            {
                return "Invalid number of arguments. Usage: cal [month] [year]";
            }

            var calendar = new GregorianCalendar();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);

            var monthYearText = firstDayOfMonth.ToString("MMMM yyyy", CultureInfo.InvariantCulture);
            int totalWidth = 22;  
            int spaces = (totalWidth - monthYearText.Length) / 2; 

            var result = new string(' ', spaces) + monthYearText + "\n"; result += "Su Mo Tu We Th Fr Sa\n";

            int dayOfWeek = (int)firstDayOfMonth.DayOfWeek; 

            result += new string(' ', 3 * dayOfWeek);

            for (int day = 1; day <= daysInMonth; day++)
            {
                result += (day < 10 ? day.ToString(" #") : day.ToString("D2")) + " ";
                dayOfWeek++;

                if (dayOfWeek > 6)
                {
                    result += "\n";
                    dayOfWeek = 0;
                }
            }

            return result.TrimEnd();
        }

        public string GetHelp()
        {
            return "Usage:\n" +
                   "  cal            Show current month calendar\n" +
                   "  cal [month]     Show calendar for specified month\n" +
                   "  cal [month] [year] Show calendar for specified month and year\n";
        }
    }
}
