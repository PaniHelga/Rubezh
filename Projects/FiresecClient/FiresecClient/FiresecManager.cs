﻿using System.Collections.Generic;
using System.Linq;
using FiresecClient.Converters;
using FiresecClient.Models;

namespace FiresecClient
{
    public class FiresecManager
    {
        public static CurrentConfiguration Configuration { get; set; }
        public static CurrentStates States { get; set; }
        public static Firesec.CoreConfig.config CoreConfig { get; set; }

        public static bool Connect(string login, string password)
        {
            bool result = FiresecInternalClient.Connect(login, password);
            if (result)
            {
                _loggedInUserName = login;
                BuildDeviceTree();
                Configuration.Update();
                Watcher watcher = new Watcher();
                watcher.Start();

                ServiceManager.Open();
            }
            return result;
        }

        static string _loggedInUserName;
        public static User CurrentUser
        {
            get
            {
                return Configuration.Users.FirstOrDefault(x => x.Name == _loggedInUserName);
            }
        }

        public static List<Perimission> CurrentPermissions
        {
            get
            {
                List<string> permissionIds = new List<string>();

                foreach (var groupId in CurrentUser.Groups)
                {
                    var group = Configuration.UserGroups.FirstOrDefault(x => x.Id == groupId);

                    permissionIds.AddRange(group.Permissions);
                }
                permissionIds.AddRange(CurrentUser.Permissions);

                foreach (var permissionId in CurrentUser.RemovedPermissions)
                {
                    permissionIds.Remove(permissionId);
                }

                var permissions = new List<Perimission>(from permission in Configuration.Perimissions
                               where permissionIds.Contains(permission.Id)
                               select permission);
                return permissions;
            }
        }

        public static void Disconnect()
        {
            FiresecInternalClient.Disconnect();
        }

        static void BuildDeviceTree()
        {
            CoreConfig = FiresecInternalClient.GetCoreConfig();
            Configuration = new CurrentConfiguration();
            var metadata = FiresecInternalClient.GetMetaData();
            Configuration.FillDrivrs(metadata);
            Convert();
        }

        public static void SetNewConfig()
        {
            Validator validator = new Validator();
            validator.Validate(Configuration);
            ConvertBack();
            FiresecInternalClient.SetNewConfig(CoreConfig);
        }

        static void Convert()
        {
            FiresecManager.States = new CurrentStates();
            ZoneConverter.Convert(CoreConfig);
            DirectionConverter.Convert(CoreConfig);
            SecurityConverter.Convert(CoreConfig);
            DeviceConverter.Convert(CoreConfig);
        }

        static void ConvertBack()
        {
            ZoneConverter.ConvertBack(Configuration);
            DeviceConverter.ConvertBack(Configuration);
            DirectionConverter.ConvertBack(Configuration);
            SecurityConverter.ConvertBack(Configuration);
        }

        public static void LoadFromFile(string fileName)
        {
            CoreConfig = FiresecInternalClient.LoadConfigFromFile(fileName);
            FiresecManager.States = new CurrentStates();
            ZoneConverter.Convert(CoreConfig);
            DirectionConverter.Convert(CoreConfig);
            DeviceConverter.Convert(CoreConfig);
        }

        public static void SaveToFile(string fileName)
        {
            ZoneConverter.ConvertBack(Configuration);
            DeviceConverter.ConvertBack(Configuration);
            DirectionConverter.ConvertBack(Configuration);
            FiresecInternalClient.SaveConfigToFile(CoreConfig, fileName);
        }

        public static List<Firesec.ReadEvents.journalType> ReadJournal(int startIndex, int count)
        {
            Firesec.ReadEvents.document journal = FiresecInternalClient.ReadEvents(startIndex, count);
            if (journal.Journal != null)
            {
                if (journal.Journal.Count() > 0)
                {
                    return journal.Journal.ToList();
                }
            }
            return new List<Firesec.ReadEvents.journalType>();
        }
    }
}
