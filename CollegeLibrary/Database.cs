using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace CollegeLibrary
{
    // Класс для работы с базой данных (JSON файл)
    public class Database
    {
        private string filePath = "books.json";  // Имя файла БД
        public List<Book> Books { get; set; }    // Список всех книг

        public Database()
        {
            Books = new List<Book>();
            Load();  // Загружаем данные при создании
        }

        // Загрузить данные из файла
        public void Load()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Books = JsonSerializer.Deserialize<List<Book>>(json);
                if (Books == null)
                    Books = new List<Book>();
            }
        }

        // Сохранить данные в файл
        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Books, options);
            File.WriteAllText(filePath, json);
        }

        // Добавить книгу
        public void AddBook(Book book)
        {
            // Генерируем новый ID
            int maxId = 0;
            foreach (var b in Books)
            {
                if (b.Id > maxId) maxId = b.Id;
            }
            book.Id = maxId + 1;

            Books.Add(book);
            Save();
        }

        // Удалить книгу по ID
        public void DeleteBook(int id)
        {
            Book bookToRemove = null;
            foreach (var b in Books)
            {
                if (b.Id == id)
                {
                    bookToRemove = b;
                    break;
                }
            }
            if (bookToRemove != null)
            {
                Books.Remove(bookToRemove);
                Save();
            }
        }

        // Поиск книг по названию или автору
        public List<Book> Search(string query)
        {
            List<Book> result = new List<Book>();
            query = query.ToLower();

            foreach (var b in Books)
            {
                if (b.Title.ToLower().Contains(query) ||
                    b.Author.ToLower().Contains(query))
                {
                    result.Add(b);
                }
            }
            return result;
        }
    }
}
