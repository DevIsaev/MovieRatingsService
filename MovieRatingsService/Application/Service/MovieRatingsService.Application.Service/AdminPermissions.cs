namespace MovieRatingsService.Application.Service
{
    //названия прав администратора в одной строке через запятую в одной строке
    public static class AdminPermissions
    {
        //создание, изменение и удаление фильмов
        public const string ManageMovies = "manage_movies";

        //скрытие и возврат рецензий
        public const string ModerateReviews = "moderate_reviews";

        //удаление оценок пользователей
        public const string ModerateRatings = "moderate_ratings";
    }
}
