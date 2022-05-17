using System;
using Thesis.Services;

namespace ExportData
{
    class Program
    {
        static void Main(string[] args)
        {
            var fingerprints = DatabaseService.GetFingerprintsFromDbAsync().Result;
            var faceIds = DatabaseService.GetFaceIdsFromDbAsync().Result;

            for (int i = 0; i < fingerprints.Length; i++)
            {
                Console.WriteLine(fingerprints[i]);
            }

            for (int i = 0; i < faceIds.Length; i++)
            {
                Console.WriteLine(faceIds[i]);
            }

            Console.ReadLine();
        }
    }
}
