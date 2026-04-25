using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject.Exceptions
{
    // Некорректная оценка - слишком высокая оценка
    public class BigScoreException : DomainArgumentOutOfRangeException
    {
        public BigScoreException(int score)
            : base(nameof(score), score,
                $"Оценка должна быть не больше 10, но получено {score}.")
        { }
    }
}
