﻿namespace Infrastructure
{
    public class AppSettings
    {
        public string ServiceAddress { get; set; }
        public string DefaultLogin { get; set; }
        public string DefaultPassword { get; set; }
        public bool AutoConnect { get; set; }
        public string LibVlcDllsPath { get; set; }
        public bool ShowGC { get; set; }
        public bool IsDebug { get; set; }
    }
}