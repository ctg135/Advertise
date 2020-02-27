﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Advertise.ReportGenerators;
using Advertise.ReportGenerators.Models;

namespace Advertise.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public DataBaseWorker DB { get; private set; }
        public ReportWindow(DataBaseWorker dataBaseWorker, ExcelWorker excelWorker)
        {
            InitializeComponent();
            DB = dataBaseWorker;
        }
    }
}
