using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DBContext : DbContext
    {        
        public DbSet<FingerprintModel> Fingerprints { get; set; }
        public DbSet<FaceIdModel> FaceIds { get; set; }

        public DBContext()
        {
            
        }

        public void AddFingerprint(string testName, bool succes)
        {
            var id = Fingerprints.Count() + 1;
            var entity = new FingerprintModel(id, testName, succes);

            this.Fingerprints.Add(entity);

            this.SaveChanges();
        }

        public void AddFaceId(string personName, string testName, bool succes)
        {
            var id = FaceIds.Count() + 1;
            var entity = new FaceIdModel(id, personName, testName, succes);

            this.FaceIds.Add(entity);

            this.SaveChanges();
        }
    }


}
