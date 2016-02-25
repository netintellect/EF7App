
namespace ServiceCruiser.Model.Entities.Core.Data
{
    public class Paging : NotificationObject
    {
        #region state

        private int _page;

        public int Page
        {
            get { return _page;  }
            set
            {
                _page = value;
                OnPropertyChanged(() => Page);
            }
        }

        public string SelectedPageSize { get; set; }
        public int PageSize
        {
            get
            {
                int pageSize;

                if (!int.TryParse(SelectedPageSize, out pageSize)) return 0;

                return pageSize;
            }
        }

        private int _totalItems;
        public int TotalItems
        {
            get { return _totalItems; }
            set
            {
                _totalItems = value;
                OnPropertyChanged(()=>TotalItems);
            }
        }

        public int TotalPages
        {
            get
            {
                if (TotalItems < PageSize) return 1;

                return (TotalItems / PageSize) + ((TotalItems % PageSize) > 0 ? 1 : 0);
            }
        }

        public int FromItem
        {
            get
            {
                if (Page == 0) return 1;

                return (Page * PageSize) + 1;
            }
        }

        public int ToItem
        {
            get
            {
                if (Page == 0 && TotalItems < PageSize) return TotalItems;

                if (Page == TotalPages) return TotalItems;

                return (Page * PageSize);
            }
        } 
        #endregion

        #region behavior
        public Paging()
        {
            Page = 0;
            SelectedPageSize = "10";
            TotalItems = 0;
        }

        public void RefreshState()
        {
            OnPropertyChanged(() => PageSize);
            OnPropertyChanged(() => Page);
            OnPropertyChanged(() => TotalItems);
        }
        #endregion
    }
}