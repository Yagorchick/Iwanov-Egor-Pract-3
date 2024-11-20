using System;
using System.Data.SqlClient;

namespace DB_Iwanov_Egor_Pract_3
{
    class Program
    {
        static string connectionString = "Server=sql.bsite.net\\MSSQL2016;Database=yagorchick_;User Id=yagorchick_;Password=23042003;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление данными в базе данных.");
                Console.WriteLine("Выберите таблицу для управления:");
                Console.WriteLine("1) Пользователи (Users)");
                Console.WriteLine("2) Курсы (Courses)");
                Console.WriteLine("3) Материалы (Materials)");
                Console.WriteLine("4) Материалы Курсов (CourseMaterials)");
                Console.WriteLine("5) Записи на курсы (Enrollments)");
                Console.WriteLine("6) Отзывы (Reviews)");
                Console.WriteLine("0) Выход");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ManageUsers();
                        break;
                    case "2":
                        ManageCourses();
                        break;
                    case "3":
                        ManageMaterials();
                        break;
                    case "4":
                        ManageCourseMaterials();
                        break;
                    case "5":
                        ManageEnrollments();
                        break;
                    case "6":
                        ManageReviews();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ManageUsers()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление пользователями:");
                Console.WriteLine("1) Посмотреть всех пользователей");
                Console.WriteLine("2) Добавить нового пользователя");
                Console.WriteLine("3) Обновить пользователя");
                Console.WriteLine("4) Удалить пользователя");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewUsers();
                        break;
                    case "2":
                        AddUser();
                        break;
                    case "3":
                        UpdateUser();
                        break;
                    case "4":
                        DeleteUser();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewUsers()
        {
            Console.Clear();
            Console.WriteLine("Список пользователей:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT UserID, FirstName, Username, Email, CreatedAt FROM Users", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("ID\t\tИмя\t\tНик\t\tEmail\t\t\tСоздано");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["UserID"]}\t\t{reader["FirstName"]}\t\t{reader["Username"]}\t{reader["Email"]}\t{reader["CreatedAt"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddUser()
        {
            Console.Clear();
            Console.WriteLine("Добавление пользователя:");
            Console.WriteLine("Введите имя, имя пользователя, email и пароль через запятую \",\"");
            Console.WriteLine("Пример: Иван,Vanya,email@example.com,password");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 4)
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string firstName = data[0];
            string username = data[1];
            string email = data[2];
            string passwordHash = data[3];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Users (FirstName, Username, Email, PasswordHash) OUTPUT INSERTED.UserID VALUES (@FirstName, @Username, @Email, @PasswordHash)", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                int newUserId = (int)command.ExecuteScalar();
                Console.WriteLine($"Добавление успешно! Его ID - {newUserId}.");
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("Обновление пользователя:");
            Console.Write("Введите ID пользователя для обновления: ");
            int userId = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите новые данные через запятую \",\"");
            Console.WriteLine("Пример: Иван,Vanya,newemail@example.com,newpassword");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 4)
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string firstName = data[0];
            string username = data[1];
            string email = data[2];
            string passwordHash = data[3];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Users SET FirstName = @FirstName, Username = @Username, Email = @Email, PasswordHash = @PasswordHash WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                command.Parameters.AddWithValue("@UserID", userId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Пользователь обновлен успешно.");
                }
                else
                {
                    Console.WriteLine("Пользователь не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Удаление пользователя:");
            Console.Write("Введите ID пользователя для удаления: ");
            int userId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserID", userId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Пользователь удален успешно.");
                }
                else
                {
                    Console.WriteLine("Пользователь не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void ManageCourses()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление курсами:");
                Console.WriteLine("1) Посмотреть все курсы");
                Console.WriteLine("2) Добавить новый курс");
                Console.WriteLine("3) Обновить курс");
                Console.WriteLine("4) Удалить курс");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewCourses();
                        break;
                    case "2":
                        AddCourse();
                        break;
                    case "3":
                        UpdateCourse();
                        break;
                    case "4":
                        DeleteCourse();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewCourses()
        {
            Console.Clear();
            Console.WriteLine("Список курсов:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT CourseID, Title, Description, InstructorID, CreatedAt FROM Courses", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("ID\tНазвание\tОписание\tИнструктор ID\tСоздано");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["CourseID"]}\t{reader["Title"]}\t{reader["Description"]}\t{reader["InstructorID"]}\t{reader["CreatedAt"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddCourse()
        {
            Console.Clear();
            Console.WriteLine("Добавление курса:");
            Console.WriteLine("Введите название, описание и ID инструктора через запятую \",\"");
            Console.WriteLine("Пример: Курс по программированию,Изучите основы программирования,1");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 3 || !int.TryParse(data[2], out int instructorId))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string title = data[0];
            string description = data[1];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Courses (Title, Description, InstructorID) OUTPUT INSERTED.CourseID VALUES (@Title, @Description, @InstructorID)", connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@InstructorID", instructorId);

                int newCourseId = (int)command.ExecuteScalar();
                Console.WriteLine($"Добавление успешно! Его ID - {newCourseId}.");
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void UpdateCourse()
        {
            Console.Clear();
            Console.WriteLine("Обновление курса:");
            Console.Write("Введите ID курса для обновления: ");
            int courseId = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите новые данные через запятую \",\"");
            Console.WriteLine("Пример: Новый курс,Обновленное описание,1");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 3 || !int.TryParse(data[2], out int instructorId))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string title = data[0];
            string description = data[1];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Courses SET Title = @Title, Description = @Description, InstructorID = @InstructorID WHERE CourseID = @CourseID", connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@InstructorID", instructorId);
                command.Parameters.AddWithValue("@CourseID", courseId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Курс обновлен успешно.");
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteCourse()
        {
            Console.Clear();
            Console.WriteLine("Удаление курса:");
            Console.Write("Введите ID курса для удаления: ");
            int courseId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Courses WHERE CourseID = @CourseID", connection);
                command.Parameters.AddWithValue("@CourseID", courseId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Курс удален успешно.");
                }
                else
                {
                    Console.WriteLine("Курс не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void ManageMaterials()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление материалами:");
                Console.WriteLine("1) Посмотреть все материалы");
                Console.WriteLine("2) Добавить новый материал");
                Console.WriteLine("3) Обновить материал");
                Console.WriteLine("4) Удалить материал");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewMaterials();
                        break;
                    case "2":
                        AddMaterial();
                        break;
                    case "3":
                        UpdateMaterial();
                        break;
                    case "4":
                        DeleteMaterial();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewMaterials()
        {
            Console.Clear();
            Console.WriteLine("Список материалов:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT MaterialID, Name, Description FROM Materials", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("ID\tНазвание\tОписание");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["MaterialID"]}\t{reader["Name"]}\t{reader["Description"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddMaterial()
        {
            Console.Clear();
            Console.WriteLine("Добавление материала:");
            Console.WriteLine("Введите название и описание через запятую \",\"");
            Console.WriteLine("Пример: Дерево,Материал для курса по столярному делу");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 2)
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string name = data[0];
            string description = data[1];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Materials (Name, Description) OUTPUT INSERTED.MaterialID VALUES (@Name, @Description)", connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Description", description);

                int newMaterialId = (int)command.ExecuteScalar();
                Console.WriteLine($"Добавление успешно! Его ID - {newMaterialId}.");
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void UpdateMaterial()
        {
            Console.Clear();
            Console.WriteLine("Обновление материала:");
            Console.Write("Введите ID материала для обновления: ");
            int materialId = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите новые данные через запятую \",\"");
            Console.WriteLine("Пример: Новый материал,Обновленное описание");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 2)
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string name = data[0];
            string description = data[1];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Materials SET Name = @Name, Description = @Description WHERE MaterialID = @MaterialID", connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@MaterialID", materialId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Материал обновлен успешно.");
                }
                else
                {
                    Console.WriteLine("Материал не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteMaterial()
        {
            Console.Clear();
            Console.WriteLine("Удаление материала:");
            Console.Write("Введите ID материала для удаления: ");
            int materialId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Materials WHERE MaterialID = @MaterialID", connection);
                command.Parameters.AddWithValue("@MaterialID", materialId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Материал удален успешно.");
                }
                else
                {
                    Console.WriteLine("Материал не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void ManageCourseMaterials()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление материалами курсов:");
                Console.WriteLine("1) Посмотреть все материалы курсов");
                Console.WriteLine("2) Добавить материал к курсу");
                Console.WriteLine("3) Удалить материал из курса");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewCourseMaterials();
                        break;
                    case "2":
                        AddCourseMaterial();
                        break;
                    case "3":
                        DeleteCourseMaterial();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewCourseMaterials()
        {
            Console.Clear();
            Console.WriteLine("Список материалов курсов:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT CourseID, MaterialID FROM CourseMaterials", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("CourseID\tMaterialID");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["CourseID"]}\t{reader["MaterialID"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddCourseMaterial()
        {
            Console.Clear();
            Console.WriteLine("Добавление материала к курсу:");
            Console.WriteLine("Введите ID курса и ID материала через запятую \",\"");
            Console.WriteLine("Пример: 1,1");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 2 || !int.TryParse(data[0], out int courseId) || !int.TryParse(data[1], out int materialId))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO CourseMaterials (CourseID, MaterialID) VALUES (@CourseID, @MaterialID)", connection);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@MaterialID", materialId);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Материал успешно добавлен к курсу.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteCourseMaterial()
        {
            Console.Clear();
            Console.WriteLine("Удаление материала из курса:");
            Console.Write("Введите ID курса и ID материала через запятую \",\": ");
            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 2 || !int.TryParse(data[0], out int courseId) || !int.TryParse(data[1], out int materialId))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM CourseMaterials WHERE CourseID = @CourseID AND MaterialID = @MaterialID", connection);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@MaterialID", materialId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Материал удален из курса успешно.");
                }
                else
                {
                    Console.WriteLine("Материал не найден в курсе.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void ManageEnrollments()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление записями на курсы:");
                Console.WriteLine("1) Посмотреть все записи");
                Console.WriteLine("2) Добавить запись на курс");
                Console.WriteLine("3) Удалить запись с курса");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewEnrollments();
                        break;
                    case "2":
                        AddEnrollment();
                        break;
                    case "3":
                        DeleteEnrollment();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewEnrollments()
        {
            Console.Clear();
            Console.WriteLine("Список записей на курсы:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT EnrollmentID, UserID, CourseID, EnrollmentDate FROM Enrollments", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("ID Записи\tUser ID\tCourseID\tДата записи");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["EnrollmentID"]}\t{reader["User ID"]}\t{reader["CourseID"]}\t{reader["EnrollmentDate"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddEnrollment()
        {
            Console.Clear();
            Console.WriteLine("Добавление записи на курс:");
            Console.WriteLine("Введите UserID и CourseID через запятую \",\"");
            Console.WriteLine("Пример: 1,1");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 2 || !int.TryParse(data[0], out int userId) || !int.TryParse(data[1], out int courseId))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Enrollments (User ID, CourseID) VALUES (@User ID, @CourseID)", connection);
                command.Parameters.AddWithValue("@User ID", userId);
                command.Parameters.AddWithValue("@CourseID", courseId);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Запись успешно добавлена.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteEnrollment()
        {
            Console.Clear();
            Console.WriteLine("Удаление записи с курса:");
            Console.Write("Введите ID записи для удаления: ");
            int enrollmentId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Enrollments WHERE EnrollmentID = @EnrollmentID", connection);
                command.Parameters.AddWithValue("@EnrollmentID", enrollmentId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Запись удалена успешно.");
                }
                else
                {
                    Console.WriteLine("Запись не найдена.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void ManageReviews()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Управление отзывами:");
                Console.WriteLine("1) Посмотреть все отзывы");
                Console.WriteLine("2) Добавить отзыв");
                Console.WriteLine("3) Удалить отзыв");
                Console.WriteLine("0) Вернуться");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewReviews();
                        break;
                    case "2":
                        AddReview();
                        break;
                    case "3":
                        DeleteReview();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }

        static void ViewReviews()
        {
            Console.Clear();
            Console.WriteLine("Список отзывов:");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT ReviewID, CourseID, UserID, Rating, Comment, CreatedAt FROM Reviews", connection);
                SqlDataReader reader = command.ExecuteReader();

                Console.WriteLine("ID Отзыва\tCourseID\tUser ID\tРейтинг\tКомментарий\tСоздано");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ReviewID"]}\t{reader["CourseID"]}\t{reader["User ID"]}\t{reader["Rating"]}\t{reader["Comment"]}\t{reader["CreatedAt"]}");
                }
                reader.Close();
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void AddReview()
        {
            Console.Clear();
            Console.WriteLine("Добавление отзыва:");
            Console.WriteLine("Введите CourseID, UserID, Рейтинг (1-5) и комментарий через запятую \",\"");
            Console.WriteLine("Пример: 1,1,5,Отличный курс!");

            string input = Console.ReadLine();
            string[] data = input.Split(',');

            if (data.Length != 4 || !int.TryParse(data[0], out int courseId) || !int.TryParse(data[1], out int userId) || !int.TryParse(data[2], out int rating))
            {
                Console.WriteLine("Неверный формат, повторите еще раз");
                Console.ReadKey();
                return;
            }

            string comment = data[3];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Reviews (CourseID, UserID, Rating, Comment) VALUES (@CourseID, @User ID, @Rating, @Comment)", connection);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@User ID", userId);
                command.Parameters.AddWithValue("@Rating", rating);
                command.Parameters.AddWithValue("@Comment", comment);

                try
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Отзыв успешно добавлен.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }

        static void DeleteReview()
        {
            Console.Clear();
            Console.WriteLine("Удаление отзыва:");
            Console.Write("Введите ID отзыва для удаления: ");
            int reviewId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Reviews WHERE ReviewID = @ReviewID", connection);
                command.Parameters.AddWithValue("@ReviewID", reviewId);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Отзыв удален успешно.");
                }
                else
                {
                    Console.WriteLine("Отзыв не найден.");
                }
            }

            Console.WriteLine("Нажмите на любую клавишу чтобы вернуться.");
            Console.ReadKey();
        }
    }
}

//Server=sql.bsite.net\\MSSQL2016;Database=yagorchick_;User Id=yagorchick_;Password=23042003;TrustServerCertificate=true;