using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpensePortal.Helpers
{
   
        public static class DateTimeHelper
        {
            /// <summary>
            /// Offsets to move the day of the year on a week, allowing
            /// for the current year Jan 1st day of week, and the Sun/Mon 
            /// week start difference between ISO 8601 and Microsoft
            /// </summary>
            private static int[] moveByDays = { 6, 7, 8, 9, 10, 4, 5 };
            /// <summary>
            /// Get the Week number of the year
            /// (In the range 1..53)
            /// This conforms to ISO 8601 specification for week number.
            /// </summary>
            /// <param name="date"></param>
            /// <returns>Week of year</returns>
            public static int WeekOfYear(this DateTime date)
            {
                DateTime startOfYear;
                DateTime endOfYear;
                if (date.Month<=3)
                {
                     startOfYear = new DateTime(date.Year -1, 4, 6);
                     endOfYear = new DateTime(date.Year , 4, 5);
                }
                else
                {
                     startOfYear = new DateTime(date.Year, 4, 6);
                     endOfYear = new DateTime(date.Year + 1, 4, 5);
                }
                
                // ISO 8601 weeks start with Monday 
                // The first week of a year includes the first Thursday 
                // This means that Jan 1st could be in week 51, 52, or 53 of the previous year...
                int numberDays = date.Subtract(startOfYear).Days +
                                moveByDays[(int)startOfYear.DayOfWeek];
                int weekNumber = numberDays / 7;
                switch (weekNumber)
                {
                    case 0:
                        // Before start of first week of this year - in last week of previous year
                        weekNumber = WeekOfYear(startOfYear.AddDays(-1));
                        break;
                    case 53:
                        // In first week of next year.
                        if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
                        {
                            weekNumber = 1;
                        }
                        break;
                }
               
                return weekNumber;
            }
            /// <summary>
            /// Finds the next date whose day of the week equals the specified day of the week.
            /// </summary>
            /// <param name="startDate">
            /// The date to begin the search.
            /// </param>
            /// <param name="desiredDay">
            /// The desired day of the week whose date will be returneed.
            /// </param>
            /// <returns>
            /// The returned date is on the given day of this week.
            /// If the given day is before given date, the date for the
            /// following week's desired day is returned.
            /// </returns>
            public static DateTime GetWeekPayDay(this DateTime startDate, DayOfWeek desiredDay = DayOfWeek.Saturday)
            {

                // (There has to be a better way to do this, perhaps mathematically.)
                // Traverse this week
                DateTime nextDate = startDate;
                while (nextDate.DayOfWeek != desiredDay)
                    nextDate = nextDate.AddDays(1D);

                return nextDate;
            }

        }


 
}