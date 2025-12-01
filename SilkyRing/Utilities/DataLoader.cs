using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SilkyRing.Models;
using SilkyRing.Properties;

namespace SilkyRing.Utilities
{
    public static class DataLoader
    {
        public static Dictionary<string, List<Grace>> GetGraces()
        {
            Dictionary<string, List<Grace>> graceDict = new Dictionary<string, List<Grace>>();
            string csvData = Resources.Graces;

            if (string.IsNullOrWhiteSpace(csvData))
                return graceDict;

            using (StringReader reader = new StringReader(csvData))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                    string[] parts = line.Split(',');
                    if (parts.Length < 5) continue;

                    bool isDlc = parts[0] == "1";
                    string mainArea = parts[1];
                    string name = parts[2];
                    long graceEntityId = long.Parse(parts[3], CultureInfo.InvariantCulture);
                    int flagId = int.Parse(parts[4], CultureInfo.InvariantCulture);

                    Grace grace = new Grace
                    {
                        IsDlc = isDlc,
                        MainArea = mainArea,
                        Name = name,
                        GraceEntityId = graceEntityId,
                        FlagId = flagId
                    };

                    if (!graceDict.ContainsKey(mainArea))
                    {
                        graceDict[mainArea] = new List<Grace>();
                    }

                    graceDict[mainArea].Add(grace);
                }
            }

            return graceDict;
        }

        public static List<Act> GetEbActs()
        {
            List<Act> ebActs = new List<Act>();
            string csvData = Resources.EbActs;
            if (string.IsNullOrWhiteSpace(csvData)) return ebActs;

            using StringReader reader = new StringReader(csvData);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                int actIdx = int.Parse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
                string name = parts[1];
                ebActs.Add(new Act(actIdx, name));
            }
            
            return ebActs;
        }
    }
}