using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace LibraryApp.Model
{
    public class Reservations
    {
        [PrimaryKey, AutoIncrement]
        public int ReservationId { get; set; } 
        public int UserId { get; set; } 
        public int BookId { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }

    public class ReservationByUser
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
