using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace pr4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Zametka> allzam = new List<Zametka>();
        private List<string> tipList = new List<string>(); 
        private string currentDate;

        public MainWindow()
        {
            InitializeComponent();
            currentDate = data.Text;
            data.SelectedDate = DateTime.Now.Date;
            LoadData();
            UpdateData();
            LoadTipList(); 
        }

        private void LoadData()
        {
            allzam = ConvertJ.Deserialization<List<Zametka>>("учет.json");
        }

        private void UpdateData()
        {
            var filteredZametki = allzam.Where(z => z.Date == currentDate).ToList();
            box.ItemsSource = filteredZametki;
            CalculateTotal();
        }

        private void SaveData()
        {
            ConvertJ.Serialization(allzam, "учет.json");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nameValue = name.Text;
            string tipValue = spisoc.Text;
            int moneyValue = int.Parse(discription.Text);
            bool actionValue = moneyValue < 0;
            moneyValue = Math.Abs(moneyValue);

            var zam = new Zametka(nameValue, tipValue, currentDate, moneyValue, actionValue);
            allzam.Add(zam);
            SaveData();
            UpdateData();

            name.Text = "";
            discription.Text = "";
            spisoc.Text = "";
        }
        

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (box.SelectedItem is Zametka selectedZametka)
            {
                string nameValue = name.Text;
                int moneyValue = Convert.ToInt32(discription.Text);
                string type = spisoc.Text;

                
                int selectedIndex = allzam.FindIndex(z => z == selectedZametka);

                selectedZametka.Name = nameValue;
                selectedZametka.Money = moneyValue;
                selectedZametka.Tip = type;

             
                if (moneyValue < 0)
                {
                    selectedZametka.Action = true; 
                    selectedZametka.Money = Math.Abs(moneyValue);
                }
                else
                {
                    selectedZametka.Action = false;
                }

                allzam[selectedIndex] = selectedZametka;
                SaveData();
                UpdateData();

                name.Text = "";
                discription.Text = "";
                spisoc.Text = "";
                box.SelectedIndex = -1;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (box.SelectedItem is Zametka selectedZametka)
            {
                allzam.Remove(selectedZametka);
                SaveData();
                UpdateData();

                name.Text = "";
                discription.Text = "";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            okno mainForm = new okno();
            mainForm.Closed += MainForm_Closed;
            mainForm.Show();
        }
        private void data_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            currentDate = data.Text;
            UpdateData();
        }


        private void LoadTipList()
        {
            tipList = ConvertJ.Deserialization<List<string>>("типзаписи.json");
            spisoc.ItemsSource = tipList;
        }

        private void CalculateTotal()
        {
            int total = 0;
            foreach (Zametka zametka in allzam)
            {
                if (zametka.Action == false)
                {
                    total += zametka.Money;
                }
                if (zametka.Action == true)
                {
                    total -= zametka.Money;
                }
            }
            textBlockTotal.Text = "Итог: " + total.ToString();
        }


        private void box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (box.SelectedItem is Zametka selectedZametka)
            {
                name.Text = selectedZametka.Name;
                if (selectedZametka.Action)
                {
                    discription.Text = "-" + selectedZametka.Money.ToString();
                }
                else
                {
                    discription.Text = selectedZametka.Money.ToString();
                }
                spisoc.Text = selectedZametka.Tip;
            }
            else
            {
                name.Text = "";
                discription.Text = "";
            }
        }

        

        private void MainForm_Closed(object sender, EventArgs e)
        {
            LoadTipList();
        }
    }
}