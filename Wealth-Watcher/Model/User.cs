﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Devices;

namespace Wealth_Watcher.Model
{
    public class User
    {
        public int userId { get; set; }

        public string userName { get; set; }
        public string password { get; set; }
        public string currencyType { get; set; }
        public string gender { get; set; }
        public decimal balance { get; set; }
    }
}
