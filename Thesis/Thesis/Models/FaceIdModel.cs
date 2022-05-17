using SQLite;
using System;

namespace Thesis.Models
{
    public class FaceIdModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string PersonName { get; set; } 
        public string TestName { get; set; }
        public bool Succes { get; set; }
        public DateTime Date { get; set; }

        public FaceIdModel()
        {                     
        }
    }
}
