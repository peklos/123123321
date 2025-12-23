using System;

namespace CollegeLibrary
{
    // Класс книги - хранит данные об одной книге
    public class Book
    {
        public int Id { get; set; }           // Уникальный номер
        public string Title { get; set; }      // Название книги
        public string Author { get; set; }     // Автор
        public int Year { get; set; }          // Год издания
        public bool IsAvailable { get; set; }  // Доступна ли книга

        public Book()
        {
            Title = "";
            Author = "";
            IsAvailable = true;
        }
    }
}
