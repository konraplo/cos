namespace QueryTextDocReportGenerator.model
{
    using System.Collections.Generic;
    public class SinequaSearch
    {
        public string QueryText { get; set; }
        public string ResultId { get; set; }
        public int ItemCount { get; set; }

        public List<SinequaDcoument> DocumentItems { get; } = new List<SinequaDcoument>();

    }
}
