using System;
using System.Drawing;
using System.Windows.Forms;

namespace CollegeLibrary
{
    public class MainForm : Form
    {
        // Элементы интерфейса
        private DataGridView dataGrid;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private TextBox txtYear;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnSearch;
        private Button btnShowAll;
        private CheckBox chkAvailable;

        // База данных
        private Database db;

        public MainForm()
        {
            db = new Database();
            InitializeComponents();
            LoadBooks();
        }

        // Создание всех элементов интерфейса
        private void InitializeComponents()
        {
            // Настройки окна
            this.Text = "Библиотека колледжа";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // === ПАНЕЛЬ ДОБАВЛЕНИЯ КНИГИ ===
            Label lblTitle = new Label();
            lblTitle.Text = "Название:";
            lblTitle.Location = new Point(10, 15);
            lblTitle.Size = new Size(70, 20);

            txtTitle = new TextBox();
            txtTitle.Location = new Point(85, 12);
            txtTitle.Size = new Size(150, 20);

            Label lblAuthor = new Label();
            lblAuthor.Text = "Автор:";
            lblAuthor.Location = new Point(245, 15);
            lblAuthor.Size = new Size(50, 20);

            txtAuthor = new TextBox();
            txtAuthor.Location = new Point(300, 12);
            txtAuthor.Size = new Size(120, 20);

            Label lblYear = new Label();
            lblYear.Text = "Год:";
            lblYear.Location = new Point(430, 15);
            lblYear.Size = new Size(35, 20);

            txtYear = new TextBox();
            txtYear.Location = new Point(470, 12);
            txtYear.Size = new Size(60, 20);

            chkAvailable = new CheckBox();
            chkAvailable.Text = "Доступна";
            chkAvailable.Location = new Point(540, 12);
            chkAvailable.Size = new Size(85, 20);
            chkAvailable.Checked = true;

            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(630, 10);
            btnAdd.Size = new Size(80, 25);
            btnAdd.Click += BtnAdd_Click;

            // === ПАНЕЛЬ ПОИСКА ===
            Label lblSearch = new Label();
            lblSearch.Text = "Поиск:";
            lblSearch.Location = new Point(10, 50);
            lblSearch.Size = new Size(50, 20);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(65, 47);
            txtSearch.Size = new Size(200, 20);

            btnSearch = new Button();
            btnSearch.Text = "Найти";
            btnSearch.Location = new Point(275, 45);
            btnSearch.Size = new Size(70, 25);
            btnSearch.Click += BtnSearch_Click;

            btnShowAll = new Button();
            btnShowAll.Text = "Показать все";
            btnShowAll.Location = new Point(355, 45);
            btnShowAll.Size = new Size(100, 25);
            btnShowAll.Click += BtnShowAll_Click;

            btnDelete = new Button();
            btnDelete.Text = "Удалить выбранную";
            btnDelete.Location = new Point(630, 45);
            btnDelete.Size = new Size(130, 25);
            btnDelete.Click += BtnDelete_Click;

            // === ТАБЛИЦА КНИГ ===
            dataGrid = new DataGridView();
            dataGrid.Location = new Point(10, 80);
            dataGrid.Size = new Size(760, 370);
            dataGrid.AllowUserToAddRows = false;
            dataGrid.ReadOnly = true;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Добавляем все элементы на форму
            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAuthor);
            this.Controls.Add(lblYear);
            this.Controls.Add(txtYear);
            this.Controls.Add(chkAvailable);
            this.Controls.Add(btnAdd);
            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearch);
            this.Controls.Add(btnShowAll);
            this.Controls.Add(btnDelete);
            this.Controls.Add(dataGrid);
        }

        // Загрузить книги в таблицу
        private void LoadBooks()
        {
            dataGrid.DataSource = null;
            dataGrid.DataSource = db.Books;
        }

        // Кнопка "Добавить"
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Проверка заполнения полей
            if (string.IsNullOrEmpty(txtTitle.Text) ||
                string.IsNullOrEmpty(txtAuthor.Text))
            {
                MessageBox.Show("Заполните название и автора!");
                return;
            }

            // Проверка года
            int year = 0;
            if (!int.TryParse(txtYear.Text, out year))
            {
                MessageBox.Show("Введите корректный год!");
                return;
            }

            // Создаем новую книгу
            Book book = new Book();
            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Year = year;
            book.IsAvailable = chkAvailable.Checked;

            // Добавляем в базу
            db.AddBook(book);

            // Очищаем поля
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtYear.Text = "";
            chkAvailable.Checked = true;

            // Обновляем таблицу
            LoadBooks();
            MessageBox.Show("Книга добавлена!");
        }

        // Кнопка "Удалить"
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите книгу для удаления!");
                return;
            }

            // Получаем ID выбранной книги
            int id = (int)dataGrid.SelectedRows[0].Cells["Id"].Value;

            // Спрашиваем подтверждение
            var result = MessageBox.Show("Удалить эту книгу?",
                "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                db.DeleteBook(id);
                LoadBooks();
            }
        }

        // Кнопка "Найти"
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text;
            if (string.IsNullOrEmpty(query))
            {
                MessageBox.Show("Введите текст для поиска!");
                return;
            }

            var found = db.Search(query);
            dataGrid.DataSource = null;
            dataGrid.DataSource = found;

            if (found.Count == 0)
            {
                MessageBox.Show("Ничего не найдено");
            }
        }

        // Кнопка "Показать все"
        private void BtnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadBooks();
        }
    }
}
