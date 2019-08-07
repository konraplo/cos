namespace Change.Intranet.Model
{
    /// <summary>
    /// Represents project task
    /// </summary>
    public class ProjectTask
    {
        /// <summary>
        /// Gets or sets the task id.
        /// </summary>
        /// <value>
        /// Task Id.
        /// </value>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the StoreOpeningTask flag.
        /// </summary>
        /// <value>
        /// true if this task is StoreOpeningTask.
        /// </value>
        public bool IsStoreOpeningTask
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Duration.
        /// </summary>
        /// <value>
        /// Task Duration.
        /// </value>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Responsible Department.
        /// </summary>
        /// <value>
        /// Responsible Department.
        /// </value>
        public string ResponsibleDepartment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Responsible .
        /// </summary>
        /// <value>
        /// Responsible person or Role
        /// </value>
        public string Responsible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the TimeBeforeGrandOpening.
        /// </summary>
        /// <value>
        /// Time ind Days
        /// </value>
        public int TimeBeforeGrandOpening
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Parent task id.
        /// </summary>
        /// <value>
        /// Parent Task Id.
        /// </value>
        public int ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent title.
        /// </summary>
        /// <value>
        /// The parent title.
        /// </value>
        public string ParentTitle
        {
            get;
            set;
        }
    }
}
