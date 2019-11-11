namespace QueryTextDocReportGenerator.model
{
    using System.Collections.Generic;

    public class SinequaProfile
    {
        public string Title { get; set; }
        public List<SinequaSearch> SearchItems { get; } = new List<SinequaSearch>();

    }
}
