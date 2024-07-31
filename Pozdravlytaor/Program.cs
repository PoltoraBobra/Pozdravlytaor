/*Программа предоставляет пользователю возможность ведения списка дней рождения. Все записи  являются объектами класса Person. 
Список реализован при помощи контейнера: List<Person> people;
Если файл со списком уже существует, он автоматически загрузится и из него будет выбрано 3 записи ближайших ДР, которые будут выведены.
После этого будет создан новый список, с которым сможет работать пользователь.
Пользователь может: добавить запись, удалить запись, отредактировать запись, вывести список, отсортировать список по ближайшим/дальнейшим ДР, сохранить
и загрузить список в файл или из файла, если такой существует. Тип файла - JSON.
При выходе из программы, в случае, если список не пуст пользователю будет выведено предупреждение о том, что все несохранённые данные будут удалены.*/


using System;
using System.Text.Json;

class Person
{
    public string FIO { get; set; }
    public string Status { get; set; }
    public int Age { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CurrentDate { get; set; }
    public int DaysBeforeBirthday { get; set; }
    public int DaysAfterBirthday { get; set; }

    public Person() { }//конструктор по-умолчанию для сериализации в JSON
    public Person(bool userInput)//конструктор
    {
        while (true)
        {
            Console.Write("Введите ФИО: ");
            FIO = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(FIO))
            {
                break;
            }
            else
            {
                Console.WriteLine("ФИО не должно быть пустым. Пожалуйста, введите заново.");
            }
        }

        while (true)
        {
            Console.Write("Введите статус (коллега/знакомый/родственник и т.п.): ");
            Status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Status))
            {
                break;
            }
            else
            {
                Console.WriteLine("Статус не должен быть пустым. Пожалуйста, введите заново.");
            }
        }

        Console.Write("Введите возраст (2 символа): ");
        Age = GetDate(4);

        while (true)
        {
            Console.Write("Введите год рождения человека (4 символа): ");
            int year = GetDate(1);

            Console.Write("Введите месяц рождения человека (2 символа): ");
            int month = GetDate(2);

            Console.Write("Введите число рождения человека (2 символа): ");
            int day = GetDate(3);

            if (IsValidDate(year, month, day))//проверка на корректность даты
            {
                BirthDate = new DateTime(year, month, day);
                break;
            }
            else
            {
                Console.WriteLine("Некорректная дата. Возможно, такого числа в таком месяце не существует. Пожалуйста, введите правильную дату заново.");
            }
        }

        CurrentDate = DateTime.Now;

        int currentDayOfYear = CurrentDate.DayOfYear;
        int birthDayOfYear = new DateTime(CurrentDate.Year, BirthDate.Month, BirthDate.Day).DayOfYear;

        if (birthDayOfYear >= currentDayOfYear)
        {
            DaysBeforeBirthday = birthDayOfYear - currentDayOfYear;
        }
        else
        {
            if (DateTime.IsLeapYear(CurrentDate.Year))
            {
                DaysBeforeBirthday = 366 - (currentDayOfYear - birthDayOfYear);
            }
            else
            {
                DaysBeforeBirthday = 365 - (currentDayOfYear - birthDayOfYear);
            }
        }

        if (birthDayOfYear <= currentDayOfYear)
        {
            DaysAfterBirthday = currentDayOfYear - birthDayOfYear;
        }
        else
        {
            if (DateTime.IsLeapYear(CurrentDate.Year))
            {
                DaysAfterBirthday = 366 - (birthDayOfYear - currentDayOfYear);
            }
            else
            {
                DaysAfterBirthday = 365 - (birthDayOfYear - currentDayOfYear);
            }
        }
    }

    public void Print()//интерфейс списка
    {
        Console.WriteLine($"ФИО: {FIO}");
        Console.WriteLine($"Статус: {Status}");
        Console.WriteLine($"Возраст (без учёта дня рождения): {Age}");
        Console.WriteLine($"Дата рождения: {BirthDate.Day}.{BirthDate.Month}.{BirthDate.Year}");
        Console.WriteLine($"Дней до следующего дня рождения: {DaysBeforeBirthday}");
        Console.WriteLine($"Дней после последнего дня рождения: {DaysAfterBirthday}");
    }

    private static int GetDate(int number)
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (number == 1)//для года
            {
                if (input != null && input.Length == 4 && int.TryParse(input, out int year) && year > 0 && year <= 9999)
                {
                    return year;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 4 цифры года.");
                }
            }
            else if (number == 2)//для месяца
            {
                if (input != null && input.Length == 2 && int.TryParse(input, out int month) && month > 0 && month <= 12)
                {
                    return month;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 2 цифры месяца.");
                }
            }
            else if (number == 3)//для дня
            {
                if (input != null && input.Length == 2 && int.TryParse(input, out int day) && day > 0 && day <= 31)
                {
                    return day;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 2 цифры дня.");
                }
            }
            else if (number == 4)//для возраста
            {
                if (input != null && input.Length == 2 && int.TryParse(input, out int vozrast) && vozrast > 0 && vozrast <= 99)
                {
                    return vozrast;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 2 цифры.");
                }
            }
        }
    }

    private static bool IsValidDate(int year, int month, int day)
    {
        try
        {
            DateTime date = new DateTime(year, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void EditData()
    {
        while (true)
        {
            Console.Write($"Предыдущее ФИО: {FIO}. Введите новое ФИО: ");
            FIO = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(FIO))
            {
                break;
            }
            else
            {
                Console.WriteLine("ФИО не должно быть пустым. Пожалуйста, введите заново.");
            }
        }

        while (true)
        {
            Console.Write($"Предыдущий статус: {Status}. Введите новый статус (коллега/знакомый/родственник и т.п.): ");
            Status = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Status))
            {
                break;
            }
            else
            {
                Console.WriteLine("Статус не должен быть пустым. Пожалуйста, введите заново.");
            }
        }

        Console.Write($"Предыдущий введённый возраст: {Age}. Введите новый возраст (2 символа): ");
        Age = GetDate(4);

        while (true)
        {
            Console.Write($"Предыдущий введённый год: {BirthDate.Year}. Введите год рождения человека (4 символа): ");
            int year = GetDate(1);

            Console.Write($"Предыдущий введённый месяц: {BirthDate.Month}. Введите месяц рождения человека (2 символа): ");
            int month = GetDate(2);

            Console.Write($"Предыдущий введённый день: {BirthDate.Day}. Введите число рождения человека (2 символа): ");
            int day = GetDate(3);


            if (IsValidDate(year, month, day))//проверка на корректность даты
            {
                BirthDate = new DateTime(year, month, day);
                break;
            }
            else
            {
                Console.WriteLine("Некорректная дата. Возможно, такого числа в таком месяце не существует. Пожалуйста, введите правильную дату заново.");
            }
        }

        CurrentDate = DateTime.Now;

        int currentDayOfYear = CurrentDate.DayOfYear;
        int birthDayOfYear = new DateTime(CurrentDate.Year, BirthDate.Month, BirthDate.Day).DayOfYear;

        if (birthDayOfYear >= currentDayOfYear)
        {
            DaysBeforeBirthday = birthDayOfYear - currentDayOfYear;
        }
        else
        {
            if (DateTime.IsLeapYear(CurrentDate.Year))
            {
                DaysBeforeBirthday = 366 - (currentDayOfYear - birthDayOfYear);
            }
            else
            {
                DaysBeforeBirthday = 365 - (currentDayOfYear - birthDayOfYear);
            }
        }

        if (birthDayOfYear <= currentDayOfYear)
        {
            DaysAfterBirthday = currentDayOfYear - birthDayOfYear;
        }
        else
        {
            if (DateTime.IsLeapYear(CurrentDate.Year))
            {
                DaysAfterBirthday = 366 - (birthDayOfYear - currentDayOfYear);
            }
            else
            {
                DaysAfterBirthday = 365 - (birthDayOfYear - currentDayOfYear);
            }
        }
    }
}


class Program
{
    static void Main()
    {
        {
            bool exit = false;
            string filePath = "people.json";
            List<Person> people = LoadPeopleOnStart(filePath);

            while (!exit)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить запись");
                Console.WriteLine("2. Удалить запись");
                Console.WriteLine("3. Редактировать запись");
                Console.WriteLine("4. Печать списка");
                Console.WriteLine("5. Сортировка списка по ближайшим дням рождения");
                Console.WriteLine("6. Сортировка списка по дням после последнего дня рождения");
                Console.WriteLine("7. Сохранение списка в файл");
                Console.WriteLine("8. Загрузка списка из файла");
                Console.WriteLine("9. Выйти");

                Console.Write("Выберите пункт: ");
                int choice = GetInt(1);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Введите данные для нового человека:");
                        Person person = new Person(true);
                        people.Add(person);
                        Console.WriteLine("Запись успешно добавлена:");
                        break;
                    case 2:
                        Console.WriteLine("Введите номер записи для удаления: ");
                        int indexToRemove = GetInt(2) - 1;
                        if (indexToRemove >= 0 && indexToRemove < people.Count)
                        {
                            people.RemoveAt(indexToRemove);
                            Console.WriteLine("Запись удалена.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректный номер записи.");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Введите номер записи для изменения: ");
                        int indexToEdit = GetInt(2) - 1;
                        if (indexToEdit >= 0 && indexToEdit < people.Count)
                        {
                            people[indexToEdit].EditData();
                        }
                        else
                        {
                            Console.WriteLine("Некорректный номер записи.");
                        }
                        break;
                    case 4:
                        if (people == null || people.Count == 0)//проверка на пустой список
                        {
                            Console.WriteLine("Список пуст. Нечего сохранять.");
                        }
                        else
                        {
                            Console.WriteLine("Список людей:");
                            for (int i = 0; i < people.Count; i++)
                            {
                                Console.WriteLine($"\nЗапись {i + 1}:");
                                people[i].Print();
                            }
                        }
                        break;
                    case 5:
                        SortByDaysBeforeBirthday(people);
                        Console.WriteLine("Список отсортирован. Выведете список используя пункт меню 4 или продолжите работу.");
                        break;
                    case 6:
                        SortByDaysAfterBirthday(people);
                        Console.WriteLine("Список отсортирован. Выведете список используя пункт меню 4 или продолжите работу.");
                        break;
                    case 7:
                        SavePeopleToFile(people, filePath);
                        break;
                    case 8:
                        people = LoadPeopleFromFile(filePath);
                        break;
                    case 9:
                        if (people.Count > 0)
                        {
                            while (true)
                            {
                                Console.WriteLine("В списке есть несохранённые записи. При выходе все несохранённые данные будут удалены.");
                                Console.Write("Вы действительно хотите выйти? (да/нет): ");
                                string exitChoice = Console.ReadLine();
                                if (!string.IsNullOrWhiteSpace(exitChoice) && (exitChoice == "да" || exitChoice == "нет" || exitChoice == "Да" || exitChoice == "Нет"))
                                {
                                    if (exitChoice == "да" || exitChoice == "Да")
                                    {
                                        exit = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 'да' или 'нет'.");
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, выберите пункт от 1 до 9.");
                        break;
                }
            }
        }
    }

    public static int GetInt(int number)
    {
        while (true)
        {
            string input = Console.ReadLine();
            if (number == 1)//для выбора пункта меню
            {
                if (input != null && input.Length == 1 && int.TryParse(input, out int numb) && numb >= 0 && numb <= 9)
                {
                    return numb;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите 1 цифру.");
                }
            }
            else if (number == 2)//для выбора номера записи
            {
                if (input != null && input.Length >= 1 && int.TryParse(input, out int zapis) && zapis >= 0 && zapis <= 9999)
                {
                    return zapis;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите корректный номер записи.");
                }
            }
        }
    }

    static void SortByDaysBeforeBirthday(List<Person> people)
    {
        for (int i = 0; i < people.Count - 1; i++)
        {
            for (int j = i + 1; j < people.Count; j++)
            {
                if (people[i].DaysBeforeBirthday > people[j].DaysBeforeBirthday)
                {
                    Person temp = people[i];
                    people[i] = people[j];
                    people[j] = temp;
                }
            }
        }
    }

    static void SortByDaysAfterBirthday(List<Person> people)
    {
        for (int i = 0; i < people.Count - 1; i++)
        {
            for (int j = i + 1; j < people.Count; j++)
            {
                if (people[i].DaysAfterBirthday > people[j].DaysAfterBirthday)
                {
                    Person temp = people[i];
                    people[i] = people[j];
                    people[j] = temp;
                }
            }
        }
    }

    static void SavePeopleToFile(List<Person> people, string filePath)
    {
        if (people == null || people.Count == 0)//проверка на пустой список
        {
            Console.WriteLine("Список пуст. Нечего сохранять.");
            return;
        }
        else
        {
            string json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);

            Console.WriteLine("Список успешно сохранен в файл.");
        }
    }

    static List<Person> LoadPeopleFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найден.");
            return new List<Person>();
        }

        string json = File.ReadAllText(filePath);
        List<Person> people = JsonSerializer.Deserialize<List<Person>>(json);
        if (people == null)
        {
            Console.WriteLine("Ошибка при десериализации данных. Список пуст.");
            return new List<Person>();
        }
        Console.WriteLine("Список успешно загружен из файла.");
        return people;
    }

    static List<Person> LoadPeopleOnStart(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<Person>();
        }

        string json = File.ReadAllText(filePath);
        List<Person> people = JsonSerializer.Deserialize<List<Person>>(json);
        if (people == null)
        {
            return new List<Person>();
        }

        SortByDaysBeforeBirthday(people);

        int countToShow = Math.Min(people.Count, 3);
        for (int i = 0; i < countToShow; i++)
        {
            Console.WriteLine($"\nЗапись {i + 1}:");
            people[i].Print();
        }

        return new List<Person>();
    }
}