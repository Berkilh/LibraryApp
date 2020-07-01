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
    class BookViewModel : INotifyPropertyChanged
    {
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
        private void OnPropertyChanged(string v)
        {
            BooksInfo.Clear();
            BooksInfo.Add(new Books
            {
                Author   = "",
                BookName = "",
                Isbn = "",
                Quantity = 0,
                AvailableQuantity = 0,
                BookId = 0
            });
            DatabaseHelpers.Select<Books>("Select * from Books").ToList().OrderByDescending(x => x.BookId).ToList().
                ForEach(x => BooksInfo.Add(new Books
                {
                    Author = x.Author,
                    BookName = x.BookName,
                    Isbn = x.Isbn,
                    Quantity = x.Quantity,
                    AvailableQuantity = x.AvailableQuantity,
                    BookId = x.BookId
                }));

        } 
        private ICommand _saveCommand { get; set; }
        private ICommand _deleteCommand { get; set; }
        private bool _canExecute = true;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public ICommand SaveCommand
        {
            get => _saveCommand;
            set => _saveCommand = value;
        }
      
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => _deleteCommand = value;
        }
        public BookViewModel()
        {
            BooksInfo = new ObservableCollection<Books>();
            
            SaveCommand = new RelayCommand(SaveUpdate, param => this.CanExecute);
            DeleteCommand = new RelayCommand(Remove, param => this.CanExecute);
        }
        private void SaveUpdate(object parameter)
        {
            if (!(parameter is Books bk)) return;
            if (bk.BookId == 0)
            {
                bk.AvailableQuantity = bk.Quantity;
                DatabaseHelpers.Insert(bk);
            }
            else
            {
                bk.AvailableQuantity = bk.Quantity;
                DatabaseHelpers.Update(bk);
            }

            OnPropertyChanged("BooksInfo");


        }
        private void Remove(object parameter)
        {
            if (!(parameter is Books bk)) return;
            if (bk.BookId == 0) return;

            DatabaseHelpers.Delete(bk);
            OnPropertyChanged("UsersInfo");

        }
    }
}
