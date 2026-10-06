using Domain.ValueObject;
using MovieRatingsService.Domain.Domain.Entities;

Console.WriteLine("=== Демонстрация доменного слоя ===\n");


Console.WriteLine("--- Создание администратора ---");
var admin = new Admin(
    new Username("superadmin"),
    new Permissions("manage_movies,moderate_reviews")
);
Console.WriteLine($"Создан: {admin}");
Console.WriteLine($"Есть право manage_movies: {admin.HasPermission("manage_movies")}");
Console.WriteLine($"Есть право delete_users: {admin.HasPermission("delete_users")}\n");


Console.WriteLine("--- Администратор создаёт фильмы ---");
var movie1 = admin.CreateMovie(
    new Title("Начало"),
    new Description("Фильм о снах внутри снов"),
    new ReleaseYear(2010),
    new Genre("Sci-Fi"),
    new PosterUrl("https://example.com/inception.jpg")
);
var movie2 = admin.CreateMovie(
    new Title("Матрица"),
    new Description("Что есть реальность?"),
    new ReleaseYear(1999),
    new Genre("Action")
);
Console.WriteLine($"Создан фильм: {movie1}");
Console.WriteLine($"Создан фильм: {movie2}\n");


Console.WriteLine("--- Создание пользователей ---");
var user1 = new User(new Username("alice"));
var user2 = new User(new Username("bob"));
Console.WriteLine($"Создан: {user1}");
Console.WriteLine($"Создан: {user2}\n");


Console.WriteLine("--- Пользователи выставляют оценки ---");
var rating1 = movie1.AddRating(user1, new Score(9));
var rating2 = movie1.AddRating(user2, new Score(6));
Console.WriteLine($"Добавлена: {rating1}");
Console.WriteLine($"Добавлена: {rating2}");
Console.WriteLine($"Средний рейтинг {movie1.Title}: {movie1.GetAverageScore():F1} ({movie1.GetRatingsCount()} оценок)\n");


Console.WriteLine("--- Пользователь alice меняет оценку ---");
var updatedRating = movie1.UpdateRating(user1, new Score(10));
Console.WriteLine($"Обновлена: {updatedRating}");
Console.WriteLine($"Новый средний рейтинг {movie1.Title}: {movie1.GetAverageScore():F1}\n");


Console.WriteLine("--- Пользователи пишут рецензии ---");
var review1 = movie1.AddReview(user1, new ReviewContent("Гениальный фильм, пересматриваю каждый год!"));
var review2 = movie1.AddReview(user2, new ReviewContent("Ничего не понял, но интересно."));
Console.WriteLine($"Добавлена: {review1}");
Console.WriteLine($"Добавлена: {review2}\n");


Console.WriteLine("--- alice редактирует рецензию ---");
var updatedReview = movie1.UpdateReview(user1, new ReviewContent("Лучший фильм Нолана, шедевр!"));
Console.WriteLine($"Обновлена: {updatedReview}\n");


Console.WriteLine("--- Администратор скрывает рецензию bob ---");
admin.HideReview(review2);
Console.WriteLine($"Статус рецензии bob: {review2.Status}");
Console.WriteLine($"Скрыта администратором: {review2.HiddenByAdmin}\n");


Console.WriteLine("--- Администратор восстанавливает рецензию bob ---");
admin.UnhideReview(review2);
Console.WriteLine($"Статус рецензии bob: {review2.Status}\n");


Console.WriteLine("--- Администратор удаляет оценку alice ---");
var deleted = admin.DeleteRating(rating1);
Console.WriteLine($"Успешно удалена: {deleted}");
Console.WriteLine($"Оценка alice активна: {rating1.IsActive}");
Console.WriteLine($"Средний рейтинг после удаления: {movie1.GetAverageScore():F1} ({movie1.GetRatingsCount()} оценок)\n");


Console.WriteLine("--- Администратор обновляет фильм Начало ---");
var updatedMovie = admin.UpdateMovie(
    movie1,
    new Title("Inception (Начало)"),
    new Description("Режиссёр Кристофер Нолан"),
    new ReleaseYear(2010),
    new Genre("Sci-Fi / Thriller"),
    null
);
Console.WriteLine($"Обновлён: {updatedMovie}\n");


Console.WriteLine("--- Попытка обновить Permissions тем же значением ---");
var permChanged = admin.UpdatePermissions(new Permissions("manage_movies,moderate_reviews"));
Console.WriteLine($"Permissions изменены: {permChanged}\n");


Console.WriteLine("--- bob пытается поставить вторую оценку на Начало ---");
try
{
    movie1.AddRating(user2, new Score(7));
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}


Console.WriteLine("--- Попытка создать Score(15) ---");
try
{
    var badScore = new Score(15);
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}


Console.WriteLine("--- Попытка удалить Начало (есть активные рецензии) ---");
try
{
    admin.DeleteMovie(movie1);
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}


Console.WriteLine("--- Попытка создать второй фильм Матрица ---");
try
{
    admin.CreateMovie(new Title("Матрица"));
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}


Console.WriteLine("--- Удаление Матрица (нет оценок и рецензий) ---");
var result = admin.DeleteMovie(movie2);
Console.WriteLine($"{movie2.Title} удалён: {result} / IsDeleted={movie2.IsDeleted}\n");


Console.WriteLine("--- Попытка повторно удалить Матрица ---");
try
{
    admin.DeleteMovie(movie2);
}
catch (Exception ex)
{
    Console.WriteLine($"Ожидаемая ошибка: {ex.GetType().Name} — {ex.Message}\n");
}