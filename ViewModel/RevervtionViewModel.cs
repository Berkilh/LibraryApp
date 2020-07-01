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

namespace LibraryApp.ViewModel
{
    class RevervtionViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<Users> _usersInfo;
        public ObservableCollection<Users> UsersInfo
        {
            get => _usersInfo;
            set
            {
                _usersInfo = value;
                OnPropertyChanged("UsersInfo");
            }
        }
        private ObservableCollection<ReservationByUser> _reservationByUser;
        public ObservableCollection<ReservationByUser> ReservationByUser
        {
            get => _reservationByUser;
            set
            {
                _reservationByUser = value;
                OnPropertyChanged("ReservationByUser");
            }
        }

        private Users _selectedUsers;
        public Users SelectedUser
        {
            get => _selectedUsers;
            set
            {

                _selectedUsers = value;
                LoadGrid();
            }
        }
        public Books SelectedBook { get; set; }
        public DateTime SelectedDate { get; set; }
        private bool _canExecute = true;

        private ObservableCollection<Books> _bookInfo;
        public ObservableCollection<Books> BooksInfo
        {
            get => _bookInfo;
            set
            {
                _bookInfo = value;
                OnPropertyChanged("BooksInfo");
            }
        }

        public RevervtionViewModel()
        {

            BooksInfo = new ObservableCollection<Books>();
            UsersInfo = new ObservableCollection<Users>();
            ReservationByUser = new ObservableCollection<ReservationByUser>();
            SelectedDate = DateTime.Today;
            SaveCommand = new RelayCommand(Save, param => CanExecute);
            ReturnBook = new RelayCommand(BookReturn, param => CanExecute);
            LoadBooksInfo();
            LoadUsersInfo();

        }
        private void LoadUsersInfo()
        {
            DatabaseHelpers.Select<Users>("Select * from Users").ToList().OrderByDescending(x => x.UserId).ToList()
                .ForEach(x => UsersInfo.Add(new Users
                {
                    UserDescription = x.Name + " " + x.Surname + " - " + x.IdentityNo,
                    Email = x.Email,
                    Name = x.Name,
                    Phone = x.Phone,
                    IdentityNo = x.IdentityNo,
                    Surname = x.Surname,
                    UserId = x.UserId
                }));
        }
        private void LoadBooksInfo()
        {
            BooksInfo.Clear();
            DatabaseHelpers.Select<Books>("Select * from Books").Where(x => x.AvailableQuantity > 0).ToList().OrderByDescending(x => x.BookId).ToList()
                .ForEach(x => BooksInfo.Add(new Books
                {
                    BookDescription = x.BookName + " - " + x.Isbn + " - " + x.Author,
                    Author = x.Author,
                    BookName = x.BookName,
                    Isbn = x.Isbn,
                    Quantity = x.Quantity,
                    AvailableQuantity = x.AvailableQuantity,
                    BookId = x.BookId
                }));
        }
        public ICommand _returnBook { get; set; }
        public ICommand ReturnBook
        {
            get => _returnBook;
            set => _returnBook = value;
        }
        private ICommand _saveCommand { get; set; }
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => _saveCommand = value;
        }
        public bool CanExecute
        {
            get => _canExecute;
            set
            {
                if (_canExecute.Equals(value))
                    return;
                _canExecute = value;
            }
        }
        private void Save(object parameter)
        {
            var res = new Reservations
            {
                ReservationDate = DateTime.Today,
                BookId = SelectedBook.BookId,
                UserId = SelectedUser.UserId,
                ReturnDate = SelectedDate
            };
            DatabaseHelpers.Insert(res);
            var book = BooksInfo.FirstOrDefault(x => x.BookId == res.BookId);
            if (book != null)
            {
                book.AvailableQuantity -= book.AvailableQuantity;
                DatabaseHelpers.Update(book);
            }
            LoadGrid();
            LoadBooksInfo();
        }
        private void BookReturn(object parameter)
        {
            if (!(parameter is ReservationByUser bookReturn)) return;
            var res = new Reservations
            {
                BookId = bookReturn.BookId,
                ReservationId = bookReturn.ReservationId,
                UserId = bookReturn.UserId
            };
            DatabaseHelpers.Delete(res);
            var book = DatabaseHelpers.Select<Books>("Select * from Books where BookId = " + bookReturn.BookId + " ").FirstOrDefault();
            if (book == null) return;
            book.AvailableQuantity = book.AvailableQuantity + 1;
            DatabaseHelpers.Update(book);
            LoadGrid();
            LoadBooksInfo();
        }
        private void LoadGrid()
        {
            ReservationByUser.Clear();
            var res = DatabaseHelpers.Select<ReservationByUser>("Select r.ReservationId , r.UserId , r.BookId ,r.ReturnDate , " +
                                                                " u.Name  , u.Surname, " +
                                                                " b.BookName as BookName ,  b.Author as Author , " +
                                                                " b.Isbn as Isbn from Reservations r " +
                                                                " left join  Books b on  r.BookId = b.BookId " +
                                                                " left join Users u on r.UserId = u.UserId " +
                                                                " where r.UserId = " + SelectedUser.UserId + " ").ToList();
            foreach (var x in res)
            {
                x.FullName = x.Name + " " + x.SurName;
                ReservationByUser.Add(x);

            }

        }
    }
}
