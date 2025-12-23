using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace CollegeLibrary
{
    public class LoginForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        // Список пользователей (логин:пароль)
        private Dictionary<string, string> users = new Dictionary<string, string>();
        private string usersFile = "users.json";

        public LoginForm()
        {
            CreateElements();
            LoadUsers();
        }

        private void CreateElements()
        {
            this.Text = "Вход в библиотеку";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Логин
            Label lblLogin = new Label();
            lblLogin.Text = "Логин:";
            lblLogin.Location = new Point(20, 20);
            this.Controls.Add(lblLogin);

            txtLogin = new TextBox();
            txtLogin.Location = new Point(100, 20);
            txtLogin.Size = new Size(150, 25);
            this.Controls.Add(txtLogin);

            // Пароль
            Label lblPassword = new Label();
            lblPassword.Text = "Пароль:";
            lblPassword.Location = new Point(20, 55);
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(100, 55);
            txtPassword.Size = new Size(150, 25);
            txtPassword.PasswordChar = '*';  // скрываем пароль
            this.Controls.Add(txtPassword);

            // Кнопка Войти
            btnLogin = new Button();
            btnLogin.Text = "Войти";
            btnLogin.Location = new Point(50, 100);
            btnLogin.Size = new Size(80, 30);
            btnLogin.Click += BtnLogin_Click;
            this.Controls.Add(btnLogin);

            // Кнопка Регистрация
            btnRegister = new Button();
            btnRegister.Text = "Регистрация";
            btnRegister.Location = new Point(150, 100);
            btnRegister.Size = new Size(100, 30);
            btnRegister.Click += BtnRegister_Click;
            this.Controls.Add(btnRegister);
        }

        // Загрузить пользователей
        private void LoadUsers()
        {
            if (File.Exists(usersFile))
            {
                string json = File.ReadAllText(usersFile);
                users = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
        }

        // Сохранить пользователей
        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(usersFile, json);
        }

        // Нажали Войти
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            // Проверяем есть ли такой пользователь
            if (users.ContainsKey(login) && users[login] == password)
            {
                // Вход успешен - открываем библиотеку
                this.Hide();
                BooksForm booksForm = new BooksForm(login);
                booksForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        // Нажали Регистрация
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Text;

            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            if (users.ContainsKey(login))
            {
                MessageBox.Show("Такой пользователь уже есть!");
                return;
            }

            // Добавляем нового пользователя
            users[login] = password;
            SaveUsers();
            MessageBox.Show("Регистрация успешна! Теперь войдите.");
        }
    }
}
