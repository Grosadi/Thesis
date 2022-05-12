using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class FaceIdModel
    {
        public int Id;
        public string PersonName;
        public string TestName;
        public bool Succes;
        public DateTime Date;

        public FaceIdModel(int id, string personName, string testName, bool succes)
        {
            this.Id = id;
            this.PersonName = personName;
            this.TestName = testName;
            this.Succes = succes;
            this.Date = DateTime.Now;
        }
    }
}
