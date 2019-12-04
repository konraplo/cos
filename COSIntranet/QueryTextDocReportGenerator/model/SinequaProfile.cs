namespace QueryTextDocReportGenerator.model
{
    using System.Collections.Generic;

    public class SinequaProfile
    {
        public string Title { get; set; }
        public List<SinequaSearch> SearchItems { get; } = new List<SinequaSearch>();
        public List<SinequaSearch> GroupedSarchItems { get; } = new List<SinequaSearch>();
        public Dictionary<string, List<SinequaDcoument>> QueryDocsByText { get; } = new Dictionary<string, List<SinequaDcoument>>();

    }
}
