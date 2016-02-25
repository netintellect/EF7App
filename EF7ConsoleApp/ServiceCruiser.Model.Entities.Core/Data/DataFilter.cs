using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Data
{
    /// <summary>
    /// Base class for filtering criteria
    /// used by most data access READ methods
    /// </summary>
    public class DataFilter : NotificationObject
    {
        #region state

        private int? _filteringId;
        public int? FilteringId
        {
            get { return _filteringId; }
            set
            {
                _filteringId = value;
                OnPropertyChanged(() => FilteringId);
            }
        }

        private List<int> _filteringIds = new List<int>();
        public List<int> FilteringIds
        {
            get { return _filteringIds; }
            set { _filteringIds = value; }
        }

        private string _sortColumn;
        public string SortColumn
        {
            get { return _sortColumn; }
            set { _sortColumn = value; }
        }

        private SortDirectionType _sortDirection = SortDirectionType.Ascending;
        public SortDirectionType SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        private Paging _paging = new Paging();
        public Paging Paging
        {
            get { return _paging; }
            set { _paging = value; }
        }

        private bool? _invertSortDirection;

        public bool? InvertSortDirection
        {
            get { return _invertSortDirection; }
            set { _invertSortDirection = value; }
        }

        public LogicalOperatorType LogicalOperator { get; set; }
        #endregion

        #region behavior
        public int ItemsToSkip
        {
            get
            {
                if (Paging.Page != 0 && Paging.PageSize != 0)
                {
                    return (Paging.Page) * Paging.PageSize;
                }
                return 0;
            }
        }

        public int ItemsToTake
        {
            get
            {
                if (Paging.PageSize != 0)
                {
                    return Paging.PageSize;
                }
                return 100;
            }
        }

        public bool TakeAllItems { get; set; }

        public override string ToString()
        {
            if (FilteringIds == null) return base.ToString();
            return string.Format("IDs = {0}", string.Join(", ", FilteringIds));
        }

        public virtual void Reset()
        {
            FilteringId = null;
            FilteringIds = null;
        }

        public virtual bool IsEmpty
        {
            get { return false; }
        }

        #endregion
    }
}
