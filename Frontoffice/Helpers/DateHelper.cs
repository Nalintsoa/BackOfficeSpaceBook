namespace Frontoffice.Helpers
{
    public static class DateHelper
    {
        public static List<string> GetFormattedDatesBetween(DateTime startDate, DateTime endDate)
        {
            var dates = new List<string>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date.ToString("yyyy-MM-dd"));
            }

            return dates;
        }
    }
}
