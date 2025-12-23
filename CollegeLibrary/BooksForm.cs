using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CollegeLibrary
{
    public class BooksForm : Form
    {
        private string currentUser;
        private List<string> books = new List<string>();
        private string fileName = "books.txt";

        private TextBox txtBook;
        private Button btnAdd;
        private Button btnDelete;
        private ListBox listBooks;

        public BooksForm(string username)
        {
            currentUser = username;
            CreateElements();
            LoadData();
        }

        private void CreateElements()
        {
            this.Text = "Библиотека - " + currentUser;
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Поле ввода
            txtBook = new TextBox();
            txtBook.Location = new Point(10, 10);
            txtBook.Size = new Size(250, 25);
            this.Controls.Add(txtBook);

            // Кнопка Добавить
            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(270, 10);
            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // Кнопка Удалить
            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Location = new Point(270, 45);
            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // Список книг
            listBooks = new ListBox();
            listBooks.Location = new Point(10, 45);
            listBooks.Size = new Size(250, 250);
            this.Controls.Add(listBooks);
        }

        // Загрузить книги из файла (каждая строка = книга)
        private void LoadData()
        {
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                books = new List<string>(lines);
            }
            UpdateList();
        }

        // Сохранить книги в файл
        private void SaveData()
        {
            File.WriteAllLines(fileName, books.ToArray());
        }

        // Обновить список на экране
        private void UpdateList()
        {
            listBooks.Items.Clear();
            foreach (string book in books)
            {
                listBooks.Items.Add(book);
            }
        }

        // Нажали "Добавить"
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtBook.Text != "")
            {
                books.Add(txtBook.Text);
                SaveData();
                UpdateList();
                txtBook.Text = "";
            }
        }

        // Нажали "Удалить"
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listBooks.SelectedIndex >= 0)
            {
                books.RemoveAt(listBooks.SelectedIndex);
                SaveData();
                UpdateList();
            }
        }
    }
}
