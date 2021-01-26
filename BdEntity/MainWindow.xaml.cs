using System;
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
using System.Data;
using System.Data.Entity;
using Microsoft.Win32;
using System.IO;

namespace BdEntity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            datagr.CanUserAddRows = false;
            using (var db = new bdtestEntities())
            {
                datagr.ItemsSource = db.StudentSet.ToList();
            }
        }

        public string GetPath()
        {
            
            var dialog = new OpenFileDialog();
            dialog.Filter = "Файлы csv|*.csv";
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }
        public void Update()
        {
            using (var db = new bdtestEntities())
            {
                datagr.ClearValue(ItemsControl.ItemsSourceProperty);
                datagr.ItemsSource = db.StudentSet.ToList();
            }
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (datagr.SelectedItem == null)
            {
                MessageBox.Show("Выделите строку!");
            }
            else
            {
                Student student = (Student)datagr.SelectedItem;
                //Console.WriteLine(datagr.SelectedItem.ToString());
                using (var db = new bdtestEntities())
                {
                    db.StudentSet.Attach(student);
                    db.StudentSet.Remove(student);
                    db.SaveChanges();
                    Update();
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (datagr.SelectedItem == null)
            {
                MessageBox.Show("Выделите строку!");
            }
            else
            {
                Student student = (Student)datagr.SelectedItem;
                using (var db = new bdtestEntities())
                {

                    student.Name = NameText.Text;
                    student.Surname = SNameText.Text;
                    student.Age = AgeText.Text;

                    db.Entry(student).State = EntityState.Modified;

                    db.SaveChanges();
                    Update();
                }
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new bdtestEntities())
            {
                var student = new Student()
                {
                    Name = NameText.Text,
                    Surname = SNameText.Text,
                    Age = AgeText.Text
                };
                db.StudentSet.Add(student);
                db.SaveChanges();
                Update();

            }
        }

        private void ImpButton_Click(object sender, RoutedEventArgs e)
        {
            PathText.Text = GetPath();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //импорт
        {
            string file = PathText.Text;
            string[] lines = File.ReadAllLines(file, System.Text.Encoding.UTF8);
            foreach (string s in lines)
            {
                string[] cells = s.Split(';');
                Student student = new Student()
                {
                    Name = cells[1],
                    Surname = cells[2],
                    Age = cells[3]
                };
                using (var db = new bdtestEntities())
                {
                    db.StudentSet.Add(student);
                    db.SaveChanges();
                    Update();
                }
            }
        }
    }
}
