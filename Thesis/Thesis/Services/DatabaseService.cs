using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Thesis.Models;
using Xamarin.Essentials;

namespace Thesis.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection db;
        private static async Task Init()
        {
            if(db != null)
            {
                return;
            }
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "Data.db");

            db = new SQLiteAsyncConnection(dbPath);

            await db.CreateTableAsync<FingerprintModel>();
            await db.CreateTableAsync<FaceIdModel>();
        }

        public static async Task AddFingerprint(string testName, bool succes)
        {
            await Init();

            var entity = new FingerprintModel()
            {
                TestName = testName,
                Succes = succes,
                Date = DateTime.Now
            };

            await db.InsertAsync(entity);

           // await CreateOutputFile();
        }

        public static async Task AddFaceId(string personName, string testName, bool succes)
        {
            await Init();            

            var entity = new FaceIdModel()
            {
                PersonName = personName,
                TestName = testName,
                Succes = succes,
                Date = DateTime.Now
            };
            
            await db.InsertAsync(entity);            
        }

        public static async Task CreateOutputFile()
        {
            await Init();                        

            var fingerprintCount = await db.Table<FingerprintModel>().CountAsync();
            var faceIdCount = await db.Table<FaceIdModel>().CountAsync();

            var fingerprints = await GetFingerprintsFromDbAsync();
            var faceIds = await GetFaceIdsFromDbAsync();

            ExportToCSVFile(fingerprints, faceIds);
        }

        public static async Task<string[]> GetFingerprintsFromDbAsync()
        {            
            await Init();

            var fingerprints = await db.Table<FingerprintModel>().ToListAsync();
            var fingerprintArray = new string[fingerprints.Count];

            for (int i = 0; i < fingerprintArray.Length; i++)
            {
                fingerprintArray[i] = FingerprintModelToString(fingerprints[i]);
            }

            return fingerprintArray;
        }

        public static async Task<string[]> GetFaceIdsFromDbAsync()
        {
            await Init();

            var faceIds = await db.Table<FaceIdModel>().ToListAsync();
            var faceIdArray = new string[faceIds.Count];

            for (int i = 0; i < faceIdArray.Length; i++)
            {
                faceIdArray[i] = FaceIdModelToString(faceIds[i]);
            }

            return faceIdArray;
        }

        public static async Task DeleteAllDataFromDb()
        {
            await db.DeleteAllAsync<FingerprintModel>();
            await db.DeleteAllAsync<FaceIdModel>();
        }

        private static string FingerprintModelToString(FingerprintModel fingerprint)
        {            
            return $"{fingerprint.Id};{fingerprint.TestName};{fingerprint.Succes};{fingerprint.Date}";
        }

        private static string FaceIdModelToString(FaceIdModel faceId)
        {
            return $"{faceId.Id};{faceId.TestName};{faceId.PersonName};{faceId.Succes};{faceId.Date}";
        }

        private static void ExportToCSVFile(string[] fingerprints, string[] faceIds)
        {
            StreamWriter fingerprintStream = new StreamWriter($"{FileSystem.AppDataDirectory}/fingerprint_export.txt");
            StreamWriter faceIdStream = new StreamWriter($"{FileSystem.AppDataDirectory}/faceId_export.txt");

            for (int i = 0; i < fingerprints.Length; i++)
            {
                fingerprintStream.WriteLine(fingerprints[i]);
            }
            fingerprintStream.Close();

            for (int i = 0; i < faceIds.Length; i++)
            {
                faceIdStream.WriteLine(faceIds[i]);
            }
            faceIdStream.Close();

            var msg = new EmailMessage()
            {
                Attachments = new List<EmailAttachment>()
                {
                    new EmailAttachment($"{FileSystem.AppDataDirectory}/fingerprint_export.txt"),
                    new EmailAttachment($"{FileSystem.AppDataDirectory}/faceId_export.txt")
                }
            };

            Task t = Email.ComposeAsync(msg);

            t.Wait();
        }
    }
}
