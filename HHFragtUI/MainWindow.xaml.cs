﻿using System;
using System.Collections.Generic;
using System.Windows;
using FragtSource;
using HHFragtUI.Assets;
using System.Windows.Controls;

namespace HHFragtUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:Window {
        public List<Package> packageList = new List<Package>();
        public PackageListController PLC = new PackageListController();
        FragtUIDependencies totalsData = new FragtUIDependencies();

        public int GLSCalculated, MOmdelingCalculated, UOmdelingCalculated, BrevCalculated;


        public MainWindow()
        {
            packageList = FetchPackageListFromController();

            InitializeComponent();

            packageDatagrid.ItemsSource = packageList;

            this.DataContext = totalsData;
            CalculateTotals();
        }

        private void Btn_gem(object sender, RoutedEventArgs e)
        {
            try
            {
                Package tempPackage = new Package()
                {
                    Date = Convert.ToDateTime(DatoInput.Text),
                    Type = UdkastType.Text,
                    Country = UdkastCountry.Text,
                    Price = UdkastPrice.Text,
                    Comment = UdkastComment.Text
                };
                PLC.AddPackageToList(tempPackage);
                LoadSearch();
                packageDatagrid.ItemsSource = packageList;
                CalculateTotals();
                packageDatagrid.Items.Refresh();
            }
            catch(Exception exception)
            {
                MessageBoxResult result = MessageBox.Show("Der opstod et problem med at gemme filen. \n\n" + exception, "Error!");
            }
        }

        private void Btn_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                Package selectedPackage = (Package)packageDatagrid.SelectedItems[0];
                PLC.DeletePackageById(selectedPackage.Id);
                LoadSearch();
                packageDatagrid.ItemsSource = packageList;
                CalculateTotals();
                packageDatagrid.Items.Refresh();
            }
            catch(Exception exception)
            {
                MessageBoxResult result = MessageBox.Show("Der opstod et problem med at slette pakken. \n\n" + exception, "Error!");
            }
        }

        private List<Package> FetchPackageListFromController()
        {
            return PLC.ReturnPackageList();
        }

        private void Btn_print(object sender, RoutedEventArgs e)
        {
            try
            {
                Print print = new Print();
                print.Printall(this.packageList, GLSCalculated, UOmdelingCalculated, MOmdelingCalculated, BrevCalculated);
                MessageBoxResult result = MessageBox.Show("Listen af pakker blev gemt i filen Print.csv på dit skrivebord.", "Print");
            }
            catch(Exception exception)
            {
                MessageBoxResult result = MessageBox.Show("Der opstod et problem ved oprettelse af filen. \n\n" + exception, "Error!");
            }
        }

        private void Btn_search(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadSearch();
                packageDatagrid.ItemsSource = packageList;
                CalculateTotals();
                packageDatagrid.Items.Refresh();
            }
            catch(Exception exception)
            {
                MessageBoxResult result = MessageBox.Show("Kunne ikke søge inden for de givene dato'er. \n\n" + exception, "Error!");
            }
        }

        void StartDateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            startDate.Text = "";
        }

        void EndDateTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            endDate.Text = "";
        }

        private void LoadSearch()
        {
            packageList = FetchPackageListFromController();
            if (startDate.Text != "" && endDate.Text != "")
            {
                packageList = packageList.FindAll(
                delegate (Package pc)
                    {
                        return pc.Date > Convert.ToDateTime(startDate.Text).AddDays(-1);
                    });

                packageList = packageList.FindAll(
                    delegate (Package pc)
                    {
                        return pc.Date < Convert.ToDateTime(endDate.Text).AddDays(+1);
                    }
                );
            }
        }

        private void CalculateTotals()
        {
            GLSCalculated = 0;
            UOmdelingCalculated = 0;
            MOmdelingCalculated = 0;
            BrevCalculated = 0;

            try
            {
                foreach (var package in packageList)
                {
                    if (package.Type == "GLS")
                    {
                        GLSCalculated++;
                    }
                    else if (package.Type == "U/Omdeling PostDanmark")
                    {
                        UOmdelingCalculated++;
                    }
                    else if (package.Type == "M/Omdeling PostDanmark")
                    {
                        MOmdelingCalculated++;
                    }
                    else if (package.Type == "Brev")
                    {
                        BrevCalculated++;
                    }
                }
            }
            catch(Exception exception)
            {
                MessageBoxResult result = MessageBox.Show("Kunne ikke finde en liste over pakker.\nDette sker typisk hvis der endten ikke er forbindelse med databasen, eller databasen er tom. \n\n" + exception, "Error!");
            }

            totalsData.GLS = GLSCalculated;
            totalsData.UOmdeling = UOmdelingCalculated;
            totalsData.MOmdeling = MOmdelingCalculated;
            totalsData.Brev = BrevCalculated;
        }
    }
}