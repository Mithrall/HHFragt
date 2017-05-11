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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HHFragtUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Btn_gem(object sender, RoutedEventArgs e) {
            UdkastDato.Text = DatoInput.Text;
            UdkastType.Text = TypeInput.SelectedValue.ToString();
            UdkastLand.Text = LandInput.Text;
            UdkastPris.Text = PrisInput.Text;
            UdkastKommentar.Text = KommentarInput.Text;
            
        }
    }
}
