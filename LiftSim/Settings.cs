using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LiftSim
{
    public class SettingsStorage
    {
        public class SettingsType
        {
            // todo wrap as property
            public double FLOOR_Y_SIZE = 3.5; // meters
            public double LIFT_Y_SPEED = 1; // meters per sec
            public double MAX_POSITION_ERROR = 0.1; // meters
            public TimeSpan DOORS_OPENING_TIME = new TimeSpan(0, 0, 0, 0, 5000);
            public TimeSpan DOORS_WAIT_TIME = new TimeSpan(0, 0, 0, 0, 5000);
            public TimeSpan ENVIROMENT_STEP = new TimeSpan(0, 0, 0, 0, 10);
        }

        private static SettingsType _settings;

        public static SettingsType Settings => _settings;

        public static void Load(string filePath)
        {
            _settings = JsonConvert.DeserializeObject<SettingsType>(File.ReadAllText(filePath));
        }
    }
}