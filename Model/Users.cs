using System.ComponentModel;
using System.Runtime.CompilerServices;
using LibraryApp.Annotations;
using LibraryApp.ViewModel;
using SQLite;

namespace LibraryApp.Model
{
    class Users : INotifyPropertyChanged
    {
        public Users()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }
        private int _identityNo;
        public int IdentityNo
        {
            get => _identityNo;
            set
            {
                if (value == _identityNo)
                    return;

                _identityNo = value;
                OnPropertyChanged(nameof(IdentityNo));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                    return;

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                if (value == _surname)
                    return;

                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (value == _email)
                    return;

                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (value == _phone)
                    return;
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        [Ignore]
        public string UserDescription { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }  
}
 