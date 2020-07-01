using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LibraryApp.Annotations;
using SQLite;

namespace LibraryApp.Model
{
    class Books : INotifyPropertyChanged
    {   
        




        [PrimaryKey, AutoIncrement] public int BookId { get; set; }

        private string _bookName;

        public string BookName
        {
            get => _bookName;
            set
            {
                if (value == _bookName)
                    return;

                _bookName = value;
                OnPropertyChanged(nameof(BookName));
            }
        }

        private string _author;

        public string Author
        {
            get => _author;
            set
            {
                if (value == _author)
                    return;

                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _isbn;

        public string Isbn
        {
            get => _isbn;
            set
            {
                if (value == _isbn)
                    return;

                _isbn = value;
                OnPropertyChanged(nameof(Isbn));
            }
        }

        private int _quantity;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value == _quantity)
                    return;
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private int _availableQuantity;

        public int AvailableQuantity
        {
            get => _availableQuantity;
            set
            {
                if (value == _availableQuantity)
                    return;
                _availableQuantity = value;
                OnPropertyChanged(nameof(AvailableQuantity));
            }
        }

        [Ignore]
        public string BookDescription { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
