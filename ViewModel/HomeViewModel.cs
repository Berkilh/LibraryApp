using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LibraryApp.Annotations;
using LibraryApp.Model;
using LiveCharts;
using LiveCharts.Wpf;

namespace LibraryApp.ViewModel
{
    class HomeViewModel : INotifyPropertyChanged
    {
        private Func<ChartPoint, string> _pointLabel;
        public Func<ChartPoint, string> PointLabel
        {
            get => _pointLabel;
            set
            {
                _pointLabel = value;
                OnPropertyChanged("PointLabel");
            }
        }
        public List<BookDelivery> ReservedBookList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private ObservableCollection<BookDelivery> _bookByDeliveryDate;
        public ObservableCollection<BookDelivery> BookByDeliveryDate
        {
            get => _bookByDeliveryDate;
            set
            {
                _bookByDeliveryDate = value;
                OnPropertyChanged("BookDelivery");
            }
        }
        private ObservableCollection<BookDelivery> _unDeliveryBooks;
        public ObservableCollection<BookDelivery> UnDeliveryBooks
        {
            get => _unDeliveryBooks;
            set
            {
                _unDeliveryBooks = value;
                OnPropertyChanged("UnDeliveryBooks");
            }
        }
         
        public SeriesCollection BookSeries
        {
            get
            {
                var book = DatabaseHelpers.Select<Books>("select *  from Books")
                    .ToList();
                var qty = book.Sum(x => x.AvailableQuantity);
                var resQty = book.Sum(x => x.Quantity) - book.Sum(x => x.AvailableQuantity);
                var seriesCollection = new SeriesCollection
                {
                    new PieSeries()
                    {
                        DataLabels = true,
                        Title = "Unreserved Book",
                        Values = new ChartValues<int>(new[] {qty}),
                        LabelPoint = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})"
                    },
                    new PieSeries()
                    {
                        DataLabels = true,
                        Title = "Reserved Book",
                        Values = new ChartValues<int>(new[] {resQty}),
                        LabelPoint = chartPoint =>  $"{chartPoint.Y} ({chartPoint.Participation:P})"
                    }
                };



                return seriesCollection;
            }
        }
        public SeriesCollection UserSeries
        {
            get
            {
                var users = DatabaseHelpers.Select<Books>("select *  from Users").ToList();

                var reservedUsers = ReservedBookList.Select(x => x.UserId).Distinct().Count();
                var seriesCollection = new SeriesCollection
                {
                    new PieSeries()
                    {
                        DataLabels = true,
                        Title = "Total Users",
                        Values = new ChartValues<int>(new[] {users.Count()}),
                        LabelPoint = chartPoint => $"{chartPoint.Y} ({chartPoint.Participation:P})"
                    },
                    new PieSeries()
                    {
                        DataLabels = true,
                        Title = "Book Reserved Users",
                        Values = new ChartValues<int>(new[] {reservedUsers}),
                        LabelPoint = chartPoint =>  $"{chartPoint.Y} ({chartPoint.Participation:P})"
                    }
                };



                return seriesCollection;
            }
        }
        public HomeViewModel()
        { 
            LoadReservedBookData();
            BookByDeliveryDate = new ObservableCollection<BookDelivery>();
            UnDeliveryBooks = new ObservableCollection<BookDelivery>();
            LoadBookByDeliveryDate();
            LoadUnDeliveryBooks();
            
        }

        private void LoadReservedBookData()
        {
             ReservedBookList = DatabaseHelpers.Select<BookDelivery>("Select r.ReservationId , r.UserId , r.BookId ,r.ReturnDate , " +
                                                                    " u.Name  , u.Surname, " +
                                                                    " b.BookName as BookName ,  b.Author as Author , " +
                                                                    " b.Isbn as Isbn from Reservations r " +
                                                                    " left join  Books b on  r.BookId = b.BookId " +
                                                                    " left join Users u on r.UserId = u.UserId ").ToList();

        } 

        private void LoadUnDeliveryBooks()
        { 
            ReservedBookList.Where(x => DateTime.Compare(x.ReturnDate.Date, DateTime.Today) < 0).OrderByDescending(x => x.ReturnDate).ToList()
                .ForEach(x => UnDeliveryBooks.Add(
                    new BookDelivery()
                    {
                        FullName = x.Name + " " + x.Surname,
                        BookName = x.BookName,
                        Author = x.Author,
                        ReturnDate = x.ReturnDate

                    }));

        } 
        private void LoadBookByDeliveryDate()
        { 
            ReservedBookList.Where(x => DateTime.Compare(x.ReturnDate.Date,DateTime.Today) >= 0).OrderByDescending(x => x.ReturnDate).ToList()
                .ForEach(x => BookByDeliveryDate.Add(
                    new BookDelivery()
                    {
                        FullName = x.Name + " " + x.Surname,
                        BookName = x.BookName,
                        Author = x.Author,
                        ReturnDate = x.ReturnDate 
                    }));
        }
      
    }
}
