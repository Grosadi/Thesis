using System;

namespace Database
{
    public class FingerprintModel
    {
        public int Id;
        public string TestName;
        public bool Succes;
        public DateTime Date;

        public FingerprintModel(int id,string testName, bool succes)
        {
            this.Id = id;
            this.TestName = testName;
            this.Succes = succes;
            this.Date = DateTime.Now;
        }
    }
}
