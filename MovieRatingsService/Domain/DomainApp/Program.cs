using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;


Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Демонстрация доменного слоя ===\n");

// --- Создание администратора ---
Console.WriteLine("--- Создание администратора ---");
var admin = new Admin(
    new Username("superadmin"),
    new PasswordHash("hashed_password_123"),
    new Permissions("manage_movies,moderate_reviews")
);
Console.WriteLine($"Создан: {admin}");
Console.WriteLine($"Есть право manage_movies: {admin.HasPermission("manage_movies")}");
Console.WriteLine($"Есть право delete_users: {admin.HasPermission("delete_users")}\n");

// --- Администратор создаёт фильмы ---
Console.WriteLine("--- Администратор создаёт фильмы ---");
var movie1 = admin.CreateMovie(
    new Title("Начало"),
    "Фильм о снах внутри снов",
    2010,
    "Sci-Fi",
    "https://example.com/inception.jpg"
);
var movie2 = admin.CreateMovie(
    new Title("Матрица"),
    "Что есть реальность?",
    1999,
    "Action"
);
Console.WriteLine($"Создан фильм: {movie1}");
Console.WriteLine($"Создан фильм: {movie2}\n");

// --- Создание пользователей ---
Console.WriteLine("--- Создание пользователей ---");
var user1 = new User(new Username("alice"));
var user2 = new User(new Username("bob"));
Console.WriteLine($"Создан: {user1}");
Console.WriteLine($"Создан: {user2}\n");

// --- Пользователи выставляют оценки ---
Console.WriteLine("--- Пользователи выставляют оценки ---");
var rating1 = movie1.AddRating(user1, new Score(9));
var rating2 = movie1.AddRating(user2, new Score(6));
Console.WriteLine($"Добавлена: {rating1}");
Console.WriteLine($"Добавлена: {rating2}");
Console.WriteLine($"Средний рейтинг {movie1.Title}: {movie1.GetAverageScore():F1} ({movie1.GetRatingsCount()} оценок)\n");

// --- Пользователь меняет свою оценку ---
Console.WriteLine("--- Пользователь alice меняет оценку ---");
movie1.UpdateRating(user1, new Score(10));
Console.WriteLine($"Новый средний рейтинг {movie1.Title}: {movie1.GetAverageScore():F1}\n");

// --- Пользователи пишут рецензии ---
Console.WriteLine("--- Пользователи пишут рецензии ---");
var review1 = movie1.AddReview(user1, new ReviewContent("Гениальный фильм, пересматриваю каждый год!"));
var review2 = movie1.AddReview(user2, new ReviewContent("Них## не понял, интересно."));
Console.WriteLine($"Добавлен: {review1}");
Console.WriteLine($"Добавлен: {review2}\n");

// --- Пользователь редактирует рецензию ---
Console.WriteLine("--- alice редактирует рецензию ---");
movie1.UpdateReview(user1, new ReviewContent("Лучший фильм Нолана, шедевр!"));
var updatedReview = movie1.Reviews.First(r => r.User.Id == user1.Id);
Console.WriteLine($"Обновлено: {updatedReview}\n");

// --- Администратор скрывает рецензию ---
Console.WriteLine("--- Администратор скрывает рецензию bob ---");
admin.HideReview(review2);
Console.WriteLine($"Статус рецензии bob: {review2.Status}");
Console.WriteLine($"Скрыта администратором: {review2.HiddenByAdmin}\n");

// --- Администратор восстанавливает рецензию ---
Console.WriteLine("--- Администратор восстанавливает рецензию bob ---");
admin.UnhideReview(review2);
Console.WriteLine($"Статус рецензии bob: {review2.Status}\n");

// --- Администратор удаляет оценку пользователя ---
Console.WriteLine("--- Администратор удаляет оценку alice ---");
admin.DeleteRating(rating1);
Console.WriteLine($"Оценка alice активна: {rating1.IsActive}");
Console.WriteLine($"Средний рейтинг Inception после удаления: {movie1.GetAverageScore():F1} ({movie1.GetRatingsCount()} оценок)\n");

// --- Администратор обновляет информацию о фильме ---
Console.WriteLine("--- Администратор обновляет фильм Inception ---");
admin.UpdateMovie(movie1, new Title("Inception (Начало)"), "Режиссёр Кристофер Нолан", 2010, "Sci-Fi / Thriller", null);
Console.WriteLine($"Обновлён: {movie1}\n");

// --- Попытка поставить дублирующую оценку ---
Console.WriteLine("--- bob пытается поставить вторую оценку на Начало ---");
try
{
    movie1.AddRating(user2, new Score(7));
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}

// --- Попытка создать Score с недопустимым значением ---
Console.WriteLine("--- Попытка создать Score(15) ---");
try
{
    var badScore = new Score(15);
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}

// --- Попытка удалить фильм с активными данными ---
Console.WriteLine("--- Попытка удалить Начало (есть активные рецензии) ---");
try
{
    admin.DeleteMovie(movie1);
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}

// --- Удаление фильма без зависимостей ---
Console.WriteLine("--- Удаление Матрица (нет оценок и рецензий) ---");
admin.DeleteMovie(movie2);
Console.WriteLine($"{movie2.Title} удалён: {movie2.IsDeleted}\n");

Console.WriteLine("=== Конец демонстрации ===");