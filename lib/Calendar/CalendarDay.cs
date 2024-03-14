namespace timeManager
{
    public class CalendarDay
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public CalendarDay(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }
    }
}