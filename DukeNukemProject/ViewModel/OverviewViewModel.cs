using System;
using System.Collections.Generic;
using DukeNukemProject.ViewModel.Commands;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DukeNukemProject.Model;
using DukeNukemProject.Model.Database;
using System.Windows.Input;
using DukeNukemProject.ViewModel.Resources;
using System.Windows.Threading;
using System.Globalization;



namespace DukeNukemProject.ViewModel
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        StoredProcedures sp;
        private SingleParameterCommand showOverviewCommand;
        private SingleParameterCommand deleteRowCommand;
        Overview overview;
        public string ReadyAtOfDeleted { get; set; }
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        private ObservableCollection<Overview>  overviews;
        public ObservableCollection<Overview>  Overviews
        {
            get { return overviews; }
            set
            {
                overviews = value;
                OnPropertyChanged("Duration");
                OnPropertyChanged("Overviews");
            }
        }

        public OverviewViewModel()
        {
            sp = new StoredProcedures();
            Overviews = new ObservableCollection<Overview>();
            showOverviewCommand = new SingleParameterCommand(showOverview, canShowOverview);
            deleteRowCommand = new SingleParameterCommand(deleteRow, canDeleteRow);
        }
        public ICommand ShowOverViewsOnClick
        {
            get { return showOverviewCommand; }
        }
        public void showOverview(object obj)
        {
            Overviews.Clear();
            Dictionary<string, string> targetParams = new Dictionary<string, string>();

            if (sp.MultiRowQuerySP("usp_GetOverview", targetParams))
            {
                foreach (var row in sp.DicResult)
                {
                    List<string> temp = new List<string>();
                    temp = row.Value;
                    string id = temp[0];
                    string name = temp[1];
                    string phone = temp[2];
                    string details = temp[3];
                    string readyAt = temp[4];
                    string purchaseType = temp[5];
                    string paymentType = temp[6];
                    string other = temp[7];
                    string totalPrice = temp[8];
                    overview = new Overview(
                        id, name, phone, details, readyAt, purchaseType, paymentType,
                        other, totalPrice);
                    Overviews.Add(overview);
                }
            }
        }

        public bool canShowOverview()
        {
            return true;
        }
        public ICommand DeleteRowOnClick
        {
            get { return deleteRowCommand; }
            
        }

        public void deleteRow(object obj)
        {
            int intTimeDifference = 0;
            var overview = obj as Overview;
            string id = overview.OrderId;
            Dictionary<string,string> targetParams = new Dictionary<string, string>();
            targetParams.Add("@deletedRowId", id);
            if (sp.QuerySP("usp_GetTimeDifference", targetParams))
            {
                intTimeDifference = SafeParser.Instance.StringToInt(sp.Result[0]);
                if(intTimeDifference == 0)
                {
                    targetParams = new Dictionary<string, string>();
                    targetParams.Add("@id", id);
                    sp.QuerySP("usp_GetReadyAtById", targetParams);
                    DateTime readyAt = SafeParser.Instance.StringToDateTime(sp.Result[0]);
                    string stringTimeNow = string.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", DateTime.Now);
                    DateTime timeNow = SafeParser.Instance.StringToDateTime(stringTimeNow);
                    TimeSpan difference = readyAt - timeNow;
                    double fractionalMinutes = difference.TotalMinutes;
                    intTimeDifference = (int)fractionalMinutes;
                    
                }
                TimeSpan timeDifference = new TimeSpan(0, 0, 0);
                if (intTimeDifference / 60 >= 1)
                {
                    timeDifference = new TimeSpan(0, intTimeDifference, 0);
                }
                else
                {
                    timeDifference = new TimeSpan(intTimeDifference / 60, intTimeDifference % 60, 0);
                }
                targetParams.Clear();
                targetParams.Add("@deletedRowId", id);
                sp.MultiRowQuerySP("usp_GetReadyAtAfterDeleted", targetParams);
                foreach (var element in sp.DicResult)
                {
                    DateTime readyAt = SafeParser.Instance.StringToDateTime(element.Value[0]);
                    DateTime updatedTime = readyAt.Subtract(timeDifference);
                    string stringCurrentTime = string.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", readyAt);
                    string stringUpdatedTime = string.Format(CultureInfo.InvariantCulture, "{0:dd\\/MM\\/yyyy HH:mm:ss}", updatedTime);
                    targetParams.Clear();
                    targetParams.Add("@currentTime", stringCurrentTime);
                    targetParams.Add("@updatedTime", stringUpdatedTime);
                    sp.NonQuerySP("usp_UpdateReadyAt", targetParams);
                }
            }
            targetParams = new Dictionary<string, string>();
            targetParams.Add("@itemSelectionItemId", "0");
            targetParams.Add("@customerOrderId", id);
            sp.NonQuerySP("usp_DeleteItemSelectionOrderByOrderId", targetParams);
        }
        public bool canDeleteRow()
        {
            return true;
        }
        #region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyname));
            }
        }
        #endregion
    }
}
