namespace Change.Intranet.Projects
{
    using Change.Intranet.Model;
    using System.Collections.Generic;

    /// <summary>
    /// Helpermethods with project related methods and functions.
    /// </summary>
    public class ProjectUtilities
    {
        /// <summary>
        /// Regional manager
        /// </summary>
        private const string RegionalManager = "Regional manager";

        /// <summary>
        /// Storedesign
        /// </summary>
        private const string Storedesign = "Storedesign";

        /// <summary>
        /// Create project opening tasks List
        /// </summary>
        /// <returns>Lists with all project opening tasks</returns>
        public static List<ProjectTask> CreateStoreOpeningTasks()
        {
            List<ProjectTask> tasks = new List<ProjectTask>();
            tasks.Add(new ProjectTask { Title = "Location search end (get DWG drawing, take pictures, premise condition at takeover)", Duration = 2, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Store design location visit (measurements etc.)", Duration = 1, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Initial building/renovation budget", Duration = 2, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Initial P/L signed", Duration = 1, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Premise contract signed", Duration = 4, Responsible = RegionalManager });
            tasks.Add(new ProjectTask { Title = "Drawings begin", Duration = 1, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Drawings finish", Duration = 2, Responsible = Storedesign });
            tasks.Add(new ProjectTask { Title = "Drawings approved", Duration = 2, Responsible = RegionalManager });
            return tasks;
        }
    }
}
