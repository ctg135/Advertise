﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertise.Models
{
    public class View : IModel
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
